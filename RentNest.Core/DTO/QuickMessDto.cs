using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class QuickMessDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Content { get; set; }
        public string TargetRole { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
