using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.BLL.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(User user, string password, CancellationToken cancellationToken = default);
    Task<User?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
