using TempoAgora.Models;
using TempoAgora.Service;
using System.Diagnostics;

namespace TempoAgora
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource _cancelTokenSource;
        bool _isCheckinLocation;

        string? cidade;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ObterLocalizacao(object sender, EventArgs e)
        {
            try
            {
                _cancelTokenSource = new CancellationTokenSource();

                GeolocationRequest request =
                    new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                Location? location= await Geolocation.Default.GetLocationAsync(
                    request, _cancelTokenSource.Token);
                
                if(location != null)
                {
                    lbl_latitude.Text = location.Latitude.ToString();
                    lbl_longitude.Text = location.Longitude.ToString();

                    Debug.WriteLine("---------------------------------------");
                    Debug.WriteLine(location);
                    Debug.WriteLine("---------------------------------------");
                }
            }
            catch(FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não Suporta", fnsEx.Message, "OK");
            }
            catch(FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro: Localização Desabilitada", fneEx.Message, "OK");
            }
            catch(PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "OK");
            }
        }

        private async Task<string> GetGeocodeReverseData(
            double latitude = 47.673988, double longitude = -122.121513)
        {
            IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(
                latitude, longitude);

            Placemark? placemark = placemarks?.FirstOrDefault();

            Debug.WriteLine("-----------------------------------------");
            Debug.WriteLine(placemark?.Locality);
            Debug.WriteLine("-----------------------------------------");

            if(placemark != null)
            {
                cidade = placemark.Locality;

                return
                    $"AdminArea: {placemark.AdminArea}\n" +
                    $"CountryCode: {placemark.CountryCode}\n" +
                    $"CountryName: {placemark.CountryName}\n" +
                    $"FeatureName: {placemark.FeatureName}\n" +
                    $"Locality: {placemark.Locality}\n" +
                    $"PostalCode: {placemark.PostalCode}\n" +
                    $"SubAdminArea: {placemark.SubAdminArea}" +
                    $"SubLocality: {placemark.SubLocality}\n" +
                    $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                    $"Thoroughfare: {placemark.Thoroughfare}\n";
            }
            return "nada";
        }

        private async void ObterPlacemark(object sender, EventArgs e)
        {
            double latitude = Convert.ToDouble(lbl_latitude.Text),
                longitude = Convert.ToDouble(lbl_longitude.Text);

            lbl_reverso.Text = await GetGeocodeReverseData(latitude, longitude);
        }

        private void ObterPrevisao(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch(Exception ex)
            {

            }
        }
    }

}
