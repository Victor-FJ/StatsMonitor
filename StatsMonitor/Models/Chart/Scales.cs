namespace StatsMonitor.Models.Chart
{
    public class Scales
    {
        public Axes[] YAxes { get; set; }
        public Axes[] XAxes { get; set; }

        public Scales()
        {
            XAxes = new Axes[] { new() };
            YAxes = new Axes[] { new() };
            YAxes[0].Ticks.BeginAtZero = true;
        }
    }
}