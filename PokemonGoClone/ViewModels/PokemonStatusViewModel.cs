using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Windows.Controls;
using System.Windows.Input;


namespace PokemonGoClone.ViewModels
{
    class PokemonStatusViewModel : ViewModelBase
    {

        //field of PokemonStatusViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private PokemonModel _pokemon;
        private TrainerModel _player;
        private string _originalName;
        private string _defaultName;
        private int _index;
        private int _powerUpCost;
        private int _evolveCost;

        //private bool _isEnabled;
        //private bool _evolveButtonIsEnabled;

        //ICommand of PokemonStatusViewModel
        private ICommand _changeNameCommand;
        private ICommand _becomeFirstPokemonCommand;
        private ICommand _dropPokemonCommand;
        private ICommand _powerUpCommand;
        private ICommand _evolveCommand;

        public ICommand ChangeNameCommand
        {
            get { return _changeNameCommand ?? (_changeNameCommand = new RelayCommand(x => { ChangeName(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand BecomeFirstPokemonCommand
        {
            get { return _becomeFirstPokemonCommand ?? (_becomeFirstPokemonCommand = new RelayCommand(x => { BecomeFirstPokemon(); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand DropPokemonCommand
        {
            get { return _dropPokemonCommand ?? (_dropPokemonCommand = new RelayCommand(x => { DropPokemon(); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand PowerUpCommand {
            get { return _powerUpCommand ?? (_powerUpCommand = new RelayCommand(x => { PowerUp(); }, x => PowerUpButton())); }
        }
        public ICommand EvolveCommand {
            get { return _evolveCommand ?? (_evolveCommand = new RelayCommand(x => { Evolve(); }, x => EvolveButton())); }
        }


        //Constructor of PokemonStatusViewModel
        public PokemonStatusViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            //EvolveButtonIsEnabled = false;
        }

        //Properties of PokemonStatusViewModel
        public string OriginalName
        {
            get { return _originalName; }
            set
            {
                _originalName = value;
            }
        }
        public string DefaultName
        {
            get { return _defaultName; }
            set
            {
                _defaultName = value;
                OnPropertyChanged(nameof(DefaultName));
            }
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
        public DialogViewModel DialogViewModel
        {
            get { return _dialogViewModel; }
            set
            {
                _dialogViewModel = value;
                OnPropertyChanged(nameof(DialogViewModel));
            }
        }
        public PokemonModel Pokemon
        {
            get { return _pokemon; }
            set
            {
                _pokemon = value;
                OnPropertyChanged(nameof(Pokemon));
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
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        public int PowerUpCost {
            get { return _powerUpCost; }
            private set {
                _powerUpCost = value;
                OnPropertyChanged(nameof(PowerUpCost));
            }
        }

        public int EvolveCost {
            get { return _evolveCost; }
            private set {
                _evolveCost = value;
                OnPropertyChanged(nameof(EvolveCost));
            }
        }

        //Method of PokemonStatusViewModel
        public void ChangeName(object value)
        {
            var text = value as TextBox;
            string newName = text.Text;
            if (!string.IsNullOrEmpty(newName))
            {
                Pokemon.Name = newName;
                OriginalName = newName;
            }
            DefaultName = OriginalName;
        }

        public void BecomeFirstPokemon()
        {
            PokemonModel tmp = MainWindowViewModel.Player.Pokemons[Index];
            for (int i = 0; i < Index; i++)
            {
                MainWindowViewModel.Player.Pokemons[Index - i] = MainWindowViewModel.Player.Pokemons[Index - i - 1];
            }
            MainWindowViewModel.Player.Pokemons[0] = tmp;
            DialogViewModel.PopUp($"{tmp.Name} is now your leading pokemon now.");
        }
        public void Update(TrainerModel player)
        {
            Player = player;
        }
        public void UpdateView(PokemonModel pokemon, int index)
        {
            Pokemon = pokemon;
            OriginalName = Pokemon.Name;
            DefaultName = Pokemon.Name;
            Index = index;
            Player = MainWindowViewModel.Player;
        }
        public void DropPokemon()
        {
            MainWindowViewModel.Player.DropPokemon(Pokemon);
            MainWindowViewModel.GoToBagViewModel(null);
        }

        public void PowerUp() {
            Random rnd = new Random();
            Pokemon.Level++;
            int add = rnd.Next(1, Pokemon.MaxHealthPerLevel + 1);
            Pokemon.MaxHealth += add;
            Player.Candy -= PowerUpCost;
            DialogViewModel.PopUp($"You have successfully Power Up {Pokemon.Name} \n Its MaxHealth is added by {add}");
        }
        public void Evolve() {
            Random rnd = new Random();
            int number = rnd.Next(0, Pokemon.EvolveId.Length);
            PokemonModel tmp = new PokemonModel(MainWindowViewModel.Pokemons[Pokemon.EvolveId[number] - 1].Name,
                                                Pokemon.EvolveId[number],
                                                Pokemon.Description,
                                                1,
                                                MainWindowViewModel.Pokemons[Pokemon.EvolveId[number] - 1].MaxLevel,
                                                Pokemon.MaxHealth,
                                                MainWindowViewModel.Pokemons[Pokemon.EvolveId[number] - 1].MaxHealthPerLevel,
                                                Pokemon.Accuracy,
                                                MainWindowViewModel.Pokemons[Pokemon.EvolveId[number] - 1].EvolveId,
                                                MainWindowViewModel.Pokemons[Pokemon.EvolveId[number] - 1].EvolveCost,
                                                MainWindowViewModel.Pokemons[Pokemon.EvolveId[number] - 1].PowerUpCostBase,
                                                MainWindowViewModel.Pokemons[Pokemon.EvolveId[number] - 1].PowerUpCostPerLevel,
                                                null
                                                );
            for (int i = 0; i < Pokemon.Abilities.Count; i++) {
                tmp.AddAbility(Pokemon.Abilities[i]);
            }
            string tmpName = Pokemon.Name;
            Pokemon.Name = tmp.Name;
            Pokemon.ImageSource = tmp.ImageSource;
            Pokemon.Id = tmp.Id;
            Pokemon.Level = 1;
            Pokemon.Health = tmp.MaxHealth;
            Pokemon.MaxLevel = tmp.MaxLevel;
            Pokemon.MaxHealthPerLevel = tmp.MaxHealthPerLevel;
            Pokemon.EvolveId = tmp.EvolveId;
            Pokemon.EvolveCost = tmp.EvolveCost;
            Pokemon.PowerUpCostBase = tmp.PowerUpCostBase;
            Pokemon.PowerUpCostPerLevel = tmp.PowerUpCostPerLevel;
            Player.Stardust -= EvolveCost;
            DialogViewModel.PopUp($"You have successfully evolved {tmpName} to {Pokemon.Name}");
        }
        public bool PowerUpButton() {
            if (DialogViewModel.IsVisible) {
                return false;
            }
            PowerUpCost = Pokemon.PowerUpCostBase + (Pokemon.PowerUpCostPerLevel * Pokemon.Level);
            if (Pokemon.Level == Pokemon.MaxLevel || Player.Candy < PowerUpCost) {
                return false;
            } else {
                return true;
            }            
        }
        public bool EvolveButton() {
            if (DialogViewModel.IsVisible) {
                return false;
            }
            EvolveCost = Pokemon.EvolveCost;
            if (Pokemon.EvolveId[0] == 0 || Pokemon.Level != Pokemon.MaxLevel || Player.Stardust < EvolveCost) {
                return false;
            } else {
                return true;
            }
        }
    }
}
