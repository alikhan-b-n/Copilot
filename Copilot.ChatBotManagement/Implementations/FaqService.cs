using Calabonga.UnitOfWork;
using Copilot.ChatBotManagement.Interfaces;
using Copilot.Contracts.Parameters;
using Copilot.Contracts.Responses;
using Copilot.Infrastructure;
using Copilot.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Copilot.ChatBotManagement.Implementations;

public class FaqService(IUnitOfWork<ApplicationContext> unitOfWork) : IFaqService
{
    public async Task<List<FaqResponse>> GetAllFaqs(Guid userId)
    {
        var faqs = await unitOfWork
            .GetRepository<Faq>()
            .GetAll(disableTracking: true)
            .Where(x => x.UserId == userId)
            .Select(x=>new FaqResponse
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                UserId=x.UserId
            })
            .ToListAsync();

        return faqs;
    }

    public async Task<FaqResponse> GetFaqById(Guid userId, Guid id)
    {
        var faq = await unitOfWork
            .GetRepository<Faq>()
            .GetAll(disableTracking: true)
            .Where(x => x.UserId == userId && x.Id == id)
            .Select(x => new FaqResponse
            {
                Id = x.Id,
                Question = x.Question,
                Answer = x.Answer,
                UserId=x.UserId
            })
            .FirstOrDefaultAsync();

        return faq ?? throw new ArgumentException($"$Faq is not found. UserId: {userId}, Id: {id}");
    }

    public async Task<FaqResponse> CreateFaq(Guid userId, FaqRequest faqRequest)
    {
        var faqEntity = new Faq
        {
            Question = faqRequest.Question,
            Answer = faqRequest.Answer,
            UserId = userId
        };

        await unitOfWork.GetRepository<Faq>().InsertAsync(faqEntity);
        await unitOfWork.SaveChangesAsync();

        return MapToResponse(faqEntity);
    }

    public async Task<FaqResponse> UpdateFaq(Guid id, Guid userId, FaqRequest faqRequest)
    {
        var existingFaq = await unitOfWork
            .GetRepository<Faq>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == id && x.UserId == userId);

        if (existingFaq == null)
            throw new ArgumentException(nameof(existingFaq));

        existingFaq.Question = faqRequest.Question ?? existingFaq.Question;
        existingFaq.Answer = faqRequest.Answer ?? existingFaq.Answer;

        await unitOfWork.SaveChangesAsync();
        
        return MapToResponse(existingFaq);
    }

    public async Task DeleteFaq(Guid id, Guid userId)
    {
        var faq = await unitOfWork
            .GetRepository<Faq>()
            .GetFirstOrDefaultAsync(predicate: x => x.Id == id && x.UserId == userId);

        if (faq == null)
            throw new ArgumentException(nameof(id));
        
        unitOfWork.GetRepository<Faq>().Delete(faq.Id);

        await unitOfWork.SaveChangesAsync();
    }
    
    private FaqResponse MapToResponse(Faq faqEntity)
    {
        return new FaqResponse()
        {
            Id = faqEntity.Id,
            Question = faqEntity.Question,
            Answer = faqEntity.Answer,
            UserId = faqEntity.UserId
        };
    }
}