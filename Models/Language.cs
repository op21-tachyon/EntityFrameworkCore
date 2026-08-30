using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Models
{
    public class Language
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
