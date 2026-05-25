using System.ComponentModel.DataAnnotations;

namespace DietaryFitnessProject.UI.Models;

public class RecipeFormInputModel
{
    [Display(Name = "Назва рецепту")]
    [Required(ErrorMessage = "Вкажіть назву рецепту.")]
    [StringLength(200, ErrorMessage = "Назва не може бути довшою за {1} символів.")]
    public string RecipeName { get; set; } = string.Empty;

    [Display(Name = "Інструкція")]
    [Required(ErrorMessage = "Додайте текст інструкції.")]
    [StringLength(1000, ErrorMessage = "Інструкція не може бути довшою за {1} символів.")]
    public string Instructions { get; set; } = string.Empty;

    [Display(Name = "Кількість порцій")]
    [Range(1, 10_000, ErrorMessage = "Кількість порцій має бути від {1} до {2}.")]
    public int PortionCount { get; set; } = 1;
}
