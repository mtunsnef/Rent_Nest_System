using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
using RentNest.Service.Interfaces;

namespace RentNest.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly IAzureOpenAIService _azureOpenAIService;

        public PostsController(IAzureOpenAIService azureOpenAIService)
        {
            _azureOpenAIService = azureOpenAIService;
        }
        [Route("/Post")]
        public async Task<IActionResult> Post()
        {
            return View("User/Post");
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePostWithAI([FromBody] PostDataAIDto model)
        {
            var content = await _azureOpenAIService.GenerateDataPostAsync(model);
            return Ok(new { content });
        }

    }

}
