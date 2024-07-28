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


module db 'component-custom-templates/cosmos-with-mongodb.bicep' = {
  name: '${deployment().name}-db'
  scope: resourceGroup()
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
  }
}

module api  'rf-api-main.bicep' = {
  name: '${deployment().name}-db'
  scope: resourceGroup()
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
    vaultAdministratorPrincipalId: vaultAdministratorPrincipalId
  }
}

module cs 'rf-cs-main.bicep' = {
  name: '${deployment().name}-db'
  scope: resourceGroup()
  params:{
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
    regionName: regionName
  }
}
