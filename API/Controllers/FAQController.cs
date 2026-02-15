using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FAQController : ControllerBase
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FAQDto>>> GetAllFAQs()
        {
            var faqs = await _faqService.GetAllFAQsAsync();
            return Ok(faqs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FAQDto>> GetFAQById(Guid id)
        {
            var faq = await _faqService.GetFAQByIdAsync(id);
            if (faq == null)
                return NotFound(new { message = "FAQ not found" });

            return Ok(faq);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<FAQDto>> CreateFAQ(CreateFAQDto dto)
        {
          
            var faq = await _faqService.CreateFAQAsync(dto);
            return CreatedAtAction(nameof(GetFAQById), new { id = faq.Id }, faq);
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<FAQDto>> UpdateFAQ(Guid id, UpdateFAQDto dto)
        {
            var faq = await _faqService.UpdateFAQAsync(id, dto);
            return Ok(faq);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFAQ(Guid id)
        {
            await _faqService.DeleteFAQAsync(id);
            return NoContent();
            
            
        }
    }
}
