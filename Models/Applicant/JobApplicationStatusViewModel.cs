using System.ComponentModel.DataAnnotations;

public class JobApplicationStatusViewModel
{
    public int ApplicantId { get; set; }

    [Display(Name = "Full Name")]
    public string FullName { get; set; } = default!;

    [Display(Name = "Job Name")]
    public string JobName { get; set; } = default!;


    [Display(Name = "Application Status")]
    public ApplicationStatus ApplicationStatus { get; set; }

    public int JobId { get; set;}

}