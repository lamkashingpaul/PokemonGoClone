using PokemonGoClone.Models.Pokemons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.ViewModels
{
    public class RacecourseViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;
        private Random _rng;

        private PokemonModel _choice;
        private int _bets;
        public ObservableCollection<PokemonModel> RacingPokemons { get; private set; }

        public RacecourseViewModel(MainWindowViewModel mainWindowViewModel)
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
        public PokemonModel Choice
        {
            get { return _choice; }
            set
            {
                _choice = value;
                OnPropertyChanged(nameof(Choice));
            }
        }
        public int Bets
        {
            get { return _bets; }
            set
            {
                _bets = value;
                OnPropertyChanged(nameof(Bets));
            }
        }
        public void StartRace(ObservableCollection<PokemonModel> racingPokemons, PokemonModel choice, int bets)
        {
            RacingPokemons = racingPokemons;
            Choice = choice;
            Bets = bets;
        }
    }
}
