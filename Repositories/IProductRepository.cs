using System.Collections.Generic;
using System.Threading.Tasks;
using technical_tests_backend_ssr.Models.Entities;

namespace technical_tests_backend_ssr.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task SaveChangesAsync();
    }
}
