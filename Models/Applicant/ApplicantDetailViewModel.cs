using JobBoard.Data;
using JobBoard.Data.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public int JobId { get; internal set; }
}
