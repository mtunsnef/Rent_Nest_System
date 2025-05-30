﻿using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Infrastructure.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllPostsWithAccommodation();
        Task<int?> GetAccommodationIdByPostId(int postId);

	}
}
