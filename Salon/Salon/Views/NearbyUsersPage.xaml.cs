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
        // 111 meters
        private Double MAX_RANGE = 0.01;
        //int i = 1;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var nearbyUsers = await repository.GetAllUsers();
            UsersListView.ItemsSource = nearbyUsers;
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
                await GetNearbyUsers();
                UpdateLocation();
            }
        }
        private void OnGamesClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new StartScreen();
        }
        private void OnMessageClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }

        public async Task<List<User>> GetNearbyUsers()
        {
            try
            {
                var myLocation = await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync();
                var allUsers = await repository.GetAllUsers();
                var nearbyUsers = allUsers.Where(u => u.UserLocation.CalculateDistance(myLocation, DistanceUnits.Kilometers) <= MAX_RANGE).ToList();

                return nearbyUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting nearby users: {ex.Message}");
                return null;
            }
        }

        /*private async void GetNearbyUsers()
        {
            Double LatDiff;
            Double LongDiff;
            var userRepo = new UserRepository();
            var currentUser = await userRepo.GetUser(App.CurrentUser);
            if (currentUser != null)
            {
                LocationLbl.Text = ""+i;

                var currentLat = currentUser.UserLocation.Latitude;
                var currentLong = currentUser.UserLocation.Longitude;

                var usersList = new List<User>();

                if (UsersListView.ItemsSource != null)
                {
                    usersList = ((List<User>)UsersListView.ItemsSource).ToList();
                }

                //remove users from list that are not within range
                foreach (User person in UsersListView.ItemsSource)
                {
                    LatDiff = person.UserLocation.Latitude - currentLat;
                    LongDiff = person.UserLocation.Longitude - currentLong;

                    usersList.Remove(currentUser);
                    if (Math.Abs(LatDiff) > MAX_RANGE
                        && Math.Abs(LongDiff) > MAX_RANGE)
                    {
                        usersList.Remove(person);
                    }
                }
                UsersListView.ItemsSource = usersList;
            }
            else
            {
                LocationLbl.Text = "Must be logged in!!!";
            }
            i++;
        }*/

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
    }
}
