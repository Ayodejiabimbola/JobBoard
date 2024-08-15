using JobBoard.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models.Applicant;

public class ApplicantDetailViewModel
{
    public int ApplicantId { get; set; }
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = default!;

    [Display(Name = "Email")]
    public string Email { get; set; } = default!;

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = default!;

    [Display(Name = "Gender")]
    public Gender Gender { get; set; } = default!;

    [Display(Name = "Job")]
    public string JobName { get; set; } = default!;

    [Display(Name = "Application Status")]
    public ApplicationStatus ApplicationStatus { get; set; }
    public int JobId { get; internal set; }

    public string CVPath { get; set; } = default!;
    public string CoverLetterPath { get; set; } = default!;
}
