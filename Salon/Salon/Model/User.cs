using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Salon.Model
{
    public class User
    {
        //global user id?
        public int Id { get; set; }

        //display name?
        public string Name { get; set; }
        // user model
        
        //set and get user information
        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        //probably shouldn't have the password stored locally

        public Location UserLocation { get; set; }

        public override string ToString()
        {
            return "User: " + Username;
        }
    }
}
