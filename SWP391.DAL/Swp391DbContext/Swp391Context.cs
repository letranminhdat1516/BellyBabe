using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;

namespace SWP391.DAL.Swp391DbContext;

public partial class Swp391Context : DbContext
{
    public Swp391Context()
    {
    }

    public Swp391Context(DbContextOptions<Swp391Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogCategory> BlogCategories { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<CumulativeScore> CumulativeScores { get; set; }

    public virtual DbSet<CumulativeScoreTransaction> CumulativeScoreTransactions { get; set; }

    public virtual DbSet<CustomerOption> CustomerOptions { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<FeedbackResponse> FeedbackResponses { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<MessageInboxUser> MessageInboxUsers { get; set; }

    public virtual DbSet<MessageOutboxUser> MessageOutboxUsers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PreOrder> PreOrders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blog__FA0AA70DF6C5A482");

            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("blogID");
            entity.Property(e => e.BlogContent).HasColumnName("blogContent");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.TitleName).HasColumnName("titleName");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Blog__categoryID__4F7CD00D");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Blog__userID__5070F446");
        });

        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__BlogCate__23CAF1F858B83345");

            entity.ToTable("BlogCategory");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("categoryName");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parentCategoryID");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_BlogCategory_ParentCategory");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__06B772B96C3545C6");

            entity.ToTable("Brand");

            entity.Property(e => e.BrandId).HasColumnName("brandID");
            entity.Property(e => e.BrandName)
                .HasMaxLength(100)
                .HasColumnName("brandName");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageBrand).HasColumnName("imageBrand");
        });

        modelBuilder.Entity<CumulativeScore>(entity =>
        {
            entity.HasKey(e => e.ScoreId).HasName("PK__Cumulati__B56A0D6DD4C07D90");

            entity.ToTable("CumulativeScore");

            entity.Property(e => e.ScoreId).HasColumnName("scoreID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.RatingCount).HasColumnName("ratingCount");
            entity.Property(e => e.TotalScore)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("totalScore");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Product).WithMany(p => p.CumulativeScores)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cumulativ__produ__151B244E");

            entity.HasOne(d => d.User).WithMany(p => p.CumulativeScores)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cumulativ__userI__14270015");
        });

        modelBuilder.Entity<CumulativeScoreTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Cumulati__9B57CF52CC657AD9");

            entity.ToTable("CumulativeScoreTransaction");

            entity.Property(e => e.TransactionId).HasColumnName("transactionID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.ScoreChange)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("scoreChange");
            entity.Property(e => e.ScoreId).HasColumnName("scoreID");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transactionDate");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Product).WithMany(p => p.CumulativeScoreTransactions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cumulativ__produ__1F98B2C1");

            entity.HasOne(d => d.Score).WithMany(p => p.CumulativeScoreTransactions)
                .HasForeignKey(d => d.ScoreId)
                .HasConstraintName("FK__Cumulativ__score__208CD6FA");

            entity.HasOne(d => d.User).WithMany(p => p.CumulativeScoreTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cumulativ__userI__1EA48E88");
        });

        modelBuilder.Entity<CustomerOption>(entity =>
        {
            entity.HasKey(e => e.CustomerOptionId).HasName("PK__Customer__E946505107C7D651");

            entity.ToTable("CustomerOption");

            entity.Property(e => e.CustomerOptionId).HasColumnName("customerOptionID");
            entity.Property(e => e.InboxId).HasColumnName("inboxID");
            entity.Property(e => e.MessageId).HasColumnName("messageID");
            entity.Property(e => e.OptionValue)
                .HasMaxLength(255)
                .HasColumnName("optionValue");
            entity.Property(e => e.OutboxId).HasColumnName("outboxID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Inbox).WithMany(p => p.CustomerOptions)
                .HasForeignKey(d => d.InboxId)
                .HasConstraintName("FK__CustomerO__inbox__6EF57B66");

            entity.HasOne(d => d.Message).WithMany(p => p.CustomerOptions)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__CustomerO__messa__6E01572D");

            entity.HasOne(d => d.Outbox).WithMany(p => p.CustomerOptions)
                .HasForeignKey(d => d.OutboxId)
                .HasConstraintName("FK__CustomerO__outbo__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerOptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__userI__6D0D32F4");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__Delivery__CDC3A0D26E2B94F0");

            entity.ToTable("Delivery");

            entity.Property(e => e.DeliveryId).HasColumnName("deliveryID");
            entity.Property(e => e.DeliveryFee).HasColumnName("deliveryFee");
            entity.Property(e => e.DeliveryName)
                .HasMaxLength(100)
                .HasColumnName("deliveryName");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC42E2FDFA0");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Feedback__userID__5441852A");
        });

        modelBuilder.Entity<FeedbackResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__Feedback__0C2BB651F1DF8BF1");

            entity.Property(e => e.ResponseId).HasColumnName("responseID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Feedback).WithMany(p => p.FeedbackResponses)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__FeedbackR__feedb__5812160E");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackResponses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FeedbackR__userI__59063A47");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__4808B873D1C178EF");

            entity.ToTable("Message");

            entity.Property(e => e.MessageId).HasColumnName("messageID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.MessageContent).HasColumnName("messageContent");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.Messages)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Message__userID__5CD6CB2B");
        });

        modelBuilder.Entity<MessageInboxUser>(entity =>
        {
            entity.HasKey(e => e.InboxId).HasName("PK__MessageI__FD7C285A132157D8");

            entity.ToTable("MessageInboxUser");

            entity.Property(e => e.InboxId).HasColumnName("inboxID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.FromUserId).HasColumnName("fromUserID");
            entity.Property(e => e.IsView)
                .HasDefaultValue(false)
                .HasColumnName("isView");
            entity.Property(e => e.MessageId).HasColumnName("messageID");
            entity.Property(e => e.ToUserId).HasColumnName("toUserID");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MessageInboxUserFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .HasConstraintName("FK__MessageIn__fromU__619B8048");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageInboxUsers)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__MessageIn__messa__6383C8BA");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MessageInboxUserToUsers)
                .HasForeignKey(d => d.ToUserId)
                .HasConstraintName("FK__MessageIn__toUse__628FA481");
        });

        modelBuilder.Entity<MessageOutboxUser>(entity =>
        {
            entity.HasKey(e => e.OutboxId).HasName("PK__MessageO__4FC6E63EC9E8017A");

            entity.ToTable("MessageOutboxUser");

            entity.Property(e => e.OutboxId).HasColumnName("outboxID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.FromUserId).HasColumnName("fromUserID");
            entity.Property(e => e.IsView)
                .HasDefaultValue(false)
                .HasColumnName("isView");
            entity.Property(e => e.MessageId).HasColumnName("messageID");
            entity.Property(e => e.ToUserId).HasColumnName("toUserID");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MessageOutboxUserFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .HasConstraintName("FK__MessageOu__fromU__68487DD7");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageOutboxUsers)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__MessageOu__messa__6A30C649");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MessageOutboxUserToUsers)
                .HasForeignKey(d => d.ToUserId)
                .HasConstraintName("FK__MessageOu__toUse__693CA210");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__0809337D42183D4A");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.DeliveryId).HasColumnName("deliveryID");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.RecipientAddress)
                .HasMaxLength(255)
                .HasColumnName("recipientAddress");
            entity.Property(e => e.RecipientName)
                .HasMaxLength(255)
                .HasColumnName("recipientName");
            entity.Property(e => e.RecipientPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("recipientPhone");
            entity.Property(e => e.StatusId).HasColumnName("statusID");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.VoucherId).HasColumnName("voucherID");

            entity.HasOne(d => d.Delivery).WithMany(p => p.Orders)
                .HasForeignKey(d => d.DeliveryId)
                .HasConstraintName("FK__Order__deliveryI__7E37BEF6");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK__Order__statusID__7C4F7684");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__userID__7B5B524B");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__Order__voucherID__7D439ABD");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2AEF1AE0AF");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.IsChecked).HasColumnName("isChecked");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__02FC7413");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__produ__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OrderDeta__userI__01142BA1");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__36257A381E94C308");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.StatusId).HasColumnName("statusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("statusName");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFA6D976F185");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("paymentID");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .HasColumnName("createdBy");
            entity.Property(e => e.ExpireDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("expireDate");
            entity.Property(e => e.ExternalTransactionCode)
                .HasMaxLength(100)
                .HasColumnName("externalTransactionCode");
            entity.Property(e => e.LastUpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("lastUpdatedAt");
            entity.Property(e => e.LastUpdatedBy)
                .HasMaxLength(200)
                .HasColumnName("lastUpdatedBy");
            entity.Property(e => e.MerchantId)
                .HasMaxLength(100)
                .HasColumnName("merchantID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.PaidAmount)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("paidAmount");
            entity.Property(e => e.PayTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("payTime");
            entity.Property(e => e.PaymentContent).HasColumnName("paymentContent");
            entity.Property(e => e.PaymentCurrency).HasColumnName("paymentCurrency");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("paymentDate");
            entity.Property(e => e.PaymentDestinationId)
                .HasMaxLength(500)
                .HasColumnName("paymentDestinationID");
            entity.Property(e => e.PaymentLanguage)
                .HasMaxLength(200)
                .HasColumnName("paymentLanguage");
            entity.Property(e => e.PaymentLastMessage).HasColumnName("paymentLastMessage");
            entity.Property(e => e.PaymentRefId).HasColumnName("paymentRefID");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(150)
                .HasColumnName("paymentStatus");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.RequiredAmount)
                .HasColumnType("decimal(10, 3)")
                .HasColumnName("requiredAmount");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Payment__orderID__0B91BA14");

            entity.HasOne(d => d.Product).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Payment__product__0A9D95DB");
        });

        modelBuilder.Entity<PreOrder>(entity =>
        {
            entity.HasKey(e => e.PreOrderId).HasName("PK__PreOrder__50EDC3694CFE7881");

            entity.ToTable("PreOrder");

            entity.Property(e => e.PreOrderId).HasColumnName("preOrderID");
            entity.Property(e => e.NotificationSent)
                .HasDefaultValue(false)
                .HasColumnName("notificationSent");
            entity.Property(e => e.PreOrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("preOrderDate");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Product).WithMany(p => p.PreOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PreOrder__produc__1AD3FDA4");

            entity.HasOne(d => d.User).WithMany(p => p.PreOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PreOrder__userID__19DFD96B");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D14AD6F8CAF0");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.BackInStockDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("backInStockDate");
            entity.Property(e => e.BrandId).HasColumnName("brandID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(4, 1)")
                .HasColumnName("discount");
            entity.Property(e => e.FeedbackTotal).HasColumnName("feedbackTotal");
            entity.Property(e => e.ImageLinks).HasColumnName("imageLinks");
            entity.Property(e => e.IsSelling)
                .HasDefaultValue(true)
                .HasColumnName("isSelling");
            entity.Property(e => e.IsSoldOut)
                .HasComputedColumnSql("(case when [quantity]=(0) then (1) else (0) end)", true)
                .HasColumnName("isSoldOut");
            entity.Property(e => e.NewPrice)
                .HasComputedColumnSql("([oldPrice]*((1)-[discount]/(100)))", true)
                .HasColumnType("decimal(21, 6)")
                .HasColumnName("newPrice");
            entity.Property(e => e.OldPrice).HasColumnName("oldPrice");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .HasColumnName("productName");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK__Product__brandID__48CFD27E");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Product__categor__47DBAE45");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__23CAF1F8764B4B4F");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("categoryName");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parentCategoryID");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_ProductCategory_ParentCategory");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__2D290D497726E3E0");

            entity.ToTable("Rating");

            entity.Property(e => e.RatingId).HasColumnName("ratingID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.RatingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ratingDate");
            entity.Property(e => e.RatingValue).HasColumnName("ratingValue");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Rating__productI__10566F31");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Rating__userID__0F624AF8");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98460A5E5F4688");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CDFA7733D26");

            entity.ToTable("User");

            entity.HasIndex(e => e.PhoneNumber, "UQ__User__4849DA01D260B89E").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__User__66DCF95C140069F6").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__AB6E61641BD4F3A3").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.CumulativeScore)
                .HasDefaultValue(0)
                .HasColumnName("cumulativeScore");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasColumnName("fullName");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Otp)
                .HasMaxLength(50)
                .HasColumnName("OTP");
            entity.Property(e => e.Otpexpiry)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("OTPExpiry");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User__roleID__3D5E1FD2");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Vouchers__F53389895EB790B1");

            entity.Property(e => e.VoucherId).HasColumnName("voucherID");
            entity.Property(e => e.ExpiredDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("expiredDate");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.VoucherName)
                .HasMaxLength(100)
                .HasColumnName("voucherName");

            entity.HasOne(d => d.User).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Vouchers__userID__778AC167");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
