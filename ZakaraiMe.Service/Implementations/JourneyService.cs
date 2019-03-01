﻿namespace ZakaraiMe.Service.Implementations
{
    using Contracts;
    using Data.Entities.Implementations;
    using Data.Repositories.Contracts;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ZakaraiMe.Common;

    public class JourneyService : BaseService<Journey>, IJourneyService
    {
        private readonly ICarService carService;
        private readonly UserManager<User> userManager;

        public JourneyService(IJourneyRepository repository, ICarService carService, UserManager<User> userManager) : base(repository)
        {
            this.carService = carService;
            this.userManager = userManager;
        }

        public override async Task<bool> ForeignPropertiesExistAsync(Journey item)
        {
            Car car = await carService.GetByIdAsync(item.CarId);

            if (car == null)
                return false;

            return true;
        }

        public override async Task<IEnumerable<Journey>> GetFilteredItemsAsync(User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return GetAll();

            return GetAll(j => j.DriverId == currentUser.Id || j.Passengers.Any(p => p.UserId == currentUser.Id));
        }

        public override bool IsItemDuplicate(Journey item)
        {
            return GetAll(j => j.CarId == item.CarId
                            && j.DriverId == item.DriverId
                            && j.DateTime == item.DateTime)
                    .Any();
        }

        public override async Task<bool> IsUserAuthorizedAsync(Journey item, User currentUser)
        {
            if (await userManager.IsInRoleAsync(currentUser, CommonConstants.AdministratorRole))
                return true;
            
            return item.DriverId == currentUser.Id;
        }
    }
}