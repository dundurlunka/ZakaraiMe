﻿namespace ZakaraiMe.Web.Controllers
{
    using AutoMapper;
    using Data.Entities.Implementations;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Cars;
    using Newtonsoft.Json;
    using Service.Contracts;
    using Service.Helpers;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading.Tasks;

    public class CarsController : BaseController<Car, CarFormViewModel, CarListViewModel>
    {
        private readonly IPictureService pictureService;
        private readonly ICarService carService;

        public CarsController(ICarService carService, UserManager<User> userManager, IPictureService pictureService, IMapper mapper) : base(carService, userManager, mapper)
        {
            this.pictureService = pictureService;
            this.carService = carService;
        }

        protected override string ItemName { get; set; } = "кола";

        protected override async Task<Car> GetEntityAsync(CarFormViewModel viewModel, int id)
        {
            Car car = await service.GetByIdAsync(id);

            if (car == null)
            {
                car = new Car();

                Picture carPicture = new Picture(); // Creates instance of Picture entity
                Image carPictureImage = PictureServiceHelpers.ConvertIFormFileToImage(viewModel.ImageFile); // Converts the uploaded image to System.Drawing.Image

                if (carPictureImage == null) // The uploaded file is not a picture
                {
                    return null;
                }

                bool imageInsertSuccess = await pictureService.InsertAsync(carPicture, carPictureImage); // inserts image into database and file system

                if (imageInsertSuccess) 
                {
                    car.PictureFileName = carPicture.FileName;
                }
            }

            car.Colour = viewModel.Colour;
            car.ModelId = viewModel.ModelId;
            car.OwnerId = viewModel.OwnerId;

            return car;
        }

        protected override CarFormViewModel SendFormData(Car item, CarFormViewModel viewModel)
        {
            ViewData["Makes"] = carService.GetMakesAsync().Result;

            return null;
        }

        public async Task<JsonResult> GetModels(int makeId)
        {
            IList<CarModelListViewModel> models = mapper.Map<IList<Model>, IList<CarModelListViewModel>>(await carService.GetModelsAsync(makeId));

            string json = JsonConvert.SerializeObject(models);

            return Json(models);
        }
    }
}
