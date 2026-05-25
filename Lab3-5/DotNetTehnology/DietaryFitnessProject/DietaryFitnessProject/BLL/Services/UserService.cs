using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.DAL.Entities;
using DietaryFitnessProject.DAL.Interfaces;

namespace DietaryFitnessProject.BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default)
        => _userRepository.GetAllAsync(cancellationToken);

    public Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _userRepository.GetByIdAsync(id, cancellationToken);

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => _userRepository.GetByEmailAsync(email, cancellationToken);

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
        => _userRepository.AddAsync(user, cancellationToken);

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        => _userRepository.UpdateAsync(user, cancellationToken);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        => _userRepository.DeleteAsync(id, cancellationToken);
}
