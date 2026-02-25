using Microsoft.EntityFrameworkCore;
using server.DAL.Interface;
using server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.DAL
{
    public class UserDAL : IUserDAL
    {
        private readonly ChineseOrderContext chineseOrderContext;

        public UserDAL(ChineseOrderContext chineseOrderContext)
        {
            this.chineseOrderContext = chineseOrderContext;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await chineseOrderContext.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<User> Create(User user)
        {
            await chineseOrderContext.Users.AddAsync(user);

            try
            {
                await chineseOrderContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Error saving changes: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                throw new Exception("לא ניתן להוסיף את המשתמש. ייתכן שהאימייל כבר קיים במערכת.", ex);
            }

            return user;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await chineseOrderContext.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
                return false;

            chineseOrderContext.Users.Remove(user);
            await chineseOrderContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAll()
        {
            return await chineseOrderContext.Users.ToListAsync();
        }
    }
}
