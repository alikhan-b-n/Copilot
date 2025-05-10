using Copilot.Api.Extentions;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Copilot.Api.Controllers;

[Controller]

public class FaqController : ControllerBase
{
    private readonly IFaqService _faqService;

    public FaqController(IFaqService faqService)
    {
        _faqService = faqService;
    }
    
    [HttpGet("api/faqs")]
    public async Task<IActionResult> GetAll()
    {
        var faqs = await _faqService.GetAllFaqs(User.GetId());

        return Ok(faqs);
    }
    
    [HttpGet("api/faqs/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var faq = await _faqService.GetFaqById(User.GetId(), id);

        return Ok(faq);
    }
    
    [HttpPost("api/faqs/")]
    public async Task<IActionResult> Create([FromBody] FaqRequest faqRequest)
    {
        var createdFaq = await _faqService.CreateFaq(User.GetId(), faqRequest);
        
        return Ok(createdFaq);
    }
    
    [HttpPut("api/faqs/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FaqRequest faqRequest)
    {
        var updatedFaq = await _faqService.UpdateFaq(id, User.GetId(), faqRequest);

        return Ok(updatedFaq);  
    }

    [HttpDelete("api/faqs/{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await _faqService.DeleteFaq(id, User.GetId());

        return NoContent();
    }
}