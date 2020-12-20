using System.Windows.Controls;
using System.Windows.Input;

namespace PokemonGoClone.Views
{
    /// <summary>
    /// Interaction logic for PokemonStatusView.xaml
    /// </summary>
    public partial class PokemonStatusView : UserControl
    {
        public PokemonStatusView()
        {
            InitializeComponent();
            Focusable = true;
            Loaded += (s, e) => Keyboard.Focus(this);
        }
    }
}
