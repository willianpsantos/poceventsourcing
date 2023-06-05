using NUnit.Framework;
using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.Services;
using POCEventSourcing.IoC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POCEventSourcing.Tests.Services
{
    internal class PersonServiceTests
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
            var stateManager = _di.GetService<IPersonService>();

            var person = new Person
            {
                Name = "TESTE TABLE STORAGE",
                Document = "0200456859",

                Addresses = new List<PersonAddress>
                {
                    new PersonAddress
                    {
                        Address = "RUA DAS FLORES, 123",
                        City = "CUIABÁ",
                        Region = "MT",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = 1
                    }
                }
            };

            var id = await stateManager.InsertAsync(person);

            Assert.IsNotNull(stateManager);
            Assert.IsTrue(id > 0);
        }
    }
}
