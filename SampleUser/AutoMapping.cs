using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.ViewModels;

namespace SampleUser
{
    public static class AutoMapping
    {
        public static void AddMapping(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            { 
                cfg.CreateMap<UserViewModel, User>();
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
