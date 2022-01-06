using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StatsMonitor.Models.Chart
{
    public class ChartHelperEx
    {
        private readonly ChartJs _chart;

        public List<(string label, int value)> Data { get; set; }

        public List<Color> Colors { get; set; }

        public string Label { get; set; }

        public ChartHelperEx() : this("bar")
        {
        }
        public ChartHelperEx(string type)
        {
            Data data = new()
            {
                Datasets = new Dataset[]
                {
                    new Dataset()
                }
            };

            _chart = new ChartJs()
            {
                Type = type,
                Responsive = true,
                Data = data
            };

            Data = new();
            Colors = new();
        }

        public string GetAsJson()
        {
            Update();

            return JsonConvert.SerializeObject(_chart, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        private void Update()
        {
            string[] colors = Colors.Select(color => $"rgba({color.R}, {color.G}, {color.B}, {(color.A * 1.0 / 255.0).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)})").ToArray();

            int count = Data.Count;
            string[] labels = new string[count];
            int[] values = new int[count];
            string[] backGroundColors = new string[count];

            int j = 0;
            for (int i = 0; i < count; i++)
            {
                var data = Data[i];
                labels[i] = data.label;
                values[i] = data.value;
                backGroundColors[i] = colors[j];
                if (++j == colors.Length)
                    j = 0;
            }

            _chart.Data.Labels = labels;
            _chart.Data.Datasets[0].Data = values;
            _chart.Data.Datasets[0].BackgroundColor = backGroundColors;

            if (Label != null)
                _chart.Data.Datasets[0].Label = Label;
        }
    }
}
