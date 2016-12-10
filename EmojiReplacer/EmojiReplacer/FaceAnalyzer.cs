using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Newtonsoft.Json;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Vision;
using VideoFrameAnalyzer;


namespace EmojiReplacer
{
    class FaceAnalyzer
    {
        private EmotionServiceClient _emotionClient = null;
        private FrameGrabber<Emotion[]> _grabber = new FrameGrabber<Emotion[]>();
        public Emotion[] latestResultsToDisplay;
        private double[] _similarVector = null;
        private double _threhold = 0.3;
        public int[] res = null;
        public FaceAnalyzer()
        {
            //_emotionClient = new EmotionServiceClient("<subscription key>");
            //_grabber.AnalysisFunction = Detector;
            //_grabber.NewResultAvailable += (s, e) =>
            //{
            //    //this.Dispatcher.BeginInvoke((Action)(() =>
            //    {
            //        if (e.TimedOut)
            //        {
            //            //MessageArea.Text = "API call timed out.";
            //        }
            //        else if (e.Exception != null)
            //        {
            //            string apiName = "";
            //            string message = e.Exception.Message;
            //            var faceEx = e.Exception as FaceAPIException;
            //            var emotionEx = e.Exception as Microsoft.ProjectOxford.Common.ClientException;
            //            var visionEx = e.Exception as Microsoft.ProjectOxford.Vision.ClientException;
            //            if (faceEx != null)
            //            {
            //                apiName = "Face";
            //                message = faceEx.ErrorMessage;
            //            }
            //            else if (emotionEx != null)
            //            {
            //                apiName = "Emotion";
            //                message = emotionEx.Error.Message;
            //            }
            //            else if (visionEx != null)
            //            {
            //                apiName = "Computer Vision";
            //                message = visionEx.Error.Message;
            //            }
            //            //MessageArea.Text = string.Format("{0} API call failed on frame {1}. Exception: {2}", apiName, e.Frame.Metadata.Index, message);
            //        }
            //        else
            //        {
            //            latestResultsToDisplay = e.Analysis;

            //            Analyzer();
            //            // Display the image and visualization in the right pane. 
            //            //if (!_fuseClientRemoteResults)
            //            //{
            //            //    RightImage.Source = VisualizeResult(e.Frame);
            //            //}
            //        }
            //    }
            //    //)
            //    //);
            //};
        }
        public void Start()
        { }
        private void Analyzer()
        {
            res = _similarVector.Select(x=>-1).ToArray();
            for (int i=0;i<latestResultsToDisplay.Length;i++)
            {
                var emotion = latestResultsToDisplay[i];
                double[] tmp = new double[]
                {
                    emotion.Scores.Anger,
                    emotion.Scores.Contempt,
                    emotion.Scores.Disgust,
                    emotion.Scores.Fear,
                    emotion.Scores.Happiness,
                    emotion.Scores.Neutral,
                    emotion.Scores.Sadness,
                    emotion.Scores.Surprise
                };
                double dot = -1;
                for (int k = 0; k < _similarVector.Length; k++)
                {
                
                    double a = 0;
                    for (int j = 0; i < 8; i++)
                    {
                        a += tmp[i] * _similarVector[k];
                    }
                    double b = 0;
                    for (int j = 0; i < 8; i++)
                    {
                        b += tmp[i] * tmp[i];
                    }
                    b = Math.Sqrt(b);
                    double c = 0;
                    for (int j = 0; i < 8; i++)
                    {
                        c += _similarVector[i] * _similarVector[i];
                    }
                    c = Math.Sqrt(c);
                    double d = a / b / c;

                    if (d > _threhold && d > dot)
                        res[i] = k;
                }
            }

        }

        public Emotion[] Detector(VideoFrame frame)
        {
            var jpg = frame.Image.ToMemoryStream(".jpg");
            // Submit image to API. 
            var emotions = _emotionClient.RecognizeAsync(jpg);
            return emotions.Result;
        }
        //private async Task<Emotion[]> Detector(VideoFrame frame)
        //{
        //    var jpg = frame.Image.ToMemoryStream(".jpg");
        //    // Submit image to API. 
        //    Emotion[] emotions = null;
        //    emotions = await _emotionClient.RecognizeAsync(jpg);
        //    return emotions;
        //}
    }
}
