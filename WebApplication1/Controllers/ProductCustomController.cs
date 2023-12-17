using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductCustomController : ControllerBase
    {
        private readonly MyDataContext _context;

        public ProductCustomController(MyDataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductDTO productDTO)
        {
            var newProduct = new Product
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                IsActive = true
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return Ok(newProduct);
        }

        [HttpDelete("{id}")]
        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            product.IsActive = false;
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePriceProduct(int id, [FromBody] ProductUpdatePriceDTO productDTO)
        {
            var product = _context.Products.Find(id);

            product.Price = productDTO.Price;
            _context.SaveChanges();

            return Ok(product);
        }

        [HttpPatch]
        public IActionResult DeactivateProducts([FromBody] List<int> productIds)
        {
            foreach (var productId in productIds)
            {
                var productToUpdate = _context.Products.FirstOrDefault(p => p.ProductId == productId);

                if (productToUpdate != null)
                {
                    productToUpdate.IsActive = false;
                    _context.Entry(productToUpdate).State = EntityState.Modified;
                }
            }

            _context.SaveChanges();
            return Ok("Productos eliminados correctamente.");
        }
    }
}
