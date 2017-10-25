using HürriyetJiraDashboard.Models.JsonClasses;
using JiraDashboard.Models.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace JiraDashboard.Models.Helpers
{
    public class Utility
    {
        CultureInfo _ci = new CultureInfo("en-US");

        public int[] IssueCounter(List<object> Table, int[] datecounter, DateTime start_date)
        {
            for (int i = 0; i < Table.Count; i++)
            {
                for (int j = 0; j < datecounter.Length; j++)
                {
                    if (start_date.AddDays(j).ToString("yyyy-MM-dd") == Table[i].ToString())
                    {
                        datecounter[j]++;
                    }
                    else
                    {
                    }
                }
            }
            return datecounter;
        }
        public double[] IssueProgress(double[] dailyusercount, List<object> temporaryDateList, List<object> temporaryChart, DateTime start_date)
        {
            for (int i = 0; i < temporaryDateList.Count; i++)
            {
                for (int j = 0; j < dailyusercount.Length; j++)
                {
                    if (start_date.AddDays(j).ToString("yyyy-MM-dd") == temporaryDateList[i].ToString())
                    {
                        dailyusercount[j] += Convert.ToDouble(temporaryChart[i]);
                    }
                    else
                    {

                    }
                }
            }

            return dailyusercount;
        }

        //Average Calculator

        public void AverageHelper(RootObject root, List<object> temporaryChart, List<object> temporaryDateList, List<string> uniqList)//string to object
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.assignee != null && item.fields.StoryPoint != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                    temporaryChart.Add(item.fields.StoryPoint.value);
                    if (!uniqList.Contains(item.fields.resolutiondate.Substring(0, 10) + "," + item.fields.assignee.name.ToString()))
                    {
                        uniqList.Add(item.fields.resolutiondate.Substring(0, 10) + "," + item.fields.assignee.name.ToString());
                    }
                    else
                    {

                    }
                }
            }
        }
        public double[] AverageCalculator(int[] dailyusercount, double[] avg)
        {
            double temp = 0;
            for (int i = 0; i < avg.Length; i++)
            {
                if (dailyusercount[i] != 0)
                {
                    avg[i] = avg[i] / dailyusercount[i];
                    temp++;
                }
                else
                {

                }
            }
            return avg;
        }
        public void PopulateUniqUserList(RootObject root, List<string> uniqList, DateTime start_date, int[] dailyusercount)
        {
            for (int i = 0; i < uniqList.Count; i++)
            {
                for (int j = 0; j < dailyusercount.Length; j++)
                {
                    if (start_date.AddDays(j).ToString("yyyy-MM-dd") == uniqList[i].Substring(0, 10).ToString())
                    {
                        dailyusercount[j]++;
                    }
                    else
                    {

                    }
                }
            }
        }
        public TimeSpan CalculateCumulativeAvg(List<TimeSpan> List)
        {
            try
            {
                TimeSpan Span = new TimeSpan(0, 0, 0, 0, 0);

                for (int i = 0; i < List.Count; i++)
                {
                    Span += List[i];
                }

                TimeSpan Avg = new TimeSpan((Span.Ticks / List.Count) / 3);
                return Avg;
            }
            catch
            {
                TimeSpan Err = new TimeSpan(0, 0, 0, 0);
                return Err;
            }
        }

        //Week Generator

        public void WeeklyCalculator(int dayscount, double[] data, double[] date_count)
        {
            for (int i = 0; i < dayscount; i++)
            {
                data[i / 7] += date_count[i];
            }
        }
        public void WeekGenerator(double[] double_data, string[] string_data, DateTime date)
        {
            int temp = 0;
            for (int i = 0; i < double_data.Length; i++)
            {
                string_data[i] = date.AddDays(i + temp).ToString("d MMMM yy", _ci).ToString();
                temp = temp + 6;
            }
        }

        //Table Helpers
        public void GenerateUserResolvedTable(RootObject root, List<TableModel> table, List<object> temporaryChart)
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    temporaryChart.Add(item.fields.resolutiondate.Substring(0, 10));
                    if (item.fields.assignee != null)
                    {
                        PopulateTable(table, item);
                        EditTable_wAssignee_wResolution(table, item, table.Count);
                    }
                }
            }
        }
        public void GenerateTeamCreatedTable(RootObject root, List<TableModel> table, List<object> temporaryChart)
        {
            foreach (var item in root.issues)
            {
                temporaryChart.Add(item.fields.created.Substring(0, 10));
                if (item.fields.assignee != null)
                {
                    if (item.fields.resolutiondate != null)
                    {
                        PopulateTable(table, item);
                        EditTable_wAssignee_wResolution(table, item, table.Count);
                    }
                    else
                    {
                        PopulateTable(table, item);
                        EditTable_wAssignee_woResolution(table, item, table.Count);
                    }
                }
                else
                {
                    if (item.fields.resolutiondate != null)
                    {
                        PopulateTable(table, item);
                        EditTable_woAssignee_wResolution(table, item, table.Count);
                    }
                    else
                    {
                        PopulateTable(table, item);
                        EditTable_woAssignee_woResolution(table, item, table.Count);
                    }
                }
            }
        }
        public void GenerateTeamResolvedTable(RootObject root, List<TableModel> table, List<object> temporaryChart)
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    temporaryChart.Add(item.fields.resolutiondate.Substring(0, 10));
                    if (item.fields.assignee != null)
                    {
                        PopulateTable(table, item);
                        EditTable_wAssignee_wResolution(table, item, table.Count);
                    }
                    else
                    {
                        PopulateTable(table, item);
                        EditTable_woAssignee_wResolution(table, item, table.Count);
                    }
                }
            }
        }
        public void GenerateUserStoryPointsTable(RootObject root, List<TableModel> table, List<object> temporaryChart, List<object> temporaryDateList)
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    if (item.fields.StoryPoint != null)
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(item.fields.StoryPoint.value);

                        PopulateTable(table, item);
                        EditTable_wStoryPoint_wAssignee(table, item, table.Count);
                    }
                    else
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(0);

                        PopulateTable(table, item);
                        EditTable_woStoryPoint_wAssignee(table, item, table.Count);
                    }
                }
            }
        }
        public void GenerateTeamStoryPointsTable(RootObject root, List<TableModel> table, List<object> temporaryChart, List<object> temporaryDateList)
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    if (item.fields.StoryPoint != null && item.fields.assignee != null)
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(item.fields.StoryPoint.value);

                        PopulateTable(table, item);
                        EditTable_wStoryPoint_wAssignee(table, item, table.Count);
                    }
                    else if (item.fields.StoryPoint == null && item.fields.assignee != null)
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(0);

                        PopulateTable(table, item);
                        EditTable_woStoryPoint_wAssignee(table, item, table.Count);
                    }
                    else if (item.fields.StoryPoint != null && item.fields.assignee == null)
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(item.fields.StoryPoint.value);

                        PopulateTable(table, item);
                        EditTable_wStoryPoint_woAssignee(table, item, table.Count);
                    }
                    else if (item.fields.StoryPoint == null && item.fields.assignee == null)
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(0);

                        PopulateTable(table, item);
                        EditTable_woStoryPoint_woAssignee(table, item, table.Count);
                    }
                }
            }
        }
        public void GenerateMonthlyUserStoryPointsTable(RootObject root, List<TableModel> table, List<object> temporaryChart, List<object> temporaryDateList, double[] data, DateTime date)
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    if (item.fields.StoryPoint != null)
                    {
                        int md = ((Convert.ToDateTime(item.fields.resolutiondate).Month - date.Month) + 12 * (Convert.ToDateTime(item.fields.resolutiondate).Year - date.Year));
                        data[md] += Convert.ToDouble(item.fields.StoryPoint.value);

                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(item.fields.StoryPoint.value);

                        PopulateTable(table, item);
                        EditTable_wStoryPoint_wAssignee(table, item, table.Count);
                    }
                    else
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(0);

                        PopulateTable(table, item);
                        EditTable_woStoryPoint_wAssignee(table, item, table.Count);
                    }
                }
            }
        }
        public void GenerateMonthlyTeamStoryPointsTable(RootObject root, List<TableModel> table, List<object> temporaryChart, List<object> temporaryDateList, double[] data, DateTime date)
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    if (item.fields.StoryPoint != null && item.fields.assignee != null)
                    {
                        int md = ((Convert.ToDateTime(item.fields.resolutiondate).Month - date.Month) + 12 * (Convert.ToDateTime(item.fields.resolutiondate).Year - date.Year));
                        data[md] += Convert.ToDouble(item.fields.StoryPoint.value);

                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(item.fields.StoryPoint.value);

                        PopulateTable(table, item);
                        EditTable_wStoryPoint_wAssignee(table, item, table.Count);
                    }
                    else if (item.fields.StoryPoint == null && item.fields.assignee != null)
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(0);

                        PopulateTable(table, item);
                        EditTable_woStoryPoint_wAssignee(table, item, table.Count);
                    }
                    else if (item.fields.StoryPoint != null && item.fields.assignee == null)
                    {
                        int md = ((Convert.ToDateTime(item.fields.resolutiondate).Month - date.Month) + 12 * (Convert.ToDateTime(item.fields.resolutiondate).Year - date.Year));
                        data[md] += Convert.ToDouble(item.fields.StoryPoint.value);

                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(item.fields.StoryPoint.value);

                        PopulateTable(table, item);
                        EditTable_wStoryPoint_woAssignee(table, item, table.Count);
                    }
                    else if (item.fields.StoryPoint == null && item.fields.assignee == null)
                    {
                        temporaryDateList.Add(item.fields.resolutiondate.Substring(0, 10));
                        temporaryChart.Add(0);

                        PopulateTable(table, item);
                        EditTable_woStoryPoint_woAssignee(table, item, table.Count);
                    }
                }
            }
        }

        public void GenerateEfficiencyTable(RootObject root, List<TableModel> table)
        {
            foreach (var item in root.issues)
            {
                if (item.fields.resolutiondate != null && item.fields.assignee != null && item.fields.resolution.id == GenericHelper.Done)
                {
                    if (item.changelog.histories.Where(x => x.items.Any(y => y.from == GenericHelper.CodeReview && y.to == GenericHelper.InProgress)).Count() > 0)
                    {
                        PopulateTable(table, item);
                        EditTable_wAssignee_wResolution(table, item, table.Count);
                        EditTable_wEfficiencyCount(table, item, table.Count);
                    }
                }

                else if (item.fields.resolutiondate != null && item.fields.assignee == null && item.fields.resolution.id == GenericHelper.Done)
                {
                    if (item.changelog.histories.Where(x => x.items.Any(y => y.from == GenericHelper.CodeReview && y.to == GenericHelper.InProgress)).Count() > 0)
                    {
                        PopulateTable(table, item);
                        EditTable_woAssignee_wResolution(table, item, table.Count);
                        EditTable_wEfficiencyCount(table, item, table.Count);
                    }
                }
            }
        }
        public void GenerateCumulativeList(RootObject root, List<TimeSpan> table)
        {
            foreach (var item in root.issues)
            {

                if (item.fields.resolution.id == GenericHelper.Done)
                {
                    if (item.changelog.histories.Where(x => x.items.Any(y => y.to == GenericHelper.InProgress || y.to == GenericHelper.ToDo || y.to == GenericHelper.RDF)).Count() > 0)
                    {
                        var firstHistoryCase = item.changelog.histories.Where(x => x.items.Any(y => y.to == GenericHelper.InProgress || y.to == GenericHelper.ToDo || y.to == GenericHelper.RDF)).Last();
                        table.Add(Convert.ToDateTime(item.fields.resolutiondate) - Convert.ToDateTime(firstHistoryCase.created));
                        //if (item.fields.StoryPoint != null)
                        //{
                        //    UserStoryPointCount += Convert.ToDouble(item.fields.StoryPoint.value);
                        //}
                    }
                    else
                    {
                        table.Add(Convert.ToDateTime(item.fields.resolutiondate) - Convert.ToDateTime(item.fields.created));
                        //UserStoryPointCount += Convert.ToDouble(item.fields.StoryPoint.value);
                    }
                }
                else
                {

                }
            }
        }
        public void PopulateTable(List<TableModel> Table, Issue root)
        {
            Table.Add(new TableModel
            {
                Creator = _ci.TextInfo.ToTitleCase(root.fields.creator.displayName.ToLower()),
                Created_Date = root.fields.created.Substring(0, 10),
                Issue_ID = root.key,
                Issue_Link = GenericHelper.domain + "/browse/" + root.key,
                Issue_Type = root.fields.issuetype.name,
                Issue_Icon = root.fields.issuetype.iconUrl,
                Status = root.fields.status.name.ToUpper(),
                Status_Color_Code = "label" + root.fields.status.statusCategory.id
            });
        }

        public void EditTable_woAssignee_wResolution(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Assignee_Name = "Unassigned";
                Table[count - 1].Resolution_Date = root.fields.resolutiondate.Substring(0, 10);
            }
        }
        public void EditTable_wAssignee_woResolution(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Assignee_Name = _ci.TextInfo.ToTitleCase(root.fields.assignee.displayName.ToLower());
                Table[count - 1].Resolution_Date = "Not resolved yet.";
            }
        }
        public void EditTable_wAssignee_wResolution(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Assignee_Name = _ci.TextInfo.ToTitleCase(root.fields.assignee.displayName.ToLower());
                Table[count - 1].Resolution_Date = root.fields.resolutiondate.Substring(0, 10);
            }
        }
        public void EditTable_woAssignee_woResolution(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Assignee_Name = "Unassigned";
                Table[count - 1].Resolution_Date = "Not resolved yet.";
            }
        }
        public void EditTable_wStoryPoint_wAssignee(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Story_Point = Convert.ToDouble(root.fields.StoryPoint.value);
                Table[count - 1].Assignee_Name = _ci.TextInfo.ToTitleCase(root.fields.assignee.displayName.ToLower());
                Table[count - 1].Resolution_Date = root.fields.resolutiondate.Substring(0, 10);
            }
        }
        public void EditTable_wStoryPoint_woAssignee(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Story_Point = Convert.ToDouble(root.fields.StoryPoint.value);
                Table[count - 1].Assignee_Name = "Unassigned";
                Table[count - 1].Resolution_Date = root.fields.resolutiondate.Substring(0, 10);
            }
        }
        public void EditTable_woStoryPoint_wAssignee(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Story_Point = 0;
                Table[count - 1].Assignee_Name = _ci.TextInfo.ToTitleCase(root.fields.assignee.displayName.ToLower());
                Table[count - 1].Resolution_Date = root.fields.resolutiondate.Substring(0, 10);
            }
        }
        public void EditTable_woStoryPoint_woAssignee(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Story_Point = 0;
                Table[count - 1].Assignee_Name = "Unassigned";
                Table[count - 1].Resolution_Date = root.fields.resolutiondate.Substring(0, 10);
            }
        }
        public void EditTable_wEfficiencyCount(List<TableModel> Table, Issue root, int count)
        {
            if (count != 0)
            {
                Table[count - 1].Efficiency_Point = root.changelog.histories.Where(x => x.items.Any(y => y.from == GenericHelper.CodeReview && y.to == GenericHelper.InProgress)).Count();
            }
        }

        //Date Helpers
        public int Date_Modifier(string start_date, string end_date)
        {
            int y1, y2, m1, m2, d1, d2;
            string[] split1 = start_date.Split('-');
            y1 = Convert.ToInt32(split1[0].ToString());
            m1 = Convert.ToInt32(split1[1].ToString());
            d1 = Convert.ToInt32(split1[2].ToString());

            string[] split2 = end_date.Split('-');
            y2 = Convert.ToInt32(split2[0].ToString());
            m2 = Convert.ToInt32(split2[1].ToString());
            d2 = Convert.ToInt32(split2[2].ToString());

            DateTime startdate = new DateTime(y1, m1, d1);
            DateTime finishdate = new DateTime(y2, m2, d2);

            TimeSpan different = finishdate - startdate;
            double dayscount = different.TotalDays;

            return Convert.ToInt32(dayscount + 1);
        }
        public string[] DateGenerator(string[] datedata, int[] datecounter, DateTime start_date)
        {
            for (int i = 0; i < datecounter.Length; i++)
            {
                datedata[i] = start_date.AddDays(i).ToString("d MMMM yy", _ci).ToString();
            }

            return datedata;
        }

    }
}