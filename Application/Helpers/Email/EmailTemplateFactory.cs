using Application.Interfaces;

namespace Application.Helpers.Email;

public static class EmailTemplateFactory
{
    public static IEmailTemplate GetEmailTemplate(string languageCode)
    {
        return languageCode switch
        {
            "en" => new EnglishEmailTemplate(),
            "de" => new GermanEmailTemplate(),
            "fr" => new FrenchEmailTemplate(),
            "it" => new ItalianEmailTemplate(),
            _ => new EnglishEmailTemplate()
        };
    }
}