namespace ReportingSystem.Models.DTO.Image
{
    public class ImageDto
    {
        public Guid ImageId { get; set; }
        public Guid ReportId { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
