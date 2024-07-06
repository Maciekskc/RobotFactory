param(
   [Parameter(Mandatory=$true)]
   [string]
   $resourceGroup,

   [Parameter(Mandatory=$true)]
   [string]
   $storageAccountName,

   [Parameter(Mandatory=$false)]
   [string]
   $initializeRobotCreationQueueName,

   [Parameter(Mandatory=$false)]
   [string]
   $startRobotConstructionQueueName,

   [Parameter(Mandatory=$false)]
   [string]
   $kvName
)

# Check if user is logged in to Azure
$loginCheck = az account show --output none --only-show-errors 2>&1
if ($loginCheck) {
   Write-Error "You are not logged in to Azure. Please log in and try again."
   exit 1
}

# Get connection string
$connectionString = az storage account show-connection-string --resource-group $resourceGroup --name $storageAccountName --key 'key2' --output tsv

# Get queue Endpoint
try {
    $queueEndpointUri = ''
    foreach($part in ($connectionString -split ';'))
    {
        $keyValue = $part -split '='
        if($keyValue[0] -eq 'QueueEndpoint')
        {
            $queueEndpointUri = $keyValue[1];
        }
    }
    Write-Output $queueEndpointUri
}catch {
   Write-Error "An error occurred while getting queue endpoint. Connection string might be invalid."
}

# Get queue sas tokens
$expiry = (Get-Date).AddDays(1).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")  # Set SAS token expiry to 2 hours from now

if($PSBoundParameters.ContainsKey('initializeRobotCreationQueueName')){
    $initializeRobotCreationQueueSasToken = az storage queue generate-sas --name $initializeRobotCreationQueueName --permissions ra --expiry $expiry --connection-string $connectionString --output tsv
    Write-Output "$initializeRobotCreationQueueName SAS Token: $initializeRobotCreationQueueSasToken"
}else{
    Write-Output "'initializeRobotCreationQueueName' parameter was not provided so sas token will not be generated."
}

if($PSBoundParameters.ContainsKey('startRobotConstructionQueueName')){
    $startRobotConstructionQueueSasToken = az storage queue generate-sas --name $startRobotConstructionQueueName --permissions ra --expiry $expiry --connection-string $connectionString --output tsv
    Write-Output "$startRobotConstructionQueueName SAS Token: $startRobotConstructionQueueSasToken"
}else{
    Write-Output "'startRobotConstructionQueueName' parameter was not provided so sas token will not be generated."
}

if(!$PSBoundParameters.ContainsKey('kvName')){
    Write-Output 'Skip writing to KeyVault as kv name is not provided'
    return
}

Write-Output 'Adding tokens to the KV'
if ($initializeRobotCreationQueueSasToken -ne $null){
    az keyvault secret set --name 'AzureStorageQueue--InitializeRobotCreationQueueSasToken' --vault-name $kvName --value "$($initializeRobotCreationQueueSasToken.Replace('&', '"&"'))"
}else{
    Write-Output "AzureStorageQueue--InitializeRobotCreationQueueSasToken was skipped since the value was not provided"
}

if ($startRobotConstructionQueueSasToken -ne $null){
    az keyvault secret set --name 'AzureStorageQueue--StartRobotConstructionQueueSasToken' --vault-name $kvName --value "$($startRobotConstructionQueueSasToken.Replace('&', '"&"'))"
}else{
    Write-Output "AzureStorageQueue--StartRobotConstructionQueueSasToken was skipped since the value was not provided"
}