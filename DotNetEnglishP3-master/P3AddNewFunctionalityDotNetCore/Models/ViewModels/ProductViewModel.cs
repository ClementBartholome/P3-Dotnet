using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P3AddNewFunctionalityDotNetCore.Resources.Models.Services;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever] public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "MissingName"),
         MaxLength(50, ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "NameTooLong")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "DescriptionTooLong")]
        public string Description { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "DetailsTooLong")]
        public string Details { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "MissingStock"),
         RegularExpression(@"^\d+$", ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "StockNotAnInteger"),
         Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "StockNotGreaterThanZero")]
        public string Stock { get; set; }

        [Required(ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "MissingPrice"),
         RegularExpression(@"^\d*\.?\d*$", ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "PriceNotANumber"),
         Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(ProductService), ErrorMessageResourceName = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}