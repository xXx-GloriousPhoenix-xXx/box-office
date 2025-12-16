using AutoMapper;
using BoxOffice.BLL.DTOs.GenreDtos;
using BoxOffice.DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxOffice.BLL.Mapping
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<CreateGenreDto, Genre>();
            CreateMap<Genre, GetGenreDto>();
        }
    }
}
