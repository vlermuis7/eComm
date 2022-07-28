using eCommWeb.Data;
using eCommWeb.Helpers;
using eCommWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net;
using MailKit.Security;
using MimeKit.Text;
using System.Globalization;


//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace eCommWeb.Controllers
{
    [Authorize]
    [Route("cart")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        //private readonly IdentityDbContext _identityDb;

        public CartController(ApplicationDbContext db)
        {
            _db = db;

        }

        [Route("index")]
        [HttpGet]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (cart != null)
            {
                ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Product");
            }

        }

        [Route("buy/{id}")]
        [HttpGet]
        public IActionResult Buy(int id)
        {
            Product productModel = new Product();
            var res = _db.Product.FirstOrDefault(x => x.Id == id);
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = res, Quantity = 1 });

                

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = res, Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        [Route("addToCart")]
        [HttpGet]
        public IActionResult AddToCart(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            var bodyBuilder = new BodyBuilder();

            foreach (var res in cart)
            {
                Item orderDetail = new()
                {
                    ProductId = res.Product.Id,
                    Quantity = res.Quantity,
                    SubTotal = (res.Quantity * Convert.ToInt32(res.Product.Price)),
                    PurchaseDate = DateTime.Now,
                    IdentityUserName = HttpContext.User.Identity.Name,
                };

                bodyBuilder.HtmlBody += @"<tr>
				                                    <td>" + res.Product.Name + @"</td>
                                                    <td>" + res.Product.Price?.ToString("C", CultureInfo.CurrentCulture) + @"</td>
				                                    <td style=text-align:center>" + res.Quantity + @"</td>
				                                    <td>" + (res.Quantity * Convert.ToInt32(res.Product.Price)).ToString("C", CultureInfo.CurrentCulture) + @"</td>
				                                    <td>" + DateTime.Now.ToString("yyyy-MM-dd") + @"</td>
		                                 </tr>";


                _db.Item.Add(orderDetail);
                _db.SaveChanges();
            }

            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse("dangelo19@ethereal.email"));
            message.To.Add(MailboxAddress.Parse(HttpContext.User.Identity.Name));
            //message.To.Add(MailboxAddress.Parse(HttpContext.User.Identity.Name));
            message.Subject = "Test Email Subject";


            bodyBuilder.HtmlBody = @"<p>Hi! Please find attached your purchase list. Thanks! :)</p>
                                    <table >
                                    <thead>
                                    <tr>
                                    <th> Product Name </th>
                                    <th> Price </th>                
                                    <th> Quantity </th>                 
                                    <th> Sub-Total </th>                
                                    <th> Purchase Date </th >
                                    </tr>                
                                    <thead>
                                    <tbody> " + bodyBuilder.HtmlBody +
                                    @"<tbody>
                                    </table> ";                





            message.Body = new TextPart(TextFormat.Html) { Text = bodyBuilder.HtmlBody };

            using var smtpClient = new SmtpClient();
            smtpClient.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtpClient.Authenticate("dangelo19@ethereal.email", "yxP5MZ9aZKzCJp5JZE");
            smtpClient.Send(message);
            smtpClient.Disconnect(true);


            cart.Clear();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Product");
        }

    }
}
