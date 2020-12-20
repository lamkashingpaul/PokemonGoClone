using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;

namespace PokemonGoClone.Models.Items
{
    public class PokeballModel : ItemModel
    {
        //field of Pokemonball
        private double _catchProbability;
        private string _itemType;

        //derived class constructor
        public PokeballModel(string name, int id, int charge, double catchProbability) : base(name, id, charge)
        {
            CatchProbability = catchProbability;
            ItemType = "Pokeball";
            ImageSource = $"/PokemonGoClone;component/Images/Items/Pokeball/{Id:D3}.png";
        }
        public override void Use(TrainerModel trainer, PokemonModel target)
        {
            return;
        }

        //Properties of PokeballModel
        public double CatchProbability
        {
            get { return _catchProbability; }
            set
            {
                _catchProbability = value;
            }
        }

        public string ItemType
        {
            get { return _itemType; }
            private set
            {
                _itemType = value;
            }
        }

    }
}
