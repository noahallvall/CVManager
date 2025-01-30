using System.ComponentModel.DataAnnotations;

namespace CVManager.WebApplication.Models
{
    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }
}
