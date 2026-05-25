using System.ComponentModel.DataAnnotations;

namespace DietaryFitnessProject.UI.Models;

public class ProductInputModel
{
    [Display(Name = "Назва продукту")]
    [Required(ErrorMessage = "Вкажіть назву продукту.")]
    [StringLength(100, ErrorMessage = "Назва не може бути довшою за {1} символів.")]
    [MinLength(2, ErrorMessage = "Назва продукту має містити щонайменше 2 літери.")]
    [RegularExpression(ValidationPatterns.LettersOnly, ErrorMessage = "Назва продукту може містити лише літери (без цифр).")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Калорії (на 100 г)")]
    [Range(0, 100_000, ErrorMessage = "Калорійність має бути від {1} до {2} на 100 г.")]
    public double CaloriesPer100g { get; set; }

    [Display(Name = "Білки, г (на 100 г)")]
    [Range(0, 100_000, ErrorMessage = "Білки мають бути від {1} до {2} г на 100 г.")]
    public double ProteinsPer100g { get; set; }

    [Display(Name = "Жири, г (на 100 г)")]
    [Range(0, 100_000, ErrorMessage = "Жири мають бути від {1} до {2} г на 100 г.")]
    public double FatsPer100g { get; set; }

    [Display(Name = "Вуглеводи, г (на 100 г)")]
    [Range(0, 100_000, ErrorMessage = "Вуглеводи мають бути від {1} до {2} г на 100 г.")]
    public double CarbsPer100g { get; set; }
}
