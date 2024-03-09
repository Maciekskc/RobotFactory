param webAppName string = 'rc-api' // Generate unique String for web app name
param sku string = 'F1' // The SKU of App Service Plan
param linuxFxVersion string = 'DOTNETCORE|3.0' // The runtime stack of web app
param location string = resourceGroup().location // Location for all resources
var appServicePlanName = toLower('${webAppName}-appplan')
var webSiteName = toLower('${webAppName}-app')
var webSiteIdentity = toLower('${webAppName}-msi')
param kvName string = ''

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

output identityId string = msi.properties.clientId
