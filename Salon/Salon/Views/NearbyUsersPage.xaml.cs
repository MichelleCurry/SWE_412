using Salon.Model;
using Salon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Salon
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NearbyUsersPage : ContentPage
    {
        UserRepository repository = new UserRepository();
        public NearbyUsersPage()
        {
            InitializeComponent();
        }

        private bool locationTimerEnabled = true;

        // 111 meters, 0.111 km
        private Double MAX_RANGE = 0.111;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await StartLocationUpdates();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            StopLocationUpdates();
        }
        private void StopLocationUpdates()
        {
            locationTimerEnabled = false;
        }

        // uses timer to get current location every few seconds
        private async Task StartLocationUpdates()
        {
            locationTimerEnabled = true;
            while (locationTimerEnabled)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                UsersListView.ItemsSource = await GetNearbyUsers();
                UpdateLocation();
            }
        }
        public async Task<List<User>> GetNearbyUsers()
        {
            try
            {
                var myLocation = await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync();
                var nearbyUsers = await repository.GetAllUsers();
                var usersWithinRange = new List<User>();
                foreach (var user in nearbyUsers)
                {
                    Console.WriteLine($"-------User: {user.Username} is {Location.CalculateDistance(myLocation, user.UserLocation, DistanceUnits.Kilometers)} km away");
                    if (Location.CalculateDistance(myLocation, user.UserLocation, DistanceUnits.Kilometers) <= MAX_RANGE)
                    {
                        Console.WriteLine($"-------User: {user.Username} is {Location.CalculateDistance(myLocation, user.UserLocation, DistanceUnits.Kilometers)} km away");
                        usersWithinRange.Add(user);
                    }
                    else
                    {
                        Console.WriteLine($"*****User: {user.Username} is too far away");
                    }
                }

                Console.WriteLine("+++++++There are " + usersWithinRange.Count + "People nearby");
                return usersWithinRange;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting nearby users: {ex.Message}");
                return null;
            }
        }

        // updates saved location with current user location
        private async void UpdateLocation()
        {
            try
            {
                //get location
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    //LocationLbl.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}\n");

                    // get current user and update location
                    var currentUser = await repository.GetUser(App.CurrentUser);

                    if (App.CurrentUser != null)
                    {
                        currentUser.UserLocation = location;
                        await repository.UpdateUser(currentUser);
                    }
                    /*else
                    {
                        LocationLbl.Text = "Current user is null";
                    }*/

                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                LocationLbl.Text = ("GPS not supported: \n" + fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                LocationLbl.Text = ("GPS not enabled: \n" + fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                LocationLbl.Text = ("Turn on you location permissions for Salon: \n" + pEx.Message);
            }
            catch (Exception ex)
            {
                LocationLbl.Text = ("location not found:\n" + ex.Message);
            }

        }
        public void Back_Clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new LoginPage();
            //await Navigation.PushAsync(new NearbyUsersPage());
        }
        private void OnGamesClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartScreen();
        }
        private void OnMessageClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}