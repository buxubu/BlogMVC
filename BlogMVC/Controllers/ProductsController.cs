using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogMVC.Models;
using BlogMVC.Services.IProducts;
using BlogMVC.ModelViews;
using BlogMVC.Extentions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using AutoMapper;
using Stripe.Checkout;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BlogMVC.Controllers
{
    public class ProductsController : Controller
    {


        private readonly DbBlogContext _db;
        private readonly IMapper _mapper;
        private readonly IProducts _productServices;
        public INotyfService _notyfService;
        public Cart? Cart { get; set; }

        public ProductsController(DbBlogContext db, IProducts productServices, IMapper mapper, INotyfService notyfService)
        {
            _db = db;
            _productServices = productServices;
            _mapper = mapper;
            _notyfService = notyfService;
        }


        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewData["CatePros"] = await _db.CateProducts.ToListAsync();
            ViewData["ProBestSeller"] = await _db.Products.Where(x => x.BestSaller == true).Include(x => x.ThumbProducts).ToListAsync();
            ViewData["ProHomePlag"] = await _db.Products.Where(x => x.HomeFlag == true).Include(x => x.ThumbProducts).ToListAsync();
            ViewData["Products"] = await _db.Products.ToListAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Shop(int id)
        {
            return View(_productServices.ShowProCate(id));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _db.Products == null)
            {
                return NotFound();
            }

            DetailProduct objDetailPro = new DetailProduct()
            {
                Product = await _productServices.DetailProduct(id),
                LstThumb = _productServices.GetProductThumb(id),
                Color = _productServices.GetProductColor(id),
                Size = _productServices.GetProductSize(id),
                Stock = await _productServices.GetProductStock(id)
            };

            if (objDetailPro == null)
            {
                return NotFound();
            }

            return View(objDetailPro);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int idPro, int quantity, string color, string size)
        {
            var product = await _productServices.DetailProduct(idPro);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, quantity, color, size);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return Json(new { Message = "Thành Công" });
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromCart(int idPro)
        {
            var product = await _productServices.DetailProduct(idPro);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart?.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return Json(new { Message = "Thành Công" });
        }

        [HttpGet]
        public async Task<IActionResult> IndexCart()
        {
            var session = HttpContext.Session.GetJson<Cart>("cart");
            return View(session);
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut()
        {
            ViewData["PaymentType"] = new SelectList(_db.Payments, "IdPay", "NamePayment");
            CheckOutViewModel objCheckOut = new CheckOutViewModel()
            {
                Order = new OrderViewModel(),
                Cart = HttpContext.Session.GetJson<Cart>("cart")
            };
            return View(objCheckOut);
        }
        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutViewModel model)
        {
            try
            {
                var cart = model.Cart = HttpContext.Session.GetJson<Cart>("cart");

                var domain = "https://localhost:7209/";
                var option = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"Products/Index",
                    CancelUrl = domain + $"Products/CheckOut",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
                };

                //double allTotal = 0;
                foreach (var item in cart.Lines)
                {
                    //allTotal += (double)(item.TotalFee);
                    var sessionList = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = item.Product.Discount,
                            Currency = "inr",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.NameProduct
                            }
                        },
                        Quantity = item.Quantity
                    };
                    option.LineItems.Add(sessionList);
                }
                var service = new SessionService();
                Session session = service.Create(option);
                HttpContext.Response.Headers.Add("Location", session.Url);

                await _productServices.AddOrderAOrderDetail(model);

                cart.Clear();

                _notyfService.Success("Thanh toán thành công !!!");

                return new StatusCodeResult(303);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }


        private bool ProductExists(int id)
        {
            return (_db.Products?.Any(e => e.IdProduct == id)).GetValueOrDefault();
        }
    }
}
