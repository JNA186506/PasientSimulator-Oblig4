using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;
using PasientSimulator.lib.Services;

namespace Assessment
{
    public partial class MainPage : ContentPage {
        private CaseService _caseService;

        private readonly ObservableCollection<Case> _cases; 
        

        public MainPage(CaseService caseService)
        {
            InitializeComponent();

            _caseService = caseService;

            _cases = new ObservableCollection<Case>(_caseService.GetAllCases());
        }

    }
}
