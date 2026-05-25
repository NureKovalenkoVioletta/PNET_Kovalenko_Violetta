using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject.DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task UpdateLastLoginAtUtcAsync(int userId, DateTime? lastLoginAtUtc, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (user is null)
        {
            return;
        }

        user.LastLoginAtUtc = lastLoginAtUtc;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
