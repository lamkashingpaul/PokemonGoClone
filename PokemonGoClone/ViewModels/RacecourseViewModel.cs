using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace PokemonGoClone.ViewModels
{
    public class RacecourseViewModel : ViewModelBase
    {
        // Delegate to Confirm exit
        public void ConfirmExitRacecourse(object x)
        {
            // Player can no longer enjoy the same racing after exit
            // New list of racers is created
            DialogViewModel.IsVisible = false;
            RacingTimer.Stop();
            ((ReceptionViewModel)MainWindowViewModel.ReceptionViewModel).NewRace();
            MainWindowViewModel.GoToMapViewModel(null);
        }

        // Fields of RacecourseViewModel
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;
        private DispatcherTimer _racingTimer;
        private TrainerModel _player;
        private Random _rng;

        private const int _costPerCheer = 500;
        private const int _goal = 1000000;
        private const int _progressBarFPS = 60;

        private PokemonModel _choice;
        private int _bets;
        public ObservableCollection<PokemonModel> RacingPokemons { get; private set; }
        public ObservableCollection<PokemonModel> Ranking { get; private set; }
        public ObservableCollection<int> PokemonsProgress { get; private set; }

        private ICommand _cheerCommand;
        private ICommand _exitRacecourseCommand;
        public ICommand CheerCommand
        {
            get { return _cheerCommand ?? (_cheerCommand = new RelayCommand(x => { Cheer(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand ExitRacecourseCommand
        {
            get { return _exitRacecourseCommand ?? (_exitRacecourseCommand = new RelayCommand(x => { ExitRacecourse(x); }, x => !DialogViewModel.IsVisible)); }
        }


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
        public DispatcherTimer RacingTimer
        {
            get { return _racingTimer; }
            set
            {
                _racingTimer = value;
                OnPropertyChanged(nameof(RacingTimer));
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
        public int CostPerCheer
        {
            get { return _costPerCheer; }
        }
        public int Goal
        {
            get { return _goal; }
        }
        public int ProgressBarFPS
        {
            get { return _progressBarFPS; }
        }
        public bool CheerIsEnable
        {
            get { return Player.Candy >= CostPerCheer; }
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
        public TrainerModel Player
        {
            get { return _player; }
            set
            {
                _player = value;
                OnPropertyChanged(nameof(Player));
            }
        }
        public void StartRace(ObservableCollection<PokemonModel> racingPokemons, PokemonModel choice, int bets, TrainerModel player)
        {
            Ranking = new ObservableCollection<PokemonModel>();
            RacingPokemons = racingPokemons;
            Choice = choice;
            Bets = bets;
            Player = player;
            RacingTimerStart();
        }
        private void RacingTimerStart()
        {
            RacingTimer = new DispatcherTimer();
            RacingTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(1.0 / ProgressBarFPS * 1000));
            RacingTimer.Tick += RacingTimerCount;
            RacingTimer.Start();
        }
        private void RacingTimerCount(object sender, EventArgs e)
        {
            // If all pokemons reach the goal
            if (Ranking.Count == RacingPokemons.Count)
            {
                RacingTimer.Stop();
                BettingResult();
            }

            // Pokemon runs (gains health) with random speed
            foreach(var pokemon in RacingPokemons)
            {
                pokemon.Health += Rng.Next(pokemon.MaxHealth);

                if (Ranking.Contains(pokemon) == false && pokemon.Health >= Goal)
                {
                    Ranking.Add(pokemon);
                    pokemon.Id = Ranking.Count;
                }
            }

            // Check if player's choice finishes the race
            if (Ranking.Contains(Choice))
            {
                BettingResult();
            }
        }
        private void BettingResult()
        {
            double price = 0;
            if (Choice.Id == 1)
            {
                price = Bets * 1.5;
            } else if (Choice.Id == 2)
            {
                price = Bets * 1.25;
            }
            else if (Choice.Id == 3)
            {
                price = Bets * 0.75;
            }
            else if (Choice.Id == 4)
            {
                price = Bets * 0;
            }
            Player.Stardust += (int)price;

            if (MainWindowViewModel.CurrentViewModel == this)
            {
                DialogViewModel.PopUp($"{Choice.Name} is #{Choice.Id}. You win {(int)price} Stardust. ");
            }
        }
        private void Cheer(object x)
        {
            Player.Candy -= CostPerCheer;
            OnPropertyChanged(nameof(CheerIsEnable));
            foreach (var pokemon in RacingPokemons)
            {
                pokemon.MaxHealth += Rng.Next(CostPerCheer);
            }
            Choice.MaxHealth += Rng.Next(CostPerCheer / 2, CostPerCheer);
        }
        private void ExitRacecourse(object x)
        {
            DialogViewModel.PopUp("Do you want to exit? You can no longer enjoy this race. ", null, ConfirmExitRacecourse);
        }
    }
}
