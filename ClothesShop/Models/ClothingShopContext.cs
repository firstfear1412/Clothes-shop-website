using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClothesShop.Models;

public partial class ClothingShopContext : DbContext
{
    public ClothingShopContext()
    {
    }

    public ClothingShopContext(DbContextOptions<ClothingShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BuyDtl> BuyDtls { get; set; }

    public virtual DbSet<Buying> Buyings { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartDtl> CartDtls { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Duty> Duties { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Target> Targets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BuyDtl>(entity =>
        {
            entity.HasKey(e => new { e.BuyId, e.PdId }).HasName("PK_BuyDtl");

            entity.Property(e => e.BuyId)
                .HasMaxLength(50)
                .HasColumnName("BuyID");
            entity.Property(e => e.PdId)
                .HasMaxLength(50)
                .HasColumnName("PdID");
            entity.Property(e => e.BdtlMoney).HasColumnName("BDtlMoney");
            entity.Property(e => e.BdtlPrice).HasColumnName("BDtlPrice");
            entity.Property(e => e.BdtlQty).HasColumnName("BDtlQty");
        });

        modelBuilder.Entity<Buying>(entity =>
        {
            entity.HasKey(e => e.BuyId);

            entity.ToTable("Buying");

            entity.Property(e => e.BuyId).HasMaxLength(50);
            entity.Property(e => e.BuyDocId)
                .HasMaxLength(50)
                .HasColumnName("BuyDocID");
            entity.Property(e => e.BuyRemark).HasColumnType("text");
            entity.Property(e => e.Saleman).HasMaxLength(50);
            entity.Property(e => e.StfId)
                .HasMaxLength(50)
                .HasColumnName("StfID");
            entity.Property(e => e.SupId)
                .HasMaxLength(50)
                .HasColumnName("SupID");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.Property(e => e.CartId)
                .HasMaxLength(50)
                .HasColumnName("CartID");
            entity.Property(e => e.CartCf)
                .HasMaxLength(1)
                .HasDefaultValue("N")
                .IsFixedLength()
                .HasColumnName("CartCF");
            entity.Property(e => e.CartPay)
                .HasMaxLength(1)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.CartSend)
                .HasMaxLength(1)
                .IsFixedLength();
            entity.Property(e => e.CartVoid)
                .HasMaxLength(1)
                .HasDefaultValue("N")
                .IsFixedLength();
            entity.Property(e => e.CusId)
                .HasMaxLength(50)
                .HasColumnName("CusID");
        });

        modelBuilder.Entity<CartDtl>(entity =>
        {
            entity.HasKey(e => new { e.CartId, e.PdId }).HasName("PK_CartDtl");

            entity.Property(e => e.CartId)
                .HasMaxLength(50)
                .HasColumnName("CartID");
            entity.Property(e => e.PdId)
                .HasMaxLength(50)
                .HasColumnName("PdID");
            entity.Property(e => e.CdtlMoney).HasColumnName("CDtlMoney");
            entity.Property(e => e.CdtlPrice).HasColumnName("CDtlPrice");
            entity.Property(e => e.CdtlQty).HasColumnName("CDtlQty");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color");

            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ColorName).HasMaxLength(10);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CusId);

            entity.Property(e => e.CusId)
                .HasMaxLength(50)
                .HasColumnName("CusID");
            entity.Property(e => e.CusAdd).HasColumnType("text");
            entity.Property(e => e.CusEmail).HasMaxLength(50);
            entity.Property(e => e.CusLogin).HasMaxLength(50);
            entity.Property(e => e.CusName).HasMaxLength(100);
            entity.Property(e => e.CusPass).HasMaxLength(50);
        });

        modelBuilder.Entity<Duty>(entity =>
        {
            entity.ToTable("Duty");

            entity.Property(e => e.DutyId)
                .HasMaxLength(50)
                .HasColumnName("DutyID");
            entity.Property(e => e.DutyName).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.PdId);

            entity.Property(e => e.PdId)
                .HasMaxLength(50)
                .HasColumnName("PdID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.PdDtls).HasMaxLength(100);
            entity.Property(e => e.PdName).HasMaxLength(50);
            entity.Property(e => e.PdtId).HasColumnName("PdtID");
            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.SupId).HasColumnName("SupID");
            entity.Property(e => e.TargetId).HasColumnName("TargetID");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.PdtId);

            entity.Property(e => e.PdtId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PdtID");
            entity.Property(e => e.PdtName).HasMaxLength(50);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("Size");

            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.SizeName)
                .HasMaxLength(5)
                .IsFixedLength();
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StfId).HasName("PK_Employees");

            entity.Property(e => e.StfId)
                .HasMaxLength(50)
                .HasColumnName("StfID");
            entity.Property(e => e.DutyId)
                .HasMaxLength(50)
                .HasColumnName("DutyID");
            entity.Property(e => e.StfName).HasMaxLength(100);
            entity.Property(e => e.StfPass).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupId);

            entity.Property(e => e.SupId)
                .HasMaxLength(50)
                .HasColumnName("SupID");
            entity.Property(e => e.SupAdd).HasColumnType("text");
            entity.Property(e => e.SupContact).HasMaxLength(100);
            entity.Property(e => e.SupEmail).HasMaxLength(50);
            entity.Property(e => e.SupName).HasMaxLength(100);
            entity.Property(e => e.SupRemark).HasColumnType("text");
            entity.Property(e => e.SupTel).HasMaxLength(50);
        });

        modelBuilder.Entity<Target>(entity =>
        {
            entity.HasKey(e => e.TargetId).HasName("PK_ProductBrands");

            entity.ToTable("Target");

            entity.Property(e => e.TargetId)
                .ValueGeneratedOnAdd()
                .HasColumnName("TargetID");
            entity.Property(e => e.TargetName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
