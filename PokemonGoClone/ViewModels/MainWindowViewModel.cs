using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        // All models
        

        // All available views
        private object _currentView;

        // All availabe viewmodels
        private object _currentViewModel;

        // All ICommands to navigate between views

        // Default constructor
        public MainWindowViewModel()
        {
            // Create instance for all views and viewmodels


            // Set up the startup view and viewmodels
            CurrentView = null;
            CurrentViewModel = null;
        }

        // All properties of views and viewmodels
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

    }
}
