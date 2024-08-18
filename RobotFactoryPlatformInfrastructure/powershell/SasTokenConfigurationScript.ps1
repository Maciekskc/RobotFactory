param (
    [Parameter(Mandatory = $true)]
    [string]$jsonInput
)

# Function to generate a SAS token for a queue using a connection string
function Generate-SASToken {
    param (
        [string]$storageAccountResourceGroup,
        [string]$storageAccountName,
        [string]$queueName
    )

    # Get the connection string using Azure CLI
    $connectionString = az storage account show-connection-string `
        --resource-group $storageAccountResourceGroup `
        --name $storageAccountName `
        --key 'key2' `
        --output tsv

    # Set the SAS token expiration time
    $expiry = (Get-Date).AddMonths(1).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") 

    # Generate the SAS token using the connection string
    $sasToken = az storage queue generate-sas `
        --name $queueName `
        --permissions ra `
        --expiry $expiry `
        --connection-string $connectionString `
        --output tsv

    return $sasToken
}

# Function to set secrets in Azure Key Vault
function Set-KVSecret {
    param (
        [string]$kvName,
        [string]$secretName,
        [string]$secretValue
    )

    # Set the secret in Key Vault using Azure CLI
    az keyvault secret set `
        --vault-name $kvName `
        --name $secretName `
        --value "$($secretValue.Replace('&', '"&"'))" `
        --output none
}

# Function to set application environment variables in Azure App Service
function Set-AppSettings {
    param (
        [string]$resourceGroupName,
        [string]$appName,
        [hashtable]$tokens,
        [array]$configNames
    )

    # Prepare the settings string for Azure CLI
    $settingsArray = @()
    foreach ($config in $configNames) {
        $key = $config.SecretName
        $value = $tokens[$config.QueueName].Replace('&', '"&"')
        $settingsArray += "$key=$value"
    }

    # Set the app settings using Azure CLI
    az webapp config appsettings set `
        --resource-group $resourceGroupName `
        --name $appName `
        --settings $settingsArray `
        --output none
}

# Parse the input JSON
$data = $jsonInput | ConvertFrom-Json

# Dictionary to store SAS tokens
$sasTokens = @{}

# Generate SAS tokens for each queue
foreach ($queueName in $data.QueueNames) {
    $sasTokens[$queueName] = Generate-SASToken -storageAccountResourceGroup $data.StorageAccountResourceGroup -storageAccountName $data.StorageAccountName -queueName $queueName
}

# Set secrets in ControllerSecrets' Key Vault
foreach ($secret in $data.ControllersSecrets.SecretsToSet) {
    $sasToken = $sasTokens[$secret.QueueName]
    Set-KVSecret -kvName $data.ControllersSecrets.KvName -secretName $secret.SecretName -secretValue $sasToken
}

# Set environment variables for WorkerOrganizersSettings
Set-AppSettings -resourceGroupName $data.WorkerOrganizersSettings.ResourceGroupName -appName $data.WorkerOrganizersSettings.AppName -tokens $sasTokens -configNames $data.WorkerOrganizersSettings.ConfigsToSet

# Set environment variables for WorkerAssemblersSettings
Set-AppSettings -resourceGroupName $data.WorkerAssemblersSettings.ResourceGroupName -appName $data.WorkerAssemblersSettings.AppName -tokens $sasTokens -configNames $data.WorkerAssemblersSettings.ConfigsToSet

