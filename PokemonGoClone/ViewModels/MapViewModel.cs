using PokemonGoClone.Models;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using PokemonGoClone.Views;
using PokemonGoClone.Models.Items;

namespace PokemonGoClone.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        // Delegates for DialogViewModel Action
        public void AcceptBattle(object x)
        {
            ((BattleViewModel)MainWindowViewModel.BattleViewModel).NewBattle(Player, Target);
            DialogViewModel.IsVisible = false;
            MainWindowViewModel.GoToBattleViewModel();
        }

        //
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;

        private const int _col = 11;
        private const int _row = 11;

        public TrainerModel Player;
        public TrainerModel Target;
        public List<TrainerModel> Trainers { get; private set; }
        public List<TileModel> Map { get; private set; }

        private ICommand _moveCommand;
        private ICommand _bagCommand;
        private ICommand _interactCommand;

        public ICommand MoveCommand
        {
            get { return _moveCommand ?? (_moveCommand = new RelayCommand(x => { Move(x); })); }
        }
        public ICommand BagCommand {
            get { return _bagCommand ?? (_bagCommand = new RelayCommand(x => { Bag(); })); }
        }
        public ICommand InteractCommand
        {
            get { return _interactCommand ?? (_interactCommand = new RelayCommand(x => { Interact(); })); }
        }

        // All properties of MapViewModel
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

        public MapViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;

            // Subscrible to the DialogViewModel
            DialogViewModel = new DialogViewModel(this);
        }

        public void GameInitialization(string name, int choice)
        {
            // Draw the boundary of map
            Map = new List<TileModel>
            {
                new TileModel('B', "Tile", 5, 0, 0),
                new TileModel('B', "Tile", 7, 0, COL - 1),
                new TileModel('B', "Tile", 9, ROW - 1, 0),
                new TileModel('B', "Tile", 10, ROW - 1, COL - 1)
            };

            for (int i = 0; i < ROW; i += ROW - 1)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    Map.Add(new TileModel('B', "Tile", 6, i, j));
                }
            }

            for (int i = 0; i < COL; i += COL - 1)
            {
                for (int j = 1; j < ROW - 1; j++)
                {
                    Map.Add(new TileModel('B', "Tile", 8, j, i));
                }
            }

            var rng = new Random();
            for (int i = 1; i < ROW - 1; i++)
            {
                for (int j = 1; j < COL - 1; j++)
                {
                    int randomGrass = rng.Next(1, 4);
                    Map.Add(new TileModel('G', "Tile", randomGrass, i, j));
                }
            }

            // Create player
            // Note that the player is always the Trainers[0]

            Trainers = new List<TrainerModel>
            {
                new TrainerModel(name, "Player", 1)
                {
                    XCoordinate = ROW / 2,
                    YCoordinate = COL / 2,
                    XFacing = ROW / 2 + 1,
                    YFacing = COL / 2,
                }
            };

            Player = Trainers[0];

            // Add Pokemon to player
            Trainers[0].AddPokemon((PokemonModel)(MainWindowViewModel.Pokemons.Find(x => x.Id == choice).Clone()));
            for (int i = 0; i < MainWindowViewModel.Pokemons.Count; i++) {
                Player.AddPokemon((PokemonModel)MainWindowViewModel.Pokemons[i].Clone());
                Player.AddPokemon((PokemonModel)MainWindowViewModel.Pokemons[i].Clone());
            }
            for (int i = 0; i < MainWindowViewModel.Items.Count; i++) {
                Player.AddItem((ItemModel)MainWindowViewModel.Items[i].Clone());
            }
            for (int i = 0; i < MainWindowViewModel.Items.Count; i++) {
                Player.AddItem((ItemModel)MainWindowViewModel.Items[i].Clone());
            }



            // Update the Bag View
            ((BagViewModel)MainWindowViewModel.BagViewModel).UpdatePlayer(Player);
            //((BagViewModel)MainWindowViewModel.BagViewModel).MainWindowViewModel = MainWindowViewModel;
            ((ItemViewModel)MainWindowViewModel.ItemViewModel).UpdatePlayer(Player);


            // Add more NPC trainers
            LoadOtherTrainers();

            // Create backlink to MainWindow
            MainWindowViewModel.Trainers = Trainers;
            MainWindowViewModel.Player = Player;

            // Navigate to MapView
            MainWindowViewModel.GoToMapViewModel();
        }

        public int COL
        {
            get { return _col; }
        }

        public int ROW
        {
            get { return _row; }
        }

        private void LoadOtherTrainers()
        {
            int i = 1;
            while(true)
            {
                string json;
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Trainers." + $"{i:D3}.json");
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream, Encoding.Default))
                    {
                        json = reader.ReadToEnd();
                    }

                    var values = (JObject)JsonConvert.DeserializeObject(json);

                    var rng = new Random();
                    int randomX, randomY;
                    do
                    {
                        randomX = rng.Next(1, ROW - 1);
                        randomY = rng.Next(1, ROW - 1);
                    } while (Trainers.Find(x => x.XCoordinate == randomX && x.YCoordinate == randomY) != null);

                    TrainerModel trainer = new TrainerModel(values["Name"].Value<string>(), "NPC", i)
                    { 
                        XCoordinate = randomX,
                        YCoordinate = randomY,
                        Quote = values["Quote"].Value<string>()
                    };
                    
                    int randomPokemon = rng.Next(MainWindowViewModel.Pokemons.Count) + 1;
                    trainer.AddPokemon((PokemonModel)(MainWindowViewModel.Pokemons.Find(x => x.Id == randomPokemon).Clone()));
                    Trainers.Add(trainer);

                    i += 1;
                } else
                {
                    break;
                }
            }

        }

        private void Move(object sender)
        {
            // Move() is not allowed if there is dialog overlay
            if (DialogViewModel.IsVisible == true)
            {
                return;
            }

            string command = sender as string;
            char direction = command[0];
            Trainers[0].Facing = direction;

            // Find if there is any object in front
            var frontTile = Map.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);
            var frontBeing = Trainers.Find(x => x.XCoordinate == Trainers[0].XFacing && x.YCoordinate == Trainers[0].YFacing);

            if (frontTile.Texture == 'G' && frontBeing == null)
            {
                switch (direction)
                {
                    case 'W':
                        Trainers[0].XCoordinate -= 1;
                        break;
                    case 'S':
                        Trainers[0].XCoordinate += 1;
                        break;
                    case 'A':
                        Trainers[0].YCoordinate -= 1;
                        break;
                    case 'D':
                        Trainers[0].YCoordinate += 1;
                        break;
                    default:
                        break;
                }
                Trainers[0].Facing = direction;
            }
        }

        private void Interact()
        {
            // If there is dialog overlay, Interact() is not allowed
            if (DialogViewModel.IsVisible == true)
            {
                DialogViewModel.ActionDelegateMethod?.Invoke(null);
            } else
            {
                Target = Trainers.Find(x => x.XCoordinate == Player.XFacing && x.YCoordinate == Player.YFacing);
                if (Target == null)
                {
                    return;
                }
                else
                {
                    DialogViewModel.ActionDelegateMethod = AcceptBattle;
                    DialogViewModel.Message = Target.Quote;
                }
            }

        }

        private void Bag() {
            // Bag() is not allowed if there is dialog overlay
            if (DialogViewModel.IsVisible == true)
            {
                return;
            }
            MainWindowViewModel.GoToBagViewModel();
        }
    }
}
