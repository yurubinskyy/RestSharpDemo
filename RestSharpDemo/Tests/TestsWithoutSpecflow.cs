using System;
using System.Threading;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators;
using RestSharpDemo.Base;
using RestSharpDemo.Model;
using RestSharpDemo.Utilities;

namespace RestSharpDemo
{
    [TestFixture]
    public class TestsWithoutSpecflow
    {
        private Settings _settings;

        //public TestsWithoutSpecflow(Settings settings)
        //{
        //    _settings = settings;
        //}

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            string emailString = "nilson@email.com";
            string passwordString = "nilson";

            Settings settings = new Settings();
            _settings = settings;
            _settings.BaseUrl = new Uri("http://localhost:3000/");
            _settings.RestClient.BaseUrl = _settings.BaseUrl;

            _settings.Request = new RestRequest("auth/login", Method.POST);

            _settings.Request.AddJsonBody(new { email = emailString, password = passwordString });

            //Get access token
            _settings.Response = _settings.RestClient.ExecutePostAsync(_settings.Request).GetAwaiter().GetResult();
            var access_token = _settings.Response.GetResponseObject("access_token");

            //Authentication
            var authenticator = new JwtAuthenticator(access_token);
            _settings.RestClient.Authenticator = authenticator;

        }

        [Test]
        public void TestVerifyAuthorOfThePosts1()
        {
            string url = "posts/{postid}";
            int postId = 1;
            string key = "author";
            string value = "Karthik KK";

            //perform GET operation for "posts/{postid}"
            _settings.Request = new RestRequest(url, Method.GET);

            //perform operation for post "1"
            Thread.Sleep(2000);
            _settings.Request.AddUrlSegment("postid", postId.ToString());
            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<Posts>(_settings.Request).GetAwaiter().GetResult();

            //I should see the "author" name as "Karthik KK"
            //Assert.That(_settings.Response.ErrorMessage, Is.EqualTo("200"));
            Assert.That(_settings.Response.GetResponseObjectv2(key), Is.EqualTo(value), $"The {key} is not matching");
        }

        [Test]
        public void TestVerifyAuthorOfThePosts6()
        {
            string url = "posts/{postid}";
            int postId = 6;
            string key = "author";
            string value = "ExecuteAutomation";

            //perform GET operation for "posts/{postid}"
            _settings.Request = new RestRequest(url, Method.GET);

            //perform operation for post "1"
            Thread.Sleep(2000);
            _settings.Request.AddUrlSegment("postid", postId.ToString());
            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<Posts>(_settings.Request).GetAwaiter().GetResult();

            //I should see the "author" name as "Karthik KK"
            //Assert.That(_settings.Response.ErrorMessage, Is.EqualTo("200"));
            Assert.That(_settings.Response.GetResponseObjectv2(key), Is.EqualTo(value), $"The {key} is not matching");
        }
    }
}
