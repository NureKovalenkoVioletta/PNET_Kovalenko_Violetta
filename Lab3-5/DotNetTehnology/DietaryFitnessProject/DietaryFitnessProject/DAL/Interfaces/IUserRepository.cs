using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.DAL.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task UpdateLastLoginAtUtcAsync(int userId, DateTime? lastLoginAtUtc, CancellationToken cancellationToken = default);
}
