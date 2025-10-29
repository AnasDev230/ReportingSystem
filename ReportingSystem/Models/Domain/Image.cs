namespace ReportingSystem.Models.Domain
{
    public class Image
    {
        public Guid ImageId { get; set; }

        public Guid ReportId { get; set; }
        public Report Report { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
