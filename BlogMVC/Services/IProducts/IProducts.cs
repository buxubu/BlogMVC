using AutoMapper;
using BlogMVC.Helpers;
using BlogMVC.Models;
using BlogMVC.ModelViews;
using BlogMVC.Services.ICatePro;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace BlogMVC.Services.IProducts
{
    public interface IProducts
    {
        IEnumerable<ProductViewModel> GetAllProduct();
        Task CreateProductAsync(ProductViewModel product);
        Task<ProductViewModel> DetailProduct(int idCatePro);
        IEnumerable<Product> ShowProCate(int idCatePro);
        IEnumerable<Color> GetProductColor(int idPro);
        IEnumerable<Size> GetProductSize(int idPro);
        IEnumerable<ThumbProduct> GetProductThumb(int idPro);
        Task<Stock> GetProductStock(int idPro);
        Task<string> GetNameColor(int idColor);
        Task<string> GetNameSize(int idSize);
        Task AddOrderAOrderDetail(CheckOutViewModel model);
    }
    public class RepoProducts : IProducts
    {
        private readonly DbBlogContext _db;
        private readonly IMapper _mapper;

        public RepoProducts(DbBlogContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task AddOrderAOrderDetail(CheckOutViewModel model)
        {
            model.Order.OrderDate = DateTime.Now;
            model.Order.ShipDate = DateTime.Now.Date.AddDays(3);
            model.Order.Deleted = false;
            model.Order.Paid = true;
            model.Order.PaymentDate = DateTime.Now;
            var mapOrder = _mapper.Map<Order>(model.Order);
            await _db.Orders.AddAsync(mapOrder);
            await _db.SaveChangesAsync();

            //save orderdetail
            OrderDetail objOrderDetail = new OrderDetail();
            foreach (var item in model.Cart.Lines)
            {
                objOrderDetail.IdOrderDetail = 0;
                objOrderDetail.IdOrder = mapOrder.IdOrder;
                objOrderDetail.IdProduct = item.Product.IdProduct;
                objOrderDetail.OrderNumber = 0;
                objOrderDetail.Quantity = item.Quantity;
                objOrderDetail.Discount = item.Product.Discount;
                objOrderDetail.ShipDate = DateTime.Now.Date.AddDays(3);

                await _db.OrderDetails.AddAsync(objOrderDetail);
                await _db.SaveChangesAsync();
            }

            Stock? stockPro = new Stock();
            foreach (var item in model.Cart.Lines)
            {
                stockPro = await (from pro in _db.Products
                                  join proSt in _db.ProductStocks on pro.IdProduct equals proSt.IdProduct
                                  join st in _db.Stocks on proSt.IdStock equals st.IdStock
                                  where pro.IdProduct == item.Product.IdProduct
                                  select new Stock
                                  {
                                      IdStock = proSt.IdStock,
                                      Stock1 = st.Stock1 - 1
                                  }).FirstOrDefaultAsync();
                _db.Stocks.Update(stockPro);
                await _db.SaveChangesAsync();
            }

            model.Cart.Clear();

        }

        public async Task CreateProductAsync(ProductViewModel model)
        {
            model.CreateDate = DateTime.Now;
            model.Alias = Uitilities.SEOUrl(model.NameProduct);
            model.Tag = Uitilities.SEOUrl(model.NameProduct);
            model.Descrip = Uitilities.RemoveLinksStyle(model.Descrip);

            if (model.fVideo != null)
            {
                var up = await Uitilities.UploadFileToVideo(model.fVideo);
                model.Video = up;
            }

            var mapPro = _mapper.Map<Product>(model);


            await _db.Products.AddAsync(mapPro);
            await _db.SaveChangesAsync();

            int idPro = mapPro.IdProduct;

            ProductColor objProColor = new ProductColor
            {
                IdProductColor = 0,
                IdProduct = idPro,
                IdColor = model.IdColor
            };

            ProductSize objProductSz = new ProductSize
            {
                IdProductSize = 0,
                IdProduct = idPro,
                IdSize = model.IdSize
            };

            ProductStock objProStock = new ProductStock
            {
                IdProductStock = 0,
                IdProduct = idPro,
                IdStock = model.IdStock
            };

            await _db.ProductColors.AddAsync(objProColor);
            await _db.ProductSizes.AddAsync(objProductSz);
            await _db.ProductStocks.AddAsync(objProStock);

            await _db.SaveChangesAsync();


            var upThumbPro = await Uitilities.UploadMutifileFileToFolderThumPros(model.fThumb);
            ThumbProduct thumbPro = new ThumbProduct();
            foreach (var item in upThumbPro)
            {
                thumbPro.IdThumbPro = 0;
                thumbPro.IdProduct = idPro;
                thumbPro.NameThumbPro = item;
                await _db.ThumbProducts.AddAsync(thumbPro);
                await _db.SaveChangesAsync();
            }

        }

        public async Task<ProductViewModel> DetailProduct(int idCatePro)
        {
            var getAllStu = await (from pro in _db.Products
                                   join cate in _db.CateProducts on pro.IdCatePro equals cate.IdCatePro
                                   join proSz in _db.ProductSizes on pro.IdProduct equals proSz.IdProduct
                                   join sz in _db.Sizes on proSz.IdSize equals sz.IdSize
                                   join proSt in _db.ProductStocks on pro.IdProduct equals proSt.IdProduct
                                   join st in _db.Stocks on proSt.IdStock equals st.IdStock
                                   join proCl in _db.ProductColors on pro.IdProduct equals proCl.IdProduct
                                   join cl in _db.Colors on proCl.IdColor equals cl.IdColor
                                   join th in _db.ThumbProducts on pro.IdProduct equals th.IdProduct
                                   where pro.IdProduct == idCatePro
                                   select new ProductViewModel
                                   {
                                       IdProduct = pro.IdProduct,
                                       IdCatePro = cate.IdCatePro,
                                       NameCate = cate.CatName,
                                       IdThumbPro = th.IdThumbPro,
                                       NameThumbPro = th.NameThumbPro,
                                       IdSize = sz.IdSize,
                                       Size = sz.Size1,
                                       IdColor = cl.IdColor,
                                       Color = cl.Color1,
                                       IdStock = st.IdStock,
                                       Stock = st.Stock1,
                                       NameProduct = pro.NameProduct,
                                       Price = pro.Price,
                                       Discount = pro.Discount,
                                       Descrip = pro.Descrip,
                                       Video = pro.Video,
                                       CreateDate = pro.CreateDate,
                                       BestSaller = pro.BestSaller.Value,
                                       HomeFlag = pro.HomeFlag.Value,
                                       Active = pro.Active.Value,
                                       Tag = pro.Tag,
                                       Alias = pro.Alias,
                                       MetaDesc = pro.MetaDesc,
                                       MetaKey = pro.MetaKey
                                   }).FirstOrDefaultAsync();
            return getAllStu;
        }

        public IEnumerable<ProductViewModel> GetAllProduct()
        {
            var getAllStu = (from pro in _db.Products
                             join cate in _db.CateProducts on pro.IdCatePro equals cate.IdCatePro
                             join proSz in _db.ProductSizes on pro.IdProduct equals proSz.IdProduct
                             join sz in _db.Sizes on proSz.IdSize equals sz.IdSize
                             join proSt in _db.ProductStocks on pro.IdProduct equals proSt.IdProduct
                             join st in _db.Stocks on proSt.IdStock equals st.IdStock
                             join proCl in _db.ProductColors on pro.IdProduct equals proCl.IdProduct
                             join cl in _db.Colors on proCl.IdColor equals cl.IdColor

                             //join th in _db.ThumbProducts on pro.IdProduct equals th.IdProduct
                             select new ProductViewModel
                             {
                                 IdProduct = pro.IdProduct,
                                 IdCatePro = cate.IdCatePro,
                                 NameCate = cate.CatName,
                                 //IdThumbPro = th.IdThumbPro,
                                 //NameThumbPro = th.NameThumbPro,
                                 IdSize = sz.IdSize,
                                 Size = sz.Size1,
                                 IdColor = cl.IdColor,
                                 Color = cl.Color1,
                                 IdStock = st.IdStock,
                                 Stock = st.Stock1,
                                 NameProduct = pro.NameProduct,
                                 Price = pro.Price,
                                 Discount = pro.Discount,
                                 Descrip = pro.Descrip,
                                 Video = pro.Video,
                                 CreateDate = pro.CreateDate,
                                 BestSaller = pro.BestSaller.Value,
                                 HomeFlag = pro.HomeFlag.Value,
                                 Active = pro.Active.Value,
                                 Tag = pro.Tag,
                                 Alias = pro.Alias,
                                 MetaDesc = pro.MetaDesc,
                                 MetaKey = pro.MetaKey
                             }).ToList();
            return getAllStu;

            //var getAllStu = _db.Products.ToList();
            //List<ProductViewModel> obj = new List<ProductViewModel>();
            //foreach (var item in getAllStu)
            //{
            //    obj.Add(_mapper.Map<ProductViewModel>(item));
            //}
            //return obj;
        }

        public async Task<string> GetNameColor(int idColor)
        {
            return _db.Colors.Where(x => x.IdColor == idColor).FirstOrDefault().Color1;
        }

        public async Task<string> GetNameSize(int idSize)
        {
            return _db.Sizes.Where(x => x.IdSize == idSize).FirstOrDefault().Size1;
        }

        public IEnumerable<Color> GetProductColor(int idPro)
        {
            var getProCl = (from pro in _db.Products
                            join proCl in _db.ProductColors on pro.IdProduct equals proCl.IdProduct
                            join cl in _db.Colors on proCl.IdColor equals cl.IdColor
                            where pro.IdProduct == idPro
                            select new Color
                            {
                                IdColor = cl.IdColor,
                                Color1 = cl.Color1,
                            }).ToList();
            return getProCl;
        }

        public IEnumerable<Size> GetProductSize(int idPro)
        {
            var getProSz = (from pro in _db.Products
                            join proSz in _db.ProductSizes on pro.IdProduct equals proSz.IdProduct
                            join sz in _db.Sizes on proSz.IdSize equals sz.IdSize
                            where pro.IdProduct == idPro
                            select new Size
                            {
                                IdSize = sz.IdSize,
                                Size1 = sz.Size1,
                            }).ToList();
            return getProSz;

        }

        public async Task<Stock> GetProductStock(int idPro)
        {
            var getProSz = (from pro in _db.Products
                            join proSt in _db.ProductStocks on pro.IdProduct equals proSt.IdProduct
                            join st in _db.Stocks on proSt.IdStock equals st.IdStock
                            where pro.IdProduct == idPro
                            select new Stock
                            {
                                IdStock = st.IdStock,
                                Stock1 = st.Stock1,
                            }).FirstOrDefault();
            return getProSz;
        }

        public IEnumerable<ThumbProduct> GetProductThumb(int idPro)
        {
            var getProThumb = (from pro in _db.Products
                               join th in _db.ThumbProducts on pro.IdProduct equals th.IdProduct
                               where th.IdProduct == idPro
                               select new ThumbProduct
                               {
                                   IdThumbPro = th.IdThumbPro,
                                   IdProduct = pro.IdProduct,
                                   NameThumbPro = th.NameThumbPro
                               }).ToList();
            return getProThumb;
        }

        public IEnumerable<Product> ShowProCate(int idCatePro)
        {
            var findProCate = _db.Products.Where(x => x.IdCatePro == idCatePro).Include(x => x.ThumbProducts).ToList();
            return findProCate;
        }
    }
}

