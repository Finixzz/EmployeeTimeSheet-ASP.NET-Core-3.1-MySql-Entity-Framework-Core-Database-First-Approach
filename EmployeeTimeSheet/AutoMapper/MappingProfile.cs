using AutoMapper;
using DAL.DTOS;
using DAL.Models;
using EmployeeTimeSheet.ViewModels;
using EmployeeTimeSheet.ViewModels.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeTimeSheet.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserViewModel, UserViewModelDTO>().ReverseMap();

            CreateMap<UserViewModelDTO, EditUserViewModel>().ReverseMap();

            CreateMap<CreateProjectViewModel, Project>().ReverseMap();

            CreateMap<CreateActivityViewModel, Activity>().ReverseMap();

            CreateMap<AddProjectWorkingHoursViewModel, Projectinfo>().ReverseMap();

        }
    }
}
