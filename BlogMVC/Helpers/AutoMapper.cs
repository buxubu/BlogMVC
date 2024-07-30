using AutoMapper;
using BlogMVC.Models;
using BlogMVC.Areas.Admin.Models;
using BlogMVC.ModelViews;

namespace BlogMVC.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Category, CategoriesViewModel>();
            CreateMap<CategoriesViewModel, Category>();
            CreateMap<News, NewsViewModel>();
            CreateMap<NewsViewModel, News>();
            CreateMap<Account, AccountsViewModel>();
            CreateMap<AccountsViewModel, Account>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductViewModel, Product>();
            CreateMap<CateProductViewModel, CateProduct>();
            CreateMap<CateProduct, CateProductViewModel>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderViewModel, Order>();
        }
    }
}
