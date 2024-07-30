using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogMVC.Models;

public partial class DbBlogContext : DbContext
{
    public DbBlogContext()
    {
    }

    public DbBlogContext(DbContextOptions<DbBlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<CateProduct> CateProducts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductColor> ProductColors { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ProductStock> ProductStocks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<ThumbProduct> ThumbProducts { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=DbBlog;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5861758AA18");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Salt)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Account__RoleID__29572725");
        });

        modelBuilder.Entity<CateProduct>(entity =>
        {
            entity.HasKey(e => e.IdCatePro).HasName("PK__CateProd__2DF520270E012E8F");

            entity.Property(e => e.IdCatePro).HasColumnName("idCatePro");
            entity.Property(e => e.Alias)
                .HasMaxLength(250)
                .HasColumnName("alias");
            entity.Property(e => e.CatName)
                .HasMaxLength(200)
                .HasColumnName("catName");
            entity.Property(e => e.Cover)
                .HasMaxLength(250)
                .HasColumnName("cover");
            entity.Property(e => e.Level).HasColumnName("level");
            entity.Property(e => e.MetaDesc)
                .HasMaxLength(250)
                .HasColumnName("metaDesc");
            entity.Property(e => e.MetaKey)
                .HasMaxLength(250)
                .HasColumnName("metaKey");
            entity.Property(e => e.ParentId).HasColumnName("parentId");
            entity.Property(e => e.Published).HasColumnName("published");
            entity.Property(e => e.SchemaMakup).HasColumnName("schemaMakup");
            entity.Property(e => e.Thumb).HasColumnName("thumb");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CaId).HasName("PK__Categori__A679D9A0402066FD");

            entity.Property(e => e.CaId).HasColumnName("CaID");
            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.CatName).HasMaxLength(255);
            entity.Property(e => e.Cover).HasMaxLength(255);
            entity.Property(e => e.Icon).HasMaxLength(255);
            entity.Property(e => e.MetaDesc).HasMaxLength(255);
            entity.Property(e => e.MetaKey).HasMaxLength(255);
            entity.Property(e => e.Thumb).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.IdColor).HasName("PK__Colors__504A3B8867E504D7");

            entity.Property(e => e.IdColor).HasColumnName("idColor");
            entity.Property(e => e.Color1)
                .HasMaxLength(20)
                .HasColumnName("color");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__News__AA1260383AD7832C");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.CaId).HasColumnName("CaID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IsHot).HasColumnName("isHot");
            entity.Property(e => e.IsNewFeed).HasColumnName("isNewFeed");
            entity.Property(e => e.ShortContent).HasMaxLength(255);
            entity.Property(e => e.Thumb).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.News)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__AccountID__2A4B4B5E");

            entity.HasOne(d => d.Ca).WithMany(p => p.News)
                .HasForeignKey(d => d.CaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__CaID__2B3F6F97");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__C8AAF6FFDA5AE217");

            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.CustomerAddress).HasMaxLength(250);
            entity.Property(e => e.CustomerEmail).HasMaxLength(100);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.CustomerPhone).HasMaxLength(12);
            entity.Property(e => e.Deleted).HasColumnName("deleted");
            entity.Property(e => e.IdPay).HasColumnName("idPay");
            entity.Property(e => e.Note)
                .HasMaxLength(250)
                .HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.Paid).HasColumnName("paid");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("paymentDate");
            entity.Property(e => e.ShipDate)
                .HasColumnType("datetime")
                .HasColumnName("shipDate");

            entity.HasOne(d => d.IdPayNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdPay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__note__5BE2A6F2");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.IdOrderDetail).HasName("PK__OrderDet__D04A4263EBEFFFA9");

            entity.Property(e => e.IdOrderDetail).HasColumnName("idOrderDetail");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.OrderNumber).HasColumnName("orderNumber");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShipDate)
                .HasColumnType("datetime")
                .HasColumnName("shipDate");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__idOrd__5FB337D6");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__idPro__60A75C0F");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPay).HasName("PK__Payments__3D783EEA60F56131");

            entity.Property(e => e.IdPay).HasColumnName("idPay");
            entity.Property(e => e.NamePayment)
                .HasMaxLength(100)
                .HasColumnName("namePayment");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__5EEC79D1EAB1154D");

            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Alias)
                .HasMaxLength(250)
                .HasColumnName("alias");
            entity.Property(e => e.BestSaller).HasColumnName("bestSaller");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("createDate");
            entity.Property(e => e.Descrip).HasColumnName("descrip");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.HomeFlag).HasColumnName("homeFlag");
            entity.Property(e => e.IdCatePro).HasColumnName("idCatePro");
            entity.Property(e => e.MetaDesc)
                .HasMaxLength(250)
                .HasColumnName("metaDesc");
            entity.Property(e => e.MetaKey)
                .HasMaxLength(250)
                .HasColumnName("metaKey");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(250)
                .HasColumnName("nameProduct");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Tag)
                .HasMaxLength(250)
                .HasColumnName("tag");
            entity.Property(e => e.Video).HasColumnName("video");

            entity.HasOne(d => d.IdCateProNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCatePro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__metaKe__534D60F1");
        });

        modelBuilder.Entity<ProductColor>(entity =>
        {
            entity.HasKey(e => e.IdProductColor).HasName("PK__ProductC__61FB51C6937F7472");

            entity.ToTable("ProductColor");

            entity.Property(e => e.IdProductColor).HasColumnName("idProductColor");
            entity.Property(e => e.IdColor).HasColumnName("idColor");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            entity.HasOne(d => d.IdColorNavigation).WithMany(p => p.ProductColors)
                .HasForeignKey(d => d.IdColor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductCo__idCol__6D0D32F4");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductColors)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductCo__idPro__6C190EBB");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.HasKey(e => e.IdProductSize).HasName("PK__ProductS__EB145BE3526CAD44");

            entity.ToTable("ProductSize");

            entity.Property(e => e.IdProductSize).HasColumnName("idProductSize");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.IdSize).HasColumnName("idSize");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSi__idPro__68487DD7");

            entity.HasOne(d => d.IdSizeNavigation).WithMany(p => p.ProductSizes)
                .HasForeignKey(d => d.IdSize)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSi__idSiz__693CA210");
        });

        modelBuilder.Entity<ProductStock>(entity =>
        {
            entity.HasKey(e => e.IdProductStock).HasName("PK__ProductS__F1CAAF3353BDE6B0");

            entity.ToTable("ProductStock");

            entity.Property(e => e.IdProductStock).HasColumnName("idProductStock");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.IdStock).HasColumnName("idStock");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSt__idPro__6FE99F9F");

            entity.HasOne(d => d.IdStockNavigation).WithMany(p => p.ProductStocks)
                .HasForeignKey(d => d.IdStock)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductSt__idSto__70DDC3D8");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3A8B506FC6");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleDescription).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.IdSize).HasName("PK__Sizes__C69FA05BF6D54704");

            entity.Property(e => e.IdSize).HasColumnName("idSize");
            entity.Property(e => e.Size1)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("size");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.IdStock).HasName("PK__Stocks__A4B76DE50B3BE3BF");

            entity.Property(e => e.IdStock).HasColumnName("idStock");
            entity.Property(e => e.Stock1).HasColumnName("stock");
        });

        modelBuilder.Entity<ThumbProduct>(entity =>
        {
            entity.HasKey(e => e.IdThumbPro).HasName("PK__ThumbPro__813AE2235B157A35");

            entity.Property(e => e.IdThumbPro).HasColumnName("idThumbPro");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.NameThumbPro).HasColumnName("nameThumbPro");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ThumbProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Thumb");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.IdTran).HasName("PK__Transact__B90D7588702C47AF");

            entity.Property(e => e.IdTran).HasColumnName("idTran");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.NameAccount)
                .HasMaxLength(200)
                .HasColumnName("nameAccount");

            entity.HasOne(d => d.Account).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Accou__6383C8BA");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__idOrd__6477ECF3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
