@description('Application Name for resource to based their name on')
param appName string

@description('Cosmos DB account name, max length 44 characters, lowercase')
param accountName string = toLower('${appName}-mongodb-account')

@description('Location for the Cosmos DB account.')
param location string = resourceGroup().location

@description('The primary replica region for the Cosmos DB account.')
param primaryRegion string = resourceGroup().location


@description('Specifies the MongoDB server version to use.')
@allowed([
  '3.2'
  '3.6'
  '4.0'
  '4.2'
])
param serverVersion string = '4.2'

@description('The default consistency level of the Cosmos DB account.')
param defaultConsistencyLevel string = 'Eventual'


param defaultConsistencyLevelObject object = {
  defaultConsistencyLevel: defaultConsistencyLevel
}

@description('The name for the Mongo DB database')
param databaseName string = 'RobotFactory'

@description('Maximum autoscale throughput for the database shared with up to 25 collections')
@minValue(1000)
@maxValue(1000000)
param sharedAutoscaleMaxThroughput int = 1000

var locations = [
  {
    locationName: primaryRegion
    failoverPriority: 0
    isZoneRedundant: false
  }
]

resource account 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: toLower(accountName)
  location: location
  kind: 'MongoDB'
  properties: {
    consistencyPolicy: defaultConsistencyLevelObject
    locations: locations
    databaseAccountOfferType: 'Standard'
    enableAutomaticFailover: true
    enableFreeTier: true
    apiProperties: {
      serverVersion: serverVersion
    }
    capabilities: [
      {
        name: 'DisableRateLimitingResponses'
      }
    ]
  }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/mongodbDatabases@2022-05-15' = {
  parent: account
  name: databaseName
  properties: {
    resource: {
      id: databaseName
    }
    options: {
      autoscaleSettings: {
        maxThroughput: sharedAutoscaleMaxThroughput
      }
    }
  }
}
