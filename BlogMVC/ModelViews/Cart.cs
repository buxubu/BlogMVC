using AspNetCore;
using BlogMVC.Models;
using BlogMVC.Services.IProducts;

namespace BlogMVC.ModelViews
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        public void AddItem(ProductViewModel model, int quantity, string color, string size)
        {
            CartLine line = Lines.Where(x => x.Product.IdProduct == model.IdProduct).FirstOrDefault();
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = model,
                    Quantity = quantity,
                    Color = color,
                    Size = size
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }


        public void RemoveLine(ProductViewModel model) =>
            Lines.RemoveAll(x => x.Product.IdProduct == model.IdProduct);

        public int ComputeTotalValue() =>
            (int)Lines.Sum(x => x.Product.Discount * x.Quantity);

        public void Clear() => Lines.Clear();
    }
    public class CartLine
    {
        public int CartLineID { get; set; }
        public ProductViewModel? Product { get; set; }
        public int? Quantity { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
    }
}



