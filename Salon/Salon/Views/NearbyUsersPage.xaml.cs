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
        CancellationTokenSource cts;
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
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    LocationLbl.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}"; 
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}\n");
                }

                if (location == null)
                {
                    LocationLbl.Text = "No GPS";
                }
                else 
                {
                    LocationLbl.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                }
                await Task.Delay(5000);

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
