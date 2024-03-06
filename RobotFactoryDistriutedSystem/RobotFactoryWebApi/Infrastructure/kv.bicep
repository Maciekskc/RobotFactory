@description('Specifies the name of the key vault.')
param keyVaultName string = 'robot-factory-api-kv'

@description('Specifies the Azure location where the key vault should be created.')
param location string = resourceGroup().location

@description('Specifies whether Azure Virtual Machines are permitted to retrieve certificates stored as secrets from the key vault.')
param enabledForDeployment bool = false

@description('Specifies whether Azure Disk Encryption is permitted to retrieve secrets from the vault and unwrap keys.')
param enabledForDiskEncryption bool = false

@description('Specifies whether Azure Resource Manager is permitted to retrieve secrets from the key vault.')
param enabledForTemplateDeployment bool = false

@description('Specifies the Azure Active Directory tenant ID that should be used for authenticating requests to the key vault. Get it by using Get-AzSubscription cmdlet.')
param tenantId string = subscription().tenantId

@description('Specifies the object ID of a user, service principal or security group in the Azure Active Directory tenant for the vault. The object ID must be unique for the list of access policies. Get it by using Get-AzADUser or Get-AzADServicePrincipal cmdlets.')
param objectId string

@description('Specifies the permissions to keys in the vault. Valid values are: all, encrypt, decrypt, wrapKey, unwrapKey, sign, verify, get, list, create, update, import, delete, backup, restore, recover, and purge.')
param keysPermissions array = [
  'list'
]

@description('Specifies the permissions to secrets in the vault. Valid values are: all, get, list, set, delete, backup, restore, recover, and purge.')
param secretsPermissions array = [
  'list'
]

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
    enabledForDeployment: enabledForDeployment
    enabledForDiskEncryption: enabledForDiskEncryption
    enabledForTemplateDeployment: enabledForTemplateDeployment
    tenantId: tenantId
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    sku: {
      name: skuName
      family: 'A'
    }
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
  }
}

resource MongoConnectionString 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'MongoDatabase--ConnectionString'
  properties: {
    value: 'default'
  }
}

resource MongoDatabaseName 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'MongoDatabase--DatabaseName'
  properties: {
    value: 'default'
  }
}

resource MongoRobotCollectionName 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'MongoDatabase--RobotCollectionName'
  properties: {
    value: 'default'
  }
}

resource MongoRobotComponentCollectionName 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'MongoDatabase--RobotComponentCollectionName'
  properties: {
    value: 'default'
  }
}

resource QueueBaseUri 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'AzureStorageQueue--QueueBaseUri'
  properties: {
    value: 'default'
  }
}

resource InitializeRobotCreationQueueName 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'AzureStorageQueue--InitializeRobotCreationQueueName'
  properties: {
    value: 'default'
  }
}

resource InitializeRobotCreationQueueSasToken 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'AzureStorageQueue--InitializeRobotCreationQueueSasToken'
  properties: {
    value: 'default'
  }
}

resource StartRobotConstructionQueueName 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'AzureStorageQueue--StartRobotConstructionQueueName'
  properties: {
    value: 'default'
  }
}

resource StartRobotConstructionQueueSasToken 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'AzureStorageQueue--StartRobotConstructionQueueSasToken'
  properties: {
    value: 'default'
  }
}

resource symbolicname 'Microsoft.Authorization/roleDefinitions@2022-04-01' = {
  name: 'SecretOfficer'
  scope: resourceGroup()
  properties: {
    assignableScopes: [
      'string'
    ]
    description: 'string'
    permissions: [
      {
        dataActions: [
          'string'
        ]
      }
    ]
    roleName: 'string'
    type: 'string'
  }
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(subscription().id, keyVaultName, 'KeyVaultSecurityOfficerRoleAssignment')
  scope: keyVaultResource.id
  properties: {
    principalId: userId
    roleDefinitionId: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/b1b4b9cd-f532-48d6-8f42-80c52fe1e1d2' // Role definition for Key Vault Contributor role
  }
}
