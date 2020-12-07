using MbDotNet;
using MbDotNet.Models.Imposters;
using RestSharp;
using RestSharpDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpDemo.Base
{
    public class Settings
    {
        public Uri BaseUrl { get; set; }
        public IRestResponse Response { get; set; }
        public IRestRequest Request { get; set; }
        public RestClient RestClient { get; set; } = new RestClient();

        public HttpImposter Imposter { get; set; }
        public MountebankClient MBClient { get; set; }
    }
}
