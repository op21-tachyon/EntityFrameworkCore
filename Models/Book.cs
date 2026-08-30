using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Models
{
    public class Book
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string isActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
