using Copilot.Infrastructure.Entities;
using Copilot.Infrastructure.Entities.Altegio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Copilot.Infrastructure;

public sealed class ApplicationContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        if (Database.IsRelational())
        {
            Database.Migrate();
        }
    }

    public DbSet<CopilotChatBot> CopilotChatBots { get; set; }
    public DbSet<Plugin> Plugins { get; set; }
    public DbSet<GptModel> GptModels { get; set; }
    public DbSet<Faq> Faqs { get; set; }
    public DbSet<InstructionFile> InstructionFiles { get; set; }
    
    // Altegio
    public DbSet<AltegioCompany> AltegioCompanies { get; set; }
    
    // WhatsApp Account
    public DbSet<WhatsAppAccount> WhatsAppAccounts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<GptModel>()
            .HasData(DefaultDataConstants.Gpt35TurboModel);

        base.OnModelCreating(builder);
    }
}