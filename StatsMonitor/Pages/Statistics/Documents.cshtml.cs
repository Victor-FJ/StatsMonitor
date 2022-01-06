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
    public class DocumentsModel : StatisticsPageModel
    {
        public string TotalChart { get; set; }
        public string MostUsedChart { get; set; }


        private readonly DocumentClient _documentClient = new(new());
        public DocumentsModel()
        {
        }

        protected override async Task CustomOnGet()
        {
            Task<TotalStatisticsOfDateTime>[] tasks =
            {
                _documentClient.TotalNumberOfDocumentsAsync(DateFilter),
                _documentClient.TotalNumberOfDocumentsWithThirdPartyAsync(DateFilter),
                _documentClient.TotalNumberOfSharedocDocumentsAsync(DateFilter),
                _documentClient.TotalNumberOfSharedocDocumentsWithThirdPartyAsync(DateFilter)
            };

            Task<TotalStatisticsOfInteger> task = _documentClient.MostUsedDocumentTypesAsync(DateFilter);

            await Task.WhenAll(tasks.Select(x => (Task)x).Append(task).ToArray());


            TotalChart = MakeTotalChart(tasks.Select(x => x.Result).ToList());
            MostUsedChart = MakeMostUsedChart(task.Result);
        }

        private static string MakeTotalChart(List<TotalStatisticsOfDateTime> totalStatistics)
        {
            ChartHelper totalChartHelper = new();

            DateTime min = totalStatistics[0].Entries.Min(x => x.Key);
            DateTime max = totalStatistics[0].Entries.Max(x => x.Key);

            totalChartHelper.Chart.Data.Labels = Enumerable.Range(0, 1 + max.Subtract(min).Days)
                .Select(offset => min.AddDays(offset).ToString("d"))
                .ToArray();

            List<string> labels = new()
            {
                "Number of documents",
                "Number of documents with third party",
                "Number of share doc documents"
            };
            List<Color> colors = new()
            {
                Color.FromArgb(80, 50, 150, 0),
                Color.FromArgb(80, 50, 150, 150),
                Color.FromArgb(80, 150, 150, 0)
            };
            for (int i = 0; i < labels.Count; i++)
            {
                Dataset dataset = new()
                {
                    Label = labels[i]
                };
                totalChartHelper.FillWithValues(dataset, totalStatistics[i].Entries.Select(x => new Tuple<string, int>(x.Key.ToString("d"), x.Count)).ToList());
                ChartHelper.FillWithColors(dataset, colors[i]);
                totalChartHelper.Datasets.Add(dataset);
            }

            Options options = new();
            options.Scales.XAxes[0].Stacked = true;
            options.Scales.YAxes[0].Stacked = true;
            totalChartHelper.Chart.Options = options;

            return totalChartHelper.GetAsJson();
        }

        private static string MakeMostUsedChart(TotalStatisticsOfInteger mostUsedStatistics)
        {
            string[] labels = mostUsedStatistics.Entries.Select(x => x.Key.ToString()).ToArray();
            int[] data = mostUsedStatistics.Entries.Select(x => x.Count).ToArray();

            Dataset dataset = new()
            {
                Label = "Most used document types",
                Data = data
            };
            ChartHelper.FillWithColors(dataset, Color.FromArgb(80, 150, 0, 150));

            ChartHelper mostUsedHelper = new();
            mostUsedHelper.Chart.Data.Labels = labels;
            mostUsedHelper.Datasets.Add(dataset);

            Options options = new();
            mostUsedHelper.Chart.Options = options;

            return mostUsedHelper.GetAsJson();
        }
    }
}
