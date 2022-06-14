# Azure Function connected to SQL
This project is a reference implementation for a simple REST API using Azure Functions and Azure SQL.

## Set up
You will need:
- [Dotnet 6.0 SDK](https://dotnet.microsoft.com/download)
- [Functions Core Tools v4](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#v2)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- Git command line tools
- This project uses the *SQL Biding for Azure Functions* which is in preview. Follow [this link](https://github.com/Azure/azure-functions-sql-extension) for more information about this extension. If you are creating your own project, use the `prerelease` flag to install the extension: `dotnet add package Microsoft.Azure.WebJobs.Extensions.Sql --prerelease`.

Fork and clone this repository:
```bash
git clone https://github.com/aborsato/csa-sqlfunction.git
cd csa-sqlfunction
dotnet restore
```

## Run locally
Create a file `local.settings.json` at your root folder:
```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "SqlConnectionString": "YOUR_SQL_CONNECTION_STRING"
    }
}
```

Run the command:
```bash
func start
```

Test with `curl` command:
```bash
# List all
curl -s http://localhost:7071/api/books | jq

# Get one
curl -s http://localhost:7071/api/books/1 | jq

# Insert
curl -s --header "Content-Type: application/json" --request PUT --data '{"id":"2","author":"Alan Borsato", "title": "My Life", "pages": 865}' http://localhost:7071/api/books | jq
```


## Deploy to Azure
This assumes you already have an Azure Subscription (or know how to request one), a Resource Group and an Azure SQL database ready for connection.
The first step is to create the table for running this project:
```sql
CREATE TABLE dbo.Book (
    Id int,
    Author varchar(255),
    Title varchar(255),
    Pages int
);
ALTER TABLE dbo.Book ALTER COLUMN Id int NOT NULL;
ALTER TABLE dbo.Book ADD CONSTRAINT PKey PRIMARY KEY CLUSTERED (Id);
```

Insert a few rows like this:
```sql
INSERT INTO dbo.Book (1, 'Alan Borsato', 'My Work', 588);
```

Follow [these steps](https://docs.microsoft.com/en-us/azure/azure-functions/create-first-function-cli-csharp?tabs=azure-cli%2Cin-process#create-supporting-azure-resources-for-your-function) to create the Functions App in your Azure subscription.

Deploy the function to Azure:
```bash
func azure functionapp publish <APP_NAME>
```

## Best practices

- [Performance and reliability](https://docs.microsoft.com/en-us/azure/azure-functions/functions-best-practices)
- [Manage connections](https://docs.microsoft.com/en-us/azure/azure-functions/manage-connections)
- [Error handling and function retries](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-error-pages?tabs=csharp)
- [Security](https://docs.microsoft.com/en-us/azure/azure-functions/security-concepts)
