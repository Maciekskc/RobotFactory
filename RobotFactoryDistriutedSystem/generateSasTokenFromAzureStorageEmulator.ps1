# Set the storage account connection string for the emulator
$storageAccountConnectionString = "UseDevelopmentStorage=true"

# Create a storage context
$storageContext = New-AzStorageContext -ConnectionString $storageAccountConnectionString

# Set the expiration time for the SAS token (adjust as needed)
$expirationTime = (Get-Date).AddHours(1)

# Create SAS token for the entire storage account with HTTPS protocol
$sasToken = New-AzStorageAccountSASToken -Service Blob,Table,Queue,File -ResourceType Service,Container,Object -Permission lrw -Protocol HttpsOrHttp -Context $storageContext -ExpiryTime $expirationTime

# Display the generated SAS token
Write-Host "SAS Token: $sasToken"
