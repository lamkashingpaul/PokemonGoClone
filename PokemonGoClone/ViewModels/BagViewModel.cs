using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using PokemonGoClone.Views;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BagViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        private List<PokemonModel> _pokemons;
        private object _pokemonStatusView;
        private object _pokemonStatusViewModel;
        private ICommand _goToPokemonStatusViewModelCommand;

        public ICommand GoToPokemonStatusViewModelCommand {
            get { return _goToPokemonStatusViewModelCommand ?? (_goToPokemonStatusViewModelCommand = new RelayCommand(x => { GoToPokemonStatusViewModel(x); })); }
        }
        public BagViewModel(TrainerModel trainer, MainWindowViewModel mainWindowViewModel)
        {
            Pokemons = trainer.Pokemons;
            MainWindowViewModel = mainWindowViewModel;
            PokemonStatusView = new PokemonStatusView();
            PokemonStatusViewModel = new PokemonStatusViewModel();
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set
            {
                _mainWindowViewModel = value;
                OnPropertyChanged(nameof(MainWindowViewModel));
            }
        }

        public object PokemonStatusViewModel {
            get { return _pokemonStatusViewModel; }
            set {
                _pokemonStatusViewModel = value;
                OnPropertyChanged(nameof(PokemonStatusViewModel));
            }
        }
        public object PokemonStatusView {
            get { return _pokemonStatusView; }
            set {
                _pokemonStatusView = value;
                OnPropertyChanged(nameof(PokemonStatusView));
            }
        }
        public void GoToPokemonStatusViewModel(object sender) {
            var pokemon = sender as PokemonModel;
            ((PokemonStatusViewModel)PokemonStatusViewModel).UpdateView(pokemon);
            MainWindowViewModel.CurrentViewModel = PokemonStatusViewModel;
            MainWindowViewModel.CurrentView = PokemonStatusView;
        }

        public List<PokemonModel> Pokemons
        {
            get { return _pokemons; }
            set {
                _pokemons = value;
                OnPropertyChanged(nameof(Pokemons));
            }
        }        
    }
}
