using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentNest.Core.Domains;
namespace RentNest.Infrastructure.DataAccess;

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

    public virtual DbSet<AccommodationDetail> AccommodationDetails { get; set; }

    public virtual DbSet<AccommodationImage> AccommodationImages { get; set; }

    public virtual DbSet<AccommodationType> AccommodationTypes { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<LandLordVerification> LandLordVerifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentItem> PaymentItems { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostApproval> PostApprovals { get; set; }

    public virtual DbSet<PostPackage> PostPackages { get; set; }

    public virtual DbSet<PostPackageDetail> PostPackageDetails { get; set; }

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
            entity.HasKey(e => e.AccommodationId).HasName("PK__Accommod__004EC32534A7989D");

            entity.ToTable("Accommodation");

            entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DepositAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("deposit_amount");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
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
                .IsUnicode(false)
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

        modelBuilder.Entity<AccommodationDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PK__Accommod__38E9A22478829C21");

            entity.HasIndex(e => e.AccommodationId, "UQ__Accommod__004EC324968F56D0").IsUnique();

            entity.Property(e => e.DetailId).HasColumnName("detail_id");
            entity.Property(e => e.AccommodationId).HasColumnName("accommodation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FurnitureStatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("furniture_status");
            entity.Property(e => e.HasAirConditioner)
                .HasDefaultValue(false)
                .HasColumnName("has_air_conditioner");
            entity.Property(e => e.HasElevator)
                .HasDefaultValue(false)
                .HasColumnName("has_elevator");
            entity.Property(e => e.HasKitchen)
                .HasDefaultValue(false)
                .HasColumnName("has_kitchen");
            entity.Property(e => e.HasParking)
                .HasDefaultValue(false)
                .HasColumnName("has_parking");
            entity.Property(e => e.HasSecurity)
                .HasDefaultValue(false)
                .HasColumnName("has_security");
            entity.Property(e => e.HasWashingMachine)
                .HasDefaultValue(false)
                .HasColumnName("has_washing_machine");
            entity.Property(e => e.IsPetsAllowed)
                .HasDefaultValue(false)
                .HasColumnName("is_pets_allowed");
            entity.Property(e => e.IsSmokingAllowed)
                .HasDefaultValue(false)
                .HasColumnName("is_smoking_allowed");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Accommodation).WithOne(p => p.AccommodationDetail)
                .HasForeignKey<AccommodationDetail>(d => d.AccommodationId)
                .HasConstraintName("FK_AccommodationDetails_Accommodation");
        });

        modelBuilder.Entity<AccommodationImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__Accommod__DC9AC955FD058F48");

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
            entity.HasKey(e => e.TypeId).HasName("PK__Accommod__2C0005989B9744E1");

            entity.ToTable("AccommodationType");

            entity.HasIndex(e => e.TypeName, "UQ__Accommod__543C4FD9BB60CC91").IsUnique();

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__46A222CDBA680711");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Email, "UQ__Account__AB6E6164D843FD9C").IsUnique();

            entity.HasIndex(e => e.AuthProviderId, "UQ__Account__C82CBBE94C800F1E").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__Account__F3DBC572FAD6F03F").IsUnique();

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

        modelBuilder.Entity<LandLordVerification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PK__LandLord__24F1796962EC3E5B");

            entity.ToTable("LandLordVerification");

            entity.HasIndex(e => e.AccountId, "UQ__LandLord__46A222CCF90629EA").IsUnique();

            entity.Property(e => e.VerificationId).HasColumnName("verification_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.IdCardBackUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("id_card_back_url");
            entity.Property(e => e.IdCardFrontUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("id_card_front_url");
            entity.Property(e => e.IdCardNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("id_card_number");
            entity.Property(e => e.RejectionReason)
                .HasColumnType("text")
                .HasColumnName("rejection_reason");
            entity.Property(e => e.Status)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("P")
                .IsFixedLength()
                .HasColumnName("status");
            entity.Property(e => e.SubmittedAt)
                .HasColumnType("datetime")
                .HasColumnName("submitted_at");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.VerifiedAt)
                .HasColumnType("datetime")
                .HasColumnName("verified_at");
            entity.Property(e => e.VerifiedBy).HasColumnName("verified_by");

            entity.HasOne(d => d.Account).WithOne(p => p.LandLordVerificationAccount)
                .HasForeignKey<LandLordVerification>(d => d.AccountId)
                .HasConstraintName("FK_LandlordVerification_Account");

            entity.HasOne(d => d.VerifiedByNavigation).WithMany(p => p.LandLordVerificationVerifiedByNavigations)
                .HasForeignKey(d => d.VerifiedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_VerifiedBy_Admin");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__ED1FC9EAA1D6AAAA");

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
            entity.Property(e => e.Purpose)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("purpose");
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
        });

        modelBuilder.Entity<PaymentItem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__PaymentI__52020FDDFA59A333");

            entity.ToTable("PaymentItem");

            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.ItemType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("item_type");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");

            entity.HasOne(d => d.Payment).WithMany(p => p.PaymentItems)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PaymentItem_Payment");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(e => e.MethodId).HasName("PK__PaymentM__747727B623CB92E1");

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
                .IsUnicode(false)
                .HasColumnName("method_name");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Post__3ED787663C62E720");

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
            entity.HasKey(e => e.ApprovalId).HasName("PK__PostAppr__C94AE61A87E95352");

            entity.Property(e => e.ApprovalId).HasColumnName("approval_id");
            entity.Property(e => e.ApprovedByAccountId).HasColumnName("approved_by_account_id");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .IsUnicode(false)
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

        modelBuilder.Entity<PostPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__PostPack__63846AE857A8F4E1");

            entity.ToTable("PostPackage");

            entity.Property(e => e.PackageId).HasColumnName("package_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DurationDays).HasColumnName("duration_days");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsFeatured)
                .HasDefaultValue(false)
                .HasColumnName("is_featured");
            entity.Property(e => e.IsPriority)
                .HasDefaultValue(false)
                .HasColumnName("is_priority");
            entity.Property(e => e.PackageName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("package_name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PostPackageDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PostPack__3213E83F728A4CB3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActualPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("actual_price");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.PostPackageId).HasColumnName("post_package_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Post).WithMany(p => p.PostPackageDetails)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_PostPackageDetails_Post");

            entity.HasOne(d => d.PostPackage).WithMany(p => p.PostPackageDetails)
                .HasForeignKey(d => d.PostPackageId)
                .HasConstraintName("FK_PostPackageDetails_Package");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__UserProf__AEBB701FC5ECE6C4");

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.AccountId, "UQ__UserProf__46A222CC435524D9").IsUnique();

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
                .IsUnicode(false)
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
