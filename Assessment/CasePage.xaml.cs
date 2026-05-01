using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;
using PasientSimulator.lib.Services.Interfaces;

namespace Assessment
{
    public partial class CasePage : ContentPage
    {
        private readonly ICaseService _caseService;
        private readonly int _caseId;
        private Case? _case;

        public CasePage(CaseService caseService, int caseId)
        {
            InitializeComponent();
            _caseService = caseService;
            _caseId = caseId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadCase();
        }

        private async Task LoadCase()
        {
            try
            {
                _case = await _caseService.GetCaseByIdAsync(_caseId);
                if (_case != null)
                {
                    BindingContext = _case; // bind UI to the model
                }
                else
                {
                    await DisplayAlert("Not found", "Case not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}