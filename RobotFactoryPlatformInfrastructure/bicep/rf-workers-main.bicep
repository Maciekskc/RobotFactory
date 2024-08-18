@description('Environment Name for resource to based their name on')
param environmentName string = 'dev'

@description('Region Name for resource to based their name on')
param regionName string = resourceGroup().location

@description('Environment Name for resource to based their name on')
param resourceVersion string

param organizersInitialSettings array = []
param assemblersInitialSettings array = []

var managedEnvironmentName = 'rfworkers'
var organizerAppName = 'rforganizer'
var assemblerAppName = 'rfassembler'

var managedEnvName = toLower('managedenv-${managedEnvironmentName}-${environmentName}-${resourceVersion}')

resource managedEnv 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: managedEnvName
  location: regionName
  properties: {}
}

module organizersApp 'component-custom-templates/aca.bicep' = {
  name: '${deployment().name}-organizers-webapp'
  scope: resourceGroup()
  params: {
    appSettings: organizersInitialSettings
    appName: organizerAppName
    location: regionName
    environmentName: environmentName
    resourceVersion: resourceVersion
    managedEnvName: managedEnvName
  }
  dependsOn: [managedEnv]
}

module assemblersApp 'component-custom-templates/aca.bicep' = {
  name: '${deployment().name}-assemblers-webapp'
  scope: resourceGroup()
  params: {
    appSettings: assemblersInitialSettings
    appName: assemblerAppName
    location: regionName
    environmentName: environmentName
    resourceVersion: resourceVersion
    managedEnvName: managedEnvName
  }
  dependsOn: [managedEnv]
}
