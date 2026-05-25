using System.Security.Cryptography;
using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace DietaryFitnessProject.BLL.Services;

public class AuthService : IAuthService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<User> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            _logger.LogWarning("Спроба реєстрації: не вказано електронну пошту.");
            throw new InvalidOperationException("Вкажіть електронну пошту.");
        }

        var email = user.Email.Trim();

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            _logger.LogWarning(
                "Спроба реєстрації: пароль не відповідає вимогам. Email={Email}",
                email);
            throw new InvalidOperationException("Пароль має містити щонайменше 6 символів.");
        }

        var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (existingUser is not null)
        {
            _logger.LogWarning(
                "Спроба реєстрації: користувач із такою поштою вже існує. Email={Email}",
                email);
            throw new InvalidOperationException("Користувач із такою електронною поштою вже існує.");
        }

        var saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
        user.Email = email;
        user.PasswordSalt = Convert.ToBase64String(saltBytes);
        user.PasswordHash = HashPassword(password, saltBytes);
        user.CreatedAtUtc = DateTime.UtcNow;
        user.LastLoginAtUtc = null;

        await _userRepository.AddAsync(user, cancellationToken);

        _logger.LogInformation(
            "Користувача зареєстровано. UserId={UserId}, Email={Email}, Ім'я={FirstName} {LastName}",
            user.UserId,
            user.Email,
            user.FirstName,
            user.LastName);

        return user;
    }

    public async Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            _logger.LogWarning(
                "Спроба входу: порожня пошта або пароль. EmailProvided={HasEmail}",
                !string.IsNullOrWhiteSpace(email));
            return null;
        }

        var normalizedEmail = email.Trim();

        var user = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
        if (user is null || string.IsNullOrWhiteSpace(user.PasswordSalt) || string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            _logger.LogWarning(
                "Невдалий вхід: користувача не знайдено або обліковий запис без пароля. Email={Email}",
                normalizedEmail);
            return null;
        }

        var saltBytes = Convert.FromBase64String(user.PasswordSalt);
        var computedHash = HashPassword(password, saltBytes);

        if (!CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(computedHash)))
        {
            _logger.LogWarning(
                "Невдалий вхід: невірний пароль. UserId={UserId}, Email={Email}",
                user.UserId,
                normalizedEmail);
            return null;
        }

        var now = DateTime.UtcNow;
        await _userRepository.UpdateLastLoginAtUtcAsync(user.UserId, now, cancellationToken);
        user.LastLoginAtUtc = now;

        _logger.LogInformation(
            "Успішний вхід. UserId={UserId}, Email={Email}",
            user.UserId,
            user.Email);

        return user;
    }

    private static string HashPassword(string password, byte[] saltBytes)
    {
        var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        return Convert.ToBase64String(hashBytes);
    }
}
