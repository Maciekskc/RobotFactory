@description('Specifies the name of the web application.')
param webAppName string = 'rc-api' // Generate unique String for web app name

@description('Specifies the SKU of the App Service Plan.')
param sku string = 'F1'

@description('Specifies the runtime stack of the web application.')
param linuxFxVersion string = 'DOTNETCORE|8.0'

@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

var appServicePlanName = toLower('${webAppName}-appplan')

var webSiteName = toLower('${webAppName}-app')

var webSiteIdentity = toLower('${webAppName}-msi')

@description('Specifies the name of the key vault.')
param kvName string


resource msi 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: webSiteIdentity
  location: resourceGroup().location
}

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: sku
  }
  kind: 'linux'
}

resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: webSiteName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      appSettings:[
        {
          name: 'AzureADManagedIdentityClientId'
          value: msi.properties.clientId
        }
        {
          name: 'KeyVaultName'
          value: kvName
        }
      ]
    }
  
  }
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${msi.id}': {}
    }
  }
}

output identityId string = msi.properties.principalId
