using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.Core;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VideoCaptureUWP
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

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            CameraCaptureUI cc = new CameraCaptureUI();
            cc.VideoSettings.Format = CameraCaptureUIVideoFormat.Mp4;
            cc.VideoSettings.MaxResolution = CameraCaptureUIMaxVideoResolution.HighDefinition;
            StorageFile sf = await cc.CaptureFileAsync(CameraCaptureUIMode.Video);
            if (sf != null)
            {
                MediaClip mc = await MediaClip.CreateFromFileAsync(sf);
                MediaComposition mcomp = new MediaComposition();
                mcomp.Clips.Add(mc);
                MediaStreamSource mss = mcomp.GeneratePreviewMediaStreamSource((int)mediaElement.ActualWidth, (int)mediaElement.ActualHeight);
                mediaElement.SetMediaStreamSource(mss);
            }
        }
    }
}
