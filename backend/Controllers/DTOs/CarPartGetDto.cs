using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CarPartGetDto : CarPartDto
    {
        public string Id { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
