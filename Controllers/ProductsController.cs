using Microsoft.AspNetCore.Mvc;
using Ferrer_Midterm_Store.Data;
using Ferrer_Midterm_Store.Models;

namespace Ferrer_Midterm_Store.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // READ - Show all products
        public IActionResult Index()
        {
            var products = _db.Products.ToList();

            return View(products);
        }

        // CREATE - Show the Add Product form
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - Save the new product
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                ModelState.AddModelError("Name", "Product name is required.");
            }

            if (product.Price <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0.");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _db.Products.Add(product);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // UPDATE - Show the Edit Product form
        public IActionResult Edit(int id)
        {
            var product = _db.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // UPDATE - Save the edited product
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                ModelState.AddModelError("Name", "Product name is required.");
            }

            if (product.Price <= 0)
            {
                ModelState.AddModelError("Price", "Price must be greater than 0.");
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            var existingProduct = _db.Products.Find(product.Id);

            if (existingProduct == null)
            {
                return RedirectToAction("Index");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Category = product.Category;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // DELETE - Delete a product
        public IActionResult Delete(int id)
        {
            var product = _db.Products.Find(id);

            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}