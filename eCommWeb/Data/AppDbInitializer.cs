using eCommWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace eCommWeb.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Product.Any())
                {
                    context.Product.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Keyboard",
                            Price = 20,
                            Photo = "keyboard.png"
                        },
                        new Product()
                        {
                            Name = "Mouse",
                            Price = 10,
                            Photo = "mouse.jpg"
                        },
                        new Product()
                        {
                            Name = "Monitor",
                            Price = 50,
                            Photo = "monitor.jpg"
                        },
                    });
                    context.SaveChanges();
                }

            }
        }
    }
}
