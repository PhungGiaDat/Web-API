using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Web_API.Data;

public partial class LoyaltyCardContext : DbContext
{
    public LoyaltyCardContext()
    {
    }

    public LoyaltyCardContext(DbContextOptions<LoyaltyCardContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<LoyaltyCard> LoyaltyCards { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Shop> Shops { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DANIEL-PHUNG;Initial Catalog=LoyaltyCard;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8D5098B34");

            entity.HasIndex(e => e.CitizenId, "UQ__Customer__6E49FA0D12CBB7A0").IsUnique();

            entity.HasIndex(e => e.PhoneNumber, "UQ__Customer__85FB4E3847695234").IsUnique();

            entity.Property(e => e.CustomerId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CitizenId)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoyaltyCard>(entity =>
        {
            entity.HasKey(e => e.CardNumber).HasName("PK__LoyaltyC__A4E9FFE83C4F375B");

            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Customer).WithMany(p => p.LoyaltyCards)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoyaltyCards_Customers");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PK__Rewards__825015B926509DBA");

            entity.Property(e => e.RewardId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RewardName).HasMaxLength(100);
        });

        modelBuilder.Entity<Shop>(entity =>
        {
            entity.HasKey(e => e.ShopId).HasName("PK__Shops__67C557C913413837");

            entity.Property(e => e.ShopId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ShopName).HasMaxLength(100);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6BA11755EC");

            entity.Property(e => e.TransactionId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CardNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.ReferenceId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TransactionTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.CardNumberNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.CardNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_LoyaltyCards");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
