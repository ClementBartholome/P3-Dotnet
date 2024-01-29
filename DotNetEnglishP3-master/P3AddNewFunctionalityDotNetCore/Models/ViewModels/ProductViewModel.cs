using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever] public int Id { get; set; }

        [Required, MaxLength(50)] public string Name { get; set; }

        [MaxLength(50)] public string Description { get; set; }

        [MaxLength(50)] public string Details { get; set; }

        [Required]  public int Stock { get; set; }

        [Required] public double Price { get; set; }
    }
}