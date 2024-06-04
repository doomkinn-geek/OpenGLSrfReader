using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLSrfReader.Services
{
    static class ImageProcessing
    {
        public static ushort[] HarmonizeImage(ushort[] sourceData)
        {
            int length = sourceData.Length;
            ushort[] result = new ushort[length];

            ushort MinPixValue = 32767;
            ushort MaxPixValue = 0;

            ushort MinHisValue;
            ushort MaxHisValue;

            float k;
            float black;
            int i = 0;

            long[] histogram = new long[32768];

            for (i = 0; i < length; i++)
            {
                ushort CurrentValue = sourceData[i];

                // LIMITING value to 14 bit
                if (CurrentValue < 0) { CurrentValue = 0; }
                if (CurrentValue > 16383) { CurrentValue = 16383; }

                // *****Max/Min value calculation*****
                if (CurrentValue > MaxPixValue) { MaxPixValue = CurrentValue; }
                if (CurrentValue < MinPixValue) { MinPixValue = CurrentValue; }

                // *****Histogram building*****
                histogram[CurrentValue]++;
            }

            // Histogram DownSearch
            for (i = 16383; i >= 0; i--)
            {
                if (histogram[i] > 100) { break; }
            }

            MaxHisValue = (ushort)i;

            // Histogram UpSearch
            for (i = 0; i <= 16383; i++)
            {
                if (histogram[i] > 100) { break; }
            }

            MinHisValue = (ushort)i;

            // Calculation of Image Dynamic
            int different = MaxHisValue - MinHisValue;
            if (different == 0) { different = 1; } // prevent division by 0

            // Calculation of constant multiplier for this dynamic
            k = (float)(16383.00 / different);

            // Set black level
            black = (float)MinHisValue;

            for (i = 0; i < length; i++)
            {
                float inpix = (float)sourceData[i];

                // black level, convert. dynRange
                float outpix = ((inpix - black) * k);

                // limiting 
                if (outpix > 16383.00) { outpix = 16383; }
                if (outpix < 0.00) { outpix = 0; }

                result[i] = (ushort)outpix;
            }

            return result;
        }

        public static ushort[] ApplyBrightnessAndContrast(ushort[] sourceData, int brightness, int contrast, bool negative = false)
        {
            int length = sourceData.Length;
            ushort[] result = new ushort[length];

            float mBrightness = (float)brightness;
            float mContrast = (float)contrast;
            float medium = 8191;
            float contrastFactor = (mContrast + medium) / medium;
            float mB = medium + mBrightness;

            if (!negative)
            {
                Parallel.For(0, length, i =>
                {
                    float oldBrightness = sourceData[i];
                    float newBrightness = ((oldBrightness - medium) * contrastFactor) + mB;
                    //newBrightness = newBrightness / 64;

                    if (newBrightness > 16383)
                    {
                        newBrightness = 16383;
                    }

                    if (newBrightness < 0)
                    {
                        newBrightness = 0;
                    }

                    result[i] = (ushort)newBrightness;
                });
            }
            else
            {
                Parallel.For(0, length, i =>
                {
                    float oldBrightness = sourceData[i];
                    float newBrightness = (((16383 - oldBrightness) - medium) * contrastFactor) + mB;
                    //newBrightness = newBrightness / 64;

                    if (newBrightness > 16383)
                    {
                        newBrightness = 16383;
                    }

                    if (newBrightness < 0)
                    {
                        newBrightness = 0;
                    }

                    result[i] = (ushort)newBrightness;
                });
            }

            return result;

        }
    }
}
