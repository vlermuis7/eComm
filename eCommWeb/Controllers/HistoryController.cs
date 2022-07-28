using eCommWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace eCommWeb.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;


        public HistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            //var query = _context.Item.Where(c => c.IdentityUserName == HttpContext.User.Identity.Name).ToList();

            //var query =  _context.Item.Include("Product").Where(c => c.IdentityUserName == HttpContext.User.Identity.Name)

        var query = from d in _context.Item
                      .Include(x => x.Product)
                      .Where(d => d.IdentityUserName == HttpContext.User.Identity.Name)
        select new { d.Product.Name, d.Product.Photo, d.Product.Price, d.Quantity, d.SubTotal, d.PurchaseDate, d.IdentityUserName };


            //Select new { product.Name, product.Photo, product.Price, item.Quantity, item.SubTotal, item.PurchaseDate, item.IdentityUserName };

            //var query = from item in _context.Item
            //            join product in _context.Product on item.Id equals product.Id
            //            //join patient in _context.Patient on doctor.Id equals patient.DoctorId
            //            where .Id.cont
            //            select new { product.Name, product.Photo, product.Price, item.Quantity, item.SubTotal, item.PurchaseDate, item.IdentityUserName };


            return Json(new { data = query });
        }






    }
}
