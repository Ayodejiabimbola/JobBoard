using System.ComponentModel.DataAnnotations;

public class EditJobViewModel
{
    public int JobId { get; set; }
    [Required]
    public string JobName { get; set; } = default!;
    [Required]
    public string JobDescription { get; set; } = default!;
    public string PictureUrl { get; set; } = default!;
}
