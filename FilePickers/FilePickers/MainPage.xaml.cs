using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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

namespace FilePickers
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

        private async void Pick_image(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fp = new FileOpenPicker(); //create an object for FileOpenPicker class
            fp.SuggestedStartLocation = PickerLocationId.PicturesLibrary; //Let the start location to browse the files be the Pictures Library
            // Goto Capabilities in Package.AppManifest file and give the capabilities to access files from pictures library
            fp.FileTypeFilter.Add(".jpeg"); //add the file types that you want to view.
            fp.FileTypeFilter.Add(".png");
            fp.FileTypeFilter.Add(".bmp");
            fp.FileTypeFilter.Add(".jpg");
            StorageFile sf= await fp.PickSingleFileAsync(); //PickSingleFileAsync will retun a StorageFile. Let us save the file in the object of StorageFile class.
                                                            // We can read the file as a Stream from the object of the storage file.

            BitmapImage bmp = new BitmapImage(); //create an object of the BitmapImage Class so you can access the image in the form of streams for image Control
            IRandomAccessStream stream =await sf.OpenAsync(FileAccessMode.Read); // Read the image as streams using IRandomAccessStreams with FileAccessMode with Read Permissions.
            bmp.SetSource(stream);// set the bmp source to IRandomAcessStream 
            image.Source = bmp;// asign the source to bmp.
        }
    }
}
