using Evernet.WebApi.Data;
using Evernet.WebApi.DTOs;
using Evernet.WebApi.Entities;
using Evernet.WebApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evernet.WebApi.Services;

public class AuthService(
    IUserRepository userRepository,
    IEmailService emailService,
    ICodeGenerator codeGenerator,
    EvernetDbContext context,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator)
    : IAuthService
{
    public async Task RegisterAsync(RegisterDto dto)
    {
        var existingUser = await userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
            throw new Exception("User with this email already exists.");

        var user = new User
        {
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsActive = false,
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        var code = codeGenerator.GenerateNumericCode();

        var verificationCode = new VerificationCode
        {
            UserId = user.Id,
            CodeHash = BCrypt.Net.BCrypt.HashPassword(code),
            Expiration = DateTime.UtcNow.AddMinutes(10),
            User = user
        };

        context.VerificationCodes.Add(verificationCode);
        await context.SaveChangesAsync();

        await emailService.SendAsync(user.Email, "Code de vérification", $"Votre code est : {code}");
    }

    public async Task VerifyCodeAsync(VerifyCodeDto dto)
    {
        var user = await userRepository.GetByIdAsync(dto.UserId);
        if (user == null)
            throw new Exception("User not found.");

        if (user.IsActive)
            throw new Exception("User is already active.");

        var verificationCode = await context.VerificationCodes
            .Where(v => v.UserId == dto.UserId && v.Expiration >= DateTime.UtcNow)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync();

        if (verificationCode == null)
            throw new Exception("Verification code not found or expired.");

        var isCodeValid = BCrypt.Net.BCrypt.Verify(dto.Code, verificationCode.CodeHash);
        if (!isCodeValid)
            throw new Exception("Invalid code.");

        user.IsActive = true;

        context.VerificationCodes.Remove(verificationCode);
        await userRepository.SaveChangesAsync();
    }

    public async Task ResendCodeAsync(ResendCodeDto dto)
    {
        var user = await userRepository.GetByIdAsync(dto.UserId);
        if (user is null)
            throw new Exception("User not found.");

        if (user.IsActive)
            throw new Exception("User is already active.");

        var lastCode = await context.VerificationCodes
            .Where(v => v.UserId == dto.UserId)
            .OrderByDescending(v => v.CreatedAt)
            .FirstOrDefaultAsync();

        if (lastCode is not null && lastCode.CreatedAt > DateTime.UtcNow.AddMinutes(-1))
            throw new Exception("Please wait a minute before requesting a new code.");

        if (lastCode is not null)
            context.VerificationCodes.Remove(lastCode);

        var newCode = codeGenerator.GenerateNumericCode();
        var codeEntity = new VerificationCode
        {
            UserId = user.Id,
            CodeHash = BCrypt.Net.BCrypt.HashPassword(newCode),
            Expiration = DateTime.UtcNow.AddMinutes(10),
            CreatedAt = DateTime.UtcNow,
            User = user
        };

        context.VerificationCodes.Add(codeEntity);
        await context.SaveChangesAsync();

        await emailService.SendAsync(user.Email,
            "Nouveau code de vérification",
            $"Voici votre nouveau code : {newCode}");
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new Exception("Invalid email or password.");

        if (!user.IsActive)
            throw new Exception("Account not verified.");

        var accessToken = jwtTokenGenerator.GenerateToken(user.Id, user.Email);

        var refreshTokenString = refreshTokenGenerator.Generate();
        var refreshToken = new RefreshToken
        {
            Token = refreshTokenString,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            User = user
        };

        context.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        return new LoginResponseDto(accessToken, refreshTokenString);
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        var stored = await context.RefreshTokens
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);

        if (stored is null || stored.IsRevoked)
            throw new Exception("Invalid refresh token.");

        var user = stored.User;

        stored.RevokedAt = DateTime.UtcNow;

        var newRefreshString = refreshTokenGenerator.Generate();
        var newRefresh = new RefreshToken
        {
            Token = newRefreshString,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            User = user
        };

        context.RefreshTokens.Add(newRefresh);

        var newAccess = jwtTokenGenerator.GenerateToken(user.Id, user.Email);

        await context.SaveChangesAsync();

        return new LoginResponseDto(newAccess, newRefreshString);
    }

    public async Task LogoutAsync(LogoutRequestDto dto)
    {
        var stored = await context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);

        if (stored is null || stored.IsRevoked)
            throw new Exception("Refresh token invalid or already revoked.");

        stored.RevokedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task RequestResetPasswordAsync(string email)
    {
        var user = await userRepository.GetByEmailAsync(email);
        if (user == null)
            throw new Exception("User not found.");

        var token = Guid.NewGuid().ToString();

        var resetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            User = user
        };

        context.PasswordResetTokens.Add(resetToken);
        await context.SaveChangesAsync();

        var resetPasswordLink = $"http://localhost:3000/reset-password?token={token}";

        await emailService.SendAsync(user.Email, "Réinitialisation de mot de passe",
            $"Cliquez sur ce lien pour réinitialiser votre mot de passe : {resetPasswordLink}");
    }

    public async Task ResetPasswordAsync(ResetPasswordDto dto)
    {
        var resetToken = await context.PasswordResetTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                t.Token == dto.Token &&
                t.ExpiresAt > DateTime.UtcNow);

        if (resetToken == null)
            throw new Exception("Invalid or expired password reset token.");

        resetToken.User.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

        context.PasswordResetTokens.Remove(resetToken);

        await context.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        var user = await userRepository.GetByIdAsync(userId)
                   ?? throw new Exception("User not found.");

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
            throw new Exception("Incorrect current password.");

        if (dto.CurrentPassword == dto.NewPassword)
            throw new Exception("The new password cannot be the same as the current password.");

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await userRepository.SaveChangesAsync();
    }
}