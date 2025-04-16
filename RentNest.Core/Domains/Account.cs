using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.Domains
{
    public class Account
    {
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Password { get; set; }
        public int? AccountRole { get; set; } // 1: Staff, 0: Member
    }
}
