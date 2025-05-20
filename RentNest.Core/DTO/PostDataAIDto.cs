using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RentNest.Core.DTO
{
    public class PostDataAIDto
    {
        public string? Address { get; set; }
        public string? Category { get; set; }

        [JsonPropertyName("category_text")]
        public string? CategoryText { get; set; } 
        public string? Area { get; set; }
        public string? Price { get; set; }
        public string? FurnitureStatus { get; set; }
        public string? NumberBedroom { get; set; }
        public string? NumberBathroom { get; set; }
        public string? ContactName { get; set; }
        public string? ContactNumber { get; set; }
        public string? SelectedAmenities { get; set; }
        public string? AiStyle { get; set; }

        [JsonPropertyName("selectedAmenities_text")]
        public string? SelectedAmenityNames { get; set; }
    }

}
