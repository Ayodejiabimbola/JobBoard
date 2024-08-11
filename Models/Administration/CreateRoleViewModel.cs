using System.ComponentModel.DataAnnotations;

public class CreateRoleViewModel
{
    [Required]
    public string RoleName { get; set; } = default!;
}