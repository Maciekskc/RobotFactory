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
    }
}
