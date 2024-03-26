@description('Specifies the name of the function app')
param appName string = 'rc-componentsupplier-fna'
var location  = resourceGroup().location

var finalizeConstructionQueueName  = 'finalize-robot-construction-queue'
var initializeRobotCreationQueueName  = 'initialize-robot-creation-queue'
var mountArmsQueueName  = 'robot-construction-mount-arms-queue'
var mountBodyQueueName  = 'robot-construction-mount-body-queue'
var mountHeadQueueName  = 'robot-construction-mount-head-queue'
var mountLegsQueueName  = 'robot-construction-mount-legs-queue'
var startConstructionQueueName  ='start-robot-construction-queue'
var storageAccountName  = 'robotfactorystorage'
var storageAccountType  = 'Standard_LRS'

module storageAcount '../../../RobotFactoryPlatformInfrastructure/storage.bicep' = {
  scope: resourceGroup()
  name: '${deployment().name}-sa'
  params: {
    finalizeConstructionQueueName: finalizeConstructionQueueName
    initializeRobotCreationQueueName: initializeRobotCreationQueueName
    mountArmsQueueName: mountArmsQueueName
    mountBodyQueueName: mountBodyQueueName
    mountHeadQueueName: mountHeadQueueName
    mountLegsQueueName: mountLegsQueueName
    startConstructionQueueName: startConstructionQueueName
    storageAccountName: storageAccountName
    storageAccountType: storageAccountType 
    location: location
  }
}

module function 'function.bicep' = {
  name: '${deployment().name}-function'
  scope: resourceGroup()
  params:{
    appName: appName
    storageAccountName: storageAcount.outputs.storageAccountName
    storageAccountKey:storageAcount.outputs.storageAccountKey
    location:location
  }
}
