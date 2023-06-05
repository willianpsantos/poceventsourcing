using NUnit.Framework;
using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.IoC;
using System;
using System.Threading.Tasks;

namespace POCEventSourcing.Tests.DB
{
    public class ReadableDBTests
    {
        private ManualDependencyInjection _di;

        [SetUp]
        public void Setup()
        {
            var builder = new ManualDependencyInjectionBuilder();

            _di =
                builder
                    .BuildConfiguration(true)
                    .AppSettingsPath("appsettings.json")
                    .Build();
        }

        [Test]
        public async Task InsertTest()
        {
            var stateManager = _di.GetService<IReadableDbEntityStateManager>();

            var person = new Person
            {
                Id = 1,
                Name = "WILLIAN SANTOS",
                Document = "01005048177",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1
            };

            var id = await stateManager.InsertAsync(person);

            Assert.IsNotNull(stateManager);
            Assert.IsTrue(id > 0);
        }

        [Test]
        public async Task UpdateTest()
        {
            var stateManager = _di.GetService<IReadableDbEntityStateManager>();

            var person = new Person
            {
                Id = 1,
                Name = "WILLIAN PEREIRA SANTOS",
                Document = "02562315910",

                CreatedAt = DateTime.UtcNow,
                CreatedBy = 1,

                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 3
            };

            await stateManager.UpdateAsync(person);

            Assert.IsNotNull(stateManager);
        }

        [Test]
        public async Task DeleteTest()
        {
            var stateManager = _di.GetService<IReadableDbEntityStateManager>();

            var person = new Person { Id = 1 };

            await stateManager.DeleteAsync(person);

            Assert.IsNotNull(stateManager);
        }
    }
}
