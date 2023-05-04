using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Salon.Model
{
    public class User
    {
        // user model
        //set and get user information
        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public Location UserLocation { get; set; }
    }
}
