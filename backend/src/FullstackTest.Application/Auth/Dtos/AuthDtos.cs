namespace FullstackTest.Application.Auth.Dtos;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string Token, string Email);
