namespace StatsMonitor.Models.Chart
{
    public class Options
    {
        public Scales Scales { get; set; }

        public Options()
        {
            Scales = new();
        }
    }
}