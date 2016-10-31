using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Builder.History;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Internals.Scorables;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Http;
using WeatherBotDemo.Api.Extensions;
using WeatherBotDemo.Services.BingTranslator;

namespace WeatherBotDemo.Api
{
    public static class BotFrameworkConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            builder
                .Register(c => new BingTranslatorClient("Test187871", "dAnT3r/eIc8KedBRUgRCV+juxpf4Wl312jn1Bd2SXzk="))
                .As<ITranslator>()
                .InstancePerDependency();

            builder
                .Register(c => new LogBotToUser(new MapToChannelData_BotToUser(
                        c.Resolve<AlwaysSendDirect_BotToUser>(),
                        new List<IMessageActivityMapper> { new KeyboardCardMapper() }), c.Resolve<IActivityLogger>()))
                    .AsSelf()
                    .InstancePerLifetimeScope();

            builder
                .Register(c => new TranslatedBotToUser(c.Resolve<LogBotToUser>(),
                        c.Resolve<ITranslator>()))
                    .As<IBotToUser>()
                    .InstancePerLifetimeScope();

            builder
                .Register(c =>
                {
                    var cc = c.Resolve<IComponentContext>();

                    Func<IPostToBot> makeInner = () =>
                    {
                        var task = cc.Resolve<DialogTask>();
                        IDialogStack stack = task;
                        IPostToBot post = task;
                        post = new ReactiveDialogTask(post, stack, cc.Resolve<IStore<IFiberLoop<DialogTask>>>(), cc.Resolve<Func<IDialog<object>>>());
                        post = new ExceptionTranslationDialogTask(post);
                        post = new LocalizedDialogTask(post);
                        post = new ScoringDialogTask<double>(post, stack, cc.Resolve<TraitsScorable<IActivity, double>>());
                        return post;
                    };

                    IPostToBot outer = new PersistentDialogTask(makeInner, cc.Resolve<IBotData>());
                    outer = new SerializingDialogTask(outer, cc.Resolve<IAddress>(), c.Resolve<IScope<IAddress>>());
                    outer = new PostUnhandledExceptionToUserTask(outer, cc.Resolve<IBotToUser>(), cc.Resolve<ResourceManager>(), cc.Resolve<TraceListener>());
                    outer = new LogPostToBot(outer, cc.Resolve<IActivityLogger>());
                    outer = new TranslatedPostToBot(outer, cc.Resolve<ITranslator>());
                    return outer;
                })
                .As<IPostToBot>()
                .InstancePerLifetimeScope();

            builder.Update(Conversation.Container);
        }
    }
}