using AspNetCoreHero.ToastNotification.Abstractions;
using JobBoard.Context;
using JobBoard.Data;
using JobBoard.Models.Applicant;
using JobBoard.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class ApplicantController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    INotyfService notyf,
    JobBoardDbContext jobBoardDbContext,
    IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment webHostEnvironment) : Controller
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly INotyfService _notyfService = notyf;
    private readonly JobBoardDbContext _jobBoardDbContext = jobBoardDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    [HttpGet]
    public async Task<IActionResult> ListAllApplicants()
    {
        var applicants = await _jobBoardDbContext.Applicants.ToListAsync();
        return View(applicants);
    }
    public IActionResult AddApplicant()
    {
        var jobs = _jobBoardDbContext.Jobs.Select(x => new SelectListItem
        {
            Text = x.JobName,
            Value = x.Id.ToString()
        }).ToList();

        var viewModel = new AddApplicantViewModel()
        {
            Jobs = jobs
        };

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> AddApplicant(AddApplicantViewModel model)
    {
        var applicantExist = await _jobBoardDbContext.Applicants.AnyAsync(x => x.FullName == model.FullName || x.Email == model.Email);
        var userDetail = await Helper.GetCurrentUserIdAsync(_httpContextAccessor, _userManager);

        if (applicantExist)
        {
            _notyfService.Warning("Applicant already exists");
            return View(model);
        }

        var applicant = new Applicant
        {
            UserId = userDetail.userId,
            FullName = model.FullName,
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            DOB = model.DOB,
            Gender = model.Gender,
            CVPath = model.CVPath,
            CoverLetterPath = model.CoverLetterPath,
            JobId = model.JobId,
            ApplicationStatus = ApplicationStatus.Submitted
        };

        if (model.CV != null && model.CV.Length > 0)
        {
            if (model.CV.Length > 1024 * 1024) 
            {
                ModelState.AddModelError("CV", "CV file size should not exceed 1MB.");
                return View(model);
            }

            var cvExtension = Path.GetExtension(model.CV.FileName).ToLower();
            var allowedExtensions = new[] { ".doc", ".docx", ".pdf" };
            if (!allowedExtensions.Contains(cvExtension))
            {
                ModelState.AddModelError("CV", "Invalid CV file type. Only .doc, .docx, and .pdf files are allowed.");
                return View(model);
            }

            var cvFileName = Path.GetFileNameWithoutExtension(model.CV.FileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(model.CV.FileName);
            var cvFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", cvFileName);
            Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, "uploads"));

            using (var stream = new FileStream(cvFilePath, FileMode.Create))
            {
                await model.CV.CopyToAsync(stream);
            }

            applicant.CVPath = cvFilePath;
        }

        if (model.CoverLetter != null && model.CoverLetter.Length > 0)
        {
            if (model.CoverLetter.Length > 1024 * 1024) 
            {
                ModelState.AddModelError("CoverLetter", "Cover letter file size should not exceed 1MB.");
                return View(model);
            }
            var coverLetterExtension = Path.GetExtension(model.CoverLetter.FileName).ToLower();
            var allowedExtensions = new[] { ".doc", ".docx", ".pdf" };
            if (!allowedExtensions.Contains(coverLetterExtension))
            {
                ModelState.AddModelError("CoverLetter", "Invalid cover letter file type. Only .doc, .docx, and .pdf files are allowed.");
                return View(model);
            }

            var coverLetterFileName = Path.GetFileNameWithoutExtension(model.CoverLetter.FileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(model.CoverLetter.FileName);
            var coverLetterFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", coverLetterFileName);
            Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, "uploads"));

            using (var stream = new FileStream(coverLetterFilePath, FileMode.Create))
            {
                await model.CoverLetter.CopyToAsync(stream);
            }

            applicant.CoverLetterPath = coverLetterFilePath;
        }

        await _jobBoardDbContext.AddAsync(applicant);
        var result = await _jobBoardDbContext.SaveChangesAsync();

        if (result > 0)
        {
            _notyfService.Success("Details submitted successfully");
            return RedirectToAction("ListAllApplicants", "Applicant");
        }

        _notyfService.Error("An error occurred during application");
        return View(model);
    }
}