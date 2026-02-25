namespace server.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
        public int Quantity { get; set; }
        public bool IsDraft { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Purchase()
        {
            IsDraft = true;
            PurchaseDate = DateTime.Now;
        }

    }
}
