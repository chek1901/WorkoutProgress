using AWO.ViewModels.Account;
using AwoAppServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AWO.Services.GymUserServices
{
    public enum UserExceptionMsg
    {
        NameExists,
        Success,
        Error
    }

    public class GymUserService : IGymUserServices
    {
        private readonly GymadminContext _context;

        public GymUserService(GymadminContext context)
        {
            _context = context;
        }

        public async Task<UserExceptionMsg> Create(string firstName, string lastName, string email, string phone)
        {
            try
            {
                if (_context.GymUsers.Any(user => user.Email == email))
                {
                    return UserExceptionMsg.NameExists;
                }
                var newGymUser = new GymUsers()
                {
                    FirstName = firstName ?? "no info",
                    LastName = lastName ?? "no info",
                    Email = email,
                    Telephone = phone ?? "no info"
                };
                await _context.GymUsers.AddAsync(newGymUser);
                await _context.SaveChangesAsync();

                return UserExceptionMsg.Success;

            }
            catch (Exception)
            {
                return UserExceptionMsg.Error;
            }
        }

        public async Task<GymUsers> Get(int id)
        {
            return await _context.GymUsers.SingleOrDefaultAsync(user => user.GymUserId == id);
        }

        public async Task Update(ManageAccountViewModel model)
        {
            var user = _context.GymUsers.SingleOrDefault(user => user.Email == model.Email);

            user.Telephone = model?.Telephone ?? "";
            user.FirstName = model?.FirstName ?? "";
            user.LastName = model?.LastName ?? "";

            _context.GymUsers.Update(user);
            await _context.SaveChangesAsync();

        }

    }
}
