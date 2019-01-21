namespace AuctionSystem.Web.Areas.Bid.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    [Area("Bid")]
    public class BidController : Controller
    {
        [Route("/bid/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            
            throw new NotImplementedException();
        }
    }
}
