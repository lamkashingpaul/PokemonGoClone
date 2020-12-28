using System;

namespace PokemonGoClone.ViewModels
{
    // This is a template for new ViewModel, not used by the program actually
    // Each viewmodel should be a child of MainWindowViewModel
    // Each viewmodel is able to access MainWindowViewModel and DialogViewModel by back links

    public class TemplateViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;
        private Random _rng;

        public TemplateViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            Rng = new Random();
        }
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewMode; }
            set
            {
                _mainWindowViewMode = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }
        public DialogViewModel DialogViewModel
        {
            get { return _dialogViewModel; }
            set
            {
                _dialogViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }
        public Random Rng
        {
            get { return _rng; }
            set
            {
                _rng = value;
                OnPropertyChanged(nameof(Rng));
            }
        }
    }
}
