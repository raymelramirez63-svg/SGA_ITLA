using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SGA_ITLA.Domain.Base;
using SGA_ITLA.Domain.Interfaces;
using SGA_ITLA.Infraestructure.Context;

namespace SGA_ITLA.Infraestructure.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly SgaContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(SgaContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var result = new OperationResult();
            try { result.Data = await _dbSet.ToListAsync(); }
            catch (Exception ex) { result.Success = false; result.Message = ex.Message; }
            return result;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var result = new OperationResult();
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null) { result.Success = false; result.Message = "No encontrado"; return result; }
                result.Data = entity;
            }
            catch (Exception ex) { result.Success = false; result.Message = ex.Message; }
            return result;
        }

        public async Task<OperationResult> SaveEntityAsync(TEntity entity)
        {
            var result = new OperationResult();
            try
            {
                entity.CreationDate = DateTime.Now;
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                result.Data = entity;
            }
            catch (Exception ex) { result.Success = false; result.Message = ex.Message; }
            return result;
        }

        public async Task<OperationResult> UpdateEntityAsync(TEntity entity)
        {
            var result = new OperationResult();
            try
            {
                entity.ModifyDate = DateTime.Now;
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                result.Data = entity;
            }
            catch (Exception ex) { result.Success = false; result.Message = ex.Message; }
            return result;
        }

        public async Task<OperationResult> DeleteEntityAsync(TEntity entity)
        {
            var result = new OperationResult();
            try
            {
                entity.Deleted = true;
                entity.DeleteDate = DateTime.Now;
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                result.Message = "Borrado lógicamente";
            }
            catch (Exception ex) { result.Success = false; result.Message = ex.Message; }
            return result;
        }
    }
}