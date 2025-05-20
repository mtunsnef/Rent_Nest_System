using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RentNest.Core.Domains;

public partial class RentNestSystemContext : DbContext
{
    public RentNestSystemContext()
    {
    }

    public RentNestSystemContext(DbContextOptions<RentNestSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accommodation> Accommodations { get; set; }

    public virtual DbSet<AccommodationAmenity> AccommodationAmenities { get; set; }

    public virtual DbSet<AccommodationDetail> AccommodationDetails { get; set; }

    public virtual DbSet<AccommodationImage> AccommodationImages { get; set; }

    public virtual DbSet<AccommodationType> AccommodationTypes { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<FavoritePost> FavoritePosts { get; set; }

    public virtual DbSet<PackagePricing> PackagePricings { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostApproval> PostApprovals { get; set; }

    public virtual DbSet<PostPackageDetail> PostPackageDetails { get; set; }

    public virtual DbSet<PostPackageType> PostPackageTypes { get; set; }

    public virtual DbSet<PromoCode> PromoCodes { get; set; }

    public virtual DbSet<PromoUsage> PromoUsages { get; set; }

    public virtual DbSet<TimeUnitPackage> TimeUnitPackages { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }
    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accommodation>(entity =>
        {
            entity.HasKey(e => e.AccommodationId).HasName("PK__Accommod__004EC32568628C1C");

            entity.ToTable("Accommodation");

            entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DepositAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("deposit_amount");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MaxOccupancy).HasColumnName("max_occupancy");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A")
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .HasColumnName("title");
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("video_url");

            entity.HasOne(d => d.Owner).WithMany(p => p.Accommodations)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_Accommodation_Owner");

            entity.HasOne(d => d.Type).WithMany(p => p.Accommodations)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accommodation_Type");
        });

        modelBuilder.Entity<AccommodationAmenity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accommod__3213E83F642AFD4D");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
            entity.Property(e => e.AmenityId).HasColumnName("amenity_id");

            entity.HasOne(d => d.Accommodation).WithMany(p => p.AccommodationAmenities)
                .HasForeignKey(d => d.AccommodationId)
                .HasConstraintName("FK_Amenities_Accommodation");

            entity.HasOne(d => d.Amenity).WithMany(p => p.AccommodationAmenities)
                .HasForeignKey(d => d.AmenityId)
                .HasConstraintName("FK_Amenities");
        });

        modelBuilder.Entity<AccommodationDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__Accommod__38E9A22467219D85");

            entity.HasIndex(e => e.AccommodationId, "UQ__Accommod__004EC32420577D2E").IsUnique();

