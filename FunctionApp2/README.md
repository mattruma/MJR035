Configuration
==

Add an `local.settings.json` file.

Copy the code snippet below into your `local.settings.json` file:

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "AzureCosmosDocumentStoreOptions:DatabaseId": "",
        "AzureCosmosDocumentStoreOptions:ConnectionString": ""
    }
}
```

For training on C# see https://www.youtube.com/playlist?list=PLdo4fOcmZ0oVxKLQCHpiUWun7vlJJvUiN.