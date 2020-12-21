using System.Windows.Controls;
using System.Windows.Input;

namespace PokemonGoClone.Views
{
    /// <summary>
    /// Interaction logic for BagView.xaml
    /// </summary>
    public partial class BagView : UserControl
    {
        public BagView()
        {
            InitializeComponent();
            Focusable = true;
            Loaded += (s, e) => Keyboard.Focus(this);
        }
    }
}