            entity.Property(e => e.DetailId).HasColumnName("detail_id");
            entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
            entity.Property(e => e.BathroomCount)
                .HasDefaultValue(0)
                .HasColumnName("bathroom_count");
            entity.Property(e => e.BedroomCount)
                .HasDefaultValue(0)
                .HasColumnName("bedroom_count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FurnitureStatus)
                .HasMaxLength(100)
                .HasColumnName("furniture_status");
            entity.Property(e => e.HasAirConditioner)
                .HasDefaultValue(false)
                .HasColumnName("has_air_conditioner");
            entity.Property(e => e.HasKitchenCabinet)
                .HasDefaultValue(false)
                .HasColumnName("has_kitchen_cabinet");
            entity.Property(e => e.HasLoft)
                .HasDefaultValue(false)
                .HasColumnName("has_loft");
            entity.Property(e => e.HasRefrigerator)
                .HasDefaultValue(false)
                .HasColumnName("has_refrigerator");
            entity.Property(e => e.HasWashingMachine)
                .HasDefaultValue(false)
                .HasColumnName("has_washing_machine");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Accommodation).WithOne(p => p.AccommodationDetail)
                .HasForeignKey<AccommodationDetail>(d => d.AccommodationId)
                .HasConstraintName("FK_Details_Accommodation");
        });

        modelBuilder.Entity<AccommodationImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Accommod__DC9AC95501F8F662");

            entity.ToTable("AccommodationImage");

            entity.Property(e => e.ImageId).HasColumnName("image_id");
            entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
            entity.Property(e => e.Caption)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("caption");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");

            entity.HasOne(d => d.Accommodation).WithMany(p => p.AccommodationImages)
                .HasForeignKey(d => d.AccommodationId)
                .HasConstraintName("FK_AccommodationImage_Accommodation");
        });

        modelBuilder.Entity<AccommodationType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__Accommod__2C00059886F2249B");

            entity.ToTable("AccommodationType");

            entity.HasIndex(e => e.TypeName, "UQ__Accommod__543C4FD96D786073").IsUnique();

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CD07E03A04");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E6164B566FDF2").IsUnique();

            entity.HasIndex(e => e.AuthProviderId, "UQ__Account__C82CBBE9A41CE12D").IsUnique();

            entity.HasIndex(e => e.Username, "UX_Account_Username")
                .IsUnique()
                .HasFilter("([Username] IS NOT NULL)");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AuthProvider)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("auth_provider");
            entity.Property(e => e.AuthProviderId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("auth_provider_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("A")
                .IsFixedLength()
                .HasColumnName("is_active");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.AmenityId).HasName("PK__Amenitie__E908452DBCD1DD3C");

            entity.Property(e => e.AmenityId).HasColumnName("amenity_id");
            entity.Property(e => e.AmenityName)
                .HasMaxLength(100)
                .HasColumnName("amenity_name");
            entity.Property(e => e.IconSvg)
                .IsUnicode(false)
                .HasColumnName("iconSvg");
        });

        modelBuilder.Entity<FavoritePost>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__Favorite__46ACF4CB9EA49921");

            entity.ToTable("FavoritePost");

            entity.HasIndex(e => new { e.AccountId, e.PostId }, "UQ_FavoritePost_Account_Post").IsUnique();

            entity.Property(e => e.FavoriteId).HasColumnName("favorite_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.PostId).HasColumnName("post_id");

            entity.HasOne(d => d.Account).WithMany(p => p.FavoritePosts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_FavoritePost_Account");

            entity.HasOne(d => d.Post).WithMany(p => p.FavoritePosts)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FavoritePost_Post");
        });

        modelBuilder.Entity<PackagePricing>(entity =>
        {
            entity.HasKey(e => e.PricingId).HasName("PK__PackageP__A25A9FB7FC81B24F");

            entity.ToTable("PackagePricing");

            entity.HasIndex(e => new { e.TimeUnitId, e.PackageTypeId, e.DurationValue }, "UQ_PackagePricing_Combination").IsUnique();

            entity.Property(e => e.PricingId).HasColumnName("pricing_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DurationValue).HasColumnName("duration_value");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PackageTypeId).HasColumnName("package_type_id");
            entity.Property(e => e.TimeUnitId).HasColumnName("time_unit_id");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.PackageType).WithMany(p => p.PackagePricings)
                .HasForeignKey(d => d.PackageTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagePricing_Type");

            entity.HasOne(d => d.TimeUnit).WithMany(p => p.PackagePricings)
                .HasForeignKey(d => d.TimeUnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PackagePricing_TimeUnit");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EAA05E95B5");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.MethodId).HasColumnName("method_id");
            entity.Property(e => e.PaymentDate)
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PostPackageDetailsId).HasColumnName("post_package_details_id");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Payment_Account");

            entity.HasOne(d => d.Method).WithMany(p => p.Payments)
                .HasForeignKey(d => d.MethodId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Payment_Method");

            entity.HasOne(d => d.PostPackageDetails).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PostPackageDetailsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_PostPackageDetails");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__PaymentM__747727B696760F17");

            entity.ToTable("PaymentMethod");

            entity.Property(e => e.MethodId).HasColumnName("method_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IconUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("icon_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MethodName)
                .HasMaxLength(100)
                .HasColumnName("method_name");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__3ED78766425ED2BF");

            entity.ToTable("Post");

            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CurrentStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("P")
                .IsFixedLength()
                .HasColumnName("current_status");
            entity.Property(e => e.PublishedAt)
                .HasColumnType("datetime")
                .HasColumnName("published_at");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.ViewCount)
                .HasDefaultValue(0)
                .HasColumnName("view_count");

            entity.HasOne(d => d.Accommodation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AccommodationId)
                .HasConstraintName("FK_Post_Accommodation");

            entity.HasOne(d => d.Account).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_Account");
        });

        modelBuilder.Entity<PostApproval>(entity =>
        {
            entity.HasKey(e => e.ApprovalId).HasName("PK__PostAppr__C94AE61A6B85498A");

            entity.Property(e => e.ApprovalId).HasColumnName("approval_id");
            entity.Property(e => e.ApprovedByAccountId).HasColumnName("approved_by_account_id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.ProcessedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("processed_at");
            entity.Property(e => e.RejectionReason)
                .HasMaxLength(255)
                .HasColumnName("rejection_reason");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("P")
                .IsFixedLength()
                .HasColumnName("status");

            entity.HasOne(d => d.ApprovedByAccount).WithMany(p => p.PostApprovals)
                .HasForeignKey(d => d.ApprovedByAccountId)
                .HasConstraintName("FK_PostApprovals_Approver");

            entity.HasOne(d => d.Post).WithMany(p => p.PostApprovals)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_PostApprovals_Post");
        });

        modelBuilder.Entity<PostPackageDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PostPack__3213E83FF0F20B5C");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pending")
                .HasColumnName("payment_status");
            entity.Property(e => e.PaymentTransactionId)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("payment_transaction_id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.PricingId).HasColumnName("pricing_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");

            entity.HasOne(d => d.Post).WithMany(p => p.PostPackageDetails)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_PostPackageDetails_Post");

            entity.HasOne(d => d.Pricing).WithMany(p => p.PostPackageDetails)
                .HasForeignKey(d => d.PricingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PostPackageDetails_Pricing");
        });

        modelBuilder.Entity<PostPackageType>(entity =>
        {
            entity.HasKey(e => e.PackageTypeId).HasName("PK__PostPack__DFBEE40AC386C709");

            entity.ToTable("PostPackageType");

            entity.Property(e => e.PackageTypeId).HasColumnName("package_type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.PackageTypeName)
                .HasMaxLength(100)
                .HasColumnName("package_type_name");
            entity.Property(e => e.Priority).HasColumnName("priority");
        });

        modelBuilder.Entity<PromoCode>(entity =>
        {
            entity.HasKey(e => e.PromoId).HasName("PK__PromoCod__84EB4CA5A997DA63");

            entity.ToTable("PromoCode");

            entity.HasIndex(e => e.Code, "UQ__PromoCod__357D4CF9D950FBF5").IsUnique();

            entity.Property(e => e.PromoId).HasColumnName("promo_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.DiscountAmount)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discount_amount");
            entity.Property(e => e.DiscountPercent)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("discount_percent");
            entity.Property(e => e.DurationDays).HasColumnName("duration_days");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.IsNewUserOnly)
                .HasDefaultValue(false)
                .HasColumnName("is_new_user_only");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PromoUsage>(entity =>
        {
            entity.HasKey(e => e.PromoUsageId).HasName("PK__PromoUsa__60E8C638B1CA274A");

            entity.ToTable("PromoUsage");

            entity.Property(e => e.PromoUsageId).HasColumnName("promo_usage_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.PromoId).HasColumnName("promo_id");
            entity.Property(e => e.UsedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("used_at");

            entity.HasOne(d => d.Account).WithMany(p => p.PromoUsages)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_PromoUsage_Account");

            entity.HasOne(d => d.Post).WithMany(p => p.PromoUsages)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PromoUsage_Post");

            entity.HasOne(d => d.Promo).WithMany(p => p.PromoUsages)
                .HasForeignKey(d => d.PromoId)
                .HasConstraintName("FK_PromoUsage_Promo");
        });

        modelBuilder.Entity<TimeUnitPackage>(entity =>
        {
            entity.HasKey(e => e.TimeUnitId).HasName("PK__TimeUnit__8304AF4737F0EEAD");

            entity.ToTable("TimeUnitPackage");

            entity.Property(e => e.TimeUnitId).HasColumnName("time_unit_id");
            entity.Property(e => e.Data)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("data");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.TimeUnitName)
                .HasMaxLength(20)
                .HasColumnName("time_unit_name");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__UserProf__AEBB701F61E73369");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.AccountId, "UQ__UserProf__46A222CCB14D318C").IsUnique();

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("avatar_url");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .HasColumnName("last_name");
            entity.Property(e => e.Occupation)
                .HasMaxLength(100)
                .HasColumnName("occupation");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Account).WithOne(p => p.UserProfile)
                .HasForeignKey<UserProfile>(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserProfile_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
