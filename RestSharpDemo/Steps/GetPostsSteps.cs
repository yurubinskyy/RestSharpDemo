using NUnit.Framework;
using RestSharp;
using RestSharpDemo.Base;
using RestSharpDemo.Model;
using RestSharpDemo.Utilities;
using System.Threading;
using TechTalk.SpecFlow;

namespace RestSharpDemo.Steps
{

    [Binding]
    public class GetPostsSteps
    {
        private Settings _settings;
        public GetPostsSteps(Settings settings) => _settings = settings;


        [Given(@"I perform GET operation for ""(.*)""")]
        public void GivenIPerformGETOperationFor(string url)
        {
            _settings.Request = new RestRequest(url, Method.GET);
        }

        [Given(@"I perform operation for post ""(.*)""")]
        public void GivenIPerformOperationForPost(int postId)
        {
            Thread.Sleep(2000);
            _settings.Request.AddUrlSegment("postid", postId.ToString());
            _settings.Response = _settings.RestClient.ExecuteAsyncRequest<Posts>(_settings.Request).GetAwaiter().GetResult();
        }

        [Then(@"I should see the ""(.*)"" name as ""(.*)""")]
        public void ThenIShouldSeeTheNameAs(string key, string value)
        {
            Assert.That(_settings.Response.GetResponseObject(key), Is.EqualTo(value), $"The {key} is not matching");
        }

    }
}
