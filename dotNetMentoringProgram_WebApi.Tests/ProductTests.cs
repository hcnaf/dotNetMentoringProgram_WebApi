using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using dotNetMentoringProgram_WebApi.Models;

namespace dotNetMentoringProgram_WebApi.Tests
{
    public class ProductTests
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
        }

        [Test]
        public void GetProducts_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _httpClient.GetAsync("https://localhost:7234/product"));
        }

        [Test]
        public void GetProducts_StatusCodeIs200()
        {
            var response = _httpClient.GetAsync("https://localhost:7234/api/products").Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void GetProducts_ResponseContainsAnyProduct()
        {
            var response = _httpClient.GetAsync("https://localhost:7234/api/products").Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var products = JsonConvert.DeserializeObject<Product[]>(json);
            Assert.IsTrue(products != null);
            Assert.IsTrue(products.Any());
        }

        [Test]
        public void Create_CheckStatusCodeIsOk()
        {
            var product = new Product()
            {
                ProductId = 9999,
                ProductName = "TestProduct",
                CategoryId = 1,
                Discontinued = true,
                QuantityPerUnit = "yes",
                ReorderLevel = 1,
                UnitPrice = 1,
                UnitsInStock = 1,
                UnitsOnOrder = 1,
            };
            var uri = $"https://localhost:7234/api/products/";
            var json = JsonConvert.SerializeObject(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync(uri, data).Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void Read_StatusCodeIs200()
        {
            var response = _httpClient.GetAsync("https://localhost:7234/api/products/1").Result;
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        [Test]
        public void Update_LastElement_NameIsChanged()
        {
            var nameToSet = "New name: " + DateTime.Now.ToLongTimeString();
            var responseOnReceiveAll = _httpClient.GetAsync("https://localhost:7234/api/products").Result;
            var jsonProducts = responseOnReceiveAll.Content.ReadAsStringAsync().Result;
            var products = JsonConvert.DeserializeObject<Product[]>(jsonProducts);

            var lastProductOnPage = products
                .OrderByDescending(x => x.ProductId).First();
            var oldName = lastProductOnPage.ProductName;

            lastProductOnPage.ProductName = nameToSet;

            var jsonUpdatedProduct = JsonConvert.SerializeObject(lastProductOnPage);
            var data = new StringContent(jsonUpdatedProduct, Encoding.UTF8, "application/json");

            var _ = _httpClient.PutAsync("https://localhost:7234/api/products/" + lastProductOnPage.ProductId, data).Result;


            var responseOnReceiveSame = _httpClient.GetAsync($"https://localhost:7234/api/products/" + lastProductOnPage.ProductId).Result;
            var sameProduct = JsonConvert.DeserializeObject<Product>(responseOnReceiveSame.Content.ReadAsStringAsync().Result);

            Assert.AreNotEqual(oldName, sameProduct.ProductName);
        }

        [Test]
        public void Delete_NotExisting_StatusCodeIs404()
        {
            var response = _httpClient.DeleteAsync("https://localhost:7234/api/products/-1").Result;
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}