using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace JiraDashboard.Models.Helpers
{
    public class JiraAuthHelper
    {
        public bool Login(out IDictionary<string, string> cookies)
        {
            var client = new RestClient(GenericHelper.domain);
            var request = new RestRequest("/rest/auth/1/session", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { username = GenericHelper.username, password = GenericHelper.password });

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                cookies = response.Cookies.ToDictionary(x => x.Name, x => x.Value);
                return true;
            }
            else
            {
                cookies = null;
                return false;
            }
        }
    }
}