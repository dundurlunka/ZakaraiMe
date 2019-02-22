﻿namespace ZakaraiMe.Service.Implementations
{
    using Contracts;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CarService : BaseService<Car>, ICarService
    {
        public CarService(IBaseRepository<Car> repository) : base(repository)
        {
        }

        public override Task<bool> ForeignPropertiesExistAsync(Car item)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<Car>> GetFilteredItemsAsync(User currentUser)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsItemDuplicate(Car item)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsUserAuthorized(Car item, User currentUser)
        {
            throw new System.NotImplementedException();
        }        
    }
}
