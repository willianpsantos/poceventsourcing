using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCEventSourcing.Tests.Utils
{
    internal class ReadJsonTests
    {
        [SetUp]
        public void Setup()
        {
           
        }

        [Test]
        public async Task ReadTest()
        {
            IDictionary<string, string> mappings = new Dictionary<string, string>();
            var appPath = AppContext.BaseDirectory;

            using (StreamReader reader = new StreamReader($"{appPath}/entities.mappings.json"))
            {
                var json = await reader.ReadToEndAsync();
                mappings = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
            }

            Assert.IsNotNull(mappings);
            Assert.IsTrue(mappings.Count > 0);
        }
    }
}
