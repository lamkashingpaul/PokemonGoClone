using PokemonGoClone.Models.Pokemons;
using PokemonGoClone.Models.Trainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoClone.Models.Items
{
    public class PotionModel : ItemModel
    {
        //field of Potion
        private string _itemType;
        private int _healHP;
        public PotionModel(string name, int id, int charge, int healHP) : base(name, id, charge) {
            HealHP = healHP;
            ItemType = "Pokeball";
            ImageSource = $"/PokemonGoClone;component/Images/Items/Potion/{Id:D3}.png";
        }
        public override void Use(TrainerModel trainer, PokemonModel target)
        {
            target.Health = target.MaxHealth;
            Charge -= 1;
        }

        //Properties of PokeballModel
        public int HealHP {
            get { return _healHP; }
            set {
                _healHP = value;
            }
        }

        public string ItemType {
            get { return _itemType; }
            private set {
                _itemType = value;
            }
        }
    }
}
