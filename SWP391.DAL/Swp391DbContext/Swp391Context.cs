using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SWP391.DAL.Entities;
using WebApplication1.Entities;

namespace SWP391.DAL.Swp391DbContext
{
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
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<FeedbackResponse> FeedbackResponses { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<MessageInboxUser> MessageInboxUsers { get; set; }
        public virtual DbSet<MessageOutboxUser> MessageOutboxUsers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PreOrder> PreOrders { get; set; }
        public virtual DbSet<PriceUpdate> PriceUpdates { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<CustomerOption> CustomerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentId).ValueGeneratedOnAdd();
                entity.Property(e => e.PaymentContent).HasMaxLength(250);
                entity.Property(e => e.PaymentCurrency).HasMaxLength(10);
                entity.Property(e => e.PaymentRefId).HasMaxLength(50);
                entity.Property(e => e.RequiredAmount).HasColumnType("decimal(19, 2)");
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.ExpireDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentLanguage).HasMaxLength(10);
                entity.Property(e => e.MerchantId).HasMaxLength(50);
                entity.Property(e => e.PaymentDestinationId).HasMaxLength(50);
                entity.Property(e => e.PaidAmount).HasColumnType("decimal(19, 2)");
                entity.Property(e => e.PaymentStatus).HasMaxLength(20);
                entity.Property(e => e.PaymentLastMessage).HasMaxLength(250);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy).HasMaxLength(50);
                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.Amount).HasColumnType("decimal(19, 2)");
                entity.Property(e => e.ExternalTransactionCode).HasMaxLength(100);
                entity.Property(e => e.OrderId);
                entity.Property(e => e.PayTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasKey(e => e.BlogId).HasName("PK__Blog__FA0AA70D0ECF7352");

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
                    .HasConstraintName("FK__Blog__categoryID__5629CD9C");

                entity.HasOne(d => d.User).WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Blog__userID__571DF1D5");
            });

            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__BlogCate__23CAF1F871E25074");

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
                entity.HasKey(e => e.BrandId).HasName("PK__Brand__06B772B9E759E807");

                entity.ToTable("Brand");

                entity.Property(e => e.BrandId).HasColumnName("brandID");
                entity.Property(e => e.BrandName)
                    .HasMaxLength(100)
                    .HasColumnName("brandName");
                entity.Property(e => e.Description).HasColumnName("description");
            });

            modelBuilder.Entity<CumulativeScore>(entity =>
            {
                entity.HasKey(e => e.ScoreId).HasName("PK__Cumulati__B56A0D6D73A7C34A");

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
                    .HasConstraintName("FK__Cumulativ__produ__123EB7A3");

                entity.HasOne(d => d.User).WithMany(p => p.CumulativeScores)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Cumulativ__userI__114A936A");
            });

            modelBuilder.Entity<CumulativeScoreTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__Cumulati__9B57CF527BBE837F");

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
                    .HasConstraintName("FK__Cumulativ__produ__2B0A656D");

                entity.HasOne(d => d.Score).WithMany(p => p.CumulativeScoreTransactions)
                    .HasForeignKey(d => d.ScoreId)
                    .HasConstraintName("FK__Cumulativ__score__2BFE89A6");

                entity.HasOne(d => d.User).WithMany(p => p.CumulativeScoreTransactions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Cumulativ__userI__2A164134");
            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(e => e.DeliveryId).HasName("PK__Delivery__CDC3A0D2080982B7");

                entity.ToTable("Delivery");

                entity.Property(e => e.DeliveryId).HasColumnName("deliveryID");
                entity.Property(e => e.DeliveryFee)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("deliveryFee");
                entity.Property(e => e.DeliveryMethod)
                    .HasMaxLength(50)
                    .HasColumnName("deliveryMethod");
                entity.Property(e => e.DeliveryName)
                    .HasMaxLength(100)
                    .HasColumnName("deliveryName");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId).HasName("PK__Feedback__2613FDC4477CBFC1");

                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title");
                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.Feedbacks)
                    .HasPrincipalKey(p => p.UserName)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK__Feedback__userNa__5AEE82B9");
            });

            modelBuilder.Entity<FeedbackResponse>(entity =>
            {
                entity.HasKey(e => e.ResponseId).HasName("PK__Feedback__0C2BB651271C5CEE");

                entity.Property(e => e.ResponseId).HasColumnName("responseID");
                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");
                entity.Property(e => e.FeedbackId).HasColumnName("feedbackID");
                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Feedback).WithMany(p => p.FeedbackResponses)
                    .HasForeignKey(d => d.FeedbackId)
                    .HasConstraintName("FK__FeedbackR__feedb__5EBF139D");

                entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.FeedbackResponses)
                    .HasPrincipalKey(p => p.UserName)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK__FeedbackR__userN__5FB337D6");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.HasKey(e => e.ManufacturerId).HasName("PK__Manufact__02B553A9D7B1C336");

                entity.ToTable("Manufacturer");

                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturerID");
                entity.Property(e => e.ManufacturerName)
                    .HasMaxLength(100)
                    .HasColumnName("manufacturerName");
            });


            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId).HasName("PK__Message__4808B8736D47BBE9");

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
                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.Messages)
                    .HasPrincipalKey(p => p.UserName)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK__Message__userNam__6383C8BA");
            });

            modelBuilder.Entity<MessageInboxUser>(entity =>
            {
                entity.HasKey(e => e.InboxId).HasName("PK__MessageI__FD7C285A902A21AE");

                entity.ToTable("MessageInboxUser");

                entity.Property(e => e.InboxId).HasColumnName("inboxID");
                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");
                entity.Property(e => e.FromUserName)
                    .HasMaxLength(50)
                    .HasColumnName("fromUserName");
                entity.Property(e => e.IsView)
                    .HasDefaultValue(false)
                    .HasColumnName("isView");
                entity.Property(e => e.MessageId).HasColumnName("messageID");
                entity.Property(e => e.ToUserName)
                    .HasMaxLength(50)
                    .HasColumnName("toUserName");

                entity.HasOne(d => d.FromUserNameNavigation).WithMany(p => p.MessageInboxUserFromUserNameNavigations)
                    .HasPrincipalKey(p => p.UserName)
                    .HasForeignKey(d => d.FromUserName)
                    .HasConstraintName("FK__MessageIn__fromU__68487DD7");

                entity.HasOne(d => d.Message).WithMany(p => p.MessageInboxUsers)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK__MessageIn__messa__6A30C649");

                entity.HasOne(d => d.ToUserNameNavigation).WithMany(p => p.MessageInboxUserToUserNameNavigations)
                    .HasPrincipalKey(p => p.UserName)
                    .HasForeignKey(d => d.ToUserName)
                    .HasConstraintName("FK__MessageIn__toUse__693CA210");
            });

            modelBuilder.Entity<MessageOutboxUser>(entity =>
            {
                entity.HasKey(e => e.OutboxId).HasName("PK__MessageO__4FC6E63E97C3049F");

                entity.ToTable("MessageOutboxUser");

                entity.Property(e => e.OutboxId).HasColumnName("outboxID");
                entity.Property(e => e.DateCreated)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("dateCreated");
                entity.Property(e => e.FromUserName)
                    .HasMaxLength(50)
                    .HasColumnName("fromUserName");
                entity.Property(e => e.IsView)
                    .HasDefaultValue(false)
                    .HasColumnName("isView");
                entity.Property(e => e.MessageId).HasColumnName("messageID");
                entity.Property(e => e.ToUserName)
                    .HasMaxLength(50)
                    .HasColumnName("toUserName");

                entity.HasOne(d => d.FromUserNameNavigation).WithMany(p => p.MessageOutboxUserFromUserNameNavigations)
                    .HasPrincipalKey(p => p.UserName)
                    .HasForeignKey(d => d.FromUserName)
                    .HasConstraintName("FK__MessageOu__fromU__6EF57B66");

                entity.HasOne(d => d.Message).WithMany(p => p.MessageOutboxUsers)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK__MessageOu__messa__70DDC3D8");

                entity.HasOne(d => d.ToUserNameNavigation).WithMany(p => p.MessageOutboxUserToUserNameNavigations)
                    .HasPrincipalKey(p => p.UserName)
                    .HasForeignKey(d => d.ToUserName)
                    .HasConstraintName("FK__MessageOu__toUse__6FE99F9F");
            });

            modelBuilder.Entity<CustomerOption>(entity =>
            {
                entity.HasKey(e => e.CustomerOptionId).HasName("PK__Customer__E9465051D9CDE185");

                entity.ToTable("CustomerOption");

                entity.Property(e => e.CustomerOptionId).HasColumnName("customerOptionID");
                entity.Property(e => e.InboxId).HasColumnName("inboxID");
                entity.Property(e => e.MessageId).HasColumnName("messageID");
                entity.Property(e => e.OptionValue)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("optionValue");
                entity.Property(e => e.OutboxId).HasColumnName("outboxID");
                entity.Property(e => e.UserId).HasColumnName("userID");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId).HasName("PK__Order__0809337D5AB0A0D7");

                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("orderID");
                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");
                entity.Property(e => e.Checked).HasColumnName("checked");
                entity.Property(e => e.DeliveryFee)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("deliveryFee");
                entity.Property(e => e.DeliveryId).HasColumnName("deliveryID");
                entity.Property(e => e.DeliveryMethod)
                    .HasMaxLength(50)
                    .HasColumnName("deliveryMethod");
                entity.Property(e => e.Note).HasColumnName("note");
                entity.Property(e => e.OrderDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("orderDate");
                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasColumnName("paymentMethod");
                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .HasColumnName("phoneNumber");
                entity.Property(e => e.StatusId).HasColumnName("statusID");
                entity.Property(e => e.TotalPrice)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("totalPrice");
                entity.Property(e => e.UserId).HasColumnName("userID");
                entity.Property(e => e.VoucherId).HasColumnName("voucherID");

                entity.HasOne(d => d.Delivery).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.DeliveryId)
                    .HasConstraintName("FK__Order__deliveryI__7F2BE32F");

                entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__Order__statusID__7D439ABD");

                entity.HasOne(d => d.User).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Order__userID__7C4F7684");

                entity.HasOne(d => d.Voucher).WithMany(p => p.Orders)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FK__Order__voucherID__7E37BEF6");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__E4FEDE2AFDFFAC07");

                entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailID");
                entity.Property(e => e.OrderId).HasColumnName("orderID");
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("price");
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
                entity.HasKey(e => e.StatusId).HasName("PK__OrderSta__36257A38D51A12E3");

                entity.ToTable("OrderStatus");

                entity.Property(e => e.StatusId).HasColumnName("statusID");
                entity.Property(e => e.StatusName)
                    .HasMaxLength(50)
                    .HasColumnName("statusName");
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.PaymentId).ValueGeneratedOnAdd();
                entity.Property(e => e.PaymentContent).HasMaxLength(250);
                entity.Property(e => e.PaymentCurrency).HasMaxLength(10);
                entity.Property(e => e.PaymentRefId).HasMaxLength(50);
                entity.Property(e => e.RequiredAmount).HasColumnType("decimal(19, 2)");
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.ExpireDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentLanguage).HasMaxLength(10);
                entity.Property(e => e.MerchantId).HasMaxLength(50);
                entity.Property(e => e.PaymentDestinationId).HasMaxLength(50);
                entity.Property(e => e.PaidAmount).HasColumnType("decimal(19, 2)");
                entity.Property(e => e.PaymentStatus).HasMaxLength(20);
                entity.Property(e => e.PaymentLastMessage).HasMaxLength(250);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedBy).HasMaxLength(50);
                entity.Property(e => e.LastUpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.Amount).HasColumnType("decimal(19, 2)");
                entity.Property(e => e.ExternalTransactionCode).HasMaxLength(100);
                entity.Property(e => e.OrderId);
                entity.Property(e => e.PayTime).HasColumnType("datetime");
            });




            modelBuilder.Entity<PreOrder>(entity =>
            {
                entity.HasKey(e => e.PreOrderId).HasName("PK__PreOrder__50EDC36990C827F6");

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
                    .HasConstraintName("FK__PreOrder__produc__17F790F9");

                entity.HasOne(d => d.User).WithMany(p => p.PreOrders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PreOrder__userID__17036CC0");
            });

            modelBuilder.Entity<PriceUpdate>(entity =>
            {
                entity.HasKey(e => e.PriceId).HasName("PK__PriceUpd__366E4C229EAE0F0C");

                entity.ToTable("PriceUpdate");

                entity.Property(e => e.PriceId).HasColumnName("priceID");
                entity.Property(e => e.DateApplied)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("dateApplied");
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 3)")
                    .HasColumnName("price");
                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.HasOne(d => d.Product).WithMany(p => p.PriceUpdates)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__PriceUpda__produ__4F7CD00D");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId).HasName("PK__Product__2D10D14A864A7EFF");

                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("productID");
                entity.Property(e => e.BackInStockDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime")
                    .HasColumnName("backInStockDate");
                entity.Property(e => e.BrandId).HasColumnName("brandID");
                entity.Property(e => e.CategoryId).HasColumnName("categoryID");
                entity.Property(e => e.IsSelling)
                    .HasDefaultValue(true)
                    .HasColumnName("isSelling");
                entity.Property(e => e.IsSoldOut)
                    .HasComputedColumnSql("(case when [quantity]=(0) then (1) else (0) end)", true)
                    .HasColumnName("isSoldOut");
                entity.Property(e => e.MadeIn)
                    .HasMaxLength(100)
                    .HasColumnName("madeIn");
                entity.Property(e => e.ManufacturerId).HasColumnName("manufacturerID");
                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .HasColumnName("productName");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.StorageInstructions).HasColumnName("storageInstructions");
                entity.Property(e => e.SuitableAge)
                    .HasMaxLength(200)
                    .HasColumnName("suitableAge");
                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .HasColumnName("unit");
                entity.Property(e => e.UsageInstructions).HasColumnName("usageInstructions");

                entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__Product__brandID__4BAC3F29");

                entity.HasOne(d => d.Category).WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Product__categor__4AB81AF0");

                entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK__Product__manufac__49C3F6B7");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__ProductC__23CAF1F8FB432CEE");

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
                entity.HasKey(e => e.RatingId).HasName("PK__Rating__2D290D49FE37C65C");

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
                    .HasConstraintName("FK__Rating__productI__0D7A0286");

                entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Rating__userID__0C85DE4D");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("PK__Role__CD98460A9D396774");

                entity.ToTable("Role");

                entity.Property(e => e.RoleId).HasColumnName("roleID");
                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CDF01902B1B");

                entity.ToTable("User");

                entity.HasIndex(e => e.PhoneNumber, "UQ__User__4849DA01A62ACA6E").IsUnique();

                entity.HasIndex(e => e.UserName, "UQ__User__66DCF95C6E24EDF5").IsUnique();

                entity.HasIndex(e => e.Email, "UQ__User__AB6E6164E0F0F311").IsUnique();

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
                entity.Property(e => e.OTP)
                    .HasMaxLength(50)
                    .HasColumnName("OTP");
                entity.Property(e => e.OTPExpiry)
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
                entity.HasKey(e => e.VoucherId).HasName("PK__Vouchers__F5338989F0E10E7F");

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
                    .HasConstraintName("FK__Vouchers__userID__787EE5A0");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
    
