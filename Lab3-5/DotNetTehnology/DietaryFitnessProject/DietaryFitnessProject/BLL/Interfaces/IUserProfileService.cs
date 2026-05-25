using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Interfaces;

public interface IUserProfileService
{
    Task<User?> GetCurrentProfileAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> UpdateCurrentProfileAsync(int userId, User profile, CancellationToken cancellationToken = default);
}
