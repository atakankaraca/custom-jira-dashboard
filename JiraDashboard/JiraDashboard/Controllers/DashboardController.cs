using HürriyetJiraDashboard.Models.JsonClasses;
using JiraDashboard.Models.DTO;
using JiraDashboard.Models.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static JiraDashboard.Models.Helpers.JsonSettings;

namespace JiraDashboard.Controllers
{
    [LoginControl]
    public class DashboardController : Controller
    {

        #region Definitions
        //Globalization
        CultureInfo _ci = new CultureInfo("en-US");

        //Helpers
        JiraAuthHelper jAuth = new JiraAuthHelper();
        JiraJsonFetcher fetcher = new JiraJsonFetcher();
        Utility utility = new Utility();
        CommonController cm = new CommonController();

        //Model
        Settings model = new Settings();

        //List Definitions
        List<SelectListItem> ProjectsList = new List<SelectListItem>();
        List<string> UniqUserList = new List<string>();


        //Lists and Tables of Created & Resolved
        List<object> tempUserResolvedChart = new List<object>();
        List<object> tempTeamCreatedChart = new List<object>();
        List<object> tempTeamResolvedChart = new List<object>();

        List<TableModel> userResolvedTable = new List<TableModel>();
        List<TableModel> teamResolvedTable = new List<TableModel>();
        List<TableModel> teamCreatedTable = new List<TableModel>();

        //Lists and Tables of StoryPoints
        List<object> tempUserStoryPointsChart = new List<object>();
        List<object> tempTeamStoryPointsChart = new List<object>();
        List<object> tempAvgTeamStoryPointsChart = new List<object>();

        List<object> tempUserStoryPointsDateData = new List<object>();
        List<object> tempTeamStoryPointsDateData = new List<object>();
        List<object> tempAvgStoryPointsDateData = new List<object>();

        List<TableModel> userStoryPointsTable = new List<TableModel>();
        List<TableModel> teamStoryPointsTable = new List<TableModel>();

        //Lists and Tables of Efficiency
        List<TableModel> userEfficiencyTable = new List<TableModel>();
        List<TableModel> teamEfficiencyTable = new List<TableModel>();

        //Lists of Cumulative
        List<TimeSpan> UserCumulativeList = new List<TimeSpan>();
        List<TimeSpan> TeamCumulativeList = new List<TimeSpan>();

        //Cookie
        IDictionary<string, string> cookies;
        #endregion

        // Created and Resolved Issues
        public ActionResult Issue()
        {
            FirstLoad();
            FillModel();
            ViewBag.PageHead = "Issues";
            return View(model);
        }

