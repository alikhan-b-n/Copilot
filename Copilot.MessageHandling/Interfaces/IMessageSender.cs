namespace Copilot.MessageHandling.Interfaces;

public interface IMessageSender
{
    Task SendMessage(long idInstance, string chatId, string message);
}