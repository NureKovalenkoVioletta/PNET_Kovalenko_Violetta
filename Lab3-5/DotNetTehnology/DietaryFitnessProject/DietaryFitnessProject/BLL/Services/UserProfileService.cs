using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserRepository _userRepository;

    public UserProfileService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User?> GetCurrentProfileAsync(int userId, CancellationToken cancellationToken = default)
        => _userRepository.GetByIdAsync(userId, cancellationToken);

    public async Task<bool> UpdateCurrentProfileAsync(int userId, User profile, CancellationToken cancellationToken = default)
    {
        var existing = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        existing.FirstName = profile.FirstName;
        existing.LastName = profile.LastName;
        existing.Email = profile.Email;
        existing.Gender = profile.Gender;
        existing.Age = profile.Age;
        existing.Height = profile.Height;
        existing.Weight = profile.Weight;
        existing.ActivityLevel = profile.ActivityLevel;
        existing.Goal = profile.Goal;

        await _userRepository.UpdateAsync(existing, cancellationToken);
        return true;
    }
}
