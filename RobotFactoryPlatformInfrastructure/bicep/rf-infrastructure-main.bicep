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

var sharedResourceGroupName = toLower('rg-${appName}shared-${environmentName}-${resourceVersion}')
var componentSuplierResourceGroupName = toLower('rg-${appName}supplier-${environmentName}-${resourceVersion}')
var controllerResourceGroupName = toLower('rg-${appName}controller-${environmentName}-${resourceVersion}')

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

resource controllerResourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: controllerResourceGroupName
  location: location
}

module api 'rf-api-main.bicep' = {
  name: '${deployment().name}-factoryapi'
  scope: controllerResourceGroup
  params: {
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
    vaultAdministratorPrincipalId: vaultAdministratorPrincipalId
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
  }
}
