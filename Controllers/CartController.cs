using Microsoft.AspNetCore.Mvc;
using Ferrer_Midterm_Store.Data;
using Ferrer_Midterm_Store.Models;

namespace Ferrer_Midterm_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Add product to cart
        public IActionResult AddToCart(int id)
        {
            var product = _db.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var cartItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1
            };

            _db.CartItems.Add(cartItem);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Show cart
        public IActionResult Index()
        {
            var cartItems = _db.CartItems.ToList();

            return View(cartItems);
        }

        // Update quantity
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var cartItem = _db.CartItems.Find(id);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;

                if (cartItem.Quantity < 1)
                {
                    cartItem.Quantity = 1;
                }

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Remove item from cart
        public IActionResult Remove(int id)
        {
            var cartItem = _db.CartItems.Find(id);

            if (cartItem != null)
            {
                _db.CartItems.Remove(cartItem);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}