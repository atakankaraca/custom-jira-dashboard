using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace JiraDashboard.Models.Helpers
{
    public class JiraJsonFetcher
    {
        public string GetProjectsJson(IDictionary<string, string> cookies)
        {
            var client = new RestClient(GenericHelper.domain);
            var request = new RestRequest("/rest/api/latest/project", Method.GET);
            request.RequestFormat = DataFormat.Json;

            foreach (var cookie in cookies)
                request.AddCookie(cookie.Key, cookie.Value);

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                content = content.Insert(0, "{" + "projects" + ":");
                content = content.Insert(content.Length, "}");
                return content;
            }
            else
            {
                return "a";
            }
        }
        public string GetUserNames(IDictionary<string, string> cookies, string project)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + "&fields=assignee&maxResults=" + GenericHelper.userquerylength + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetJson(IDictionary<string, string> cookies, string domain, string url)
        {
            var client = new RestClient(domain);
            var request = new RestRequest(url);
            request.RequestFormat = DataFormat.Json;

            foreach (var cookie in cookies)
            {
                request.AddCookie(cookie.Key, cookie.Value);
            }
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                return content;
            }
            else
            {
                return "";
            }
        }
        public string GetUserStoryPoints(IDictionary<string, string> cookies, string project, string assignee, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20assignee=%22" + assignee + "%22%20AND%20resolutiondate>" + start_date + "%20AND%20resolutiondate<'" + end_date + "%2023:59'&fields=resolutiondate,status,assignee,created,creator,resolution,issuetype," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetTeamStoryPoints(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20resolutiondate>" + start_date + "%20AND%20resolutiondate<'" + end_date + "%2023:59'&fields=resolutiondate,status,assignee,created,creator,resolution,issuetype," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetAvgTeamStoryPoints(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20resolutiondate>" + start_date + "%20AND%20resolutiondate<'" + end_date + "%2023:59'&fields=resolutiondate,status,assignee,created,creator,resolution,issuetype," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetWeeklyUserStoryPoints(IDictionary<string, string> cookies, string project, string assignee, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20assignee=%22" + assignee + "%22%20AND%20resolutiondate>" + start_date + "%20AND%20resolutiondate<'" + end_date + "%2023:59'&fields=resolutiondate,created,status,assignee,creator,resolution,issuetype," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetWeeklyTeamStoryPoints(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + "%20AND%20resolutiondate%3E" + start_date + "%20AND%20resolutiondate%3C%27" + end_date + "%2023:59%27&fields=resolutiondate,created,status,assignee,creator,resolution,issuetype," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetMonthlyUserStoryPoints(IDictionary<string, string> cookies, string project, string assignee, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + "%20AND%20assignee=%22" + assignee + "%22%20AND%20resolutiondate%3E" + start_date + "%20AND%20resolutiondate%3C%27" + end_date + "%2023:59%27&fields=resolutiondate,created,status,assignee,resolution,creator,issuetype," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetMonthlyTeamStoryPoints(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + "%20AND%20resolutiondate%3E" + start_date + "%20AND%20resolutiondate%3C%27" + end_date + "%2023:59%27&fields=resolutiondate,status,resolution,assignee,created,creator,issuetype," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetTeamCreatedIssues(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20created%3E" + start_date + "%20AND%20created%3C'" + end_date + " 23:59'&expand=changelog&fields=created,status,resolutiondate,assignee,creator,issuetype&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetTeamResolvedIssues(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20resolutiondate%3E" + start_date + "%20AND%20resolutiondate%3C'" + end_date + " 23:59'&expand=changelog&fields=created,status,resolution,resolutiondate,assignee,creator,issuetype&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetUserCreatedIssues(IDictionary<string, string> cookies, string project, string assignee, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20assignee=%22" + assignee + "%22%20AND%20created%3E" + start_date + "%20AND%20created%3C'" + end_date + " 23:59'&expand=changelog&fields=created,status,assignee,creator,issuetype&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetUserResolvedIssues(IDictionary<string, string> cookies, string project, string assignee, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=%22" + project + "%22%20AND%20assignee=%22" + assignee + "%22%20AND%20resolutiondate%3E" + start_date + "%20AND%20resolutiondate%3C'" + end_date + " 23:59'&expand=changelog&fields=created,status,resolution,resolutiondate,assignee,creator,issuetype&maxResults=" + GenericHelper.maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetUserCumulative(IDictionary<string, string> cookies, string project, string assignee, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + " AND assignee=%22" + assignee + "%22 AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59'&expand=changelog&fields=created,resolutiondate,resolution,history," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            //string url = "/rest/api/latest/search?jql=project=" + project + " AND assignee=" + assignee + " AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59' AND created>" + start_date + " AND created<'" + end_date + " 23:59'&maxResults=" + maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetTeamCumulative(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + " AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59'&expand=changelog&fields=created,resolutiondate,resolution,history," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            //string url = "/rest/api/latest/search?jql=project=" + project + " AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59' AND created>" + start_date + " AND created<'" + end_date + " 23:59'&maxResults=" + maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetUserEfficiency(IDictionary<string, string> cookies, string project, string assignee, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + " AND assignee=%22" + assignee + "%22 AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59'&expand=changelog&fields=creator,status,created,assignee,resolutiondate,resolution,issuetype,history," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            //string url = "/rest/api/latest/search?jql=project=" + project + " AND assignee=" + assignee + " AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59' AND created>" + start_date + " AND created<'" + end_date + " 23:59'&maxResults=" + maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }
        public string GetTeamEfficiency(IDictionary<string, string> cookies, string project, string start_date, string end_date)
        {
            string url = "/rest/api/latest/search?jql=project=" + project + " AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59'&expand=changelog&fields=creator,created,assignee,status,resolutiondate,resolution,issuetype,history," + GenericHelper.storypointfield + "&maxResults=" + GenericHelper.maxresults + "";
            //string url = "/rest/api/latest/search?jql=project=" + project + " AND resolutiondate>" + start_date + " AND resolutiondate<'" + end_date + " 23:59' AND created>" + start_date + " AND created<'" + end_date + " 23:59'&maxResults=" + maxresults + "";
            string jsonContent = GetJson(cookies, GenericHelper.domain, url);
            return jsonContent;
        }

    }
}