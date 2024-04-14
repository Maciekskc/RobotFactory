@description('Specifies the name of the key vault.')
param keyVaultName string = 'robot-factory-api-kv'
param vaultAdministratorPrincipalId string = ''
var location  = resourceGroup().location

module app 'web-app.bicep' = {
  name: '${deployment().name}-app'
  scope: resourceGroup()
  params:{
    location: location
    kvName: keyVaultName
  }
}

module vault 'kv.bicep' = {
  name: '${deployment().name}-vault'
  scope: resourceGroup()
  params:{
    location: location
    principalId: vaultAdministratorPrincipalId
    appIdentityId: app.outputs.identityId
    keyVaultName: keyVaultName
  }
}
