using System.ComponentModel.DataAnnotations;

namespace DietaryFitnessProject.UI.Models;

public class RecipeIngredientLineInputModel
{
    [Display(Name = "Продукт")]
    public int ProductId { get; set; }

    [Display(Name = "Грами")]
    public double Grams { get; set; }
}
