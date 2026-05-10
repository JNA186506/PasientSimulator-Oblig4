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
    public ObservableCollection<string> Comments { get; } = new ObservableCollection<string>();


    public CasePage(HubConnection hubConnection, ICaseService caseService, Case currCase)
    {
        InitializeComponent();
        _caseService = caseService;
        _hubConnection = hubConnection;
        _case = currCase;
        // assign case id if available
        try
        {
            _caseId = currCase?.CaseId ?? 0;
        }
        catch
        {
            _caseId = 0;
        }
        // populate initial comments if any
        PopulateCommentsFromCase();
        

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
    }

    private async void OnAddCommentClicked(object sender, EventArgs e)
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
            // Add to local collection so UI shows it immediately
            Comments.Insert(0, commentText);

            // Optionally send to backend if service available
            try
            {
                // Uncomment and adapt if AddComment exists on the service
                // await _caseService.AddComment(_caseId, commentText);
            }
            catch
            {
                // ignore service errors for now
            }

            vurdering.Text = string.Empty; // clear input
            await LoadCase(); // refresh case bindings
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Failed to add comment: {ex.Message}", "OK");
        }
    }

    private void PopulateCommentsFromCase()
    {
        Comments.Clear();
        if (_case == null)
            return;
        var prop = _case.GetType().GetProperty("Comments");
        if (prop == null)
            return;
        var value = prop.GetValue(_case) as System.Collections.IEnumerable;
        if (value == null)
            return;
        foreach (var item in value)
        {
            if (item == null)
                continue;
            var textProp = item.GetType().GetProperty("Text") ?? item.GetType().GetProperty("CommentText") ?? item.GetType().GetProperty("Message");
            var text = textProp != null ? textProp.GetValue(item)?.ToString() : item.ToString();
            if (!string.IsNullOrEmpty(text))
                Comments.Add(text);
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