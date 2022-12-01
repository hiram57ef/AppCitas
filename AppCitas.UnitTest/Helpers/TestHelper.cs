using AppCitas;
using AppCitas.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppCitas.UnitTest.Helpers
{
    public sealed class TestHelper
    {
        private static readonly Lazy<TestHelper> _lazyInstance =
            new Lazy<TestHelper>(() => new TestHelper());

        public static TestHelper Instance
        {
            get
            {
                return _lazyInstance.Value;
            }
        }
        public HttpClient Client { get; set; }

        private TestHelper()
        {
            
            Client = new UnitTests.APIWebApplicationFactory<Startup>().CreateDefaultClient();
            
        }
    }
}
