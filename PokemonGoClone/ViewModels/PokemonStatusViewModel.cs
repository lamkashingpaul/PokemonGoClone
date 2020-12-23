using PokemonGoClone.Models.Abilities;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
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
        //private bool _isEnabled;
        //private bool _evolveButtonIsEnabled;

        //ICommand of PokemonStatusViewModel
        private ICommand _changeNameCommand;
        private ICommand _becomeFirstPokemonCommand;
        private ICommand _showAbilityDescriptionCommand;
        private ICommand _dropPokemonCommand;
        public ICommand ChangeNameCommand
        {
            get { return _changeNameCommand ?? (_changeNameCommand = new RelayCommand(x => { ChangeName(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand BecomeFirstPokemonCommand
        {
            get { return _becomeFirstPokemonCommand ?? (_becomeFirstPokemonCommand = new RelayCommand(x => { BecomeFirstPokemon(); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand ShowAbilityDescriptionCommand
        {
            get { return _showAbilityDescriptionCommand ?? (_showAbilityDescriptionCommand = new RelayCommand(x => { ShowAbilityDescription(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand DropPokemonCommand
        {
            get { return _dropPokemonCommand ?? (_dropPokemonCommand = new RelayCommand(x => { DropPokemon(); }, x => !DialogViewModel.IsVisible)); }
        }
        /*
        public bool IsEnabled {
            get { return _isEnabled; }
            set {
                _isEnabled = value;
                OnPropertyChanged(nameof(EvolveButtonIsEnabled));
            }
        }
        public bool EvolveButtonIsEnabled {
            get { return Pokemon.Id == 4; }
            set {
                _evolveButtonIsEnabled = value;
                OnPropertyChanged(nameof(EvolveButtonIsEnabled));
            }
        }
        */

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

        //Method of PokemonStatusViewModel
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
        }
        private void ChangeName(object value)
        {
            string result;
            var text = value as TextBox;
            string newName = text.Text;
            if (!string.IsNullOrEmpty(newName))
            {
                Pokemon.Name = newName;
                OriginalName = newName;
                result = $"Your pokemon has new name \"{newName}\".";
            } else
            {
                result = $"You must provide a name for your pokemon.";
            }
            DialogViewModel.PopUp(result);
            DefaultName = OriginalName;
        }

        private void BecomeFirstPokemon()
        {
            PokemonModel tmp = Player.Pokemons[Index];
            for (int i = 0; i < Index; i++)
            {
                Player.Pokemons[Index - i] = Player.Pokemons[Index - i - 1];
            }
            Player.Pokemons[0] = tmp;
            DialogViewModel.PopUp($"{tmp.Name} is now your leading pokemon now.");
        }
        private void ShowAbilityDescription(object x)
        {
            var ability = x as AbilityModel;
            DialogViewModel.PopUp(ability.Description);
        }
        private void DropPokemon()
        {
            if (Player.Pokemons.Count == 1)
            {
                DialogViewModel.PopUp("You are not allowed to drop your last Pokemon.");
            } else
            {
                Player.DropPokemon(Pokemon);
                MainWindowViewModel.GoToBagViewModel(null);
            }
        }
    }
}
