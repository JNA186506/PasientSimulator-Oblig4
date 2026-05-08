using Microsoft.AspNetCore.SignalR.Client;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Assessment;

[QueryProperty(nameof(Case), "case")]
public partial class CasePage : ContentPage
{
    private readonly int _caseId;
    private readonly HubConnection _hubConnection;
    private readonly ICaseService _caseService;
    private Case _case;


    public CasePage(HubConnection hubConnection, ICaseService caseService, Case currCase)
    {
        InitializeComponent();
        _caseService = caseService;
        _hubConnection = hubConnection;
        _case = currCase;
        

    }
    async Task Connect()
    {
        _hubConnection.On<int>("CaseUpdated", async (caseid) =>
        {
            await MainThread.InvokeOnMainThreadAsync(LoadCase);
        });
        if (_hubConnection.State == HubConnectionState.Disconnected) {
            await _hubConnection.StartAsync();
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Connect();
        await LoadCase();
    }
    private async void OnEditorTextChanged(object Sender, TextChangedEventArgs e)
    {
        if (_case == null)
            return;
        var commentText = vurdering.Text?.Trim();
        if (string.IsNullOrEmpty(commentText))
        {
            await DisplayAlertAsync("Validation", "Comment cannot be empty.", "OK");
            return;
        }
        try
        {
          //  await _caseService.AddComment(_caseId, commentText);
            vurdering.Text = string.Empty; // clear input
            await LoadCase(); // refresh case to show new comment
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Failed to add comment: {ex.Message}", "OK");
        }
    }

    private async Task LoadCase()
    {
        try
        {
            if (_case != null)
                BindingContext = _case; // bind UI to the model
            else
                await DisplayAlertAsync("Not found", "Case not found.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}