using System.ComponentModel;
using Calabonga.UnitOfWork;
using Copilot.AI.Plugins.Interfaces;
using Copilot.Infrastructure.Entities;
using Copilot.WhatsApp.Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;

namespace Copilot.AI.Plugins.Plugins;

public class OperatorPlugin(IFlowSellApi flowSellApi, IUnitOfWork unitOfWork) : ICopilotPlugin
{
    private OperatorPluginParameter _parameter = new();

    public class OperatorPluginParameter : ICopilotPlugin.ParameterBase
    {
        public IEnumerable<string> NumberList { get; set; }

        public Guid WhatsAppAccountId { get; set; }
    }

    public void SetParameters<T>(T parameters) where T : ICopilotPlugin.ParameterBase
    {
        _parameter = (parameters as OperatorPluginParameter)!;
    }

    [KernelFunction, Description("Execute when user asks to call operator, e.g. call operator.")]
    public async Task<string> CallOperator()
    {
        await Task.CompletedTask;
        var whatsAppAccount = await unitOfWork
            .GetRepository<WhatsAppAccount>()
            .GetAll(disableTracking: true)
            .FirstOrDefaultAsync(x => x.Id == _parameter.WhatsAppAccountId);

        foreach (var number in _parameter.NumberList)
        {
            await flowSellApi.SendMessage(
                idInstance: whatsAppAccount!.IdInstance,
                apiTokenInstance: whatsAppAccount.ApiTokenInstance,
                chatId: number,
                message: MessageTemplate.Replace("{{phone}}", _parameter.Dialogue.PhoneNumber)
            );
        }

        return "Okay, operator will come soon.";
    }

    private const string MessageTemplate =
        """
        🚨 Operator Alert! 🚨

        📞 Client Requesting Assistance

        Client Number: wa.me/{{phone}}
        """;
}