using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Infrastructure.DataAccess;
using RentNest.Infrastructure.Repositories.Interfaces;
using RentNest.Core.UtilHelper;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using RentNest.Core.Enums;


namespace RentNest.Infrastructure.Repositories.Implements
{
    public class PostRepository : IPostRepository
    {
        private readonly PostDAO _postDAO;
        private readonly AccommodationDAO _accommodationDAO;
        private readonly AccommodationDetailDAO _accommodationDetailDAO;
        private readonly AccommodationImageDAO _accommodationImageDAO;
        private readonly AccommodationAmenityDAO _accommodationAmenityDAO;
        private readonly PostPackageDetailDAO _postPackageDetailDAO;
        public PostRepository(PostDAO postDAO, AccommodationDAO accommodationDAO, AccommodationDetailDAO accommodationDetailDAO, AccommodationImageDAO accommodationImageDAO, AccommodationAmenityDAO accommodationAmenityDAO, PostPackageDetailDAO postPackageDetailDAO)
        {
            _postDAO = postDAO;
            _accommodationDAO = accommodationDAO;
            _accommodationDetailDAO = accommodationDetailDAO;
            _accommodationImageDAO = accommodationImageDAO;
            _accommodationAmenityDAO = accommodationAmenityDAO;
            _postPackageDetailDAO = postPackageDetailDAO;
        }

        public async Task<List<Post>> GetAllPostsByUserAsync(int accountId)
        {
            return await _postDAO.GetAllPostsByUserAsync(accountId);
        }

        public async Task<List<Post>> GetAllPostsWithAccommodation()
        {
            return await _postDAO.GetAllPostsWithAccommodation();
        }

        public async Task<Post?> GetPostDetailWithAccommodationDetailAsync(int postId)
        {
            return await _postDAO.GetPostDetailWithAccommodationDetailAsync(postId);
        }

        public async Task<int> SavePost(LandlordPostDto dto)
        {
            using var transaction = await _postDAO.BeginTransactionAsync();

            try
            {
                var addressParts = (dto.Address ?? "").Split(',').Select(p => p.Trim()).ToArray();

                string streetAddress = addressParts.ElementAtOrDefault(0) ?? "";
                string wardName = addressParts.ElementAtOrDefault(1) ?? "";
                string districtName = addressParts.ElementAtOrDefault(2) ?? "";
                string provinceName = addressParts.ElementAtOrDefault(3) ?? "";

                var accommodation = new Accommodation
                {
                    Title = dto.AccommodationDescription ?? "",
                    Description = dto.AccommodationDescription ?? "",
                    Address = streetAddress,
                    WardName = wardName,
                    DistrictName = districtName,
                    ProvinceName = provinceName,
                    Price = dto.Price,
                    Area = dto.Area,
                    TypeId = dto.AccommodationTypeId,
                    OwnerId = dto.OwnerId ?? 0,
                };
                await _accommodationDAO.AddAsync(accommodation);

                var detail = new AccommodationDetail
                {
                    AccommodationId = accommodation.AccommodationId,
                    FurnitureStatus = dto.FurnitureStatus,
                    BedroomCount = dto.NumberBedroom ?? 0,
                    BathroomCount = dto.NumberBathroom ?? 0,
                    HasAirConditioner = dto.HasAirConditioner ?? false,
                    HasKitchenCabinet = dto.HasKitchenCabinet ?? false,
                    HasLoft = dto.HasLoft ?? false,
                    HasRefrigerator = dto.HasRefrigerator ?? false,
                    HasWashingMachine = dto.HasWashingMachine ?? false
                };
                await _accommodationDetailDAO.AddAsync(detail);

                if (dto.AmenityIds != null)
                {
                    foreach (var amenityId in dto.AmenityIds)
                    {
                        var aa = new AccommodationAmenity
                        {
                            AccommodationId = accommodation.AccommodationId,
                            AmenityId = amenityId
                        };
                        await _accommodationAmenityDAO.AddAsync(aa);
                    }
                }

                if (dto.Images != null)
                {
                    foreach (var image in dto.Images)
                    {
                        string imageUrl = await UploadImageAndGetUrlAsync(image);

                        var img = new AccommodationImage
                        {
                            AccommodationId = accommodation.AccommodationId,
                            ImageUrl = imageUrl,
                            Caption = null
                        };
                        await _accommodationImageDAO.AddAsync(img);
                    }
                }

                var post = new Post
                {
                    Title = dto.titlePost,
                    Content = dto.contentPost,
                    CurrentStatus = PostStatusHelper.ToDbValue(PostStatus.Unpaid),
                    PublishedAt = DateTime.Now,
                    AccountId = dto.OwnerId ?? 0,
                    AccommodationId = accommodation.AccommodationId
                };
                await _postDAO.AddAsync(post);

                var postpackageDetail = new PostPackageDetail
                {
                    PostId = post.PostId,
                    PricingId = dto.PricingId ?? 0,
                    TotalPrice = (decimal)(dto.TotalPrice),
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    PaymentStatus = PaymentStatusHelper.ToDbValue(PaymentStatus.Pending),
                    PaymentTransactionId = null,
                    CreatedAt = DateTime.Now,
                };
                await _postPackageDetailDAO.AddAsync(postpackageDetail);

                await transaction.CommitAsync();

                return post.PostId;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private async Task<string> UploadImageAndGetUrlAsync(IFormFile file)
        {
            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var uploadFolder = Path.Combine("wwwroot", "uploads");

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            var fullPath = Path.Combine(uploadFolder, filename);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);
            
            return "/uploads/" + filename;
        }
    }
}
