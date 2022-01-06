using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using StatsMonitor.Models;
using StatsMonitor.Models.Chart;

namespace StatsMonitor.Pages.Statistics
{
    public class TasksModel : PageModel
    {
        public string ChartJson { get; set; }
        public string Message { get; set; }

        [BindProperty]
        public TaskFilter TaskFilter { get; set; }

        public int NumberOfEntries { get; set; }
        public IEnumerable<StatisticsEntryOfDateTime> Entries { get; set; }

        public int AverageNumberOfTasks { get; set; }

        public int NumberOfDedicatedTasks { get; set; }
        public int NumberOfCustomTasks { get; set; }


        private readonly TaskClient _taskClient = new(new());
        public TasksModel()
        {
            TaskFilter = new TaskFilter()
            {
                Status = Models.TaskStatus.Active,
                ToDate = DateTime.MaxValue
            };
        }

        public async Task OnGet()
        {
            try
            {
                TotalStatisticsOfDateTime statistics = await _taskClient.TotalNumberOfTasksAsync(TaskFilter);
                NumberOfEntries = statistics.TotalNumberOfEntries;
                Entries = statistics.Entries;
                AverageNumberOfTasks = (await _taskClient.AverageNumberOfTasksPrCompanyAsync(TaskFilter)).Result;
                NumberOfDedicatedTasks = (await _taskClient.NumberOfDedicatedTasksAsync(TaskFilter)).Result;
                NumberOfCustomTasks = (await _taskClient.NumberOfCustomTasksAsync(TaskFilter)).Result;

                ChartHelperEx helper = new();
                helper.Data = Entries.Select(x => (x.Key.ToString("d"), x.Count)).ToList();
                helper.Colors = new()
                {
                    Color.FromArgb(80, 54, 162, 235),
                    //Color.FromArgb(50, 54, 162, 235)
                };
                helper.Label = "Number of tasks";
                ChartJson = helper.GetAsJson();
            }
            //catch (HttpRequestException)
            //{
            //    Message = "Kunne ikke oprette forbindelse til serveren";
            //}
            //catch (ApiException)
            //{
            //    Message = "Serveren kastede en fejl";
            //}
            catch (Exception e)
            {
                Message = e.Message + " - " + e.GetType().ToString();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            await OnGet();
            return Page();
        }
    }
}