        public JsonResult CreatedResolved(string dates, string assignee, string projectname)
        {
            jAuth.Login(out cookies);

            userResolvedTable.Clear();
            tempUserResolvedChart.Clear();

            string[] date = dates.Split(' ');
            string start_date = date[0];
            string end_date = date[2];

            int dayscount = Convert.ToInt32(utility.Date_Modifier(start_date, end_date));
            int[] datecountUserResolved = new int[dayscount];
            int[] datecountTeamCreated = new int[dayscount];
            int[] dateCountTeamResolved = new int[dayscount];
            string[] date_data = new string[dayscount];

            DateTime d = Convert.ToDateTime(start_date);

            if (assignee != "Team")
            {
                //User Resolved
                var userResolvedIssuesJson = fetcher.GetUserResolvedIssues(cookies, projectname, assignee, start_date, end_date);
                var userResolvedIssues = JsonConvert.DeserializeObject<RootObject>(userResolvedIssuesJson);

                utility.GenerateUserResolvedTable(userResolvedIssues, userResolvedTable, tempUserResolvedChart);

                return Json(new
                {
                    DateList = utility.DateGenerator(date_data, datecountUserResolved, d),
                    ResolvedData = utility.IssueCounter(tempUserResolvedChart, datecountUserResolved, d),
                    ResolvedSum = userResolvedIssues.issues.Where(x => x.fields.resolution.id == GenericHelper.Done).Count(),
                    ResolvedDetails = userResolvedTable.OrderByDescending(x => x.Resolution_Date)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Team Created
                var teamCreatedIssuesJson = fetcher.GetTeamCreatedIssues(cookies, projectname, start_date, end_date);
                var teamCreatedIssues = JsonConvert.DeserializeObject<RootObject>(teamCreatedIssuesJson);

                utility.GenerateTeamCreatedTable(teamCreatedIssues, teamCreatedTable, tempTeamCreatedChart);

                //Team Resolved
                var teamResolvedIssuesJson = fetcher.GetTeamResolvedIssues(cookies, projectname, start_date, end_date);
                var teamResolvedIssues = JsonConvert.DeserializeObject<RootObject>(teamResolvedIssuesJson);

                utility.GenerateTeamResolvedTable(teamResolvedIssues, teamResolvedTable, tempTeamResolvedChart);

                return Json(new
                {
                    DateList = utility.DateGenerator(date_data, datecountTeamCreated, d),
                    CreatedData = utility.IssueCounter(tempTeamCreatedChart, datecountTeamCreated, d),
                    ResolvedData = utility.IssueCounter(tempTeamResolvedChart, dateCountTeamResolved, d),
                    ResolvedSum = teamResolvedIssues.issues.Where(x => x.fields.resolution.id == GenericHelper.Done).Count(),
                    CreatedSum = teamCreatedIssues.issues.Count,
                    ChartTitle = "Team",
                    CreatedDetails = teamCreatedTable.OrderByDescending(x => x.Created_Date), ResolvedDetails = teamResolvedTable.OrderByDescending(x => x.Resolution_Date)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //Story Points
        public ActionResult Storypoint()
        {
            FirstLoad();
            FillModel();
            ViewBag.PageHead = "Story Points";
            return View(model);
        }

        public JsonResult StoryPointsBetweenTwoDates(string dates, string assignee, string projectname)
        {
            jAuth.Login(out cookies);

            string[] date = dates.Split(' ');
            string start_date = date[0];
            string end_date = date[2];

            int dayscount = Convert.ToInt32(utility.Date_Modifier(start_date, end_date));
            double[] datecountSp_Team = new double[dayscount];
            double[] datecountSp_User = new double[dayscount];
            double[] datecountSp_Team_Avg = new double[dayscount];
            int[] dailyUserCount = new int[dayscount];
            string[] daily_date_data = new string[dayscount];
            int[] tempdatedata = new int[dayscount];

            DateTime d = Convert.ToDateTime(start_date);
            DateTime d2 = Convert.ToDateTime(end_date);

            if (assignee != "Team")
            {
                //User StoryPoints
                var userStoryPointsJson = fetcher.GetUserStoryPoints(cookies, projectname, assignee, start_date, end_date);
                var userStoryPoints = JsonConvert.DeserializeObject<RootObject>(userStoryPointsJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

                utility.GenerateUserStoryPointsTable(userStoryPoints, userStoryPointsTable, tempUserStoryPointsChart, tempUserStoryPointsDateData);

                //Team Story Points

                var teamStoryPointsJson = fetcher.GetTeamStoryPoints(cookies, projectname, start_date, end_date);
                var teamStoryPoints = JsonConvert.DeserializeObject<RootObject>(teamStoryPointsJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

                utility.GenerateTeamStoryPointsTable(teamStoryPoints, teamStoryPointsTable, tempTeamStoryPointsChart, tempTeamStoryPointsDateData);

                //Average Story Points

                utility.AverageHelper(teamStoryPoints, tempAvgTeamStoryPointsChart, tempAvgStoryPointsDateData, UniqUserList);
                utility.PopulateUniqUserList(teamStoryPoints, UniqUserList, d, tempdatedata);
                utility.IssueProgress(datecountSp_Team_Avg, tempAvgStoryPointsDateData, tempAvgTeamStoryPointsChart, d);

                return Json(new
                {
                    DateList = utility.DateGenerator(daily_date_data, tempdatedata, d),
                    TotalSPTeam = teamStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    TotalSPUser = teamStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done && x.fields.assignee != null && x.fields.assignee.name == assignee).Sum(x => x.fields.StoryPoint.value),
                    SPTeamAvgList = utility.AverageCalculator(tempdatedata, datecountSp_Team_Avg),
                    SPUserList = utility.IssueProgress(datecountSp_User, tempUserStoryPointsDateData, tempUserStoryPointsChart, d),
                    SPTeamList = utility.IssueProgress(datecountSp_Team, tempTeamStoryPointsDateData, tempTeamStoryPointsChart, d),
                    SPTeamDetail = teamStoryPointsTable.OrderByDescending(x => x.Resolution_Date), SPUserDetail = userStoryPointsTable.OrderByDescending(x => x.Resolution_Date),
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Team Story Points

                var teamStoryPointsJson = fetcher.GetTeamStoryPoints(cookies, projectname, start_date, end_date);
                var teamStoryPoints = JsonConvert.DeserializeObject<RootObject>(teamStoryPointsJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

                utility.GenerateTeamStoryPointsTable(teamStoryPoints, teamStoryPointsTable, tempTeamStoryPointsChart, tempTeamStoryPointsDateData);

                //Average Story Points

                utility.AverageHelper(teamStoryPoints, tempAvgTeamStoryPointsChart, tempAvgStoryPointsDateData, UniqUserList);
                utility.PopulateUniqUserList(teamStoryPoints, UniqUserList, d, tempdatedata);
                utility.IssueProgress(datecountSp_Team_Avg, tempAvgStoryPointsDateData, tempAvgTeamStoryPointsChart, d);
                
                return Json(new
                {
                    DateList = utility.DateGenerator(daily_date_data, tempdatedata, d),
                    TotalSPTeam = teamStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    SPTeamList = utility.IssueProgress(datecountSp_Team, tempTeamStoryPointsDateData, tempTeamStoryPointsChart, d),
                    SPTeamAvgList = utility.AverageCalculator(tempdatedata, datecountSp_Team_Avg),
                    SPTeamDetail = teamStoryPointsTable.OrderByDescending(x => x.Resolution_Date),
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult WeeklyStorypoints()
        {
            FirstLoad();
            FillModel();
            ViewBag.PageHead = "Weekly Story Points";
            return View(model);
        }

        public JsonResult StoryPointsWeekly(string dates, string assignee, string projectname)
        {
            jAuth.Login(out cookies);

            string[] date = dates.Split(' ');
            string date1 = date[0];
            string date2 = date[2];

            DateTime d = Convert.ToDateTime(date1);
            DateTime d2 = Convert.ToDateTime(date2);
            DateTime d3 = d.AddDays((Convert.ToDouble(1 - d.DayOfWeek) - 7) % 7);
            DateTime d4 = d2.AddDays(Convert.ToDouble(7 - d2.DayOfWeek) % 7);

            string start_date = d3.ToString("yyyy-MM-dd");
            string end_date = d4.ToString("yyyy-MM-dd");

            int dayscount = Convert.ToInt32(utility.Date_Modifier(start_date, end_date));

            double[] datecountsp_team_weekly = new double[dayscount];
            double[] datecountsp_user_weekly = new double[dayscount];
            string[] _weekly_date_data = new string[dayscount / 7];
            double[] weekly_data_user = new double[dayscount / 7];
            double[] weekly_data_team = new double[dayscount / 7];

            //User Weekly

            var userWeeklyStoryPointsJson = fetcher.GetWeeklyUserStoryPoints(cookies, projectname, assignee, start_date, end_date);
            var userWeeklyStoryPoints = JsonConvert.DeserializeObject<RootObject>(userWeeklyStoryPointsJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

            if (assignee != "Team")
            {
                utility.GenerateUserStoryPointsTable(userWeeklyStoryPoints, userStoryPointsTable, tempUserStoryPointsChart, tempUserStoryPointsDateData);
                utility.IssueProgress(datecountsp_user_weekly, tempUserStoryPointsDateData, tempUserStoryPointsChart, d3);
                utility.WeeklyCalculator(dayscount, weekly_data_user, datecountsp_user_weekly);
                utility.WeekGenerator(weekly_data_user, _weekly_date_data, d3);
            }
            //Team Weekly

            var teamWeeklyStoryPointsJson = fetcher.GetWeeklyTeamStoryPoints(cookies, projectname, start_date, end_date);
            var teamWeeklyStoryPoints = JsonConvert.DeserializeObject<RootObject>(teamWeeklyStoryPointsJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

            utility.GenerateTeamStoryPointsTable(teamWeeklyStoryPoints, teamStoryPointsTable, tempTeamStoryPointsChart, tempTeamStoryPointsDateData);
            utility.IssueProgress(datecountsp_team_weekly, tempTeamStoryPointsDateData, tempTeamStoryPointsChart, d3);
            utility.WeeklyCalculator(dayscount, weekly_data_team, datecountsp_team_weekly);
            utility.WeekGenerator(weekly_data_team, _weekly_date_data, d3);

            //Story Points

            if (assignee != "Team")
            {
                return Json(new
                {
                    DateList = _weekly_date_data,
                    SPTeamList = weekly_data_team,
                    SPUserList = weekly_data_user,
                    TotalSPTeam = teamWeeklyStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    TotalSPUser = userWeeklyStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    SPTeamDetail = teamStoryPointsTable.OrderByDescending(x => x.Resolution_Date), SPUserDetail = userStoryPointsTable.OrderByDescending(x => x.Resolution_Date),
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    DateList = _weekly_date_data,
                    SPTeamList = weekly_data_team,
                    TotalSPTeam = teamWeeklyStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    SPTeamDetail = teamStoryPointsTable.OrderByDescending(x => x.Resolution_Date),
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MonthlyStorypoints()
        {
            FirstLoad();
            FillModel();
            ViewBag.PageHead = "Monthly Story Points";
            return View(model);
        }

        public JsonResult StoryPointsMonthly(string dates, string assignee, string projectname)
        {
            jAuth.Login(out cookies);

            string[] date = dates.Split(' ');
            string date1 = date[0];
            string date2 = date[2];

            DateTime d = Convert.ToDateTime(date1);
            DateTime d2 = Convert.ToDateTime(date2);

            string start_date = d.AddDays(-d.Day + 1).ToString("yyyy-MM-dd");
            string end_date = d2.AddDays(-d2.Day + 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            int month_difference = ((d2.Month - d.Month) + 12 * (d2.Year - d.Year));

            int[] monthlydata_user = new int[month_difference + 1];
            int[] monthlydata_team = new int[month_difference + 1];
            double[] montly_sp_user = new double[month_difference + 1];
            double[] montly_sp_team = new double[month_difference + 1];
            string[] _monthly_date_data = new string[month_difference + 1];

            var userMonthlyStoryPointsJson = fetcher.GetMonthlyUserStoryPoints(cookies, projectname, assignee, start_date, end_date);
            var userMonthlyStoryPoints = JsonConvert.DeserializeObject<RootObject>(userMonthlyStoryPointsJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

            if (assignee != "Team")
            {
                utility.GenerateMonthlyUserStoryPointsTable(userMonthlyStoryPoints, userStoryPointsTable, tempUserStoryPointsChart, tempUserStoryPointsDateData, montly_sp_user, d);
            }

            var teamMonthlyStoryPointsJson = fetcher.GetMonthlyTeamStoryPoints(cookies, projectname, start_date, end_date);
            var teamMonthlyStoryPoints = JsonConvert.DeserializeObject<RootObject>(teamMonthlyStoryPointsJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

            utility.GenerateMonthlyTeamStoryPointsTable(teamMonthlyStoryPoints, teamStoryPointsTable, tempTeamStoryPointsChart, tempTeamStoryPointsDateData, montly_sp_team, d);
            
            if (assignee != "Team")
            {
                return Json(new
                {
                    DateList = utility.DateGenerator(_monthly_date_data, monthlydata_user, d),
                    SPTeamList = montly_sp_team,
                    SPUserList = montly_sp_user,
                    TotalSPTeam = teamMonthlyStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    TotalSPUser = userMonthlyStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    SPTeamDetail = teamStoryPointsTable.OrderByDescending(x => x.Resolution_Date),
                    SPUserDetail = userStoryPointsTable.OrderByDescending(x => x.Resolution_Date)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    DateList = utility.DateGenerator(_monthly_date_data, monthlydata_user, d),
                    SPTeamList = montly_sp_team,
                    TotalSPTeam = teamMonthlyStoryPoints.issues.Where(x => x.fields.StoryPoint != null && x.fields.resolution.id == GenericHelper.Done).Sum(x => x.fields.StoryPoint.value),
                    SPTeamDetail = teamStoryPointsTable.OrderByDescending(x => x.Resolution_Date)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //Cumulative
        public ActionResult Cumulative()
        {
            FirstLoad();
            FillModel();
            ViewBag.PageHead = "Cumulative";
            return View(model);
        }

        public JsonResult CumulativeBetweenTwoDates(string dates, string assignee, string projectname)
        {
            jAuth.Login(out cookies);

            string[] date = dates.Split(' ');
            string start_date = date[0];
            string end_date = date[2];

            DateTime rdate_start = Convert.ToDateTime(date[0]);
            DateTime rdate_end = Convert.ToDateTime(date[2]);

            Random rand = new Random();
            int id = rand.Next(100000000, 300000000);

            if (assignee != "Team")
            {
                var userCumulativeJson = fetcher.GetUserCumulative(cookies, projectname, assignee, start_date, end_date);
                var userCumulative = JsonConvert.DeserializeObject<RootObject>(userCumulativeJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

                utility.GenerateCumulativeList(userCumulative, UserCumulativeList);

                TimeSpan UserAvg = utility.CalculateCumulativeAvg(UserCumulativeList);

                return Json(new
                {
                    Result = string.Format("{0:%d} Days {0:%h} Hours {0:%m} Minutes", UserAvg),
                    TotalUserDone = userCumulative.issues.Where(x => x.fields.resolution.id == GenericHelper.Done).Count(),
                    TotalUserStory = userCumulative.issues.Where(x => x.fields.resolution.id == GenericHelper.Done && x.fields.StoryPoint != null).Sum(y => y.fields.StoryPoint.value),
                    StartDate = rdate_start.ToString("yyyy-MM-dd", _ci),
                    EndDate = rdate_end.ToString("yyyy-MM-dd", _ci),
                    ID = id
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var teamCumulativeJson = fetcher.GetTeamCumulative(cookies, projectname, start_date, end_date);
                var teamCumulative = JsonConvert.DeserializeObject<RootObject>(teamCumulativeJson, new JsonSerializerSettings { ContractResolver = new CustomDataContractResolver() });

                utility.GenerateCumulativeList(teamCumulative, TeamCumulativeList);

                TimeSpan TeamAvg = utility.CalculateCumulativeAvg(TeamCumulativeList);
                
                return Json(new
                {
                    Result = string.Format("{0:%d} Days {0:%h} Hours {0:%m} Minutes", TeamAvg),
                    TotalTeamDone = teamCumulative.issues.Where(x => x.fields.resolution.id == GenericHelper.Done).Count(),
                    TotalTeamStory = teamCumulative.issues.Where(x => x.fields.resolution.id == GenericHelper.Done && x.fields.StoryPoint != null).Sum(y => y.fields.StoryPoint.value),
                    StartDate = rdate_start.ToString("yyyy-MM-dd", _ci),
                    EndDate = rdate_end.ToString("yyyy-MM-dd", _ci),
                    ID = id
                }, JsonRequestBehavior.AllowGet);
            }
        }

        //Efficiency
        public ActionResult Efficiency()
        {
            FirstLoad();
            FillModel();
            ViewBag.PageHead = "Efficiency";
            return View(model);
        }

        public JsonResult EfficiencyBetweenTwoDates(string dates, string assignee, string projectname)
        {
            jAuth.Login(out cookies);

            string[] date = dates.Split(' ');
            string start_date = date[0];
            string end_date = date[2];

            if (assignee != "Team")
            {
                var userEfficiencyJson = fetcher.GetUserEfficiency(cookies, projectname, assignee, start_date, end_date);
                var userEfficiency = JsonConvert.DeserializeObject<RootObject>(userEfficiencyJson);

                utility.GenerateEfficiencyTable(userEfficiency, userEfficiencyTable);

                return Json(new
                {
                    EfficiencyDetails = userEfficiencyTable.OrderByDescending(x => x.Efficiency_Point),
                    TotalCount = userEfficiencyTable.Sum(x => x.Efficiency_Point)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var teamEfficiencyJson = fetcher.GetTeamEfficiency(cookies, projectname, start_date, end_date);
                var teamEfficiency = JsonConvert.DeserializeObject<RootObject>(teamEfficiencyJson);

                utility.GenerateEfficiencyTable(teamEfficiency, teamEfficiencyTable);
                
                return Json(new
                {
                    EfficiencyDetails = teamEfficiencyTable.OrderByDescending(x => x.Efficiency_Point),
                    TotalCount = teamEfficiencyTable.Sum(x => x.Efficiency_Point)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public void FillModel()
        {
            model.MaxResults = GenericHelper.maxresults;
            model.UserSearchLenght = GenericHelper.userquerylength;
            model.StoryPointField = GenericHelper.storypointfield;
            model.RepeatInterval = GenericHelper.repeatinterval;
        }

        public void FirstLoad()
        {
            jAuth.Login(out cookies);
            ProjectsList = cm.GetProjects(cookies);
            ViewBag.ProjectsDD = ProjectsList.OrderBy(x => x.Text);
        }
    }
}