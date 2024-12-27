using AutoMapper;
using DataAccessLayer.Models;
using DataAccessLayer.Models.MyInMemoryApi.Models;
using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer;
using ServicesLayer.Contracts;
using System.ComponentModel.DataAnnotations;

namespace ServicesLayer.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<ResultModel<User>> SignUpAsync(UserViewModel userViewModel)
        {
            try
            {
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(userViewModel);
                bool isValid = Validator.TryValidateObject(userViewModel, validationContext, validationResults);

                if (!isValid)
                {
                    var errors = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                    return new ResultModel<User> { Success = false, Message = errors };
                }

                if (await _context.Users.AnyAsync(u => u.UserName == userViewModel.UserName))
                {
                    return new ResultModel<User> { Success = false, Message = "نام کاربری قبلا استفاده شده است." };
                }

                var user = _mapper.Map<User>(userViewModel);
                user.Id = Guid.NewGuid();
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = passwordHash;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                user.Password = "";

                return new ResultModel<User> { Success = true, Message = "کاربر با موفقیت ثبت شد.", Data = user };
            }
            catch (Exception ex)
            {
                return new ResultModel<User> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ResultModel<User>> LoginAsync(UserViewModel userViewModel)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == userViewModel.UserName);

                if (user == null || !BCrypt.Net.BCrypt.Verify(userViewModel.Password, user.Password))
                {
                    return new ResultModel<User> { Success = false, Message = "نام کاربری یا رمز عبور نادرست است." };
                }

                user.Password = "";

                return new ResultModel<User> { Success = true, Message = "ورود موفق‌.", Data = user };
            }
            catch (Exception ex)
            {
                return new ResultModel<User> { Success = false, Message = ex.Message };
            }
        }




    }
}