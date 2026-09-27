using AcademicTrack.Domain.Entities;

namespace AcademicTrack.Application.Auth.Interfaces;

public interface IJwtService
{
    (string Token, DateTime Expiration) GenerateToken(User user);
}
