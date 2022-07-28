using eCommWeb.Data;
using eCommWeb.Helpers;
using eCommWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace eCommWeb.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "product");

            ViewBag.products = _db.Product;
            return View();
        }
    }
}
