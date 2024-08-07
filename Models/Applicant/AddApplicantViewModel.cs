using JobBoard.Data.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models.Applicant;

public class AddApplicantViewModel
{
    public int ApplicantId { get; set; }
    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "This field is required")]
    public string FullName { get; set; } = default!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "This field is required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = default!;

    [Display(Name = "Phone Number")]
    [Required(ErrorMessage = "This field is required")]
    public string PhoneNumber { get; set; } = default!;

    [Display(Name = "Date Of Birth")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DOB { get; set; }

    [Display(Name = "Gender")]
    [Required(ErrorMessage = "Please select a gender")]
    public Gender Gender { get; set; }

    [Display(Name = "CV")]
    [Required(ErrorMessage = "Please Upload your CV")]
    public IFormFile CV { get; set; } = default!;

    [Display(Name = "Cover Letter")]
    [Required(ErrorMessage = "Please Upload your Cover Letter")]
    public IFormFile CoverLetter { get; set; } = default!;

    [Required]
    public string CVPath { get; set; } = default!;

    [Required]
    public string CoverLetterPath { get; set; } = default!;

    [Display(Name = "Jobs")]
    [Required(ErrorMessage = "Select job to apply for")]
    public int JobId { get; set; }
    public List<SelectListItem> Jobs { get; set; } = new List<SelectListItem>();
    // public List<Job> Jobs { get; set; } = new List<Job>();

    [Display(Name = "Status")]
    public ApplicationStatus ApplicationStatus { get; set; } = default!;
}