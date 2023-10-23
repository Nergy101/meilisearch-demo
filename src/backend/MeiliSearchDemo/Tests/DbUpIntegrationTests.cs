using DbUp;
using DbUp.Engine;
using FluentAssertions;
using Infrastructure;

namespace Tests
{
    [TestClass]
    public class DbUpIntegrationTests
    {

        private readonly UpgradeEngine _upgradeEngine;

        public DbUpIntegrationTests()
        {
            _upgradeEngine = DeployChanges.To
                .SQLiteDatabase("DataSource=:memory:")
                .WithScriptsEmbeddedInAssembly(typeof(DemoSeeder).Assembly)
                .LogToConsole()
                .LogScriptOutput()
                .JournalToSQLiteTable("MigrationHistory")
                .Build();
        }

        [TestMethod]
        public void RunMigrations_DoesntThrowExceptions()
        {
            // Arrange Act
            Action act = () => _upgradeEngine.PerformUpgrade();

            // Assert
            act.Should().NotThrow();
        }
    }
}