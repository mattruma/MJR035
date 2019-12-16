Param(
    [string]
    $Name,
    [string]
    $Location,
    [string]
    $Build
)

# The variable names that will be used to create the Various Resources in Azure

$ResourceGroupName = "$($Name)-$($Build)-rg"
$KeyVaultName = "$($Name)-$($Build)-kv"
$FunctionAppName = "$($Name)-$($Build)-app"
$FunctionAppStorageName = "$($Name)$($Build)appstore"
$ApplicationInsightsName = "$($Name)-$($Build)-ai"
$StorageAccountName = "$($Name)$($Build)store"

$CosmosDbAccountName = "$($Name)-$($Build)-cosmos"
$CosmosDbDatabaseName = "$($Name)-$($Build)-db"
$CosmosDbCollectionName = "data"

#region Create Azure Resource Group

az group create `
    --name $ResourceGroupName `
    --location $Location

#endregion

#region Create Azure Key Vault

az keyvault create `
    --name $KeyVaultName `
    --resource-group $ResourceGroupName

#endregion

#region Create Azure Application Insights & Save Key (in Key Vault)

 $AppInsightsInstrumentationKey = az resource create `
    --resource-type "Microsoft.Insights/components" `
    --name $ApplicationInsightsName `
    --resource-group $ResourceGroupName -l $location `
    --properties '{ \"Application_Type\": \"other\" }' `
    --query "properties.InstrumentationKey" `
    -o tsv

az keyvault secret set --vault-name $KeyVaultName --name "APPINSIGHTSINSTRUMENTATIONKEY" --value $AppInsightsInstrumentationKey

#endregion

#region Create Azure Function App's Storage Account

az storage account create `
    -g $ResourceGroupName `
    -n $FunctionAppStorageName `
    --kind StorageV2 `
    --sku "Standard_RAGRS" `
    --https-only true

#endregion

#region Create Azure Function App

az functionapp create `
    -n $FunctionAppName `
    --consumption-plan-location $Location `
    --resource-group $ResourceGroupName `
    --storage-account $FunctionAppStorageName `
    --app-insights-key $AppInsightsInstrumentationKey `
    --runtime dotnet

#endregion

#region Create Storage Account

az storage account create `
    -g $ResourceGroupName `
    -n $StorageAccountName `
    --kind StorageV2 `
    --sku "Standard_RAGRS" `
    --https-only true

$StorageAccountId = az storage account show -n $StorageAccountName -g $ResourceGroupName --query id -o tsv
$StorageAccountPrimaryConnectionString = az storage account show-connection-string --ids $StorageAccountId --key Primary -o tsv
$StorageAccountSecondaryConnectionString = az storage account show-connection-string --ids $StorageAccountId --key Secondary -o tsv

az keyvault secret set --vault-name $KeyVaultName --name "STORAGE-PRIMARY-CSTRING" --value $StorageAccountPrimaryConnectionString
az keyvault secret set --vault-name $KeyVaultName --name "STORAGE-SECONDARY-CSTRING" --value $StorageAccountSecondaryConnectionString

#endregion

#region Create Azure Cosmos DB

az cosmosdb create `
    -g $ResourceGroupName `
    -n $CosmosDbAccountName `
    --kind GlobalDocumentDB

[string[]]$CosmosDbExistingDatabases = az cosmosdb database list `
    -n $CosmosDbAccountName -g $ResourceGroupName --query "[].id" -o tsv

if ($null -eq $CosmosDbExistingDatabases -or -NOT $CosmosDbExistingDatabases.Contains($CosmosDbDatabaseName)) {
    az cosmosdb database create -g $ResourceGroupName -n $CosmosDbAccountName `
        -d $CosmosDbDatabaseName
} else {
    Write-Output "------------------------------------------------------------"
    Write-Output "Database with name '$($CosmosDbDatabaseName)' already exists"
    Write-Output "------------------------------------------------------------"
}

[string[]]$CosmosDbExistingCollections = az cosmosdb collection list `
    -d $CosmosDbDatabaseName -n $CosmosDbAccountName -g $ResourceGroupName `
    --query "[].id" -o tsv

if ($null -eq $CosmosDbExistingCollections -or -NOT $CosmosDbExistingCollections.Contains($CosmosDbCollectionName)) {
    az cosmosdb collection create -g $ResourceGroupName -n $CosmosDbAccountName `
        -d $CosmosDbDatabaseName -c $CosmosDbCollectionName --default-ttl 2592000 `
        --partition-key-path '/docId' --throughput 400
} else {
    Write-Output "----------------------------------------------------------------"
    Write-Output "Collection with name '$($CosmosDbCollectionName)' already exists"
    Write-Output "----------------------------------------------------------------"
}

[string[]]$CosmosKeys = az cosmosdb keys list -n $CosmosDbAccountName -g $ResourceGroupName `
    --query "[primaryMasterKey,secondaryMasterKey]" -o tsv

[string]$CosmosDocumentEndpoint = az cosmosdb show -n $CosmosDbAccountName -g $ResourceGroupName --query "documentEndpoint" -o tsv

$CosmosPrimaryKey = $CosmosKeys[0]
$CosmosSecondaryKey = $CosmosKeys[1]

[string]$CosmosPrimaryConnectionString = "AccountEndpoint=$($CosmosDocumentEndpoint);AccountKey=$($CosmosPrimaryKey);"
[string]$CosmosSecondaryConnectionString = "AccountEndpoint=$($CosmosDocumentEndpoint);AccountKey=$($CosmosSecondaryKey);"

az keyvault secret set --vault-name $KeyVaultName --name "COSMOS-ENDPOINT" --value $CosmosDocumentEndpoint
az keyvault secret set --vault-name $KeyVaultName --name "COSMOS-PRIMARY-KEY" --value $CosmosKeys[0]
az keyvault secret set --vault-name $KeyVaultName --name "COSMOS-SECONDARY-KEY" --value $CosmosKeys[1]
az keyvault secret set --vault-name $KeyVaultName --name "COSMOS-DATABASE-NAME" --value $CosmosDbDatabaseName
az keyvault secret set --vault-name $KeyVaultName --name "COSMOS-COLLECTION-NAME" --value $CosmosDbCollectionName
az keyvault secret set --vault-name $KeyVaultName --name "COSMOS-PRIMARY-CSTRING" --value $CosmosPrimaryConnectionString
az keyvault secret set --vault-name $KeyVaultName --name "COSMOS-SECONDARY-CSTRING" --value $CosmosSecondaryConnectionString


#endregion



