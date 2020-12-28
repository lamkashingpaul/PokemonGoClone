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
            if (Trainers.Contains(Target) == false)
            {
                DialogViewModel.PopUp("Your target is already gone. ");
            }
            else if (Player.Pokemons[0].Health == 0)
            {
                DialogViewModel.PopUp("Your lending pokemon cannot fight, please select other pokemon. ");
            }
            else
            {
                ((BattleViewModel)MainWindowViewModel.BattleViewModel).NewBattle(Player, Target, Player.Pokemons[0], Target.Pokemons[0], Target.Type);
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
        public void EnterReception(object x)
        {
            DialogViewModel.IsVisible = false;
            MainWindowViewModel.GoToReceptionViewModel(null);
        }
        public void EnterBattle(object x) {
            // You may want to do some update before going into GymViewModel
            DialogViewModel.IsVisible = false;
            ((GymViewModel)MainWindowViewModel.GymViewModel).ChallangePlayer();
            MainWindowViewModel.GoToBattleViewModel(null);

        }
        public void NotAccept(object x) {
            // You may want to do some update before going into GymViewModel
            DialogViewModel.PopUp("You must accept the Battle", NotAccept, EnterBattle);
        }


        public void BackToStart(object x)
        {
            DialogViewModel.Close(x);
            MainWindowViewModel.GoToStartViewModel(null);
        }

        // All fields of MapViewModel
        private MainWindowViewModel _mainWindowViewMode;
        private DialogViewModel _dialogViewModel;
        private DispatcherTimer _gymTimer;
        private DispatcherTimer _spawnTimer;

        private Random _rng;

        private const int _col = 11;
        private const int _row = 11;
        private const int _maxWildPokemon = 7;

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
        public int MaxWildPokemon
        {
            get { return _maxWildPokemon; }
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
        public DispatcherTimer GymTimer {
            get { return _gymTimer; }
            set { 
                _gymTimer = value;
                OnPropertyChanged(nameof(GymTimer));
            }
        }
        public DispatcherTimer SpawnTimer
        {
            get { return _spawnTimer; }
            set
            {
                _spawnTimer = value;
                OnPropertyChanged(nameof(SpawnTimer));
            }
        }

        // Constructor of MapViewModel
        public MapViewModel(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            DialogViewModel = (DialogViewModel)MainWindowViewModel.DialogViewModel;
            Rng = new Random();
            GymTimer = new DispatcherTimer();
            SpawnTimer = new DispatcherTimer();
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

            // Link Player to different ViewModels
            ((BagViewModel)MainWindowViewModel.BagViewModel).UpdatePlayer(Player);
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).UpdatePlayer(Player);
            ((ItemViewModel)MainWindowViewModel.ItemViewModel).UpdatePlayer(Player);
            ((ShopViewModel)MainWindowViewModel.ShopViewModel).UpdatePlayer(Player);
            ((GymViewModel)MainWindowViewModel.GymViewModel).UpdatePlayer(Player);
            ((ReceptionViewModel)MainWindowViewModel.ReceptionViewModel).UpdatePlayer(Player);

            //Initialize the timers
            GymTimerInit();
            SpawnTimerInit();
            ((ReceptionViewModel)MainWindowViewModel.ReceptionViewModel).RefreshmentTimerInit();

            // Add more NPC trainers
            LoadReception();
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
            // Link Player to different ViewModels
            ((BagViewModel)MainWindowViewModel.BagViewModel).UpdatePlayer(Player);
            ((PokemonStatusViewModel)MainWindowViewModel.PokemonStatusViewModel).UpdatePlayer(Player);
            ((ItemViewModel)MainWindowViewModel.ItemViewModel).UpdatePlayer(Player);
            ((ShopViewModel)MainWindowViewModel.ShopViewModel).UpdatePlayer(Player);
            ((GymViewModel)MainWindowViewModel.GymViewModel).UpdatePlayer(Player);
            ((GymViewModel)MainWindowViewModel.GymViewModel).UpdateTrainers(Trainers);
            ((ReceptionViewModel)MainWindowViewModel.ReceptionViewModel).UpdatePlayer(Player);

            //Initialize the timers
            GymTimerInit();
            SpawnTimerInit();
            ((ReceptionViewModel)MainWindowViewModel.ReceptionViewModel).RefreshmentTimerInit();

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
        private void LoadReception()
        {
            SafeDistance(out int randomX, out int randomY);
            var reception = new TrainerModel("Reception", "Reception", 1)
            {
                XCoordinate = randomX,
                YCoordinate = randomY,
                Quote = $"Do you want to enter the reception of racecourse? ",
            };
            Trainers.Add(reception);
        }
        private void LoadOtherTrainers()
        {
            string json;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("PokemonGoClone.Resources.Trainers.trainers.json");
            if (stream != null)
            {
                using (var reader = new StreamReader(stream, Encoding.Default))
                {
                    json = reader.ReadToEnd();
                }

                var jArray = JArray.Parse(json);
                
                foreach(var obj in jArray)
                {
                    SafeDistance(out int randomX, out int randomY);
                    TrainerModel trainer = new TrainerModel(obj["Name"].Value<string>(), "NPC", obj["Id"].Value<int>())
                    {
                        XCoordinate = randomX,
                        YCoordinate = randomY,
                        Quote = obj["Quote"].Value<string>()
                    };

                    int randomPokemon;
                    do
                    {
                        randomPokemon = Rng.Next(MainWindowViewModel.Pokemons.Count) + 1;
                    } while (MainWindowViewModel.BeastsId.Contains(randomPokemon));

                    trainer.AddPokemon((PokemonModel)MainWindowViewModel.Pokemons.Find(x => x.Id == randomPokemon).Clone());
                    Trainers.Add(trainer);
                }
            }
        }

        private void LoadGym() {
            SafeDistance(out int randomX, out int randomY);
            TrainerModel gym = new TrainerModel("Gym", "Gym", 1) {
                XCoordinate = randomX,
                YCoordinate = randomY,
                Quote = $"Do you want to enter the gym? ",
            };

            Trainers.Add(gym);
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
                else if (Target.Type == "NPC" || Target.Type == "WildPokemon")
                {
                    DialogViewModel.PopUp(Target.Quote, null, AcceptBattle);
                } else if (Target.Type == "Gym")
                {
                    DialogViewModel.PopUp(Target.Quote, null, EnterGym);
                } else if (Target.Type == "Reception")
                {
                    DialogViewModel.PopUp(Target.Quote, null, EnterReception);
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

        public void GymTimerInit()
        {
            if (((GymViewModel)MainWindowViewModel.GymViewModel).CurrentOccupier == Player) {
                //int time = Rng.Next(300, 600);
                int time = 5;   // Debug
                GymTimer.Interval = new TimeSpan(0, 0, time);
                GymTimer.Tick += GymTimerCount;
                GymTimer.Start();
            } else { 
                // do nothing
            }
        }

        public void GymTimerCount(object sender, EventArgs e) {
            DialogViewModel.PopUp("Someone challange you! You must accept", NotAccept, EnterBattle);
            GymTimer.Stop();
        }

        public void SpawnTimerInit()
        {
            int spawnTimer = Rng.Next(30, 60);
            SpawnTimer.Interval = new TimeSpan(0, 0, spawnTimer);
            SpawnTimer.Tick += SpawnTimerCount;
            SpawnTimer.Start();
        }

        // Random Wild Pokemon Spawn
        public void SpawnTimerCount(object sender, EventArgs e)
        {
            // Check the total number of wild pokemon in the map
            int numberOfWildPokmeon = Trainers.Where(x => x.Type.Equals("WildPokemon")).Count();

            double randomSpawnChane = 0.75;
            double randomDestoryChane = 0.25;

            // If the map is full of pokmeon
            if (numberOfWildPokmeon >= MaxWildPokemon)
            {
                randomSpawnChane = 0;
                randomDestoryChane = 0.75;
            }

            // Random spawn
            if (Rng.NextDouble() <= randomSpawnChane)
            {
                int wildPokemonIndex = Rng.Next(MainWindowViewModel.Pokemons.Count);
                var wildPokemon = MainWindowViewModel.Pokemons[wildPokemonIndex];

                SafeDistance(out int randomX, out int randomY);

                TrainerModel wildPokemonCarrier = new TrainerModel($"newPokemon", "WildPokemon", 0)
                {
                    Quote = $"It is {wildPokemon.Name} (Id: {wildPokemon.Id}). Do you want to catch it? ",
                    XCoordinate = randomX,
                    YCoordinate = randomY,
                    ImageSource = wildPokemon.ImageSource,
                };
                wildPokemonCarrier.AddPokemon((PokemonModel)wildPokemon.Clone());
                Trainers.Add(wildPokemonCarrier);
            }

            // Random destroy oldest pokemon
            if (Rng.NextDouble() <= randomDestoryChane)
            {
                var randomWildPokemonCarrier = Trainers.Where(x => x.Type.Equals("WildPokemon")).FirstOrDefault();
                if(randomWildPokemonCarrier != null)
                {
                    Trainers.Remove(randomWildPokemonCarrier);
                }
            }

            // Update time interval for next random spawn
            int spawnTimer = Rng.Next(30, 60);
            SpawnTimer.Interval = new TimeSpan(0, 0, spawnTimer);
        }

        // Buildings and Trainers must separated at least by a cell to ensure their safety
        // Wild pokemons are free to spawn anywhere however, since they aren't permanent
        private void SafeDistance(out int xCoordinate, out int yCoordinate)
        {
            int[] dX = new int[] {0, -1, -1, 0, 1, 1, 1, 0, -1 };
            int[] dY = new int[] { 0, 0, 1, 1, 1, 0, -1, -1, -1 };
            int randomX, randomY;
            while (true)
            {
                bool safe = true;
                randomX = Rng.Next(1, ROW - 1);
                randomY = Rng.Next(1, ROW - 1);

                foreach(var neighbors in dX.Zip(dY, (x, y) => (dX: x, dY: y)))
                {
                    int neighborX = randomX + neighbors.dX;
                    int neighborY = randomY + neighbors.dY;
                    if (Trainers.Where(x => x.XCoordinate == neighborX && x.YCoordinate == neighborY).FirstOrDefault() != null)
                    {
                        safe = false;
                        break;
                    }
                }

                if (safe)
                {
                    break;
                }
            }
            xCoordinate = randomX;
            yCoordinate = randomY;
        }
    }
}
