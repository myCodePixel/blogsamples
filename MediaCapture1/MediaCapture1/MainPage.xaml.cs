using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MediaCapture1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public enum RecordingMode
        {
            Initializing,
            Recording,
            Stopped
        };

        private MediaCapture CaptureMedia;
        private IRandomAccessStream AudioStream;
        private FileSavePicker FileSave;
        private DispatcherTimer DishTImer;
        private TimeSpan SpanTime;
        private AudioEncodingFormat selectedFormat;
        private AudioEncodingQuality SelectedQuality;

        public MainPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitMediaCapture();
            LoadAudioEncodings();
            LoadAudioQualities();
            UpdateRecordingControls(RecordingMode.Initializing);
            InitTimer();

        }

        private void InitTimer()
        {
            DishTImer = new DispatcherTimer();
            DishTImer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            DishTImer.Tick += DishTImer_Tick;
        }

        private void DishTImer_Tick(object sender, object e)
        {
            SpanTime = SpanTime.Add(DishTImer.Interval);
            Duration.DataContext = SpanTime;
        }

        private void UpdateRecordingControls(RecordingMode recordingMode)
        {
            
        }

        private void LoadAudioQualities()
        {
            var audioQualities = Enum.GetValues(typeof(AudioEncodingFormat)).Cast<AudioEncodingQuality>();
            AudioQuality.ItemsSource = audioQualities;
            AudioQuality.SelectedItem=AudioEncodingQuality.Auto;
        }

        private void LoadAudioEncodings()
        {
            var audioEncodingFormats = Enum.GetValues(typeof(AudioEncodingFormat)).Cast<AudioEncodingFormat>();
            AudioFormat.ItemsSource = audioEncodingFormats;
            AudioFormat.SelectedItem = AudioEncodingFormat.Mp3;
        }

        private async Task InitMediaCapture()
        {
            CaptureMedia = new MediaCapture();
            var captureSettings = new MediaCaptureInitializationSettings();
            captureSettings.StreamingCaptureMode = StreamingCaptureMode.Audio;
            await CaptureMedia.InitializeAsync(captureSettings);
            CaptureMedia.Failed += CaptureMedia_Failed;
            CaptureMedia.RecordLimitationExceeded += CaptureMedia_RecordLimitationExceeded;
        }

        private async void CaptureMedia_RecordLimitationExceeded(MediaCapture sender)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
             {
                 await sender.StopRecordAsync();
                 var warningMessage = new MessageDialog("The media recording has been stopper as you have exceeded the maximum length", "Recording Stopped");
                 await warningMessage.ShowAsync();

             });
        }

        private async void CaptureMedia_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
             {
                 var warningMessage = new MessageDialog("The media recording Failed " + errorEventArgs.Message, "Capture Failed");
                 await warningMessage.ShowAsync();
             });
        }

        

        

        private async void Record_click(object sender, RoutedEventArgs e)
        {
            MediaEncodingProfile encodingProfile = null;
            switch(selectedFormat)
            {
                case AudioEncodingFormat.Mp3:
                    encodingProfile = MediaEncodingProfile.CreateMp3(SelectedQuality);
                    break;
                case AudioEncodingFormat.Mp4:
                    encodingProfile = MediaEncodingProfile.CreateMp3(SelectedQuality);
                    break;
                case AudioEncodingFormat.Wma:
                    encodingProfile = MediaEncodingProfile.CreateWav(SelectedQuality);
                    break;
                case AudioEncodingFormat.Avi:
                    encodingProfile = MediaEncodingProfile.CreateMp3(SelectedQuality);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();

            }

            AudioStream = new InMemoryRandomAccessStream();
            await CaptureMedia.StartRecordToStreamAsync(encodingProfile, AudioStream);
            UpdateRecordingControls(RecordingMode.Recording);
            DishTImer.Start();

        }

        private async void Stop_Click(object sender, RoutedEventArgs e)
        {
            await CaptureMedia.StopRecordAsync();
            DishTImer.Stop();
            UpdateRecordingControls(RecordingMode.Stopped);
        }

        private async void Save_click(object sender, RoutedEventArgs e)
        {
            var mediaFile = await FileSave.PickSaveFileAsync();
            if(mediaFile!=null)
            {
                using (var dataReader = new DataReader(AudioStream.GetInputStreamAt(0)))
                {
                    await dataReader.LoadAsync((uint)AudioStream.Size);
                    byte[] buffer = new byte[(int)AudioStream.Size];
                    dataReader.ReadBytes(buffer);
                    await FileIO.WriteBytesAsync(mediaFile, buffer);
                    UpdateRecordingControls(RecordingMode.Initializing);

                }
            }
        }

        private void AudioQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedQuality = (AudioEncodingQuality)AudioQuality.SelectedItem;
        }

        private void AudioFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedFormat = (AudioEncodingFormat)AudioFormat.SelectedItem;
            InitFileSavePicker();
        }

        private void InitFileSavePicker()
        {
            FileSave = new FileSavePicker();
            FileSave.FileTypeChoices.Add("Encoding", new List<string>() { selectedFormat.ToFileExtension() });
            FileSave.SuggestedStartLocation = PickerLocationId.MusicLibrary;

        }
    }
}
