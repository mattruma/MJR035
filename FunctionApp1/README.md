Configuration
==

Add an `local.settings.json` file.

Copy the code snippet below into your `local.settings.json` file:

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet"
    }
}
```

Functions
==

* CaseNumberGenerateHttpTrigger
* AccountFetchByIdHttpTrigger
* AccountAddHttpTrigger
* AccountUpdateByIdHttpTrigger
* AccountDeleteByIdHttpTrigger
* RescheduleDaysCalculateHttpTrigger
* NextPaymentDataCalculateHttpTrigger

Entities
==

* Accounts
* Vehicles
* ???

Azure
==

* Resource Group
* Cosmos DB 
* Function App
* App Insights
* Storage Account for Functions
* Storage Account for Queues, Blobs
* Logic app for Scenario 5
* Logic app for Scenario 6

Bonus
==

* Azure Key Vault ???
* Azure DevOps ???