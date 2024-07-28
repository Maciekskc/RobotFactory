param(
   [Parameter(Mandatory=$true)]
   [string]
   $appName="rf",

   [Parameter(Mandatory=$true)]
   [string]
   $vaultAdministratorPrincipalId="64b79990-5c8f-4561-b190-804d4910e8d7",

   [Parameter(Mandatory=$false)]
   [string]
   $appVersion="001",

   [Parameter(Mandatory=$false)]
   [string]
   $environment="dev",

   [Parameter(Mandatory=$false)]
   [string]
   $location="westeurope"
)

# Check if user is logged in to Azure
$loginCheck = az account show --output none --only-show-errors 2>&1
if ($loginCheck) {
   Write-Error "You are not logged in to Azure. Please log in and try again."
   exit 1
}
$resourceGroupName = "rg-$appName-$environment-$appVersion"
$rgCreationResult = az group create --name $resourceGroupName --location $location

az deployment group what-if --resource-group $resourceGroupName --template-file ..\bicep\rf-infrastructure-main.bicep --parameters environmentName=$environment appName=$appName regionName=$location resourceVersion=$appVersion