using Copilot.Infrastructure.Entities;

namespace Copilot.Infrastructure;

public static class DefaultDataConstants
{
    public static readonly GptModel Gpt35TurboModel = new()
    {
        Id = Guid.Parse("2A6E2029-64EF-464A-83AC-80F20200FD38"),
        CreationDate = new DateTimeOffset(2023, 12, 9, 10, 0, 0, TimeSpan.FromHours(6)),
        Name = "gpt-3.5-turbo"
    };
}