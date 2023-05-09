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

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var nearbyUsers = await repository.GetAllUsers();
            UsersListView.ItemsSource = nearbyUsers;
            StartLocationUpdates();
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
        private void StartLocationUpdates()
        {
            locationTimerEnabled = true;
            Device.StartTimer(TimeSpan.FromSeconds(3), () =>
            {
                UpdateLocation();
                return locationTimerEnabled;
            });
        }

        private async void GetNearbyUsers()
        {
            var userRepo = new UserRepository();
            var nearbyUsers = await userRepo.GetUser(App.CurrentUser);
            if (nearbyUsers != null)
            {
                UsersListView.ItemsSource = new List<User> { nearbyUsers };
            }
            else
            {
                UsersListView.ItemsSource = new List<User>();
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
    }
}
