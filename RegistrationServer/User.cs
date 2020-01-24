using System;
using System.Collections.Generic;
using System.Text;

namespace RegistrationServer
{
    public class User : Entity
    {
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
    }
}
