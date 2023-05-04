using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Essentials;

namespace Salon.Services
{
    class LocationTracking
    {
        private CancellationTokenSource cts;

        // returns current user location
        public async Task<Location> GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                return location;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex+"location not found");
                return null;
            }

        }
    }
}
