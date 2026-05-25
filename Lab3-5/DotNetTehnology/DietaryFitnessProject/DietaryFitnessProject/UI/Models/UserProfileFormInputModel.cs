using System.ComponentModel.DataAnnotations;
using DietaryFitnessProject.DAL.Enums;

namespace DietaryFitnessProject.UI.Models;

public class UserProfileFormInputModel
{
    [Display(Name = "Ім'я")]
    [Required(ErrorMessage = "Вкажіть ім'я.")]
    [StringLength(100, ErrorMessage = "Ім'я не може бути довшим за {1} символів.")]
    [MinLength(2, ErrorMessage = "Ім'я має містити щонайменше 2 літери.")]
    [RegularExpression(ValidationPatterns.LettersOnly, ErrorMessage = "Ім'я може містити лише літери (без цифр).")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Прізвище")]
    [Required(ErrorMessage = "Вкажіть прізвище.")]
    [StringLength(100, ErrorMessage = "Прізвище не може бути довшим за {1} символів.")]
    [MinLength(2, ErrorMessage = "Прізвище має містити щонайменше 2 літери.")]
    [RegularExpression(ValidationPatterns.LettersOnly, ErrorMessage = "Прізвище може містити лише літери (без цифр).")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Електронна пошта")]
    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Некоректна адреса електронної пошти.")]
    [StringLength(100, ErrorMessage = "Адреса не може бути довшою за {1} символів.")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Стать")]
    [Required(ErrorMessage = "Оберіть стать.")]
    public Gender Gender { get; set; }

    [Display(Name = "Вік (повних років)")]
    [Range(10, 120, ErrorMessage = "Вік має бути від {1} до {2} років.")]
    public int Age { get; set; } = 25;

    [Display(Name = "Зріст (см)")]
    [Range(0, 300, ErrorMessage = "Зріст має бути від {1} до {2} см.")]
    public double Height { get; set; }

    [Display(Name = "Вага (кг)")]
    [Range(0, 500, ErrorMessage = "Вага має бути від {1} до {2} кг.")]
    public double Weight { get; set; }

    [Display(Name = "Рівень активності")]
    [Required(ErrorMessage = "Оберіть рівень активності.")]
    public ActivityLevel ActivityLevel { get; set; }

    [Display(Name = "Мета")]
    [Required(ErrorMessage = "Оберіть мету.")]
    public GoalType Goal { get; set; }
}
