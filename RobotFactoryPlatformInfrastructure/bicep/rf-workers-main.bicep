@description('Application Name for resource to based their name on')
param appName string

@description('Environment Name for resource to based their name on')
param  environmentName string

@description('Region Name for resource to based their name on')
param regionName string = resourceGroup().location

@description('Environment Name for resource to based their name on')
param resourceVersion string

@description('AppPlan Name that web app will be assigned too')
param appServicePlanName string

@description('Specifies the resource group name of the webapp plan')
param appServicePlanResourceGroupName string

param organizersInitialSettings array = []
param assemblersInitialSettings array = []

var organizerAppName = 'rforganizer'
var assemblerAppName = 'rfassembler'

module organizersApp 'component-custom-templates/web-app.bicep' = {
  name: '${deployment().name}-organizers-webapp'
  scope: resourceGroup()
  params: {
    appServicePlanName: appServicePlanName
    appServicePlanResourceGroupName: appServicePlanResourceGroupName
    appSettings: organizersInitialSettings
    appName: organizerAppName
    location: regionName
    environmentName: environmentName
    resourceVersion: resourceVersion
  }
}

module assemblersApp 'component-custom-templates/web-app.bicep' = {
  name: '${deployment().name}-assemblers-webapp'
  scope: resourceGroup()
  params: {
    appServicePlanName: appServicePlanName
    appServicePlanResourceGroupName: appServicePlanResourceGroupName
    appSettings: assemblersInitialSettings
    appName: assemblerAppName
    location: regionName
    environmentName: environmentName
    resourceVersion: resourceVersion
  }
}
