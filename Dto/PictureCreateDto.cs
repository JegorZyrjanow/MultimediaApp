using System.ComponentModel.DataAnnotations;

namespace MultimediaApp.Dto;

internal class PictureCreateDto
{
    [Required]
    public string Name { get; set; }

    public string? Tag { get; set; } // Replace with List<string>

    [Required]
    public string Path { get; set; }
}
