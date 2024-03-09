@description('Specifies the name of the key vault.')
param keyVaultName string

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
param principalId string

param appIdentityId string

@description('Specifies the SecretUser RoleId')
var kvSecretUserRoleId = '4633458b-17de-408a-b874-0445c86b69e6'
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
    accessPolicies:[]
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

resource ServiceResponse 'Microsoft.KeyVault/vaults/secrets@2021-06-01-preview' = {
  parent: keyVault
  name: 'ServiceResponse'
  properties: {
    value: 'This is response deployed by bicep template'
  }
}

resource secretOfficerRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(subscription().id, keyVaultName, 'KeyVaultSecurityOfficerRoleAssignment')
  scope: keyVault
  properties: {
    principalId: principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', kvSecretOfficerRoleId)
  }
}

resource secretUserRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(appIdentityId, resourceGroup().id, kvSecretUserRoleId)
  scope: keyVault
  properties: {
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', kvSecretUserRoleId)
    principalId: appIdentityId
  }
}


output kv object = keyVault
