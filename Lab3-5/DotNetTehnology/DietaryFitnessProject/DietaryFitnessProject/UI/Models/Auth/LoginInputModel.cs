using System.ComponentModel.DataAnnotations;

namespace DietaryFitnessProject.UI.Models.Auth;

public class LoginInputModel
{
    [Display(Name = "Електронна пошта")]
    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Вкажіть пароль.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
