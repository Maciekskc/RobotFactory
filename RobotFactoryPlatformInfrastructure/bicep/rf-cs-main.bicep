@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

@description('Application Name for resource to based their name on')
param appName string

@description('Environment Name for resource to based their name on')
param  environmentName string

@description('Region Name for resource to based their name on')
param regionName string = location

@description('Environment Name for resource to based their name on')
param  resourceVersion string

@description('The language worker runtime to load in the function app.')
@allowed([
  'node'
  'dotnet'
  'java'
])
param runtime string = 'dotnet'

var finalizeConstructionQueueName  = 'finalize-robot-construction-queue'
var initializeRobotCreationQueueName  = 'initialize-robot-creation-queue'
var mountArmsQueueName  = 'robot-construction-mount-arms-queue'
var mountBodyQueueName  = 'robot-construction-mount-body-queue'
var mountHeadQueueName  = 'robot-construction-mount-head-queue'
var mountLegsQueueName  = 'robot-construction-mount-legs-queue'
var startConstructionQueueName  = 'start-robot-construction-queue'
var storageAccountType  = 'Standard_LRS'

module storageAcount 'component-custom-templates/storage.bicep' = {
  scope: resourceGroup()
  name: '${deployment().name}-sa'
  params: {
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
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

module function 'component-custom-templates/function.bicep' = {
  name: '${deployment().name}-function'
  scope: resourceGroup()
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
    appSettings:[
      {
        name: 'AzureWebJobsStorage'
        value: 'DefaultEndpointsProtocol=https;AccountName=${storageAcount.outputs.storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAcount.outputs.storageAccountKey}'
      }
      {
        name: 'StorageQueueConnection'
        value: 'DefaultEndpointsProtocol=https;AccountName=${storageAcount.outputs.storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAcount.outputs.storageAccountKey}'
      }
      {
        name: 'StorageQueueName'
        value: initializeRobotCreationQueueName
      }
      {
        name: 'FUNCTIONS_WORKER_RUNTIME'
        value: runtime
      }
      {
        name: 'RobotFactoryApiUri'
        value: 'TODO'
      }
    ]
  }
}
