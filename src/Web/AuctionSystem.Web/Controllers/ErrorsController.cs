namespace AuctionSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorsController : BaseController
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            return this.View();
        }
        
        [Route("error/403")]
        public IActionResult Error403()
        {
            return this.View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            return this.View();
        }
    }
}