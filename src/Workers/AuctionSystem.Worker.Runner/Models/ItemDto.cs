namespace AuctionSystem.Worker.Runner.Models
{
    public class ItemDto
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public bool IsEmailSent { get; set; }

        public decimal WinnerAmount { get; set; }
    }
}
