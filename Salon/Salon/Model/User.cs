using System;
using System.Collections.Generic;
using System.Text;

namespace Salon.Model
{
    internal class User
    {
        //global user id?
        public int Id { get; set; }

        //display name?
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        //probably shouldn't have the password stored locally

        public string Email { get; set; }
    }
}
