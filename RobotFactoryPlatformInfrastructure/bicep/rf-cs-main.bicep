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
param initializeRobotCreationQueueName  string = 'initialize-robot-creation-queue'
param storageAccountName string 
@secure()
param storageAccountKey string 

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
        value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccountKey}'
      }
      {
        name: 'StorageQueueConnection'
        value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccountKey}'
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
