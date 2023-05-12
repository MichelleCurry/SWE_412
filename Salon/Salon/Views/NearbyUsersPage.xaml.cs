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
        private UserRepository repository;
        public NearbyUsersPage()
        {
            InitializeComponent();
            repository = new UserRepository();
        }

        private bool locationTimerEnabled = true;

        private const Double MAX_RANGE = 0.15;        // 150 meters, 0.15 km
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Run(async () =>
            {
                while (locationTimerEnabled)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    var nearbyUsers = await GetNearbyUsers();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UsersListView.ItemsSource = nearbyUsers;
                    });
                    UpdateLocation();
                }
            });
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
        public async Task<List<User>> GetNearbyUsers()
        {
            try
            {
                var myLocation = await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync();
                var allUsers = new List<User>();
                try
                {
                    allUsers = await repository.GetAllUsers();
                    var usersWithinRange = new List<User>();
                    if (allUsers == null) // add null check here
                    {
                        return new List<User>();
                    }
                    for (int i = 0; i < allUsers.Count; i++)
                    {   
                        try
                        {
                            if (Location.CalculateDistance(myLocation, allUsers[i].UserLocation, DistanceUnits.Kilometers) <= MAX_RANGE)
                            {
                                usersWithinRange.Add(allUsers[i]);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Users does not have location details saved");
                        }                        
                    }
                    return usersWithinRange;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting nearby users: {ex.Message}");
                    return null;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location: {ex.Message}");
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

                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                ErrorLbl.Text = ("GPS not supported: \n" + fnsEx.Message);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                ErrorLbl.Text = ("GPS not enabled: \n" + fneEx.Message);
            }
            catch (PermissionException pEx)
            {
                ErrorLbl.Text = ("Turn on you location permissions for Salon: \n" + pEx.Message);
            }
            catch (Exception ex)
            {
                ErrorLbl.Text = ("location not found:\n" + ex.Message);
            }

        }
        public void Back_Clicked(object sender, EventArgs args)
        {
            App.Current.MainPage = new LoginPage();
            //await Navigation.PushAsync(new NearbyUsersPage());
        }
        private void OnGamesClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new Games.Tic_Tac_Toe.StartPage();
        }
        private void OnMessageClicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}