namespace Healin.Shared.DTOs
{
    public class FilterDTO
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
        public string Filter { get; set; } = "";
        public string Order { get; set; } = "";
    }
}
