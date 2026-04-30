using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CarPartDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part name is missing")]
        public string Name
        {
            get => field ?? string.Empty;
            set => field = value?.Trim() ?? string.Empty;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Part number is missing")]
        public string PartNumber
        {
            get => field ?? string.Empty;
            set => field = value?.Trim() ?? string.Empty;
        }

        public string Description
        {
            get => field ?? string.Empty;
            set => field = value?.Trim() ?? string.Empty;
        }
    }
}
