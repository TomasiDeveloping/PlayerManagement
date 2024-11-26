using Application.Classes;
using Application.Interfaces;

namespace Application.Helpers.Email;

public class FrenchEmailTemplate : IEmailTemplate
{
    public EmailContent ConfirmEmail(string userName, string callBack)
    {
        throw new NotImplementedException();
    }

    public EmailContent ResendConfirmationEmail(string userName, string callBack)
    {
        throw new NotImplementedException();
    }

    public EmailContent InviteUserEmail(string invitingUserName, string allianceName, string callBack)
    {
        var emailContent = new EmailContent()
        {
            Subject = "Invitation au Last War Playermanager",
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
                        <h1>Invitation au Last War Playermanager</h1>
                    </div>
                    <div class='content'>
                        <p>{invitingUserName} vous invite à rejoindre le Last War Playermanager.</p>
                        <p>Votre alliance, <strong>{allianceName}</strong>, attend avec impatience votre participation ! Cliquez sur le bouton ci-dessous pour accepter l'invitation :</p>
                        <a href='{callBack}' class='button'>Rejoindre maintenant</a>
                    </div>
                    <div class='footer'>
                        <p>Ce courriel a été généré automatiquement. Veuillez ne pas répondre à ce courriel.</p>
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
            Subject = "Réinitialisation du mot de passe - Last War Playermanager",
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
                    <h1>Demande de réinitialisation du mot de passe</h1>
                </div>
                <div class='content'>
                    <p>Bonjour {userName},</p>
                    <p>Nous avons reçu une demande de réinitialisation du mot de passe pour votre compte Last War Playermanager. Si vous n'avez pas fait cette demande, vous pouvez ignorer cet e-mail.</p>
                    <p>Pour réinitialiser votre mot de passe, veuillez cliquer sur le bouton ci-dessous. Le lien est valable pour les 2 prochaines heures :</p>
                    <a href='{callBack}' class='button'>Réinitialiser le mot de passe</a>
                </div>
                <div class='footer'>
                    <p>Cet e-mail a été généré automatiquement. Veuillez ne pas répondre à cet e-mail.</p>
                </div>
            </div>
        </body>
    </html>"
        };

        return emailContent;

    }
}