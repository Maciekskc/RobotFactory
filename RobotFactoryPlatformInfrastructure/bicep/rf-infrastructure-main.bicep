param vaultAdministratorPrincipalId string = ''

@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

@description('Application Name for resource to based their name on')
param appName string = 'rf'

@description('Environment Name for resource to based their name on')
param  environmentName string = 'dev'

@description('Region Name for resource to based their name on')
param regionName string = location

@description('Environment Name for resource to based their name on')
param resourceVersion string = '001'

var finalizeConstructionQueueName  = 'finalize-robot-construction-queue'
var initializeRobotCreationQueueName  = 'initialize-robot-creation-queue'
var mountArmsQueueName  = 'robot-construction-mount-arms-queue'
var mountBodyQueueName  = 'robot-construction-mount-body-queue'
var mountHeadQueueName  = 'robot-construction-mount-head-queue'
var mountLegsQueueName  = 'robot-construction-mount-legs-queue'
var startConstructionQueueName  = 'start-robot-construction-queue'
var storageAccountType  = 'Standard_LRS'

module db 'component-custom-templates/cosmos-with-mongodb.bicep' = {
  name: '${deployment().name}-mongodatabase'
  scope: resourceGroup()
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
  }
}

module api  'rf-api-main.bicep' = {
  name: '${deployment().name}-factoryapi'
  scope: resourceGroup()
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
    vaultAdministratorPrincipalId: vaultAdministratorPrincipalId
  }
}

module storageAcount 'component-custom-templates/storage.bicep' = {
  scope: resourceGroup()
  name: '${deployment().name}-storageaccount'
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

module cs 'rf-cs-main.bicep' = {
  name: '${deployment().name}-componentsupplier'
  scope: resourceGroup()
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
    initializeRobotCreationQueueName: initializeRobotCreationQueueName
    storageAccountName: storageAcount.outputs.storageAccountName
    storageAccountKey: storageAcount.outputs.storageAccountKey
  }
}
