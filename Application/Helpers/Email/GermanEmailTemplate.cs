using Application.Classes;
using Application.Interfaces;

namespace Application.Helpers.Email;

public class GermanEmailTemplate : IEmailTemplate
{
    public EmailContent ConfirmEmail(string userName, string callBack)
    {
        var emailContent = new EmailContent()
        {
            Subject = "Bitte bestätigen Sie Ihre E-Mail-Adresse",
            Content = $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f9f9f9;
                        color: #333333;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 100%;
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        background-color: #ffffff;
                        border: 1px solid #dddddd;
                    }}
                    .header {{
                        text-align: center;
                        padding: 10px 0;
                        background-color: #4CAF50;
                        color: #ffffff;
                    }}
                    .content {{
                        padding: 20px;
                    }}
                    .button {{
                        display: inline-block;
                        padding: 15px 25px;
                        background-color: #4CAF50;
                        color: white;
                        text-align: center;
                        text-decoration: none;
                        font-size: 16px;
                        border-radius: 5px;
                        margin: 20px 0;
                    }}
                    .footer {{
                        text-align: center;
                        padding: 10px;
                        font-size: 12px;
                        color: #999999;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Willkommen, {userName}!</h1>
                    </div>
                    <div class='content'>
                        <p>Vielen Dank, dass Sie sich für <strong>Last War Playermanager</strong> registriert haben.</p>
                        <p>Bitte bestätigen Sie Ihre E-Mail-Adresse, indem Sie auf den folgenden Button klicken. Diese Bestätigung ist nur 2 Stunden gültig:</p>
                        <a href='{callBack}' class='button'>E-Mail bestätigen</a>
                    </div>
                    <div class='footer'>
                        <p>Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.</p>
                    </div>
                </div>
            </body>
        </html>"
        };

        return emailContent;
    }

    public EmailContent ResendConfirmationEmail(string userName, string callBack)
    {
        var emailContent = new EmailContent()
        {
            Subject = "Erneute Bestätigung Ihrer E-Mail-Adresse",
            Content = $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f9f9f9;
                        color: #333333;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 100%;
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        background-color: #ffffff;
                        border: 1px solid #dddddd;
                    }}
                    .header {{
                        text-align: center;
                        padding: 10px 0;
                        background-color: #FFA500;
                        color: #ffffff;
                    }}
                    .content {{
                        padding: 20px;
                    }}
                    .button {{
                        display: inline-block;
                        padding: 15px 25px;
                        background-color: #FFA500;
                        color: white;
                        text-align: center;
                        text-decoration: none;
                        font-size: 16px;
                        border-radius: 5px;
                        margin: 20px 0;
                    }}
                    .footer {{
                        text-align: center;
                        padding: 10px;
                        font-size: 12px;
                        color: #999999;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Hallo, {userName}!</h1>
                    </div>
                    <div class='content'>
                        <p>Sie haben eine erneute Bestätigung Ihrer E-Mail-Adresse angefordert.</p>
                        <p>Bitte bestätigen Sie Ihre E-Mail-Adresse, indem Sie auf den folgenden Button klicken. Diese Bestätigung ist nur 2 Stunden gültig:</p>
                        <a href='{callBack}' class='button'>E-Mail bestätigen</a>
                    </div>
                    <div class='footer'>
                        <p>Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.</p>
                    </div>
                </div>
            </body>
        </html>"
        };

        return emailContent;
    }

    public EmailContent InviteUserEmail(string invitingUserName, string allianceName, string callBack)
    {
        var emailContent = new EmailContent()
        {
            Subject = "Einladung zum Last War Playermanager",
            Content = $@"
        <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f9f9f9;
                        color: #333333;
                        margin: 0;
                        padding: 0;
                    }}
                    .container {{
                        width: 100%;
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        background-color: #ffffff;
                        border: 1px solid #dddddd;
                    }}
                    .header {{
                        text-align: center;
                        padding: 10px 0;
                        background-color: #FFA500;
                        color: #ffffff;
                    }}
                    .content {{
                        padding: 20px;
                    }}
                    .button {{
                        display: inline-block;
                        padding: 15px 25px;
                        background-color: #FFA500;
                        color: white;
                        text-align: center;
                        text-decoration: none;
                        font-size: 16px;
                        border-radius: 5px;
                        margin: 20px 0;
                    }}
                    .footer {{
                        text-align: center;
                        padding: 10px;
                        font-size: 12px;
                        color: #999999;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Einladung zum Last War Playermanager</h1>
                    </div>
                    <div class='content'>
                        <p>{invitingUserName} lädt Sie ein, dem Last War Playermanager beizutreten.</p>
                        <p>Ihre Allianz, <strong>{allianceName}</strong>, freut sich auf Ihre Teilnahme! Klicken Sie auf den folgenden Button, um der Einladung zu folgen:</p>
                        <a href='{callBack}' class='button'>Jetzt beitreten</a>
                    </div>
                    <div class='footer'>
                        <p>Diese E-Mail wurde automatisch generiert. Bitte antworten Sie nicht auf diese E-Mail.</p>
                    </div>
                </div>
            </body>
        </html>"
        };

        return emailContent;
    }

    public EmailContent ResetPasswordEmail(string userName, string callBack)
    {
        var emailContent = new EmailContent()
        {
            Subject = "Passwort-Zurücksetzung - Last War Playermanager",
            Content = $@"
    <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f9f9f9;
                    color: #333333;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #ffffff;
                    border: 1px solid #dddddd;
                }}
                .header {{
                    text-align: center;
                    padding: 10px 0;
                    background-color: #FFA500;
                    color: #ffffff;
                }}
                .content {{
                    padding: 20px;
                }}
                .button {{
                    display: inline-block;
                    padding: 15px 25px;
                    background-color: #FFA500;
                    color: white;
                    text-align: center;
                    text-decoration: none;
                    font-size: 16px;
                    border-radius: 5px;
                    margin: 20px 0;
                }}
                .footer {{
                    text-align: center;
                    padding: 10px;
                    font-size: 12px;
                    color: #999999;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Anfrage zur Passwort-Zurücksetzung</h1>
                </div>
                <div class='content'>
                    <p>Hallo {userName},</p>
                    <p>Wir haben eine Anfrage erhalten, das Passwort für dein Last War Playermanager-Konto zurückzusetzen. Wenn du diese Anfrage nicht gestellt hast, kannst du diese E-Mail einfach ignorieren.</p>
                    <p>Um dein Passwort zurückzusetzen, klicke bitte auf den untenstehenden Button. Der Link ist für die nächsten 2 Stunden gültig:</p>
                    <a href='{callBack}' class='button'>Passwort zurücksetzen</a>
                </div>
                <div class='footer'>
                    <p>Diese E-Mail wurde automatisch generiert. Bitte antworte nicht auf diese E-Mail.</p>
                </div>
            </div>
        </body>
    </html>"
        };

        return emailContent;
    }
}
