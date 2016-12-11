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
using System.IO;
using Microsoft.ProjectOxford.Common.Contract;

namespace EmojiReplacer
{
    class FaceAnalyzer
    {
        public Queue<Emotion[]> EmotionQueue = new Queue<Emotion[]>();
        private EmotionServiceClient _emotionClient = null;
        private FrameGrabber<Emotion[]> _grabber = new FrameGrabber<Emotion[]>();
        public Emotion[] latestResultsToDisplay;
        private Queue<EmotionServiceClient> Clients = new Queue<EmotionServiceClient>();
        private double[][] _similarVector = new double[][] 
        {
            new double[]{1.35727838e-08,1.35727838e-08,1.35727838e-08,1.35727838e-08,1.35727838e-08,1.35727838e-08,1.35727838e-08,1.35727838e-08},
            new double[]{0.000178523027,0.000178523027,0.000178523027,0.000178523027,0.000178523027,0.000178523027,0.000178523027,0.000178523027},
            new double[]{2.15965633e-06,2.15965633e-06,2.15965633e-06,2.15965633e-06,2.15965633e-06,2.15965633e-06,2.15965633e-06,2.15965633e-06},
            new double[]{0.001781409,0.001781409,0.001781409,0.001781409,0.001781409,0.001781409,0.001781409,0.001781409},
            new double[]{8.85037662e-05,8.85037662e-05,8.85037662e-05,8.85037662e-05,8.85037662e-05,8.85037662e-05,8.85037662e-05,8.85037662e-05},
            new double[]{0.0007612241,0.0007612241,0.0007612241,0.0007612241,0.0007612241,0.0007612241,0.0007612241,0.0007612241},
            new double[]{0.0001304286,0.0001304286,0.0001304286,0.0001304286,0.0001304286,0.0001304286,0.0001304286,0.0001304286},
            new double[]{1.32630146e-06,1.32630146e-06,1.32630146e-06,1.32630146e-06,1.32630146e-06,1.32630146e-06,1.32630146e-06,1.32630146e-06},
            new double[]{7.503085e-11,7.503085e-11,7.503085e-11,7.503085e-11,7.503085e-11,7.503085e-11,7.503085e-11,7.503085e-11},
            new double[]{0.201804966,0.201804966,0.201804966,0.201804966,0.201804966,0.201804966,0.201804966,0.201804966},
            new double[]{3.427151e-05,3.427151e-05,3.427151e-05,3.427151e-05,3.427151e-05,3.427151e-05,3.427151e-05,3.427151e-05},
            new double[]{0.0147484476,0.0147484476,0.0147484476,0.0147484476,0.0147484476,0.0147484476,0.0147484476,0.0147484476},
            new double[]{0.00317437,0.00317437,0.00317437,0.00317437,0.00317437,0.00317437,0.00317437,0.00317437},
            new double[]{9.814246e-05,9.814246e-05,9.814246e-05,9.814246e-05,9.814246e-05,9.814246e-05,9.814246e-05,9.814246e-05},
            new double[]{0.0188797563,0.0188797563,0.0188797563,0.0188797563,0.0188797563,0.0188797563,0.0188797563,0.0188797563},
            new double[]{0.00226253062,0.00226253062,0.00226253062,0.00226253062,0.00226253062,0.00226253062,0.00226253062,0.00226253062},
            new double[]{0.00162561273,0.00162561273,0.00162561273,0.00162561273,0.00162561273,0.00162561273,0.00162561273,0.00162561273},
            new double[]{0.07951822,0.07951822,0.07951822,0.07951822,0.07951822,0.07951822,0.07951822,0.07951822},
            new double[]{0.2982602,0.2982602,0.2982602,0.2982602,0.2982602,0.2982602,0.2982602,0.2982602},
            new double[]{0.125074178,0.125074178,0.125074178,0.125074178,0.125074178,0.125074178,0.125074178,0.125074178},
            new double[]{0.00405515824,0.00405515824,0.00405515824,0.00405515824,0.00405515824,0.00405515824,0.00405515824,0.00405515824},
            new double[]{0.00371108716,0.00371108716,0.00371108716,0.00371108716,0.00371108716,0.00371108716,0.00371108716,0.00371108716},
            new double[]{0.000362736842,0.000362736842,0.000362736842,0.000362736842,0.000362736842,0.000362736842,0.000362736842,0.000362736842},
            new double[]{0.241328314,0.241328314,0.241328314,0.241328314,0.241328314,0.241328314,0.241328314,0.241328314}
        };
        private double _threhold = 0.3;
        public int[] res = null;
        public FaceAnalyzer()
        {
            _emotionClient = new EmotionServiceClient("60d3219c11f04f06ac8e91dcd2657abd");
            Clients.Enqueue(new EmotionServiceClient("60d3219c11f04f06ac8e91dcd2657abd"));
            Clients.Enqueue(new EmotionServiceClient("60d3219c11f04f06ac8e91dcd2657abd"));
            Clients.Enqueue(new EmotionServiceClient("60d3219c11f04f06ac8e91dcd2657abd"));
            Clients.Enqueue(new EmotionServiceClient("60d3219c11f04f06ac8e91dcd2657abd"));
            Clients.Enqueue(new EmotionServiceClient("60d3219c11f04f06ac8e91dcd2657abd"));

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
        public async Task<VideoAggregateRecognitionResult> Analyze(Stream fs)
        {
            var videoOperation = await _emotionClient.RecognizeInVideoAsync(fs);
            VideoOperationResult operationResult = null;
            while (true)
            {
                operationResult = await _emotionClient.GetOperationResultAsync(videoOperation);
                if (operationResult.Status == VideoOperationStatus.Succeeded || operationResult.Status == VideoOperationStatus.Failed)
                {
                    break;
                }
                
                Task.Delay(3000).Wait();
            }
            var emotionRecognitionJsonString = ((VideoOperationInfoResult<VideoAggregateRecognitionResult>)operationResult).ProcessingResult;
            //var emotionRecognitionTracking = JsonConvert.DeserializeObject<EmotionRecognitionResult>(emotionRecognitionTrackingResultJsonString, settings);
            return emotionRecognitionJsonString;
        }
        public int[] Analyzer(Emotion[] emotions)
        {
            int[] res = null;
            res = _similarVector.Select(x=>-1).ToArray();
            for (int i=0;i<emotions.Length;i++)
            {
                var emotion = emotions[i];
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
                    for (int j = 0; j < 8; j++)
                    {
                        a += tmp[j] * _similarVector[k][j];
                    }
                    double b = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        b += tmp[j] * tmp[j];
                    }
                    b = Math.Sqrt(b);
                    double c = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        c += _similarVector[k][j] * _similarVector[k][j];
                    }
                    c = Math.Sqrt(c);
                    double d = a / b / c;

                    if (d > _threhold && d > dot)
                        res[i] = k;
                }
            }
            return res;
        } 

        public Emotion[] Detector(VideoFrame frame)
        {
            var jpg = frame.Image.ToMemoryStream(".jpg");
            // Submit image to API. 
            var emotions = _emotionClient.RecognizeAsync(jpg);

            return emotions.Result;
        }
        private async void DetectorAsync(VideoFrame frame)
        {
            var jpg = frame.Image.ToMemoryStream(".jpg");
            // Submit image to API. 
            Emotion[] emotions = null;
            var client = Clients.Dequeue();
            emotions = await client.RecognizeAsync(jpg);
            Clients.Enqueue(client);
            lock(EmotionQueue)
                EmotionQueue.Enqueue(emotions);
            
        }
    }
}
