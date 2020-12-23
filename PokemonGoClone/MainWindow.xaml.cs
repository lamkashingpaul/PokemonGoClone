using System.Windows;

namespace PokemonGoClone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FocusVisualStyleRemover.Init();
        }
        public class FocusVisualStyleRemover
        {
            static FocusVisualStyleRemover()
            {
                EventManager.RegisterClassHandler(typeof(FrameworkElement), FrameworkElement.GotFocusEvent, new RoutedEventHandler(RemoveFocusVisualStyle), true);
            }

            public static void Init()
            {
                // intentially empty
            }

            private static void RemoveFocusVisualStyle(object sender, RoutedEventArgs e)
            {
                (sender as FrameworkElement).FocusVisualStyle = null;
            }
        }
    }
}
