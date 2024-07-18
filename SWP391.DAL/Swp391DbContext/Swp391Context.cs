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

    public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }

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

    public virtual DbSet<RatingCategory> RatingCategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Statistic> Statistics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.BlogId).HasName("PK__Blog__FA0AA70DBDC1B343");

            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("blogID");
            entity.Property(e => e.BlogContent).HasColumnName("blogContent");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.TitleName).HasColumnName("titleName");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Blog__categoryID__59063A47");

            entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Blog__userID__59FA5E80");
        });

        modelBuilder.Entity<BlogCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__BlogCate__23CAF1F8D28D8568");

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
            entity.HasKey(e => e.BrandId).HasName("PK__Brand__06B772B99E7010EA");

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
            entity.HasKey(e => e.ScoreId).HasName("PK__Cumulati__B56A0D6D451C65F6");

            entity.ToTable("CumulativeScore");

            entity.Property(e => e.ScoreId).HasColumnName("scoreID");
            entity.Property(e => e.AvailablePoints).HasColumnName("availablePoints");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.RatingCount).HasColumnName("ratingCount");
            entity.Property(e => e.TotalScore).HasColumnName("totalScore");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User).WithMany(p => p.CumulativeScores)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cumulativ__userI__0E6E26BF");
        });

        modelBuilder.Entity<CumulativeScoreTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Cumulati__9B57CF526A0A7849");

            entity.ToTable("CumulativeScoreTransaction");

            entity.Property(e => e.TransactionId).HasColumnName("transactionID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.ScoreChange).HasColumnName("scoreChange");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("transactionDate");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .HasColumnName("transactionType");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Order).WithMany(p => p.CumulativeScoreTransactions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Cumulativ__order__18EBB532");

            entity.HasOne(d => d.User).WithMany(p => p.CumulativeScoreTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Cumulativ__userI__17F790F9");
        });

        modelBuilder.Entity<CustomerOption>(entity =>
        {
            entity.HasKey(e => e.CustomerOptionId).HasName("PK__Customer__E9465051C4ACFA59");

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
                .HasConstraintName("FK__CustomerO__inbox__6FE99F9F");

            entity.HasOne(d => d.Message).WithMany(p => p.CustomerOptions)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__CustomerO__messa__6EF57B66");

            entity.HasOne(d => d.Outbox).WithMany(p => p.CustomerOptions)
                .HasForeignKey(d => d.OutboxId)
                .HasConstraintName("FK__CustomerO__outbo__70DDC3D8");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerOptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CustomerO__userI__6E01572D");
        });

        modelBuilder.Entity<DeliveryMethod>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("PK__Delivery__CDC3A0D22F70D5A2");

            entity.ToTable("DeliveryMethod");

            entity.Property(e => e.DeliveryId).HasColumnName("deliveryID");
            entity.Property(e => e.DeliveryFee).HasColumnName("deliveryFee");
            entity.Property(e => e.DeliveryName)
                .HasMaxLength(100)
                .HasColumnName("deliveryName");
            entity.Property(e => e.OrderId).HasColumnName("orderID");

            entity.HasOne(d => d.Order).WithMany(p => p.DeliveryMethods)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__DeliveryM__order__797309D9");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC4F5D4EC6C");

            entity.ToTable("Feedback");

            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.OrderDetailId)
                .HasConstraintName("FK_Feedback_OrderDetail");

            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Feedback__produc__4D94879B");

            entity.HasOne(d => d.RatingCategory).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.RatingCategoryId)
                .HasConstraintName("FK_Feedback_RatingCategory");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Feedback__userID__4CA06362");
        });

        modelBuilder.Entity<FeedbackResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId).HasName("PK__Feedback__0C2BB651A3314E8F");

            entity.Property(e => e.ResponseId).HasColumnName("responseID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Feedback).WithMany(p => p.FeedbackResponses)
                .HasForeignKey(d => d.FeedbackId)
                .HasConstraintName("FK__FeedbackR__feedb__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.FeedbackResponses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__FeedbackR__userI__52593CB8");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Message__4808B8730A845C83");

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
                .HasConstraintName("FK__Message__userID__5DCAEF64");
        });

        modelBuilder.Entity<MessageInboxUser>(entity =>
        {
            entity.HasKey(e => e.InboxId).HasName("PK__MessageI__FD7C285AE470B1D8");

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
                .HasConstraintName("FK__MessageIn__fromU__628FA481");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageInboxUsers)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__MessageIn__messa__6477ECF3");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MessageInboxUserToUsers)
                .HasForeignKey(d => d.ToUserId)
                .HasConstraintName("FK__MessageIn__toUse__6383C8BA");
        });

        modelBuilder.Entity<MessageOutboxUser>(entity =>
        {
            entity.HasKey(e => e.OutboxId).HasName("PK__MessageO__4FC6E63E06130C52");

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
                .HasConstraintName("FK__MessageOu__fromU__693CA210");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageOutboxUsers)
                .HasForeignKey(d => d.MessageId)
                .HasConstraintName("FK__MessageOu__messa__6B24EA82");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MessageOutboxUserToUsers)
                .HasForeignKey(d => d.ToUserId)
                .HasConstraintName("FK__MessageOu__toUse__6A30C649");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__0809337D38CE5B8F");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Note).HasColumnName("note");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("orderDate");
            entity.Property(e => e.PointsUsed)
                .HasDefaultValue(0)
                .HasColumnName("pointsUsed");
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
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.VoucherId).HasColumnName("voucherID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__userID__6FE99F9F");

            entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("FK__Order__voucherID__70DDC3D8");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2A17EBCB57");

            entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
            entity.Property(e => e.IsChecked).HasColumnName("isChecked");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__03F0984C");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderDeta__produ__02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__OrderDeta__userI__02084FDA");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__36257A3869D46FF8");

            entity.ToTable("OrderStatus");

            entity.Property(e => e.StatusId).HasColumnName("statusID");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("statusName");
            entity.Property(e => e.StatusUpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("statusUpdateDate");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderStatuses)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderStat__order__7C4F7684");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__A0D9EFA659149954");

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
                .HasConstraintName("FK__Payment__orderID__0C85DE4D");

            entity.HasOne(d => d.Product).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Payment__product__0B91BA14");
        });

        modelBuilder.Entity<PreOrder>(entity =>
        {
            entity.HasKey(e => e.PreOrderId).HasName("PK__PreOrder__50EDC369D8147BF5");

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
                .HasConstraintName("FK__PreOrder__produc__208CD6FA");

            entity.HasOne(d => d.User).WithMany(p => p.PreOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PreOrder__userID__1F98B2C1");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D14A4F73731E");

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
            entity.Property(e => e.FeedbackTotal)
                .HasDefaultValue(0)
                .HasColumnName("feedbackTotal");
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
            entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__23CAF1F8ECB45117");

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
            entity.HasKey(e => e.RatingId).HasName("PK__Rating__2D290D4987DA8EF6");

            entity.ToTable("Rating");

            entity.Property(e => e.RatingId).HasColumnName("ratingID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.RatingCategoryId).HasColumnName("ratingCategoryID");
            entity.Property(e => e.RatingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ratingDate");
            entity.Property(e => e.RatingValue).HasColumnName("ratingValue");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Rating__productI__151B244E");

            entity.HasOne(d => d.RatingCategory).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.RatingCategoryId)
                .HasConstraintName("FK__Rating__ratingCa__160F4887");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Rating__userID__14270015");
        });

        modelBuilder.Entity<RatingCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__RatingCa__23CAF1F88D5FCFCD");

            entity.ToTable("RatingCategory");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("categoryName");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.TotalRatings)
                .HasDefaultValue(0)
                .HasColumnName("totalRatings");

            entity.HasOne(d => d.Product).WithMany(p => p.RatingCategories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RatingCat__produ__10566F31");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98460AD8F60C6F");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("roleID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Statistic>(entity =>
        {
            entity.HasKey(e => e.StatisticsId).HasName("PK__Statisti__7002690A19DF9AB8");

            entity.Property(e => e.StatisticsId).HasColumnName("statisticsID");
            entity.Property(e => e.Date)
                .HasMaxLength(10)
                .HasColumnName("date");
            entity.Property(e => e.ItemsSold).HasColumnName("itemsSold");
            entity.Property(e => e.NumberOfOrders).HasColumnName("numberOfOrders");
            entity.Property(e => e.OrderId).HasColumnName("orderID");
            entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryID");
            entity.Property(e => e.ProductId).HasColumnName("productID");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("totalAmount");

            entity.HasOne(d => d.Order).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Statistic__order__2FCF1A8A");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("FK__Statistic__produ__2DE6D218");

            entity.HasOne(d => d.Product).WithMany(p => p.Statistics)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Statistic__produ__2EDAF651");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CDF654DBF21");

            entity.ToTable("User");

            entity.HasIndex(e => e.PhoneNumber, "UQ__User__4849DA01FCDFEE13").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__User__AB6E6164FA94CA3F").IsUnique();

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
                .HasConstraintName("FK__User__roleID__3C69FB99");
            entity.Property(e => e.IsFirstLogin)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PK__Vouchers__F53389898A672A5B");

            entity.Property(e => e.VoucherId).HasColumnName("voucherID");
            entity.Property(e => e.ExpiredDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("expiredDate");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.VoucherCode)
                .HasMaxLength(30)
                .HasColumnName("voucherCode");
            entity.Property(e => e.VoucherName)
                .HasMaxLength(100)
                .HasColumnName("voucherName");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("price");
            entity.Property(e => e.MinimumBillAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("MinimumBillAmount");
            entity.HasOne(d => d.User).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Vouchers__userID__74AE54BC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
