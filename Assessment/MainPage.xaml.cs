using System.Collections.ObjectModel;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Assessment;

public partial class MainPage : ContentPage
{
    private readonly ICaseService _caseService;
    private ObservableCollection<Case> _cases;
    private HubConnection _hubConnection;

    public MainPage(CaseService caseService, HubConnection hubConnection)
    {
        InitializeComponent();

        _caseService = caseService;
        _hubConnection = hubConnection;
    }

    async Task Connect()
    {
        _hubConnection.On<int>("CaseUpdated", async (caseid) =>
        {
            await MainThread.InvokeOnMainThreadAsync(LoadData);
        });
        if (_hubConnection.State == HubConnectionState.Disconnected)
        {
            await _hubConnection.StartAsync();
        }
    }

    public async void NavigateButton_Click(object sender, EventArgs e)
    {
        var button = sender as Button;
        // Redirects to CasePage.xaml
        if (button?.CommandParameter is Case SelectedCase)
        {
            // Use the same query parameter name as the QueryProperty on CasePage ("Case")
            await Navigation.PushAsync(new CasePage(_hubConnection, _caseService, SelectedCase)); 
        }
        else {
            await DisplayAlert("Error", "Cannot find selected case.", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Connect();
        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            var cases = await _caseService.GetAllCases();
            _cases = new ObservableCollection<Case>(cases);
            CaseView.ItemsSource = _cases;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load cases: {ex.Message}\n\n{ex.InnerException?.Message}", "OK");
        }
    }
}