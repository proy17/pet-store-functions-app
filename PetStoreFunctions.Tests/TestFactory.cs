using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace PetStoreFunctions.Tests
{
    public class TestFactory
    {
        public static IEnumerable<object[]> Data()
        {
            return new List<object[]>
            {
                new object[] { "petType", "Cat" },

            };
        }

        public static Dictionary<string, StringValues> CreateDictionary(string key, string value)
        {
            var qs = new Dictionary<string, StringValues>
            {
                { key, value }
            };
            return qs;
        }

        public static DefaultHttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
        {
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection(CreateDictionary(queryStringKey, queryStringValue))
            };
            return request;
        }
        public static IRestClient MockRestClient<T>(HttpStatusCode httpStatusCode, string json)
        {
            var mockIRestClient = new Mock<IRestClient>();
            mockIRestClient.Setup(x => x.Execute<T>(It.IsAny<IRestRequest>()))
              .Returns(new RestResponse<T>
              {
                  Data = JsonConvert.DeserializeObject<T>(json),
                  StatusCode = httpStatusCode
              });
            return mockIRestClient.Object;
        }


        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }
    }
}
