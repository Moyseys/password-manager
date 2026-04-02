namespace SharedDto.Messages;

using SharedDto.Enums;

public record EmailMessage(
    string To,
    string Subject,
    TemplateEnum TemplateName,
    Dictionary<string, string> TemplateData,
    bool IsHtml = false
);