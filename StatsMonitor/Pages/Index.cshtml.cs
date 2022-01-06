using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StatsMonitor.Models.Chart;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StatsMonitor.Pages
{
    public class IndexModel : PageModel
    {
        public ChartJs Chart { get; set; }
        public string ChartJson { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            //var chartData = @"
            //{
            //type: 'bar',
            //responsive: true,
            //data:
            //{
            //    labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
            //    datasets: [{
            //        label: '# of Votes',
            //        data: [12, 1, 15, 5, 20, 3],
            //        backgroundColor: [
            //        'rgba(255, 99, 132, 0.2)',
            //        'rgba(54, 162, 235, 0.2)',
            //        'rgba(255, 206, 86, 0.2)',
            //        'rgba(75, 192, 192, 0.2)',
            //        'rgba(153, 102, 255, 0.2)',
            //        'rgba(255, 159, 64, 0.2)'
            //            ],
            //        borderColor: [
            //        'rgba(255, 99, 132, 1)',
            //        'rgba(54, 162, 235, 1)',
            //        'rgba(255, 206, 86, 1)',
            //        'rgba(75, 192, 192, 1)',
            //        'rgba(153, 102, 255, 1)',
            //        'rgba(255, 159, 64, 1)'
            //            ],
            //        borderWidth: 1
            //    }]
            //}
            //}";
            //Chart = JsonConvert.DeserializeObject<ChartJs>(chartData);


            ChartHelper chartHelper = new();

            var data = Enumerable.Range(0, 80)
                .Select(x => new Tuple<string, int>(DateTime.Now.AddDays(x).ToString("d"), x))
                .ToList();

            Dataset dataset = new();
            chartHelper.Datasets.Add(dataset);
            chartHelper.FillWithValues(dataset, data);
            ChartHelper.FillWithColors(dataset, Color.BlueViolet);
            dataset.Label = "Test";
            ChartJson = chartHelper.GetAsJson();

            //ChartJs chart = new();

            //Data diagramData = new()
            //{
            //    Datasets = new Dataset[]
            //    {
            //        new Dataset()
            //    }
            //};
            //chart = new ChartJs()
            //{
            //    Type = "bar",
            //    Responsive = true,
            //    Data = diagramData
            //};

            //string color = "rgba(255, 99, 132, 0.2)";



            //chart.Data.Labels = data.Select(x => DateTime.Now.AddDays(x).ToString("d")).ToArray();
            //chart.Data.Datasets[0].Data = data;

            //chart.Data.Datasets[0].BackgroundColor = Enumerable.Repeat(color, data.Count()).ToArray();
            //chart.Data.Datasets[0].Label = "Test";

            //ChartJson = JsonConvert.SerializeObject(chart, new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //});
        }
    }
}
