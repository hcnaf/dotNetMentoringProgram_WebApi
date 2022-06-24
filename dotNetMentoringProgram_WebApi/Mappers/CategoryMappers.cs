using dotNetMentoringProgram_WebApi.Models;
using dotNetMentoringProgram_WebApi.ViewModels;

namespace dotNetMentoringProgram_WebApi.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryVM ToCategoryVM(this Category category) =>
            new CategoryVM
            {
                CategoryId = category.CategoryId,
                Description = category.Description,
                CategoryName = category.CategoryName,
            };

        public static Category ToCategory(this CategoryVM category) =>
            new Category
            {
                CategoryId = category.CategoryId,
                Description = category.Description,
                CategoryName = category.CategoryName,
            };
    }
}
