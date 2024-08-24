using Bogus;
using MongoDB.Bson;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;

namespace RobotFactory.TestHelpers
{
    public static class ModelCreators
    {
        public static Faker<RobotComponent> RobotComponentGenerator = new Faker<RobotComponent>()
            .StrictMode(true)
            .RuleFor(o => o.Id, ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.RobotId, f => ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.ComponentType, f => f.PickRandom<RobotComponentType>())
            .RuleFor(o => o.CreatedAt, DateTime.UtcNow)
            .RuleFor(o => o.MountedAt, f => null);

        public static Faker<Arm> ArmComponentGenerator = new Faker<Arm>()
            .StrictMode(false)
            .RuleFor(o => o.Id, ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.RobotId, f => ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.ArmSite, f => f.PickRandom<ArmSiteType>())
            .RuleFor(o => o.CreatedAt, DateTime.UtcNow)
            .RuleFor(o => o.MountedAt, f => null);

        public static Faker<Leg> LegComponentGenerator = new Faker<Leg>()
            .StrictMode(false)
            .RuleFor(o => o.Id, ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.RobotId, f => ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.LegSite, f => f.PickRandom<LegSiteType>())
            .RuleFor(o => o.CreatedAt, DateTime.UtcNow)
            .RuleFor(o => o.MountedAt, f => null);

        public static Faker<Body> BodyComponentGenerator = new Faker<Body>()
            .StrictMode(false)
            .RuleFor(o => o.Id, ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.RobotId, f => ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.CreatedAt, DateTime.UtcNow)
            .RuleFor(o => o.MountedAt, f => null)
            .RuleFor(o => o.ArmsNumbers, f => 2 ^ f.Random.Number(1,2))
            .RuleFor(o => o.LegsNumber, f => 2 ^ f.Random.Number(1));

        public static Faker<Head> HeadComponentGenerator = new Faker<Head>()
            .StrictMode(false)
            .RuleFor(o => o.Id, ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.RobotId, f => ObjectId.GenerateNewId().ToString())
            .RuleFor(o => o.CreatedAt, DateTime.UtcNow)
            .RuleFor(o => o.MountedAt, f => null)
            .RuleFor(o => o.CPUCoresNumber, f => 2 ^ f.Random.Number(1, 4));

        public static Faker<Robot> RobotGenerator = new Faker<Robot>()
            .StrictMode(false)
            .WithRobotGeneratorOptions();

        private static Faker<Robot> WithRobotGeneratorOptions(this Faker<Robot> faker)
        {
            var fakeHelper = new Faker();
            RobotConstrucionStatus constructionStatus = fakeHelper.PickRandom<RobotConstrucionStatus>();

            faker.RuleFor(o => o.Id, ObjectId.GenerateNewId().ToString());
            faker.RuleFor(o => o.OrderedAt, f => f.Date.Recent(10).ToUniversalTime());

            switch (constructionStatus)
            {
                case RobotConstrucionStatus.Initialized:
                    break;

                case RobotConstrucionStatus.AwaitingComponents:
                    break;

                case RobotConstrucionStatus.ConstructionStarted:
                    faker.RuleFor(o => o.Body, f => BodyComponentGenerator.Generate());
                    break;

                case RobotConstrucionStatus.Constructed:
                    faker.RuleFor(o => o.Id, ObjectId.GenerateNewId().ToString());
                    faker.RuleFor(o => o.Head,
                        f => HeadComponentGenerator.Generate());
                    faker.RuleFor(o => o.Body, f => BodyComponentGenerator.Generate());
                    faker.RuleFor(o => o.Arms, (f, o) => ArmComponentGenerator.Generate(o.Body.ArmsNumbers));
                    faker.RuleFor(o => o.Legs, (f, o) => LegComponentGenerator.Generate(o.Body.LegsNumber));
                    faker.RuleFor(o => o.FinalizedAt, DateTime.UtcNow);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return faker;
        }
    }
}
