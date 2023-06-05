using NUnit.Framework;
using POCEventSourcing.Entities;
using POCEventSourcing.Interfaces.Cache;
using POCEventSourcing.Interfaces.DB;
using POCEventSourcing.IoC;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace POCEventSourcing.Tests.DB
{
    public class CacheDBTests
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
            var stateManager = _di.GetService<ICacheDbEntityStateManager>();

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
        public void GetFromCacheTest()
        {
            var cacheManager = _di.GetService<ICacheManager>();

            var obj = cacheManager.Get<Person>("Person-1");

            Assert.IsNotNull(obj);
            Assert.IsTrue(obj.Id > 0);
            Assert.IsNotNull(obj.UpdatedAt);
        }

        [Test]
        public void GetFromListCache()
        {
            var cacheManager = _di.GetService<ICacheListManager>();

            var list = cacheManager.GetAll<Person>();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count() > 0);
        }

        [Test]
        public async Task UpdateTest()
        {
            var stateManager = _di.GetService<ICacheDbEntityStateManager>();

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
            var stateManager = _di.GetService<ICacheDbEntityStateManager>();

            var person = new Person { Id = 1 };

            await stateManager.DeleteAsync(person);

            Assert.IsNotNull(stateManager);
        }
    }
}
