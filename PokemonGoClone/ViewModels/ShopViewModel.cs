using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PokemonGoClone.ViewModels
{
    public class ShopViewModel : ViewModelBase
    {
        //field of ShopViewModel
        private MainWindowViewModel _mainWindowViewModel;
        private DialogViewModel _dialogViewModel;
        private List<ItemModel> _defaultItem;
        private TrainerModel _trainer;
        private ItemModel _choose;
        public PokemonModel _random;
        private ICommand _selectedItemCommand;
        private ICommand _buyCommand;
        private ICommand _randomCommand;
        public ICommand SelectedItemCommand
        {
            get { return _selectedItemCommand ?? (_selectedItemCommand = new RelayCommand(x => { SelectedItem(x); })); }
        }
        public ICommand BuyCommand
        {
            get { return _buyCommand ?? (_buyCommand = new RelayCommand(x => { Buy(); })); }
        }

        public ICommand RandomCommand
        {
            get { return _randomCommand ?? (_randomCommand = new RelayCommand(x => { RandomPokemon(); })); }
        }

        //constructor
        public ShopViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
        }

        //properties of ShopViewModel
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

        public TrainerModel Trainer
        {
            get { return _trainer; }
            set
            {
                _trainer = value;
                OnPropertyChanged(nameof(Trainer));
            }
        }

        public List<ItemModel> DefaultItem
        {
            get { return _defaultItem; }
            set
            {
                _defaultItem = value;
                OnPropertyChanged(nameof(DefaultItem));
            }
        }
        public ItemModel Choose
        {
            get { return _choose; }
            set
            {
                _choose = value;
                OnPropertyChanged(nameof(Choose));
            }
        }
        public int CurrentChargeofChoose
        {
            get { return Trainer.Items.Where(x => x.Id == Choose.Id).FirstOrDefault()?.Charge ?? 0; }
        }

        public PokemonModel Random
        {
            get { return _random; }
            set
            {
                _random = value;
                OnPropertyChanged(nameof(Random));
            }
        }

        //Method of ShopViewModel
        public void UpdatePlayer(TrainerModel trainer)
        {
            DefaultItem = MainWindowViewModel.Items;
            Trainer = trainer;
            Choose = MainWindowViewModel.Items[0];
        }

        public void SelectedItem(object item)
        {
            Choose = item as ItemModel;
            OnPropertyChanged(nameof(CurrentChargeofChoose));
        }

        public void Buy()
        {
            if (Trainer.Candy < Choose.Cost)
            {
                DialogViewModel.PopUp("You don't have enough Candy.");
            } else
            {
                Trainer.Candy -= Choose.Cost;
                Trainer.AddItem(Choose);
                OnPropertyChanged(nameof(CurrentChargeofChoose));
            }
        }

        public void RandomPokemon()
        {
            // Trainer.Money = 500;
            if (Trainer.Candy < 500) {
                DialogViewModel.PopUp("You don't have enough Candy.");
            } else {
                Trainer.Candy -= 500;
                PokemonModel pokemon = RandomPokemonMethod();
                Random = pokemon;
                Trainer.AddPokemon(pokemon);
            }
        }

        //factory Method
        public PokemonModel RandomPokemonMethod()
        {
            Random rnd = new Random();
            int x = rnd.Next(0, MainWindowViewModel.Pokemons.Count);
            int lucky = rnd.Next(0, 10000);
            PokemonModel pokemon = MainWindowViewModel.Pokemons[x];
            if (((pokemon.Id >= 144 && pokemon.Id <= 146) || (pokemon.Id >= 150 && pokemon.Id <= 151)) && lucky == 8888) { //special Pokemon
                pokemon.Accuracy = 1;
                return pokemon;
            } else if ((pokemon.Id >= 144 && pokemon.Id <= 146) || (pokemon.Id >= 150 && pokemon.Id <= 151)) {
                pokemon = MainWindowViewModel.Pokemons[x - 8];
            } else {
                int originalHealth = pokemon.MaxHealth;
                pokemon.MaxHealth = pokemon.Health = rnd.Next(originalHealth - 200, originalHealth + 101);
                if (pokemon.MaxHealth <= originalHealth) {
                    pokemon.Accuracy = rnd.Next(60, 70) / 100.0;
                    pokemon.Description = "It is Normal (N) Pokemon!";
                } else if (pokemon.MaxHealth <= originalHealth + 90) {
                    pokemon.Accuracy = rnd.Next(70, 80) / 100.0;
                    pokemon.Description = "It is Rare (R) Pokemon!";
                } else if (pokemon.MaxHealth < originalHealth + 100) {
                    pokemon.Accuracy = rnd.Next(80, 90) / 100.0;
                    pokemon.Description = "It is Super Rare (SR) Pokemon!";
                } else {
                    pokemon.Accuracy = rnd.Next(90, 96) / 100.0;
                    pokemon.Description = "It is Ultra Rare (UR) Pokemon!";
                }
            }

            return pokemon;
        }


    }
}

