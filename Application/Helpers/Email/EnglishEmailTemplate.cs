using Application.Classes;
using Application.Interfaces;

namespace Application.Helpers.Email;

public class EnglishEmailTemplate : IEmailTemplate
{
    public EmailContent ConfirmEmail(string userName, string callBack)
    {
        var emailContent = new EmailContent()
        {
            Subject = "Please Confirm Your Email Address",
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
                        <h1>Welcome, {userName}!</h1>
                    </div>
                    <div class='content'>
                        <p>Thank you for registering with <strong>Last War Playermanager</strong>.</p>
                        <p>Please confirm your email address by clicking the button below. This confirmation is only valid for 2 hours:</p>
                        <a href='{callBack}' class='button'>Confirm Email</a>
                    </div>
                    <div class='footer'>
                        <p>This email was automatically generated. Please do not reply to this email.</p>
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
            Subject = "Resend Confirmation of Your Email Address",
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
                        <h1>Hello, {userName}!</h1>
                    </div>
                    <div class='content'>
                        <p>You have requested to resend the confirmation of your email address.</p>
                        <p>Please confirm your email address by clicking the button below. This confirmation is valid for 2 hours only:</p>
                        <a href='{callBack}' class='button'>Confirm Email</a>
                    </div>
                    <div class='footer'>
                        <p>This email was automatically generated. Please do not reply to this email.</p>
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
            Subject = "Invitation to Last War Playermanager",
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
                        <h1>Invitation to Last War Playermanager</h1>
                    </div>
                    <div class='content'>
                        <p>{invitingUserName} is inviting you to join the Last War Playermanager.</p>
                        <p>Your alliance, <strong>{allianceName}</strong>, is looking forward to your participation! Click the button below to accept the invitation:</p>
                        <a href='{callBack}' class='button'>Join Now</a>
                    </div>
                    <div class='footer'>
                        <p>This email was generated automatically. Please do not reply to this email.</p>
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
            Subject = "Password Reset - Last War Playermanager",
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
                    <h1>Password Reset Request</h1>
                </div>
                <div class='content'>
                    <p>Hello {userName},</p>
                    <p>We received a request to reset the password for your Last War Playermanager account. If you did not make this request, you can simply ignore this email.</p>
                    <p>To reset your password, please click the button below. The link is valid for the next 2 hours:</p>
                    <a href='{callBack}' class='button'>Reset Password</a>
                </div>
                <div class='footer'>
                    <p>This email was automatically generated. Please do not reply to this email.</p>
                </div>
            </div>
        </body>
    </html>"
        };

        return emailContent;

    }
}