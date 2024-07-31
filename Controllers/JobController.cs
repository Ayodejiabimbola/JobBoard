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
}