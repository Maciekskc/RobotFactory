@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

@description('Application Name for resource to based their name on')
param appName string = 'rf'

@description('Environment Name for resource to based their name on')
param  environmentName string = 'dev'

@description('Environment Name for resource to based their name on')
param  resourceVersion string = '01'

@description('Specifies the name of the key vault.')
param keyVaultName string = toLower('kv-${appName}-${environmentName}-${resourceVersion}')

@description('Specifies the Azure Active Directory tenant ID that should be used for authenticating requests to the key vault. Get it by using Get-AzSubscription cmdlet.')
param tenantId string = subscription().tenantId

@description('Array of secrets that will be by default added to KV. Construction [{key: <keyname>, value: <keyvalue>}, ...]')
param kvInitialSecrets array

@description('Principal Id to assigne secret officer role.')
param vaultAdministratorPrincipalId string

@description('Specifies the SecretOfficer RoleId')
var kvSecretOfficerRoleId = 'b86a8fe4-44ce-4948-aee5-eccb2c155cd7'

@description('Specifies whether the key vault is a standard vault or a premium vault.')
@allowed([
  'standard'
  // 'premium'
])
param skuName string = 'standard'

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: keyVaultName
  location: location
  properties: {
    tenantId: tenantId
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    sku: {
      name: skuName
      family: 'A'
    }
    accessPolicies:[]
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
  }
}

resource secrets 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = [for secret in kvInitialSecrets: {
  parent: keyVault
  name: secret.key
  properties: {
    value: secret.value
  }
}]

resource secretOfficerRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(subscription().id, keyVaultName, kvSecretOfficerRoleId)
  scope: keyVault
  properties: {
    principalId: vaultAdministratorPrincipalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', kvSecretOfficerRoleId)
  }
}

output keyVaultId string = keyVault.id
