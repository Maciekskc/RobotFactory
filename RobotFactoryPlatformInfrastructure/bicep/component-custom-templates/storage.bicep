@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

@description('Application Name for resource to based their name on')
param appName string = 'rf'

@description('Environment Name for resource to based their name on')
param  environmentName string = 'dev'

@description('Environment Name for resource to based their name on')
param  resourceVersion string = '01'

@description('Storage Account type')
@allowed([
  'Premium_LRS'
  'Premium_ZRS'
  'Standard_GRS'
  'Standard_GZRS'
  'Standard_LRS'
  'Standard_RAGRS'
  'Standard_RAGZRS'
  'Standard_ZRS'
])
param storageAccountType string

@description('Specifies the name of the webapp plan')
var storageAccountName = toLower('sa${appName}${environmentName}${location}${resourceVersion}')

@description('List of queues to create')
param queueNames array

resource sa 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'StorageV2'
  properties: {}
}

resource queueServices 'Microsoft.Storage/storageAccounts/queueServices@2023-01-01' = {
  name: 'default'
  parent: sa
}

resource queues 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = [ for name in queueNames: {
  parent: queueServices
  name: name
  properties:{}
}]

output storageAccountName string = storageAccountName
output storageAccountKey string = sa.listKeys().keys[0].value
output storageAccountId string = sa.id

