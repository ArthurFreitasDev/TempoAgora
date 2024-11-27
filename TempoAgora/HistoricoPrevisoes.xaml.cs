using System.Collections.ObjectModel;
using TempoAgora.Models;
namespace TempoAgora;

public partial class HistoricoPrevisoes : ContentPage
{
    ObservableCollection<HistoricoTempo> lista_tempo = new ObservableCollection<HistoricoTempo>();
    public HistoricoPrevisoes()
	{
		InitializeComponent();
        lst_tempo.ItemsSource = lista_tempo;
    }

    protected async override void OnAppearing()
    {
        lista_tempo.Clear();

        var allPrevisoes = await App.DbTempo.GetAllTempo();
        foreach (HistoricoTempo i in allPrevisoes)
        {  
            lista_tempo.Add(i);
        }
    }
}