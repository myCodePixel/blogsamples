using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaCapture1
{
   public enum AudioEncodingFormat
    {
        Mp3,Mp4,Avi,Wma
    };
    public static class AudioEncodingFormatExceptions
    {
        public static string ToFileExtension(this AudioEncodingFormat encodingFormat)
        {
            switch (encodingFormat)
            {
                case AudioEncodingFormat.Mp3:
                    return ".mp3";
                    
                case AudioEncodingFormat.Mp4:
                    return ".mp4";
                case AudioEncodingFormat.Wma:
                    return ".wma";
                case AudioEncodingFormat.Avi:
                    return ".avi";
                default:
                    throw new ArgumentOutOfRangeException("Encoding FOrmat");

            }
        }
    }

}
