using AcademicTrack.Application.Auth.DTOs;
using AcademicTrack.Application.Auth.Interfaces;
using AcademicTrack.Domain.Entities;
using AcademicTrack.Domain.Exceptions;
using AcademicTrack.Domain.Repositories;

namespace AcademicTrack.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtService _jwtService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.UsernameOrEmail) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Debe ingresar el usuario/correo y la contraseña.");
        }

        var user = await _userRepository.GetByUsernameOrEmailAsync(request.UsernameOrEmail);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("La cuenta de usuario está desactivada.");
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        var (token, expiration) = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Expiration = expiration,
            User = MapToDto(user)
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Trim().Length < 3)
        {
            throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres.");
        }

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
        {
            throw new ArgumentException("Debe proporcionar un correo electrónico válido.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
        {
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");
        }

        if (await _userRepository.ExistsUsernameAsync(request.Username))
        {
            throw new InvalidOperationException("El nombre de usuario ya se encuentra registrado.");
        }

        if (await _userRepository.ExistsEmailAsync(request.Email))
        {
            throw new InvalidOperationException("El correo electrónico ya se encuentra registrado.");
        }

        var user = new User
        {
            Username = request.Username.Trim(),
            Email = request.Email.Trim().ToLowerInvariant(),
            FullName = string.IsNullOrWhiteSpace(request.FullName) ? request.Username.Trim() : request.FullName.Trim(),
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = string.IsNullOrWhiteSpace(request.Role) ? "Docente" : request.Role.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var (token, expiration) = _jwtService.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Expiration = expiration,
            User = MapToDto(user)
        };
    }

    public async Task<UserDto> GetCurrentUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
        {
            throw new KeyNotFoundException("Usuario no encontrado o inactivo.");
        }

        return MapToDto(user);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}
