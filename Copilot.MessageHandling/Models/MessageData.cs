using System.Text.Json.Serialization;

namespace Copilot.MessageHandling.Models;

/// <summary>
/// Represents the message data received in a webhook.
/// </summary>
public class MessageData
{
    [JsonPropertyName("typeMessage")] public TypeMessage TypeMessage { get; set; }

    [JsonPropertyName("textMessageData")] public TextMessageData? TextMessageData { get; set; }

    [JsonPropertyName("extendedTextMessageData")]
    public ExtendedTextMessageData? ExtendedTextMessageData { get; set; }

    [JsonPropertyName("fileMessageData")] public FileMessageData? FileMessageData { get; set; }

    [JsonPropertyName("templateButtonReplyMessage")]
    public TemplateButtonReplyMessage? TemplateButtonReplyMessage { get; set; }

    [JsonPropertyName("buttonsResponseMessage")]
    public ButtonsResponseMessage? ButtonsResponseMessage { get; set; }

    [JsonPropertyName("listResponseMessage")]
    public ListResponseMessage? ListResponseMessage { get; set; }

    [JsonPropertyName("buttonsMessage")] public ButtonsMessage? ButtonsMessage { get; set; }
}

public class TextMessageData
{
    [JsonPropertyName("textMessage")] public string? TextMessage { get; set; }
}

public class ExtendedTextMessageData
{
    [JsonPropertyName("text")] public string? Text { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("jpegThumbnail")] public string? JpegThumbnail { get; set; }
}

public class FileMessageData
{
    [JsonPropertyName("downloadUrl")] public string? DownloadUrl { get; set; }

    [JsonPropertyName("caption")] public string? Caption { get; set; }
}

public class TemplateButtonReplyMessage
{
    [JsonPropertyName("stanzaId")] public string? StanzaId { get; set; }

    [JsonPropertyName("selectedIndex")] public int SelectedIndex { get; set; }

    [JsonPropertyName("selectedId")] public string? SelectedId { get; set; }

    [JsonPropertyName("selectedDisplayText")]
    public string? SelectedDisplayText { get; set; }
}

public class ButtonsResponseMessage
{
    [JsonPropertyName("stanzaId")] public string? StanzaId { get; set; }

    [JsonPropertyName("selectedButtonId")] public string? SelectedButtonId { get; set; }

    [JsonPropertyName("selectedButtonText")]
    public string? SelectedButtonText { get; set; }
}

public class ListResponseMessage
{
    [JsonPropertyName("stanzaId")] public string? StanzaId { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("listType")] public int ListType { get; set; }

    [JsonPropertyName("singleSelectReply")]
    public string? SingleSelectReply { get; set; }
}

public class ButtonsMessage
{
    [JsonPropertyName("contentText")] public string? ContentText { get; set; }

    public List<Button>? Buttons { get; set; }
}

public class Button
{
    public string? ButtonId { get; set; }
    public string? ButtonText { get; set; }
}