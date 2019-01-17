﻿namespace ZakaraiMe.Service.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Data.Entities.Contracts;
    using Data.Entities.Implementations;

    public interface IBaseService<TEntity> where TEntity : class, IBaseEntity
    {
        Task InsertAsync(TEntity item);

        void Update(TEntity item);

        void Delete(TEntity item);

        Task<TEntity> GetByIdAsync(int id);

        IEnumerable<TEntity> GetAll(Func<TEntity, bool> where = null);

        bool IsUserAuthorized(TEntity item, User currentUser);

        Task<IEnumerable<TEntity>> GetFilteredItemsAsync(User currentUser);

        Task<bool> ForeignPropertiesExistAsync(TEntity item);

        Task SaveAsync();

        bool IsItemDuplicate(TEntity item);
    }
}
