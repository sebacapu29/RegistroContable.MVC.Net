using AutoMapper;
using RegistroContable.Entities;
using RegistroContable.MVC.Models;

namespace RegistroContable.MVC.Servicios
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Cuenta, CuentaCreacionViewModel>();
        }
    }
}
