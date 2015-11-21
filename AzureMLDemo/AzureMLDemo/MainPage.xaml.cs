using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace AzureMLDemo
{
    public sealed partial class MainPage : Page
    {
        // !! Azure ML endpoint parameters. Get them from your Azure ML web service dashboard and put them here.
        const string _apiKey = @"Q3tnqOuXgZ1uJ8ycaJeVGeijzOs8N5nPCBI2EDjV1iN5/FTKTqCzd0GhDcOYm8i0lJHzo5ASD63TgBfQv4RulQ==";
        const string _requestUri = @"https://ussouthcentral.services.azureml.net/workspaces/3d4ca463d4e6452389e1fb64d1886ff6/services/5590d149178f4a8eb8da9d62c05ed82e/execute?api-version=2.0&details=true";

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary> 
        /// Invoked when this page is about to be displayed in a Frame. 
        /// </summary> 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // checks if endpoint settings was privided
            // if not, prints a disclamer and disables "Get estimate" button
            if (String.IsNullOrWhiteSpace(_apiKey) || String.IsNullOrWhiteSpace(_requestUri))
            {
                tbResult.Text = "Additional configuration is needed!\nIt looks like you have forgoten to provide your requestUri and apiKey in MainPage.xaml.cs.\nPlease make requested modifications and redeploy the app.";
                btnGetEstimate.IsEnabled = false;
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Handler for the click event of the "Get Estimate" button.
        /// </summary>
    private async void GetEstimate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // gets estimate from Azure ML service and sets it to result TextBlock
            var price = await CallAzureML(tbxMake.Text, tbxBodyStyle.Text, slWheelBase.Value, tbxNumberOfCylinders.Text, (int)slEngineSize.Value, (int)slHorsepowers.Value, (int)slPeakRPM.Value, (int)slHighwayMPG.Value);
            tbResult.Text = String.Format("You are lucky!\nToday it is as cheap as {0:c}. Don't miss your chance!", price);
        }
        catch (Exception ex)
        {
            // Shows error in result TextBlock
            tbResult.Text = String.Format("Oops! Something went wrong.\nThis can be helpful:\n{0}", ex.ToString());
        }
    }

        /// <summary>
        /// Utility method calling Azure ML Service with specified parameters
        /// </summary>
        private async Task<float> CallAzureML(string make, string bodyStyle, double wheelBase, string numberOfCylinders, int engineSize, int horsepowers, int peakRPM, int highwayMPG)
        {
            // builds request body anonymous class
            var requestBody = new
            {
                Inputs = new Dictionary<string, object>()
                {
                    {
                        "input",
                        new
                        {
                            ColumnNames = new string[] {"make", "body-style", "wheel-base", "num-of-cylinders", "engine-size", "horsepower", "peak-rpm", "highway-mpg", "price"},
                            Values = new string[,] {  { make, bodyStyle, wheelBase.ToString(), numberOfCylinders, engineSize.ToString(), horsepowers.ToString(), peakRPM.ToString(), highwayMPG.ToString(), "0" } }
                        }
                    }
                },
                GlobalParameters = new Dictionary<string, int>()
            };

            try
            {
                // opens http client and sets some connection parameters
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", _apiKey);
                    client.BaseAddress = new Uri(_requestUri);

                    // makes request
                    var response = await client.PostAsJsonAsync("", requestBody);

                    // checks response status
                    if (response.IsSuccessStatusCode)
                    {
                        // if status code is ok, gets response as a string and parses it to dynamic class using Newtonsoft.Json 
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var val = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        // attempts to get a price value and returns it if it is not null
                        float? price = (float)val?.Results?.prediction?.value?.Values[0][0];
                        if (price == null)
                            throw new InvalidDataException("Response message has unknown format or is empty.");

                        return (float)price;
                    }
                    else
                    {
                        // if status code is bad, gets error response as string and formats it to good-looking json by twofold (de)serialization 
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject(responseContent);
                        var formattedResponseContent = JsonConvert.SerializeObject(responseObject, Formatting.Indented);

                        var message = String.Format("Server returned error status code {0} with message {1}", response.StatusCode, formattedResponseContent);
                        throw new InvalidDataException(message);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
