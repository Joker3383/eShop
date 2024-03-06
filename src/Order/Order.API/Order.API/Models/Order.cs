namespace Order.API.Models
{
    public class Order : IEntityWithSubId<int>
    {

        public int Id { get; set; }
        public int SubId { get; set; }
        public double TotalSum { get; set; }
        public DateTime DateOfOrder { get; set; }
        public int BasketId { get; set; }
    }
}

