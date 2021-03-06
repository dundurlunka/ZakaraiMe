﻿namespace ZakaraiMe.Web.Models.Cars
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Entities.Implementations;

    public class CarListViewModel : ListViewModel, IMapFrom<Car>
    {
        public string Colour { get; set; }

        public virtual Model Model { get; set; }

        public int OwnerId { get; set; }

        public User Owner { get; set; }

        public string PictureFileName { get; set; }
    }
}
