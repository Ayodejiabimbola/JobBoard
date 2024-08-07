using System.ComponentModel;

public enum ApplicationStatus
{
    [Description("Submitted")]
    Submitted,
    [Description("Under review")]
    UnderReview,
    [Description("Short-listed")]
    ShortListed,
    [Description("Interview scheduled")]
    InterviewScheduled,
    [Description("Qualified")]
    Qualified,
    [Description("Disqualified")]
    Disqualified
}