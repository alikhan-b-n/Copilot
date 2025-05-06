using System.Text.Json.Serialization;

namespace Copilot.MessageHandling.Models;

public enum TypeMessage
{
    [JsonPropertyName("textMessage")] TextMessage,

    [JsonPropertyName("extendedTextMessage")] ExtendedTextMessage,

    [JsonPropertyName("imageMessage")] ImageMessage,

    [JsonPropertyName("videoMessage")] VideoMessage,

    [JsonPropertyName("documentMessage")] DocumentMessage,

    [JsonPropertyName("audioMessage")] AudioMessage,

    [JsonPropertyName("quotedMessage")] QuotedMessage,
}