using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models.Job;

public class JobDetailViewModel
{
    [Display(Name = "Job Name")]
    public string Name { get; set; } = default!;

    [Display(Name = "Job Description")]
    public string? Description { get; set; }
    
    [Display(Name = "Picture URL")]
    public string? PictureUrl { get; set; }
}
