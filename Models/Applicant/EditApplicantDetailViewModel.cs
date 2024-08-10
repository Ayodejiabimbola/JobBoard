using JobBoard.Data.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

public class EditApplicantDetailViewModel
{
    public int ApplicantId { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime DOB { get; set; }
    public Gender Gender { get; set; }
}
