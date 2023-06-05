using NUnit.Framework;
using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.IoC;
using System;
using System.Threading.Tasks;

namespace POCEventSourcing.Tests
{
    public class AllDBsTests
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
            var stateManager = _di.GetService<IFullDbEntityStateManager>();

            var person = new Person
            {
                Name = "NICOLI VIEIRA",
                Document = "02562315910",
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
            var stateManager = _di.GetService<IFullDbEntityStateManager>();

            var person = new Person
            {
                Id = 2,
                Name = "NICOLI VIEIRA DA SILVA",
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
            var stateManager = _di.GetService<IFullDbEntityStateManager>();

            var person = new Person { Id = 2 };

            await stateManager.DeleteAsync(person);

            Assert.IsNotNull(stateManager);
        }
    }
}