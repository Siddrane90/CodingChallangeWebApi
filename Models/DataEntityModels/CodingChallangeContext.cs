using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CodingChallangeWebApi.Models.DataEntityModels;

public partial class CodingChallangeContext : DbContext
{
    public CodingChallangeContext()
    {
    }

    public CodingChallangeContext(DbContextOptions<CodingChallangeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<PaymentProvider> PaymentProviders { get; set; }

    public virtual DbSet<PaymentTypeAndRule> PaymentTypeAndRules { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=D:\\MyDevelopment\\AvisaCodeChallange\\Resources\\Database\\CodingChallange.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PaymentValuePostComission).HasColumnType("NUMERIC (20, 2)");
            entity.Property(e => e.PaymentValuePreComission).HasColumnType("NUMERIC (20, 2)");
            entity.Property(e => e.ProviderOrderId).HasColumnName("ProviderOrderID");

            entity.HasOne(d => d.OptimalPaymentMethodSelectedNavigation).WithMany(p => p.Orders).HasForeignKey(d => d.OptimalPaymentMethodSelected);

            entity.HasOne(d => d.PaymentProviderSelectedNavigation).WithMany(p => p.Orders).HasForeignKey(d => d.PaymentProviderSelected);

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<PaymentProvider>(entity =>
        {
            entity.HasIndex(e => e.ProviderName, "IX_PaymentProviders_ProviderName").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Apiurl).HasColumnName("APIUrl");
        });

        modelBuilder.Entity<PaymentTypeAndRule>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ComissionValue).HasColumnType("NUMERIC (20, 2)");
            entity.Property(e => e.MaximumAmount).HasColumnType("NUMERIC (20, 2)");
            entity.Property(e => e.MinimumAmount).HasColumnType("NUMERIC (20, 2)");
            entity.Property(e => e.ProviderId).HasColumnName("ProviderID");

            entity.HasOne(d => d.Provider).WithMany(p => p.PaymentTypeAndRules).HasForeignKey(d => d.ProviderId);
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasIndex(e => e.ProductName, "IX_ProductDetails_ProductName").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PricePerUnit).HasColumnType("NUMERIC");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
