using System.Text.Json.Serialization;

namespace Copilot.MessageHandling.Models;

/// <summary>
/// Represents the model for the WhatsApp webhook.
/// </summary>
public class WhatsappWebhookModel
{
    [JsonPropertyName("typeWebhook")] public TypeWebhook TypeWebhook { get; set; }

    [JsonPropertyName("instanceData")] public InstanceData? InstanceData { get; set; }

    [JsonPropertyName("idMessage")] public string? IdMessage { get; set; }

    [JsonPropertyName("timestamp")] public long Timestamp { get; set; }

    [JsonPropertyName("senderData")] public SenderData? SenderData { get; set; }

    [JsonPropertyName("messageData")] public MessageData? MessageData { get; set; }

    [JsonPropertyName("status")] public string? Status { get; set; }

    [JsonPropertyName("chatId")] public string? ChatId { get; set; }

    [JsonPropertyName("from")] public string? From { get; set; }

    public string? GetTextMessage()
    {
        if (TypeWebhook == TypeWebhook.IncomingCall)
            return "INCOMING CALL";

        if (MessageData is null)
            return string.Empty;

        return MessageData.TypeMessage switch
        {
            TypeMessage.AudioMessage => MessageData.FileMessageData?.DownloadUrl,
            TypeMessage.DocumentMessage => MessageData.FileMessageData?.DownloadUrl,
            TypeMessage.ImageMessage =>
                $"{MessageData.FileMessageData?.DownloadUrl} - {MessageData.FileMessageData?.Caption}",
            TypeMessage.QuotedMessage => MessageData.ExtendedTextMessageData?.Text,
            TypeMessage.TextMessage => MessageData.TextMessageData?.TextMessage,
            TypeMessage.VideoMessage =>
                $"{MessageData.FileMessageData?.DownloadUrl} - {MessageData.FileMessageData?.Caption}",
            TypeMessage.ExtendedTextMessage => MessageData.ExtendedTextMessageData?.Text,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

public class InstanceData
{
    [JsonPropertyName("idInstance")] public long IdInstance { get; set; }

    [JsonPropertyName("wid")] public string? Wid { get; set; }

    [JsonPropertyName("typeInstance")] public string? TypeInstance { get; set; }
}

public class SenderData
{
    [JsonPropertyName("chatId")] public string? ChatId { get; set; }

    [JsonPropertyName("sender")] public string? Sender { get; set; }

    [JsonPropertyName("senderName")] public string? SenderName { get; set; }
}

public enum TypeWebhook
{
    [JsonPropertyName("incomingMessageReceived")]
    IncomingMessageReceived,

    [JsonPropertyName("outgoingMessageReceived")]
    OutgoingMessageReceived,

    [JsonPropertyName("incomingCall")] IncomingCall,

    [JsonPropertyName("outgoingMessageStatus")]
    OutgoingMessageStatus,

    [JsonPropertyName("stateInstanceChanged")]
    StateInstanceChanged,

    [JsonPropertyName("deviceInfo")] DeviceInfo,

    [JsonPropertyName("statusInstanceChanged")]
    StatusInstanceChanged,

    [JsonPropertyName("outgoingAPIMessageReceived")]
    // ReSharper disable once InconsistentNaming
    OutgoingAPIMessageReceived
}