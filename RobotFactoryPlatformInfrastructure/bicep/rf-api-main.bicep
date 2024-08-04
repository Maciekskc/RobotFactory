param vaultAdministratorPrincipalId string = ''

@description('Application Name for resource to based their name on')
param appName string

@description('Environment Name for resource to based their name on')
param  environmentName string

@description('Region Name for resource to based their name on')
param regionName string = resourceGroup().location

@description('Environment Name for resource to based their name on')
param  resourceVersion string

@description('Specifies the SecretUser RoleId')
var kvSecretUserRoleId = '4633458b-17de-408a-b874-0445c86b69e6'

@description('Specifies the name of the webapp managed identity')
var webSiteIdentity = toLower('id-${appName}-${environmentName}-${regionName}-${resourceVersion}')

@description('Specifies the name of the key vault.')
var keyVaultName = toLower('kv-${appName}-${environmentName}-${resourceVersion}')

param apiInitialSecrets array = []

var appInitialSettings = [
  {
    key: 'AzureADManagedIdentityClientId'
    value: msi.properties.clientId
  }
  {
    key: 'KeyVaultName'
    value: keyVaultName
  }
]

var appIdentity  = {
  type: 'UserAssigned'
  userAssignedIdentities: {
    '${msi.id}': {}
  }
}

resource msi 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: webSiteIdentity
  location: resourceGroup().location
}

module vault 'component-custom-templates/kv.bicep' = {
  name: '${deployment().name}-vault'
  scope: resourceGroup()
  params: {
    keyVaultName: keyVaultName
    kvInitialSecrets: apiInitialSecrets
    vaultAdministratorPrincipalId: vaultAdministratorPrincipalId
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
  }
}

resource existingKeyVault 'Microsoft.KeyVault/vaults@2021-06-01-preview' existing = {
  name: keyVaultName
  scope: resourceGroup()
}

resource secretUserRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name:  guid(msi.id, resourceGroup().id, kvSecretUserRoleId)
  scope: existingKeyVault
  properties: {
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', kvSecretUserRoleId)
    principalId: msi.properties.principalId
  }
  dependsOn:[
    vault
  ]
}

module app 'component-custom-templates/web-app.bicep' = {
  name: '${deployment().name}-webapp'
  scope: resourceGroup()
  params: {
    appIdentity: appIdentity
    appSettings: appInitialSettings
    appName: appName
    environmentName: environmentName
    resourceVersion: resourceVersion
  }
}

output appUri string = 'https://${app.outputs.appHostName}'
