using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StatsMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StatsMonitor.Pages.Statistics.Shared
{
    public abstract class StatisticsPageModel : PageModel
    {
        public string ExceptionMessage { get; set; }

        [BindProperty]
        public DateFilter DateFilter { get; set; }

        protected StatisticsPageModel()
        {
            DateFilter = new DateFilter()
            {
                FromDate = DateTime.MinValue,
                ToDate = DateTime.MaxValue
            };
        }

        protected abstract Task CustomOnGet();

        public async Task OnGet()
        {
            try
            {
                await CustomOnGet();
            }
            catch (HttpRequestException)
            {
                ExceptionMessage = "Kunne ikke oprette forbindelse til serveren";
            }
            catch (ApiException)
            {
                ExceptionMessage = "Serveren kastede en fejl";
            }
            catch (Exception e)
            {
                ExceptionMessage = e.Message + " - " + e.GetType().ToString();
            }
        }

        public async Task OnPost()
        {
            await OnGet();
        }
    }
}
