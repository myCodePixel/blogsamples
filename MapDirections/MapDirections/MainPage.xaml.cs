using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.Services.Maps;
using Windows.Devices.Geolocation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MapDirections
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.MapControl1.PointerPressed += MapControl1_PointerPressed;
        }

        private void MapControl1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }
    }
}
