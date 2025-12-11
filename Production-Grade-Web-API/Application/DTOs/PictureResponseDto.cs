namespace Production_Grade_Web_API.Application.DTO
{
    public class PictureResponseDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string AltText { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }

}
