using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CameraCaptureUWp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI cc = new CameraCaptureUI();
            cc.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            cc.PhotoSettings.CroppedAspectRatio = new Size(3, 4);
            cc.PhotoSettings.MaxResolution = CameraCaptureUIMaxPhotoResolution.HighestAvailable;
            StorageFile sf = await cc.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (sf != null)
            {
                BitmapImage bmp = new BitmapImage();
                IRandomAccessStream rs = await sf.OpenAsync(FileAccessMode.Read);
                bmp.SetSource(rs);
                image.Source = bmp;
            }
        }
    }
}
