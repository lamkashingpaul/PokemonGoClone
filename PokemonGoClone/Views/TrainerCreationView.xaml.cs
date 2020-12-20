using System.Windows.Controls;
using System.Windows.Input;

namespace PokemonGoClone.Views
{
    /// <summary>
    /// Interaction logic for TrainerCreationView.xaml
    /// </summary>
    public partial class TrainerCreationView : UserControl
    {
        public TrainerCreationView()
        {
            InitializeComponent();
            Focusable = true;
            Loaded += (s, e) => Keyboard.Focus(this);
        }
    }
}
