using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models.Auth;

public class LoginViewModel
{
    [Required(ErrorMessage ="Username is required!")]
    public string Username { get; set; } = default!;

    [Required(ErrorMessage ="Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}