namespace StatsMonitor.Models.Chart
{
    public class Axes
    {
        public Ticks Ticks { get; set; }

        public bool Stacked { get; set; }

        public Axes()
        {
            Ticks = new();
        }
    }
}