@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

@description('Application Name for resource to based their name on')
param appName string = 'rf'

@description('Environment Name for resource to based their name on')
param  environmentName string = 'dev'

@description('Region Name for resource to based their name on')
param regionName string = location

@description('Environment Name for resource to based their name on')
param  resourceVersion string = '01'

@description('Specifies the name of the webapp plan')
var appServicePlanName = toLower('appplan-${appName}-${environmentName}-${regionName}-${resourceVersion}')

@description('Specifies the name of the webapp')
var webSiteName = toLower('app-${appName}-${environmentName}-${regionName}-${resourceVersion}')

@description('Specifies the SKU of the App Service Plan.')
param sku string = 'F1'

@description('Specifies the runtime stack of the web application.')
param linuxFxVersion string = 'DOTNETCORE|8.0'

@description('App Identity assignment. Unless user assigned is provided, default value is system assigned.')
param appIdentity object = {
  type: 'SystemAssigned'
}


@description('Specifieds initial appsettings of webpp')
param appSettings array

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
      appSettings:[ for item in appSettings: { name: item.key, value: item.value} ]
    }
  
  }
  identity: appIdentity
}
