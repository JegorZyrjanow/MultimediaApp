using System;
using System.ComponentModel.DataAnnotations;

namespace MultimediaApp.Model;

[Serializable] // Replace XML writing with regular DB Sys
public class Picture
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string? Tag { get; set; } // Replace with List<string>

    [Required]
    public string Path { get; set; }
}

