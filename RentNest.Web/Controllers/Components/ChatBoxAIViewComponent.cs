using Microsoft.AspNetCore.Mvc;

namespace RentNest.Web.Controllers.Components
{
    public class ChatBoxAIViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
