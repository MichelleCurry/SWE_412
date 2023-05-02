using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Salon.Services
{
    public class UserRepository

    {
        // initialize the entry point for the firebase database
        private readonly FirebaseClient firebase = new FirebaseClient("https://salon-ff877-default-rtdb.firebaseio.com/",
            new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult("N5McCfKNM9bATDjl3NNOYuMboIexN6SlnqPGDuXA")
            });

        // Register a new user
        public async Task<bool> RegisterUser(Model.User user)
        {
            try
            {
                var users = firebase.Child("users");

                // Check if email already exists
                var existingUser = await users
                    .OrderBy("Email")
                    .EqualTo(user.Email)
                    .OnceAsync<Model.User>();

                if (existingUser.Count > 0)
                {
                    return false;
                }

                users = firebase.Child("users");

                // Check if username already exists
                var existingUsername = await users
                    .OrderBy("Username")
                    .EqualTo(user.Username)
                    .OnceAsync<Model.User>();

                if (existingUsername.Count > 0)
                {
                    return false;
                }
                else
                {
                    await users.PostAsync(user);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error registering user: {ex.Message}");
                return false;
            }
        }


        public async Task<Model.User> Login(string lUsername, string lPassword)
        {
            try
            {
                // Retrieve the user with the specified username
                var user = (await firebase
                    .Child("users")
                    .OrderBy("Username")
                    .EqualTo(lUsername)
                    .OnceAsync<Model.User>())
                    .FirstOrDefault()
                    .Object;

                if (user == null)
                {
                    return null;
                }

                // Verify the password
                if (user.Password != lPassword)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error logging in: {ex.Message}");
                return null;
            }
        }

    }
}