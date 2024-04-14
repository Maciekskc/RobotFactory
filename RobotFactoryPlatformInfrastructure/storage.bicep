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
param storageAccountType string = 'Standard_LRS'

@description('The storage account location.')
param location string = resourceGroup().location

@description('The name of the storage account')
param storageAccountName string = 'robotfactorystorage'


@description('Initialize robot creation queue name')
param initializeRobotCreationQueueName string 

@description('Start construction queue name')
param startConstructionQueueName string 

@description('Mount body queue name')
param mountBodyQueueName string 

@description('Mount head queue name')
param mountHeadQueueName string 

@description('Mount arms queue name')
param mountArmsQueueName string 

@description('Mount legs queue name')
param mountLegsQueueName string 

@description('Finalize construction queue name')
param finalizeConstructionQueueName string 


resource sa 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'StorageV2'
  properties: {}
}

resource initializeRobotCreationQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = {
  name: '${sa.name}/default/${initializeRobotCreationQueueName}'
  properties:{}
}

resource startConstructionQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = {
  name: '${sa.name}/default/${startConstructionQueueName}'
  properties:{}
}

resource mountBodyQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = {
  name: '${sa.name}/default/${mountBodyQueueName}'
  properties:{}
}

resource mountHeadQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = {
  name: '${sa.name}/default/${mountHeadQueueName}'
  properties:{}
}

resource mountArmsQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = {
  name: '${sa.name}/default/${mountArmsQueueName}'
  properties:{}
}

resource mountLegsQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = {
  name: '${sa.name}/default/${mountLegsQueueName}'
  properties:{}
}

resource finalizeConstructionQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2023-01-01' = {
  name: '${sa.name}/default/${finalizeConstructionQueueName}'
  properties:{}
}

output storageAccountName string = storageAccountName
output storageAccountId string = sa.id

