# Azure SQL Database 101: Инструкция по установке

1. Развернуть Azure SQL Database в соответствии с ЛР 1-2 (Azure).doc
2. Проверить подключение с помощью SQL Server Management Studio
3. В коммандной строке выполнить “C:\Program Files (x86)\Microsoft SQL Server\130\DAC\bin\sqlpackage.exe” /a:Import /sf:<локальный путь к файлу AdventureWorksLT.bacpac> /tsn:<имя сервера>.database.windows.net /tdn:<название базы> /tu:<имя пользователя>@<имя сервера> /tp:<пароль>
4. Покомандно изучить скрипт DemoScript.sql

Пример команды:
“C:\Program Files (x86)\Microsoft SQL Server\130\DAC\bin\sqlpackage.exe” /a:Import /sf:C:\Data\AdventureWorksLT.bacpac /tsn:mysqlserver.database.windows.net /tdn:DemoDB /tu:sysadmin@cgrd7z8kac /tp:Pa55w0rd
