using System.ComponentModel.DataAnnotations;
using DietaryFitnessProject.UI.Models;

namespace DietaryFitnessProject.UI.Models.Auth;

public class RegisterInputModel : UserProfileFormInputModel
{
    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Вкажіть пароль.")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Пароль має містити щонайменше {1} символів.")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Підтвердження пароля")]
    [Required(ErrorMessage = "Підтвердіть пароль.")]
    [Compare(nameof(Password), ErrorMessage = "Паролі не збігаються.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}
