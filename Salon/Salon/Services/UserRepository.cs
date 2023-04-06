using Firebase.Database;
using Newtonsoft.Json;
using Salon.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Salon.Services
{
    internal class UserRepository
    {
        // new data
        FirebaseClient firebaseClient = new FirebaseClient("https://salon-ff877-default-rtdb.firebaseio.com/");
        
        //honestly not sure
        //best guess is it returns T/F waits for call on RegPage then adds user object to data base
        public async Task<bool> Save(User user)
        {
        var data = await firebaseClient.Child(nameof(User)).PostAsync(JsonConvert.SerializeObject(user));
        if (!string.IsNullOrEmpty(data.Key))
        {
            return true;
        }
            return false;
        }

    }
}
