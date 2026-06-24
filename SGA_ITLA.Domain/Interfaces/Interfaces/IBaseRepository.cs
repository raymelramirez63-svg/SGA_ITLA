using System.Collections.Generic;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;

namespace SGA_ITLA.Domain.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> SaveEntityAsync(TEntity entity);
        Task<OperationResult> UpdateEntityAsync(TEntity entity);
        Task<OperationResult> DeleteEntityAsync(TEntity entity);
    }
}