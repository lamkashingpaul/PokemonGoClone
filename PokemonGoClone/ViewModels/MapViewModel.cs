using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonGoClone.Models;
using PokemonGoClone.Models.Items;
using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using PokemonGoClone.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace PokemonGoClone.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        // Delegates for DialogViewModel Action
        public void AcceptBattle(object x)
        {
            // If player's first pokemon has 0 health, it's not allowed to fight
            if (Player.Pokemons[0].Health == 0)
            {
                DialogViewModel.PopUp("Your lending pokemon cannot fight, please select other pokemon.");
            }
            else
            {
                ((BattleViewModel)MainWindowViewModel.BattleViewModel).NewBattle(Player, Target, "");
                DialogViewModel.IsVisible = false;
                MainWindowViewModel.GoToBattleViewModel(null);
            }
        }
        public void EnterGym(object x)
        {
            // You may want to do some update before going into GymViewModel
            DialogViewModel.IsVisible = false;
            MainWindowViewModel.GoToGymViewModel(null);
        }
        public void NotAccept(object x) {
            // You may want to do some update before going into GymViewModel
            DialogViewModel.PopUp("You must accept the Battle", NotAccept, EnterGym);            
            
        }


        public void BackToStart(object x)
        {
            DialogViewModel.Close(x);
            MainWindowViewModel.GoToStartViewModel(null);
        }

        // All fields of MapViewModel
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;
        private DispatcherTimer _timer;

        private const int _col = 11;
        private const int _row = 11;

        public TrainerModel Player;
        public TrainerModel Target;
        public ObservableCollection<TrainerModel> Trainers { get; private set; }
        public ObservableCollection<TileModel> Map { get; private set; }
        public CompositeCollection Grid { get; private set; }

        // All ICommands of MapViewModel
        private ICommand _moveCommand;
        private ICommand _bagCommand;
        private ICommand _menuCommand;
        private ICommand _interactCommand;
        private ICommand _shopCommand;

        // All ICommand properites of MapViewModel
        public ICommand MoveCommand
        {
            get { return _moveCommand ?? (_moveCommand = new RelayCommand(x => { Move(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand BagCommand
        {
            get { return _bagCommand ?? (_bagCommand = new RelayCommand(x => { Bag(); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand MenuCommand
        {
            get { return _menuCommand ?? (_menuCommand = new RelayCommand(x => { Menu(x); }, x => !DialogViewModel.IsVisible)); }
        }
        public ICommand ShopCommand
        {
            get { return _shopCommand ?? (_shopCommand = new RelayCommand(x => { Shop(); }, x => !DialogViewModel.IsVisible)); }
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
        public int COL
        {
            get { return _col; }
        }
        public int ROW
        {
            get { return _row; }
        }
        public DispatcherTimer Timer {
            get { return _timer; }
            set { 
                _timer = value;
            }
        }

        // Constructor of MapViewModel
        public MapViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
        }

        // Game initialization for new game
        public void GameInitialization(string name, int choice)
        {
            LoadMap();


            // Create player
            // Note that the player is always the Trainers[0]

            Trainers = new ObservableCollection<TrainerModel>
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
            // Add all pokemon to player if there is cheat code
            if (Player.Name == "WhosYourDaddy")
            {
                for (int i = 0; i < MainWindowViewModel.Pokemons.Count; i++)
                {
                    Player.AddPokemon((PokemonModel)MainWindowViewModel.Pokemons[i].Clone());
                }
                Player.Candy = 9999999;
                Player.Stardust = 9999999;
            } else
            {
                Player.AddPokemon((PokemonModel)MainWindowViewModel.Pokemons.Find(x => x.Id == choice).Clone());
            }

            // Add Items to player
            for (int i = 0; i < MainWindowViewModel.Items.Count; i++)
            {
                Player.AddItem((ItemModel)MainWindowViewModel.Items[i].Clone());
            }

            // Update the Bag, Item and Shop ViewModel
            ((BagViewModel)MainWindowViewModel.BagViewModel).UpdatePlayer(Player);
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).Update(Player);
            ((ItemViewModel)MainWindowViewModel.ItemViewModel).UpdatePlayer(Player);
            ((ShopViewModel)MainWindowViewModel.ShopViewModel).UpdatePlayer(Player);
            ((GymViewModel)MainWindowViewModel.GymViewModel).UpdatePlayer(Player);

            //Initialize the timer
            Timer = new DispatcherTimer();
            RunTimer();
                       

            // Add more NPC trainers
            LoadOtherTrainers();
            LoadGym();
           
            ((GymViewModel)MainWindowViewModel.GymViewModel).UpdateTrainers(Trainers);

            // Create CompositeCollection from view binding
            Grid = new CompositeCollection
            {
                new CollectionContainer() { Collection = Map },
                new CollectionContainer() { Collection = Trainers }
            };

            // Create backlink to MainWindow
            MainWindowViewModel.Trainers = Trainers;
            MainWindowViewModel.Player = Player;

            // Navigate to MapView
            MainWindowViewModel.GoToMapViewModel(null);
        }

        // Game initialization for loading from save
        public void GameLoad(ObservableCollection<TrainerModel> trainers)
        {
            LoadMap();
            Trainers = trainers;
            Player = Trainers[0];
            // Update the Bag, Item and Shop ViewModel
            ((BagViewModel)MainWindowViewModel.BagViewModel).UpdatePlayer(Player);
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).Update(Player);
            ((ItemViewModel)MainWindowViewModel.ItemViewModel).UpdatePlayer(Player);
            ((ShopViewModel)MainWindowViewModel.ShopViewModel).UpdatePlayer(Player);
            ((GymViewModel)MainWindowViewModel.GymViewModel).UpdatePlayer(Player);

            // Create CompositeCollection from view binding
            Grid = new CompositeCollection
            {
                new CollectionContainer() { Collection = Map },
                new CollectionContainer() { Collection = Trainers }
            };
            // Create backlink to MainWindow
            MainWindowViewModel.Trainers = Trainers;
            MainWindowViewModel.Player = Player;
            // Navigate to MapView after loading
            MainWindowViewModel.GoToMapViewModel(null);
            DialogViewModel.PopUp("Loaded Successfully");
        }
        private void LoadMap()
        {
            // Draw the boundary of map
            Map = new ObservableCollection<TileModel>
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
        }

        private void LoadOtherTrainers()
        {
            int i = 1;
            while (true)
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
                    } while (Trainers.Where(x => x.XCoordinate == randomX && x.YCoordinate == randomY).FirstOrDefault() != null);

                    TrainerModel trainer = new TrainerModel(values["Name"].Value<string>(), "NPC", i)
                    {
                        XCoordinate = randomX,
                        YCoordinate = randomY,
                        Quote = values["Quote"].Value<string>()
                    };
                                        
                    int randomPokemon = rng.Next(MainWindowViewModel.Pokemons.Count) + 1;
                    trainer.AddPokemon((PokemonModel)MainWindowViewModel.Pokemons.Find(x => x.Id == randomPokemon).Clone());
                    Trainers.Add(trainer);

                    i += 1;
                }
                else
                {
                    break;
                }
            }

        }

        private void LoadGym() {
            var rnd = new Random();
            int randomX, randomY;
            do {
                randomX = rnd.Next(1, ROW - 1);
                randomY = rnd.Next(1, ROW - 1);
            } while (Trainers.Where(x => x.XCoordinate == randomX && x.YCoordinate == randomY).FirstOrDefault() != null);
            TrainerModel gym = new TrainerModel("Gym", "Gym", 999) {
                XCoordinate = randomX,
                YCoordinate = randomY,
            };

            Trainers.Add(gym);

        }

        // Random Wild Pokemon Spawn
        private void WildPokemonSpawn()
        {

        }


        // All RelayCommands of MapViewModel
        private void Move(object sender)
        {
            string command = sender as string;
            char direction = command[0];
            Player.Facing = direction;

            // Find if there is any object in front
            var frontTile = Map.Where(x => x.XCoordinate == Player.XFacing && x.YCoordinate == Player.YFacing).FirstOrDefault();
            var frontBeing = Trainers.Where(x => x.XCoordinate == Player.XFacing && x.YCoordinate == Player.YFacing).FirstOrDefault();

            if (frontTile.Texture == 'G' && frontBeing == null)
            {
                switch (direction)
                {
                    case 'W':
                        Player.XCoordinate -= 1;
                        break;
                    case 'S':
                        Player.XCoordinate += 1;
                        break;
                    case 'A':
                        Player.YCoordinate -= 1;
                        break;
                    case 'D':
                        Player.YCoordinate += 1;
                        break;
                    default:
                        break;
                }
                Player.Facing = direction;
            }
        }

        private void Interact()
        {
            // If there is dialog overlay, Interact() is used to invoke [OK] Button
            if (DialogViewModel.IsVisible == true)
            {
                DialogViewModel.ActionDelegateMethod?.Invoke(null);
            }
            else
            {
                Target = Trainers.Where(x => x.XCoordinate == Player.XFacing && x.YCoordinate == Player.YFacing).FirstOrDefault();
                if (Target == null)
                {
                    return;
                }
                else if (Target.Type == "NPC")
                {
                    Timer.Stop();
                    DialogViewModel.PopUp(Target.Quote, null, AcceptBattle);
                } else if (Target.Type == "Gym")
                {
                    DialogViewModel.PopUp(Target.Quote, null, EnterGym);
                }
            }
        }

        private void Bag()
        {
            MainWindowViewModel.GoToBagViewModel(null);
        }
        private void Menu(object sender)
        {
            DialogViewModel.PopUp("Back to Start?", null, BackToStart);
        }
        private void Shop()
        {
            MainWindowViewModel.GoToShopViewModel(null);
        }

        public void RunTimer() {
            if (((GymViewModel)MainWindowViewModel.GymViewModel).Player == ((GymViewModel)MainWindowViewModel.GymViewModel).CurrentOccupier) {
                Random rnd = new Random();
                int time = rnd.Next(300, 600);
                Timer.Interval = new TimeSpan(0, 0, time);
                Timer.Tick += TimeCount;
                Timer.Start();
            } else { 
                //nothing
            }

        }

        public void TimeCount(object sender, EventArgs e) {
            DialogViewModel.PopUp("Someone challange you! You must accept", NotAccept, EnterGym);
            Timer.Stop();
        }

    }
}
