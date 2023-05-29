using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AudioPleer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int mode = 0;
        string[] archiveFiles;
        int currentTrack = 0;

        string[] Traks;
        bool ordered = false;

        public MediaPlayer Media = new MediaPlayer();
        public DispatcherTimer Timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            //Uri relative = new Uri("Start-stop.jpg", UriKind.Relative);

            Media.MediaEnded += IFComplete;
            Media.MediaOpened += MediaOpened;
            Timer.Tick += TrackPosition;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Media.Position = TimeSpan.FromSeconds(e.NewValue);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {


            if (StartButton.Content == "Start")
            {
                Media.Play();
                StartButton.Content = "Stop";
            }
            else
            {
                Media.Pause();
                StartButton.Content = "Start";
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (mode == 0)
            {
                mode = 1;
                RepeatButton.Content = "Отменить повтор";
            }
            else
            {
                mode = 0;
                RepeatButton.Content = "Повтор";
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowDialog();

            Traks = Directory.GetFiles(fbd.SelectedPath);
            archiveFiles = Directory.GetFiles(fbd.SelectedPath);

            Media.Open(new Uri(Traks[0]));
            Media.Play();
            StartButton.Content = "Stop";


        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            currentTrack++;
            if (currentTrack >= Traks.Length)
                currentTrack = 0;

            Media.Open(new Uri(Traks[currentTrack]));
            Media.Play();

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            currentTrack--;
            if (currentTrack < 0)
                currentTrack = Traks.Length - 1;

            Media.Open(new Uri(Traks[currentTrack]));
            Media.Play();

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (ordered == false)
            {
                Random rnd = new Random();
                Traks = Traks.OrderBy(x => rnd.Next()).ToArray();
                ordered = true;
            }
            else
            {
                Traks = archiveFiles;
                ordered = false;
            }

            currentTrack = 0;
            Media.Open(new Uri(Traks[currentTrack]));
            Media.Play();

        }

        public void MediaOpened(object sender, EventArgs e)
        {
            TrackTime.Maximum = Media.NaturalDuration.TimeSpan.Seconds;
        }

        public void TrackPosition(object sender, EventArgs e)
        {
            StartTime.Content = Media.Position.ToString(@"mm\:ss");
            if (Media.HasAudio)
                TimeLeft.Content = TimeSpan.FromSeconds(Media.NaturalDuration.TimeSpan.Seconds - Media.Position.TotalSeconds).ToString(@"mm\:ss");
            TrackTime.Value = Media.Position.TotalSeconds;
        }

        public void IFComplete(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                Media.Position = TimeSpan.Zero;
                Media.Play();
            }
            else
            {
                currentTrack++;
                if (currentTrack >= Traks.Length)
                    currentTrack = 0;

                Media.Open(new Uri(Traks[currentTrack]));
                Media.Play();
            }

        }
    }
}
