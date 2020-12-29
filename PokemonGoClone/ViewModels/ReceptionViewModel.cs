using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace PokemonGoClone.ViewModels
{
    public class ReceptionViewModel : ViewModelBase
    {
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;
        private DispatcherTimer _refreshmentTimer;
        private TrainerModel _player;
        private int _secondUntilRefreshment;
        private Random _rng;

        private const int _numberOfRacer = 4;
        private const int _refreshmentperiod = 60;

        public ObservableCollection<PokemonModel> RacingPokemons { get; private set; }
        private ICommand _placeBetsCommand;
        private ICommand _refreshmentCommand;
        public ICommand PlaceBetsCommand
        {
            get { return _placeBetsCommand ?? (_placeBetsCommand = new RelayCommand(x => { PlaceBets(x); }, x => !DialogViewModel.IsVisible)); }

        }
        public ICommand RefreshmentCommand
        {
            get { return _refreshmentCommand ?? (_refreshmentCommand = new RelayCommand(x => { Refreshment(x); }, x => !DialogViewModel.IsVisible)); }

        }

        public ReceptionViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            Rng = new Random();
            NewRace();
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
        public int NumberOfRacer
        {
            get { return _numberOfRacer; }
        }
        public int Refreshmentperiod
        {
            get { return _refreshmentperiod; }
        }
        public DispatcherTimer RefreshmentTimer
        {
            get { return _refreshmentTimer; }
            set
            {
                _refreshmentTimer = value;
                OnPropertyChanged(nameof(RefreshmentTimer));
            }
        }
        public int SecondUntilRefreshment
        {
            get { return _secondUntilRefreshment; }
            set
            {
                _secondUntilRefreshment = value;
                OnPropertyChanged(nameof(SecondUntilRefreshment));
                OnPropertyChanged(nameof(RefreshmentIsEnable));
            }
        }
        public bool RefreshmentIsEnable
        {
            get { return SecondUntilRefreshment <= 0; }
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
        public void UpdatePlayer(TrainerModel player)
        {
            Player = player;
        }
        public void RefreshmentTimerInit()
        {
            SecondUntilRefreshment = Refreshmentperiod;
            RefreshmentTimer = new DispatcherTimer();
            RefreshmentTimer.Interval = new TimeSpan(0, 0, 1);
            RefreshmentTimer.Tick += RefreshmentTimerCount;
            RefreshmentTimer.Start();
        }
        private void RefreshmentTimerCount(object sender, EventArgs e)
        {
            SecondUntilRefreshment -= 1;
            if (SecondUntilRefreshment <= 0)
            {
                SecondUntilRefreshment = 0;
                RefreshmentTimer.Stop();
            }
        }
        public void NewRace()
        {

            // The race is simulated using pokemons health
            // Pokemons start from Health = 0 and they try to reach the Goal as soon as possible
            // The rank of Pokemon is represented using its Id

            RacingPokemons = new ObservableCollection<PokemonModel>();
            for (int i = 0; i < NumberOfRacer; i++)
            {
                int randomPokemonId;
                do
                {
                    randomPokemonId = Rng.Next(MainWindowViewModel.Pokemons.Count) + 1;
                } while (RacingPokemons.Where(x => x.Id == randomPokemonId).FirstOrDefault() != null);
                RacingPokemons.Add((PokemonModel)MainWindowViewModel.Pokemons.Where(x => x.Id == randomPokemonId).FirstOrDefault().Clone());
            }
            foreach(var pokemon in RacingPokemons)
            {
                pokemon.Health = 0;
                pokemon.Id = 0;
            }
        }
        private void Refreshment(object x)
        {
            foreach (var pokemon in Player.Pokemons)
            {
                pokemon.FullyRestore();
            }
            SecondUntilRefreshment = Refreshmentperiod;
            RefreshmentTimer.Start();
            DialogViewModel.PopUp("All your Pokemons are fully recoverd. ");
        }
        private void PlaceBets(object sender)
        {
            var objects = (object[])sender;
            var textBox = objects[0] as TextBox;

            if (!(objects[1] is PokemonModel pokemonModel))
            {
                DialogViewModel.PopUp("You must select a Pokemon to bet on. ");
                return;
            }

            string input = textBox.Text;
            if (string.IsNullOrEmpty(input))
            {
                DialogViewModel.PopUp("You must input number of stardust you want to bet.");
                return;
            }

            if (int.TryParse(input, out int bets) == false || bets < 0)
            {
                DialogViewModel.PopUp("Invalid input of number of stardust. ");
                return;
            }

            if (Player.Stardust < bets)
            {
                DialogViewModel.PopUp("Not enough stardust. ");
                return;
            }

            ((RacecourseViewModel)MainWindowViewModel.RacecourseViewModel).StartRace(RacingPokemons, pokemonModel, bets, Player);
            MainWindowViewModel.GoToRacecourseViewModel(null);
        }
    }
}
