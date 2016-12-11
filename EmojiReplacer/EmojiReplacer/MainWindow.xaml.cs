
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VideoFrameAnalyzer;
using System.Drawing;

namespace EmojiReplacer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        //Button openBtn, playBtn, stopBtn, backBtn, forwardBtn;
       // MediaElement mediaElement_MouseLeftButtonUp;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, RoutedEventArgs e)
        {
            
            
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            if(openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                In_mediaElement.Source = new Uri(openfile.FileName, UriKind.Absolute);
                playBtn.IsEnabled = true;
            }
            
        }



        private void PlayerPause()
        {
            //SetPlayer(true);
            if (playBtn.Content.ToString() == "Play")
            {
                In_mediaElement.Play();


                Out_mediaElement.Source = In_mediaElement.Source;


                // WriteableBitmap bitmap = new WriteableBitmap(In_mediaElement);

                // Cv2.ImRead("C:\\Users\\Xmagicer\\Desktop\\hacka.jpg");

                //Image img = Image("C:\\Users\\Xmagicer\\Desktop\\hacka.jpg");  
                Mat now =new Mat("C:\\Users\\Xmagicer\\Desktop\\hacka.jpg");
                //byte img = new byte();
              //  now = Mat.FromImageData(In_mediaElement.Source.ToString, ImreadModes.Color);
                VideoFrameMetadata data ;
                data.Index = 0;
                data.Timestamp = DateTime.Now;
                VideoFrame frame = new VideoFrame(now, data);
                FaceAnalyzer faceimage = new FaceAnalyzer();


                // faceimage.Detector(frame);
                Microsoft.ProjectOxford.Emotion.Contract.Emotion[] emoji= faceimage.Detector(frame);
               Console.WriteLine(emoji[0]);

                //WriteableBitmap bitmap = new WriteableBitmap(this.Player, null);

                //Image img = ImageFrome

                Out_mediaElement.Play();
                playBtn.Content = "Pause";
                In_mediaElement.ToolTip = "Click to Pause";
            }
            else
            {
                In_mediaElement.Pause();
                playBtn.Content = "Play";
                In_mediaElement.ToolTip = "Click to Play";
            }
        }

        private void SetPlayer(bool v)
        {
            throw new NotImplementedException();
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            //VideoFrame sh =In_mediaElement.PointFromScreen();

            //VideoFrameMetadata data = new VideoFrameMetadata();
            //VideoFrame a = new VideoFrame(Cv2.ImRead("C:\\Users\\Xmagicer\\Desktop\\hacka.jpg"),data);
            //using (lpllmage)
            PlayerPause();

        }

        private void mediaElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlayerPause();
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            In_mediaElement.Stop();
            Out_mediaElement.Stop();
            PlayerPause();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            In_mediaElement.Position = In_mediaElement.Position - TimeSpan.FromSeconds(10);
        }

        private void forwardBtn_Click(object sender, RoutedEventArgs e)
        {
            In_mediaElement.Position = In_mediaElement.Position + TimeSpan.FromSeconds(10);
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        //private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

    }
}
