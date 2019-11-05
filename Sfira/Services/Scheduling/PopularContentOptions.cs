namespace MroczekDotDev.Sfira.Services.Scheduling
{
    public class PopularContentOptions
    {
        public int IntervalInMinutes { get; set; } = 10;
        public int PeriodInMinutes { get; set; } = 1440;
        public int SamplesPerMinute { get; set; } = 256;
        public int PopularUsersCount { get; set; } = 8;
        public int TrendingTagsCount { get; set; } = 8;
    }
}
