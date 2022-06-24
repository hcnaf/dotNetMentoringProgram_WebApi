using dotNetMentoringProgram_WebApi.Models;
using dotNetMentoringProgram_WebApi.ViewModels;

namespace dotNetMentoringProgram_WebApi.Mappers
{
    public static class ProductMappers
    {
        public static ProductVM ToProductVM(this Product product) =>
            new ProductVM
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                UnitsInStock = product.UnitsInStock,
                Discontinued = product.Discontinued,
                ProductName = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                UnitPrice = product.UnitPrice,
                UnitsOnOrder = product.UnitsOnOrder
            };

        public static Product ToProduct(this ProductVM product) =>
            new Product
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                UnitsInStock = product.UnitsInStock,
                Discontinued = product.Discontinued,
                ProductName = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                ReorderLevel = product.ReorderLevel,
                UnitPrice = product.UnitPrice,
                UnitsOnOrder = product.UnitsOnOrder
            };
    }
}
