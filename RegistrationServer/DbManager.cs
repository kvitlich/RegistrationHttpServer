using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationServer
{
    public class DbManager
    {
        public async Task AddUser(User newUser)
        {
            using (RegistrationDbContext context = new RegistrationDbContext())
            {
                context.Users.Add(newUser);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> IfExistsByPhoneNumber(string phoneNumber)
        {
            bool result;
            using (RegistrationDbContext context = new RegistrationDbContext())
            {
                if (context.Users.Where(x => x.PhoneNumber.Equals(phoneNumber)).ToList().Count > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            return result;
        }


        public async Task<bool> IfExistsByNickname(string nickName)
        {
            bool result;
            using (RegistrationDbContext context = new RegistrationDbContext())
            {
                if (context.Users.Where(x => x.PhoneNumber.Equals(nickName)).ToList().Count > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            return result;
        }
    }
}
