using Xamarin.Essentials;

namespace Salon
{
    internal interface ILocationReceiver
    {
        void ReceiveLocation(Location location);
    }
}