using Application.Classes;
using Application.Interfaces;

namespace Application.Helpers.Email;

public class ItalianEmailTemplate : IEmailTemplate
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
            Subject = "Invito al Last War Playermanager",
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
                        <h1>Invito al Last War Playermanager</h1>
                    </div>
                    <div class='content'>
                        <p>{invitingUserName} ti invita a unirti al Last War Playermanager.</p>
                        <p>La tua alleanza, <strong>{allianceName}</strong>, è in attesa della tua partecipazione! Clicca sul pulsante qui sotto per accettare l'invito:</p>
                        <a href='{callBack}' class='button'>Unisciti ora</a>
                    </div>
                    <div class='footer'>
                        <p>Questa email è stata generata automaticamente. Si prega di non rispondere a questa email.</p>
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
            Subject = "Reimpostazione della password - Last War Playermanager",
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
                    <h1>Richiesta di reimpostazione della password</h1>
                </div>
                <div class='content'>
                    <p>Ciao {userName},</p>
                    <p>Abbiamo ricevuto una richiesta di reimpostazione della password per il tuo account Last War Playermanager. Se non hai fatto questa richiesta, puoi ignorare questa email.</p>
                    <p>Per reimpostare la tua password, fai clic sul pulsante qui sotto. Il link è valido per le prossime 2 ore:</p>
                    <a href='{callBack}' class='button'>Reimposta la password</a>
                </div>
                <div class='footer'>
                    <p>Questa email è stata generata automaticamente. Per favore, non rispondere a questa email.</p>
                </div>
            </div>
        </body>
    </html>"
        };

        return emailContent;

    }
}