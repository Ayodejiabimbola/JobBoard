using AspNetCoreHero.ToastNotification.Abstractions;
using JobBoard.Context;
using JobBoard.Models.Applicant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Admin")]
public class AdminController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    INotyfService notyf,
    JobBoardDbContext jobBoardDbContext) : Controller
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly INotyfService _notyfService = notyf;
    private readonly JobBoardDbContext _jobBoardDbContext = jobBoardDbContext;

    [HttpGet]
    public async Task<IActionResult> ListAllApplicants()
    {
        var applicants = await _jobBoardDbContext.Applicants.ToListAsync();
        return View(applicants);
    }

    [HttpGet]
    [Route("Admin/ViewApplicantDetails/{applicantId}")]
    public async Task<IActionResult> ViewApplicantDetails(int applicantId)
    {
        var applicant = await _jobBoardDbContext.Applicants
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.Id == applicantId);

        if (applicant == null)
        {
            return NotFound();
        }

        if (applicant.Job == null)
        {
            throw new NullReferenceException("The Job property is null for the applicant.");
        }

        var model = new ApplicantDetailViewModel
        {
            ApplicantId = applicant.Id,
            FullName = applicant.FullName,
            Email = applicant.Email,
            PhoneNumber = applicant.PhoneNumber,
            Gender = applicant.Gender,
            JobName = applicant.Job.JobName,
            ApplicationStatus = applicant.ApplicationStatus,
            JobId = applicant.Job.Id,
            CVPath = applicant.CVPath,
            CoverLetterPath = applicant.CoverLetterPath
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateApplicationStatus(ApplicantDetailViewModel model)
    {
        var applicant = await _jobBoardDbContext.Applicants.FindAsync(model.ApplicantId);

        if (applicant == null)
        {
            return NotFound();
        }

        applicant.ApplicationStatus = model.ApplicationStatus;
        await _jobBoardDbContext.SaveChangesAsync();

        return RedirectToAction("ViewApplicantDetails", new { applicantId = model.ApplicantId });
    }

}