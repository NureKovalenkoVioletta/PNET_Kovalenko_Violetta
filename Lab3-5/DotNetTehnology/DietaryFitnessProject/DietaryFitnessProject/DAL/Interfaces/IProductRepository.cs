using DietaryFitnessProject.DAL.Entities;

namespace DietaryFitnessProject.DAL.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<IReadOnlyList<Product>> GetByIdsAsync(
        IReadOnlyCollection<int> productIds,
        CancellationToken cancellationToken = default);
}
