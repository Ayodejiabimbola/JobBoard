using AspNetCoreHero.ToastNotification.Abstractions;
using JobBoard.Context;
using JobBoard.Models.Job;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class JobController(UserManager<IdentityUser> userManager,
SignInManager<IdentityUser> signInManager,
INotyfService notyf,
JobBoardDbContext jobBoardDbContext,
IHttpContextAccessor httpContextAccessor) : Controller
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly INotyfService _notyfService = notyf;
    private readonly JobBoardDbContext _jobBoardDbContext = jobBoardDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<IActionResult> ViewJobs()
    {
        return View(await _jobBoardDbContext.Jobs.ToListAsync());
    }
    public async Task<IActionResult> JobDetails(int? id)
    {
        var job = await _jobBoardDbContext.Jobs.FindAsync(id);

        if (job == null)
        {
            return NotFound();
        }
        var jobDetails = new JobDetailViewModel()
        {
            Name = job.JobName,
            Description = job.JobDescription
        };

        return View(jobDetails);
    }

    [HttpGet]
    public IActionResult CreateJob()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateJob(CreateJobViewModel model)
    {
        var jobExist = await _jobBoardDbContext.Jobs.AnyAsync(x => x.JobName == model.JobName);
        if (jobExist)
        {
            _notyfService.Warning("Job already exist");
            return View(model);
        }

        var job = new Job
        {
            JobName = model.JobName,
            JobDescription = model.JobDescription,
            PictureUrl = model.PictureUrl
        };
        await _jobBoardDbContext.AddAsync(job);
        var result = await _jobBoardDbContext.SaveChangesAsync();

        if (result > 0)
        {
            _notyfService.Success("Job added successfully");
            return RedirectToAction("ViewJobs", "Job");
        }

        _notyfService.Error("unable to add job");
        return View(model);
    }
}