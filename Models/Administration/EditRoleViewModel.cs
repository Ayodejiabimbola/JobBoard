using System.ComponentModel.DataAnnotations;

public class EditRoleViewModel 
{
    public EditRoleViewModel()
    {
        Users = new List<string>();
    }
    public string Id { get; set; } = default!;
    [Required(ErrorMessage = "Role Name is required")]
    public string RoleName { get; set; } = default!;
    public List<string> Users { get; set; } = default!;
}