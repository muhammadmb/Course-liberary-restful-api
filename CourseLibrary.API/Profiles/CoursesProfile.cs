using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Profiles
{
    public class CoursesProfile : Profile
    {
        public CoursesProfile()
        {
            CreateMap<Entities.Course, Models.CourseDto>();
            CreateMap<Models.CourseCreationDto,Entities.Course>();
            CreateMap<Models.courseUpdateDto, Entities.Course>();
            CreateMap<Entities.Course, Models.courseUpdateDto>();
        }
    }
}
