namespace EntityFrameworkCore.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? bookPrices { get; set; }
    }
}
