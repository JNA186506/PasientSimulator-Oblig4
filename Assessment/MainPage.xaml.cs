using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using PasientSimulator.lib.Models;

namespace Assessment
{
    public partial class MainPage : ContentPage {
        private Context _context;

        private readonly ObservableCollection<Case> _cases; 
        

        public MainPage(Context context)
        {
            InitializeComponent();
            _context = context;

            _cases = new ObservableCollection<Case>();
            _context.Cases.Load();
        }

    }
}
