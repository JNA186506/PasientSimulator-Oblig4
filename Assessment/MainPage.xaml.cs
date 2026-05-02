using System.Collections.ObjectModel;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;

namespace Assessment;

public partial class MainPage : ContentPage
{
    private readonly ICaseService _caseService;
    private ObservableCollection<Case> _cases;

    public MainPage(CaseService caseService)
    {
        InitializeComponent();

        _caseService = caseService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
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