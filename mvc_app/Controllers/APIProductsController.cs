using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using mvc_app.Models;
using System.Security.Claims;

namespace mvc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIProductsController : Controller
    {
        private readonly IServiceProduct? _serviceProduct;
        public APIProductsController(IServiceProduct? serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }
        // Получение всех постов (доступно всем)
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _serviceProduct.ReadAsync();
            return Ok(products);
        }

        // Получение поста по Id (доступно всем)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProduct(int id)
        {
            var product = await _serviceProduct.GetByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        // Создание нового поста (доступно только авторизованным пользователям)
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var productCreat = await _serviceProduct.CreateAsync(product);

            return Ok(productCreat);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id,[FromBody] Product product)
        {
            var productUpdate = await _serviceProduct.UpdateAsync(id,product);

            return Ok(productUpdate);
        }

        // Удаление поста (доступно только авторизованным пользователям)
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleteProduct = await _serviceProduct.DeleteAsync(id);

            return Ok(new { message = "Пост удален" });
        }
    }
}
