using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetStoreFunction;
using PetStoreFunction.Models;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace PetStoreFunctions.Tests
{
    public class PetStoreIntegrationTests
    {
        private readonly ILogger logger = TestFactory.CreateLogger();

        [Fact]
        public async void Http_Trigger_Should_Return_Cat_Data()
        {
            var request = TestFactory.CreateHttpRequest("petType", "Cat");
            Environment.SetEnvironmentVariable("ServiceUrl", "http://agl-developer-test.azurewebsites.net/people.json");
            var response = await PetStoreRequestProcessor.Run(request, logger);
            Assert.NotNull(response);
            Assert.IsType<JObject>(response);
        }
        [Fact]
        public async void Http_Trigger_Should_Return_Null_Data_If_Invalid_Pet_Type()
        {
            var request = TestFactory.CreateHttpRequest("petType", "");
            Environment.SetEnvironmentVariable("ServiceUrl", "http://agl-developer-test.azurewebsites.net/people.json");
            var response = await PetStoreRequestProcessor.Run(request, logger);
            Assert.Null(response);
        }

        [Fact]
        public async void Http_Trigger_Should_Return_Null_If_Invalid_Query_String()
        {
            var request = TestFactory.CreateHttpRequest("invalidKey", "invalidData");
            Environment.SetEnvironmentVariable("ServiceUrl", "http://agl-developer-test.azurewebsites.net/people.json");
            var response = await PetStoreRequestProcessor.Run(request, logger);
            Assert.Null(response);
        }

    }
}
