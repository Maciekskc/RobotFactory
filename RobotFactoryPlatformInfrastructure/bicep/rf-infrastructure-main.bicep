targetScope = 'subscription'

param vaultAdministratorPrincipalId string = ''

@description('Specifies the Azure location where the resources should be deployed.')
param location string

@description('Application Name for resource to based their name on')
param appName string = 'rf'

@description('Environment Name for resource to based their name on')
param environmentName string = 'dev'

@description('Region Name for resource to based their name on')
param regionName string = location

@description('Environment Name for resource to based their name on')
param resourceVersion string = '001'

var finalizeConstructionQueueName = 'finalize-robot-construction-queue'
var initializeRobotCreationQueueName = 'initialize-robot-creation-queue'
var mountArmsQueueName = 'robot-construction-mount-arms-queue'
var mountBodyQueueName = 'robot-construction-mount-body-queue'
var mountHeadQueueName = 'robot-construction-mount-head-queue'
var mountLegsQueueName = 'robot-construction-mount-legs-queue'
var startConstructionQueueName = 'start-robot-construction-queue'
var storageAccountType = 'Standard_LRS'

var databaseName =  'RobotFactory'

var sharedResourceGroupName = toLower('rg-${appName}shared-${environmentName}-${resourceVersion}')
var componentSuplierResourceGroupName = toLower('rg-${appName}supplier-${environmentName}-${resourceVersion}')
var controllerResourceGroupName = toLower('rg-${appName}controller-${environmentName}-${resourceVersion}')
var workersResourceGroupName = toLower('rg-${appName}workers-${environmentName}-${resourceVersion}')

var apiInitialSecrets = [
  { key: 'MongoDatabase--ConnectionString', value: db.outputs.connectionString }
  { key: 'MongoDatabase--DatabaseName', value: databaseName }
  { key: 'MongoDatabase--RobotCollectionName', value: 'Robots' }
  { key: 'MongoDatabase--RobotComponentCollectionName', value: 'RobotComponents' }
  { key: 'AzureStorageQueue--QueueBaseUri', value: storageAcount.outputs.storageAccountUri }
  { key: 'AzureStorageQueue--InitializeRobotCreationQueueName', value: initializeRobotCreationQueueName }
  { key: 'AzureStorageQueue--InitializeRobotCreationQueueSasToken', value: 'default' }
  { key: 'AzureStorageQueue--StartRobotConstructionQueueName', value: startConstructionQueueName }
  { key: 'AzureStorageQueue--StartRobotConstructionQueueSasToken', value: 'default' }
  { key: 'ServiceResponse', value: 'This is response deployed by bicep template' }
]

var organizersInitialSettings = [
  { key: 'MongoDatabase__ConnectionString', value: db.outputs.connectionString }
  { key: 'MongoDatabase__DatabaseName', value: databaseName }
  { key: 'MongoDatabase__RobotCollectionName', value: 'Robots' }
  { key: 'MongoDatabase__RobotComponentCollectionName', value: 'RobotComponents' }
  { key: 'QueueServiceConfig__QueueServiceUri', value: storageAcount.outputs.storageAccountUri }
  { key: 'QueueServiceConfig__StartConstructionQueueName', value: 'start-robot-construction-queue' }
  { key: 'QueueServiceConfig__StartConstructionQueueSasToken', value: '#secret' }
  { key: 'QueueServiceConfig__MountBodyQueueName', value: 'robot-construction-mount-body-queue' }
  { key: 'QueueServiceConfig__MountBodyQueueSasToken', value: '#secret' }
  { key: 'QueueServiceConfig__FinalizeConstructionQueueName', value: 'finalize-robot-construction-queue' }
  { key: 'QueueServiceConfig__FinalizeConstructionQueueSasToken', value: '#secret' }
]

var assemblerInitialSettings = [
  { key: 'MongoDatabase__ConnectionString', value: db.outputs.connectionString }
  { key: 'MongoDatabase__DatabaseName', value: databaseName }
  { key: 'MongoDatabase__RobotCollectionName', value: 'Robots' }
  { key: 'MongoDatabase__RobotComponentCollectionName', value: 'RobotComponents' }
  { key: 'QueueServiceConfig__QueueServiceUri', value: storageAcount.outputs.storageAccountUri }
  { key: 'QueueServiceConfig__MountBodyQueueName', value: 'robot-construction-mount-body-queue' }
  { key: 'QueueServiceConfig__MountBodyQueueSasToken', value: '#secret' }
  { key: 'QueueServiceConfig__MountHeadQueueName', value: 'robot-construction-mount-head-queue' }
  { key: 'QueueServiceConfig__MountHeadQueueSasToken', value: '#secret' }
  { key: 'QueueServiceConfig__MountArmsQueueName', value: 'robot-construction-mount-arms-queue' }
  { key: 'QueueServiceConfig__MountArmsQueueSasToken', value: '#secret' }
  { key: 'QueueServiceConfig__MountLegsQueueName', value: 'robot-construction-mount-legs-queue' }
  { key: 'QueueServiceConfig__MountLegsQueueSasToken', value: '#secret' }
  { key: 'QueueServiceConfig__FinalizeConstructionQueueName', value: 'finalize-robot-construction-queue' }
  { key: 'QueueServiceConfig__FinalizeConstructionQueueSasToken', value: '#secret' }
]

resource sharedResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: sharedResourceGroupName
  location: location
}

module db 'component-custom-templates/cosmos-with-mongodb.bicep' = {
  name: '${deployment().name}-mongodatabase'
  scope: sharedResourceGroup
  params: {
    appName: appName
    environmentName: environmentName
    databaseName: databaseName
  }
}

module storageAcount 'component-custom-templates/storage.bicep' = {
  scope: sharedResourceGroup
  name: '${deployment().name}-storageaccount'
  params: {
    appName: appName
    environmentName: environmentName
    queueNames: [
      finalizeConstructionQueueName
      initializeRobotCreationQueueName
      mountArmsQueueName
      mountBodyQueueName
      mountHeadQueueName
      mountLegsQueueName
      startConstructionQueueName
    ]
    storageAccountType: storageAccountType
  }
}

module appsServicePlan 'component-custom-templates/app-plan.bicep' = {
  scope: sharedResourceGroup
  name: '${deployment().name}-appsPlan'
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    sku: 'F1'
  }
}

resource controllerResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: controllerResourceGroupName
  location: location
}

module api 'rf-api-main.bicep' = {
  name: '${deployment().name}-factoryapi'
  scope: controllerResourceGroup
  params: {
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
    vaultAdministratorPrincipalId: vaultAdministratorPrincipalId
    apiInitialSecrets: apiInitialSecrets
  }
}

resource componentSuplierResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: componentSuplierResourceGroupName
  location: location
}

module cs 'rf-cs-main.bicep' = {
  name: '${deployment().name}-componentsupplier'
  scope: componentSuplierResourceGroup
  params: {
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    initializeRobotCreationQueueName: initializeRobotCreationQueueName
    storageAccountName: storageAcount.outputs.storageAccountName
    storageAccountKey: storageAcount.outputs.storageAccountKey
    controlerApiUrl: api.outputs.appUri
  }
}

resource workersSuplierResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: workersResourceGroupName
  location: location
}

module workers 'rf-workers-main.bicep' = {
  name: '${deployment().name}-workers'
  scope: workersSuplierResourceGroup
  params: {
    environmentName: environmentName
    resourceVersion: resourceVersion
    organizersInitialSettings: organizersInitialSettings
    assemblersInitialSettings: assemblerInitialSettings
  }
}
