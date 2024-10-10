using Application.Classes;
using Application.DataTransferObjects.Authentication;

namespace Application.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken);

    Task<Result<LoginResponseDto>> RegisterToApplicationAsync(RegisterRequestDto  registerRequestDto, CancellationToken cancellationToken);
}