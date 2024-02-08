namespace RobotFactory.Workers.ConstructionOrganizers.Helpers
{
    public class QueueServicesConfiguration
    {
        public string QueueServiceUri
        {
            get;
            set;
        }
        public string StartConstructionQueueName
        {
            get;
            set;
        }

        public string StartConstructionQueueSasToken
        {
            get;
            set;
        }

        public string MountBodyQueueName {
            get;
            set;
        }

        public string MountBodyQueueSasToken
        {
            get;
            set;
        }
    }
}
