using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using RestSharpDemo.Model;
using RestSharpDemo.Utilities;

namespace RestSharpDemo 
{ 
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}", Method.GET);
            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            //Lib 1 - Dictionary based response
            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            //Lib 2 - JSON based response
            JObject obs = JObject.Parse(response.Content);
            Assert.That(obs["author"].ToString(), Is.EqualTo("Karthik KK"), "Author is not correct");
        }

        [Test]
        public void PostWithAnonymousBody()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}/profile", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { name = "Raj" });

            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            var result = response.DeserializeResponse()["name"];


            Assert.That(result, Is.EqualTo("Raj"), "Author is not correct");
        }

        [Test]
        public void PostWithTypeClassBody()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new Posts() { id = "17", author = "Execute Automation", title = "RestSharp demo course" });

            var response = client.Execute<Posts>(request);

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            Assert.That(response.Data.author, Is.EqualTo("Execute Automation"), "Author is not correct");
        }

        [Test]
        public void PostWithAsync()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new Posts() { id = "18", author = "Execute Automation", title = "RestSharp demo course" });

            //var response = client.Execute<Posts>(request);

            //Lib 3 - Using Async

            var response = client.ExecutePostAsync<Posts>(request).GetAwaiter().GetResult();

            //var deserialize = new JsonDeserializer();
            //var output = deserialize.Deserialize<Dictionary<string, string>>(response);
            //var result = output["author"];

            Assert.That(response.Data.author, Is.EqualTo("Execute Automation"), "Author is not correct");

        }

        [Test]
        public void AuthenticationMechanism()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("auth/login", Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(new { email = "karthik@email.com", password = "haha123" });

            var response = client.ExecutePostAsync(request).GetAwaiter().GetResult();
            var access_token = response.DeserializeResponse()["access_token"];

            var jwtAuth = new JwtAuthenticator(access_token);
            client.Authenticator = jwtAuth;

            var getRequest = new RestRequest("posts/{postid}", Method.GET);
            getRequest.AddUrlSegment("postid", 5);

            //Perform Get operation
            var result = client.ExecuteAsyncRequest<Posts>(getRequest).GetAwaiter().GetResult();
            Assert.That(result.Data.author, Is.EqualTo("Karthik KK"), "The author is not correct");

        }

        [Test]
        public void AuthenticationMechanismWithJSONFile()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("auth/login", Method.POST);

            var file = @"TestData/Data.json";

            request.RequestFormat = DataFormat.Json;
            var jsonData = JsonConvert.DeserializeObject<User>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file)).ToString());
            request.AddJsonBody(jsonData);

            var response = client.ExecutePostAsync(request).GetAwaiter().GetResult();
            var access_token = response.DeserializeResponse()["access_token"];

            var jwtAuth = new JwtAuthenticator(access_token);
            client.Authenticator = jwtAuth;

            var getRequest = new RestRequest("posts/{postid}", Method.GET);
            getRequest.AddUrlSegment("postid", 5);

            //Perform Get operation
            var result = client.ExecuteAsyncRequest<Posts>(getRequest).GetAwaiter().GetResult();
            Assert.That(result.Data.author, Is.EqualTo("Karthik KK"), "The author is not correct");

        }

        private class User
        {
            public string email { get; set; }
            public string password { get; set; }
        }
    }
}
 