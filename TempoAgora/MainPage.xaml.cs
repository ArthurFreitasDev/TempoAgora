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

        private async void ObterPrevisao(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(cidade))
                {
                    Tempo? previsao = await DataService.GetPrevisaodoTempo(cidade);

                    string dados_previsao = "";

                    if (previsao != null)
                    {
                        dados_previsao = $"Humidade: {previsao.Humidity} \n" +
                                         $"Nascer do Sol: {previsao.Sunrise} \n " +
                                         $"Pôr do Sol: {previsao.Sunset} \n" +
                                         $"Temperatura: {previsao.Temperature} \n" +
                                         $"Titulo: {previsao.Title} \n" +
                                         $"Visibilidade: {previsao.Visibility} \n" +
                                         $"Vento: {previsao.Wind} \n" +
                                         $"Previsão: {previsao.Weather} \n" +
                                         $"Descrição: {previsao.WeatherDescription} \n";

                        HistoricoTempo a = new HistoricoTempo()
                        {
                            Humidity = previsao.Humidity,
                            Sunrise = previsao.Sunrise,
                            Sunset = previsao.Sunset,
                            Temperature = previsao.Temperature,
                            Title = previsao.Title,
                            Visibility = previsao.Visibility,
                            Wind = previsao.Wind,
                            Weather = previsao.Weather,
                            WeatherDescription = previsao.WeatherDescription
                        };
                        await App.DbTempo.InsertTempo(a);
                    }
                    else
                    {
                        dados_previsao = $"Sem dados, previsão nula.";
                    }

                    Debug.WriteLine("-------------------------------------------");
                    Debug.WriteLine(dados_previsao);
                    Debug.WriteLine("-------------------------------------------");

                    lbl_previsao.Text = dados_previsao;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro ", ex.Message, "OK");
            }
        }

        private async void HistoricoPrevisao(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoricoPrevisoes());
        }
    }

}
