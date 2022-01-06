using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StatsMonitor.Models.Chart;
using StatsMonitor.Pages.Statistics.Shared;

namespace StatsMonitor.Pages.Statistics
{
    public class UsersModel : StatisticsPageModel
    {
        public string TestChart { get; set; }

        public string ChangeToChart { get; set; }


        public async Task TestMethod()
        {
            TestChart = ChangeToChart;
            await Task.Delay(1000);
        }

        public async Task<IActionResult> OnPostDelete(int id)
        {
            await TestMethod();

            return RedirectToPage("Users");
        }

        public void OnPostTest()
        {

        }

        protected override async Task CustomOnGet()
        {
            ChartHelperEx helper = new();
            helper.Label = "Test";
            helper.Data = new List<(string label, int value)>
            {
                ("Første", 1),
                ("Anden", 2),
                ("Tredje", 3),
            };
            helper.Colors.Add(Color.FromArgb(80, 100, 20, 50));
            TestChart = helper.GetAsJson();
            helper.Label = "Test2";
            helper.Data.Add(("Fjere", 4));
            helper.Colors.Add(Color.FromArgb(80, 50, 100, 20));
            ChangeToChart = helper.GetAsJson();

            await Task.Delay(1000);
        }
    }
}
