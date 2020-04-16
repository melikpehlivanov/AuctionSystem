namespace AuctionSystem.Worker.Runner.Models
{
    using System;

    public class ItemDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool IsEmailSent { get; set; }

        public decimal WinnerAmount { get; set; }

        public string UserEmail { get; set; }

        public string UserFullName { get; set; }
    }
}
