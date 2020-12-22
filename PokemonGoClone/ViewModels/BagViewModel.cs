using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System.Collections.Generic;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class BagViewModel : ViewModelBase
    {
        //field of BagViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private List<PokemonModel> _pokemons;
        private TrainerModel _player;
        private ICommand _selectedPokemonCommand;

        public ICommand SelectedPokemonCommand
        {
            get { return _selectedPokemonCommand ?? (_selectedPokemonCommand = new RelayCommand(x => { SelectedPokemon(x); }, x => !DialogViewModel.IsVisible)); }
        }

        //constructor of BagViewModel
        public BagViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
        }

        //properties of BagViewModel
        public MainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel; }
            set
            {
                _mainWindowViewModel = value;
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

        public List<PokemonModel> Pokemons
        {
            get { return _pokemons; }
            set
            {
                _pokemons = value;
                OnPropertyChanged(nameof(Pokemons));
            }
        }
        public TrainerModel Player
        {
            get { return _player; }
            set
            {
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }

        //method of BagViewModel
        public void SelectedPokemon(object sender)
        {
            var pokemon = sender as PokemonModel;
            int index = MainWindowViewModel.Player.Pokemons.IndexOf(pokemon);
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).UpdateView(pokemon, index);
            MainWindowViewModel.GoToPokemonStatusViewModel(null);
        }

        public void UpdatePlayer(TrainerModel player)
        {
            Player = player;
            Pokemons = player.Pokemons;
        }
    }
}
