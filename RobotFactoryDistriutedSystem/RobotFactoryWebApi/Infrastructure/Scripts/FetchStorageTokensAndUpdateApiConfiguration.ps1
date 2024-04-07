param(
   [Parameter(Mandatory=$true)]
   [string]
   $resourceGroup,

   [Parameter(Mandatory=$true)]
   [string]
   $storageAccountName,

   [Parameter(Mandatory=$false)]
   [string]
   $initializeRobotCreationQueueSasToken,

   [Parameter(Mandatory=$false)]
   [string]
   $startRobotConstructionQueueSasToken,

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
if($PSBoundParameters.ContainsKey('initializeRobotCreationQueueSasToken')){
    $expiry = (Get-Date).AddHours(2).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")  # Set SAS token expiry to 2 hours from now
    $sasToken = az storage queue generate-sas --name $initializeRobotCreationQueueSasToken --permissions rw --expiry $expiry --connection-string $connectionString --output tsv
    Write-Output "SAS Token: $sasToken"
}else{
    Write-Output "'initializeRobotCreationQueueSasToken' parameter was not provided so sas token will not be generated."
}



if(!$PSBoundParameters.ContainsKey($kvName)){
    Write-Output 'Skip writing to KeyVault as kv name is not provided'
    return
}

