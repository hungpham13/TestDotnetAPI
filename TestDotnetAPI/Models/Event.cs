namespace TestDotnetAPI.Models
{
    public class Event
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string PerformerName { get; }
        public DateTime Time { get; }
        public string Status { get; }
        public List<Stream> Streams { get; }
        public string mainImage { get; }
        public string coverImage { get; }
        public List<Attendance> Attendances { get; }
    }
}