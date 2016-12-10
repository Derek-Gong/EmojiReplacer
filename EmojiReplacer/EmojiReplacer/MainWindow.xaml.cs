using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using OpenCvSharp;

namespace EmojiReplacer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
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
            //openfile.Filter = "视频|*.mp4";
           
            

            /*
               ShellContainer selectedFolder = null;
               selectedFolder = KnownFolders.SampleVideos as ShellContainer;
               CommonOpenFileDialog cfd = new Microsoft.WindowsAPICodePack.Dialogs.CommonOpenFileDialog();
               cfd.InitialDirectoryShellContainer = selectedFolder;
               cfd.EnsureReadOnly = true;
               cfd.Filters.Add(new Microsoft.WindowsAPICodePack.Dialogs.CommonFileDialogFilter("WMV Files", "*.wmv"));
               cfd.Filters.Add(new CommonFileDialogFilter("AVI Files", "*.avi")); 
               cfd.Filters.Add(new CommonFileDialogFilter("MP3 Files", "*.mp3"));

               if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
               {
                   In_mediaElement.Source = new Uri(cfd.FileName, UriKind.Relative);
                   playBtn.IsEnabled = true;
               }
               */
        }

        private void PlayerPause()
        {
            //SetPlayer(true);
            if (playBtn.Content.ToString() == "Play")
            {
                In_mediaElement.Play();
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
            PlayerPause();
        }

        private void mediaElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlayerPause();
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            In_mediaElement.Stop();
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

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
