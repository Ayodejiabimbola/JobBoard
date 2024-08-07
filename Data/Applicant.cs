using JobBoard.Data.Enum;

namespace JobBoard.Data;
public class Applicant : BaseEntity
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public DateTime DOB { get; set; }
    public Gender Gender { get; set; } = default!;
    public int JobId { get; set; }
    public Job Job { get; set; } = default!;
    public string CVPath { get; set; } = default!;
    public string CoverLetterPath { get; set; } = default!;
    public string UserId { get; internal set; } = default!;
    public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Submitted;
}