param(
   [Parameter(Mandatory=$true)]
   [string]
   $resourceGroup,

   [Parameter(Mandatory=$true)]
   [string]
   $storageAccountName
)

# Check if user is logged in to Azure
$loginCheck = az account show --output none --only-show-errors 2>&1
if ($loginCheck) {
   Write-Error "You are not logged in to Azure. Please log in and try again."
   exit 1
}

# Get connection string
try {
    $connectionString = az storage account show-connection-string --resource-group $resourceGroup --name $storageAccountName --key 'key2' --output tsv
}
catch {
   Write-Error "An error occurred while getting the connection string. Please verify your resource group and storage account name."
}


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