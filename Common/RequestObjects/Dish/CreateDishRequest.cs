using Common.Utils;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.RequestObjects.Dish;

public class CreateDishRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [NotNullOrEmptyOrWhitespace(ErrorMessage = "Name cannot be empty or whitespace.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Image is required.")]
    [ImageValidation(ErrorMessage = "Image must be a valid image file and less than 2MB in size.")]
    public IFormFile ImageFile { get; set; }

    [Required, MinLength(1)]
    public int[] TagList { get; set; }

}