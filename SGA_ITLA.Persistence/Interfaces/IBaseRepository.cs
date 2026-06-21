using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Persistence.Interfaces;

namespace SGA_ITLA.Persistence.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<OperationResult> SaveEntityAsync(TEntity entity);
        Task<OperationResult> UpdateEntityAsync(TEntity entity);
        Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetEntityByIdAsync(int id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    }
}