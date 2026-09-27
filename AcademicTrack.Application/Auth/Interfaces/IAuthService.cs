using AcademicTrack.Application.Auth.DTOs;

namespace AcademicTrack.Application.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<UserDto> GetCurrentUserAsync(int userId);
}
