namespace RobotFactory.Workers.ComponentAssemblers.Helpers
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

        public string MountHeadQueueName
        {
            get;
            set;
        }

        public string MountHeadQueueSasToken
        {
            get;
            set;
        }

        public string MountArmsQueueName
        {
            get;
            set;
        }

        public string MountArmsQueueSasToken
        {
            get;
            set;
        }

        public string MountLegsQueueName
        {
            get;
            set;
        }

        public string MountLegsQueueSasToken
        {
            get;
            set;
        }

        public string FinalizeConstructionQueueName
        {
            get;
            set;
        }

        public string FinalizeConstructionQueueSasToken
        {
            get;
            set;
        }
    }
}
