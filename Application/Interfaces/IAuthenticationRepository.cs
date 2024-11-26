using Application.Classes;
using Application.DataTransferObjects.Authentication;

namespace Application.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken);

    Task<Result> RegisterToApplicationAsync(SignUpRequestDto  signUpRequestDto, CancellationToken cancellationToken);

    Task<Result> RegisterUserAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken);

    Task<Result> EmailConfirmationAsync(ConfirmEmailRequestDto confirmEmailRequestDto);

    Task<Result> ResendConfirmationEmailAsync(EmailConfirmationRequestDto emailConfirmationRequestDto);

    Task<Result> InviteUserAsync(InviteUserDto inviteUserDto, CancellationToken cancellationToken);

    Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

    Task<Result> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);

}