using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace StatsMonitor.Models.Chart
{
    public class ChartHelper
    {
        public ChartJs Chart { get; private set; } = new()
        {
            Type = "bar",
            Responsive = true,
            Data = new()
        };

        public List<Dataset> Datasets { get; set; }

        public ChartHelper()
        {
            Datasets = new();
        }

        public void Update()
        {
            Chart.Data.Datasets = Datasets.ToArray();
        }

        public string GetAsJson()
        {
            Update();

            return JsonConvert.SerializeObject(Chart, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public void FillWithValues(Dataset dataset, List<Tuple<string, int>> values)
        {
            if (Chart.Data.Labels == null)
                Chart.Data.Labels = values.Select(x => x.Item1).ToArray();
            dataset.Data = Enumerable.Repeat(0, Chart.Data.Labels.Length).ToArray();
            List<string> labels = Chart.Data.Labels.ToList();
            foreach (var tuple in values)
            {
                int index = labels.IndexOf(tuple.Item1);
                dataset.Data[index] = tuple.Item2;
            }
        }

        public static void FillWithColors(Dataset dataset, params Color[] colors)
        {
            string[] colorsAsString = colors
                .Select(color => $"rgba({color.R}, {color.G}, {color.B}, {(color.A * 1.0 / 255.0).ToString("0.0", System.Globalization.CultureInfo.InvariantCulture)})")
                .ToArray();

            if (dataset.Data == null)
                return;
            int count = dataset.Data.Length;
            dataset.BackgroundColor = new string[count];
            int j = 0;
            for (int i = 0; i < count; i++)
            {
                dataset.BackgroundColor[i] = colorsAsString[j++];
                if (j >= colorsAsString.Length)
                    j = 0;
            }
        }
    }
}
