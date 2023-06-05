using NUnit.Framework;
using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.IoC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POCEventSourcing.Tests
{
    public class WritableDBTests
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
            var stateManager = _di.GetService<IWritableDbEntityStateManager>();

            var person = new Person
            {
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
            var stateManager = _di.GetService<IWritableDbEntityStateManager>();

            var person = new Person
            {
                Id = 15,
                Name = "WILLIAN PEREIRA SANTOS",
                Document = "02562315910",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = 3,

                Addresses = new List<PersonAddress>
                {
                    new PersonAddress
                    {
                        Address = "RUA 10",
                        City = "CUIABÁ",
                        Region = "MT",
                        UpdatedAt = DateTime.UtcNow,
                        UpdatedBy = 1
                    }
                }                
            };

            await stateManager.UpdateAsync(person);

            Assert.IsNotNull(stateManager);
        }

        [Test]
        public async Task DeleteTest()
        {
            var stateManager = _di.GetService<IWritableDbEntityStateManager>();

            var person = new Person { Id = 1 };

            await stateManager.DeleteAsync(person);

            Assert.IsNotNull(stateManager);
        }
    }
}