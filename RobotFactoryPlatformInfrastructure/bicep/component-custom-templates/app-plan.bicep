@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

@description('Application Name for resource to based their name on')
param appName string = 'rf'

@description('Environment Name for resource to based their name on')
param  environmentName string = 'dev'

@description('Environment Name for resource to based their name on')
param  resourceVersion string = '01'

@description('Specifies the name of the webapp plan')
param appServicePlanName string = toLower('appplan-${appName}-${environmentName}-${resourceVersion}')

@description('Specifies the SKU of app plan. If empty, use F1 by default')
param sku string = 'F1'

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

output appServicePlanName string = appServicePlan.name
output appServicePlanResourceGroupName string = resourceGroup().name
