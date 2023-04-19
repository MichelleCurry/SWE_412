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
        public NearbyUsersPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            while (true)
            { 
                await UpdateLocation();
            }
        }

        private async Task UpdateLocation()
        {
            try
            {
                    // Get location
                    var location = await Geolocation.GetLastKnownLocationAsync();
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(3));

                if (location != null)
                {
                    // Update label with location
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        location = await Geolocation.GetLocationAsync(request);
                        LocationLbl.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                        Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                    });

                    await Task.Delay(1000);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                LocationLbl.Text = ("GPS not supported");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                LocationLbl.Text = ("GPS not enabled");
            }
            catch (PermissionException pEx)
            {
                LocationLbl.Text = ("Turn on you location permissions for Salon");
            }
            catch (Exception ex)
            {
                LocationLbl.Text = ("location not found");
            }
            
        }
    }
}
