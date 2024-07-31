public class Job : BaseEntity
{
    public string JobName { get; set; } = default!;
    public string JobDescription { get; set; } = default!;
    public string PictureUrl { get; set; } = default!;
}