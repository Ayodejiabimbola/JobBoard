using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models.Job;
public class CreateJobViewModel
{
    [Required(ErrorMessage ="Job Name is required!")]
    public string JobName { get; set; } = default!;

    [Required(ErrorMessage ="Add Job Description ")]
    public string JobDescription { get; set; } = default!;

    [Required(ErrorMessage ="Add imagepath ")]
    public string PictureUrl { get; set; } = default!;
}
