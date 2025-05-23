﻿using RentNest.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Interfaces
{
    public interface ITimeUnitPackageService
    {
        Task<IEnumerable<TimeUnitPackage>> GetAll();
    }
}
