using Microsoft.AspNetCore.Mvc;
using PASARELASTRIPE.Models;
using Stripe;
using Stripe.Checkout;

namespace PASARELASTRIPE.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    NameProduct= "Bitcoin",
                    DescriptionProduct = "Bitcoins is a cryptoCurrenci",
                    Price = 25,
                    Amount= 2

                },
                    new ProductEntity
                {
                    NameProduct= "Ether",
                    DescriptionProduct = "Ether is a cryptoCurrenci",
                    Price = 25,
                    Amount= 2

                },
                        new ProductEntity
                {
                    NameProduct= "NFT",
                    DescriptionProduct = "NFT is a cryptoCurrenci",
                    Price = 25,
                    Amount= 2

                }
            };
            return View(productList);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());


            if(session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString();
                return View("Succes");
            }
            return View("Login");


        }

        public IActionResult Succes()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }


        public IActionResult CheckOut()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    NameProduct= "Bitcoin",
                    DescriptionProduct = "Bitcoins is a cryptoCurrenci",
                    Price = 25,
                    Amount= 2

                },
                    new ProductEntity
                {
                    NameProduct= "Ether",
                    DescriptionProduct = "Ether is a cryptoCurrenci",
                    Price = 25,
                    Amount= 2

                },
                        new ProductEntity
                {
                    NameProduct= "NFT",
                    DescriptionProduct = "NFT is a cryptoCurrenci",
                    Price = 25,
                    Amount= 2

                }
            };

            var domain = "https://localhost:7187/";

            var options = new SessionCreateOptions
            {
                SuccessUrl=domain + $"CheckOut/OrderConfirmation",
                CancelUrl = domain + "CheckOut/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode ="payment"
            };

            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(item.Price * item.Amount),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.NameProduct.ToString(),
                        }
                    },
                    Quantity = item.Amount
                };

                options.LineItems.Add(sessionListItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            TempData["Session"] = session.Id;

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

          
        }
    }
}
