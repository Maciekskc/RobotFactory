@description('Specifies the Azure location where the resources should be deployed.')
param location string = resourceGroup().location

@description('Application Name for resource to based their name on')
param appName string = 'rf'

@description('Environment Name for resource to based their name on')
param  environmentName string = 'dev'

@description('Environment Name for resource to based their name on')
param  resourceVersion string = '001'

@description('Specifies the name of the webapp')
var containerAppName = toLower('aca-${appName}-${environmentName}-${resourceVersion}')

@description('App Identity assignment. Unless user assigned is provided, default value is system assigned.')
param appIdentity object = {
  type: 'SystemAssigned'
}

@description('Specifieds environment variables for the container')
param appSettings array

param managedEnvName string

param containerImage string = 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'

resource managedEnv 'Microsoft.App/managedEnvironments@2024-03-01' existing = {
  name: managedEnvName
}

resource containerApp 'Microsoft.App/containerApps@2024-03-01' = {
  name: containerAppName
  location: location
  identity: appIdentity
  properties: {
    managedEnvironmentId: managedEnv.id
    configuration: {
      ingress: {
        external: true
        targetPort: 80
      }
    }
    template: {
      containers: [
        {
          name: appName
          image: containerImage
          env: [ for item in appSettings: { name: item.key, value: item.value} ]
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
        }
      ]
    }
  }
}
