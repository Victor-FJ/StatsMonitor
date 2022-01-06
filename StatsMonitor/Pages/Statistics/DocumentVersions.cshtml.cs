using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatsMonitor.Models;
using StatsMonitor.Models.Chart;
using StatsMonitor.Pages.Statistics.Shared;

namespace StatsMonitor.Pages.Statistics
{
    public class DocumentVersionsModel : StatisticsPageModel
    {
        public string TotalChart { get; set; }
        public string SplitChart { get; set; }


        private readonly DocumentVersionClient _documentVersionClient = new(new());
        public DocumentVersionsModel()
        {
        }

        protected override async Task CustomOnGet()
        {
            var task1 = _documentVersionClient.TotalNumberOfDocumentVersionsAsync(DateFilter);
            var task2 = _documentVersionClient.AverageNumberOfVersionsPrDocumentAsync(DateFilter);
            var task3 = _documentVersionClient.SplitBetweenTheTypeOfDocumentsInPercentageAsync(DateFilter);

            await Task.WhenAll(task1, task2, task3);

            var totalDocumentVersions = task1.Result;

            ChartHelperEx totalHelper = new();
            totalHelper.Data = totalDocumentVersions.Entries.Select(x => (x.Key.ToString("d"), x.Count)).ToList();
            totalHelper.Colors = new()
            {
                Color.FromArgb(80, 54, 162, 235),
                Color.FromArgb(50, 54, 162, 235)
            };
            totalHelper.Label = "Number of documentversions";
            TotalChart = totalHelper.GetAsJson();



            var typePercentage = task3.Result.Result;

            ChartHelperEx splitHelper = new("pie");
            splitHelper.Data.Add((nameof(typePercentage.Questionnarie), typePercentage.Questionnarie));
            splitHelper.Data.Add((nameof(typePercentage.ExternalLink), typePercentage.ExternalLink));
            splitHelper.Data.Add((nameof(typePercentage.FileUpload), typePercentage.FileUpload));
            splitHelper.Colors = new()
            {
                Color.FromArgb(255, 99, 132),
                Color.FromArgb(54, 162, 235),
                Color.FromArgb(255, 205, 86)
            };
            splitHelper.Label = "Number of documentversions";
            SplitChart = splitHelper.GetAsJson();
        }
    }
}
