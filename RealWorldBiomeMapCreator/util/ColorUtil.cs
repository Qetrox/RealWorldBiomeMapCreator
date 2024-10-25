using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

namespace RealWorldBiomeMapCreator.util
{
    public static class ColorUtil
    {
        public static Rgba32 CalculateAverageColor(Image<Rgba32> image)
        {
            long totalR = 0, totalG = 0, totalB = 0;
            int pixelCount = image.Width * image.Height;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Rgba32 pixel = image[x, y];
                    totalR += pixel.R;
                    totalG += pixel.G;
                    totalB += pixel.B;
                }
            }

            byte avgR = (byte)(totalR / pixelCount);
            byte avgG = (byte)(totalG / pixelCount);
            byte avgB = (byte)(totalB / pixelCount);

            return new Rgba32(avgR, avgG, avgB);
        }

        public static bool IsColorCloseTo(Rgba32 color1, Rgba32 color2, int tolerance)
        {
            int diffR = Math.Abs(color1.R - color2.R);
            int diffG = Math.Abs(color1.G - color2.G);
            int diffB = Math.Abs(color1.B - color2.B);

            if (diffR < 0) diffR = -diffR;
            if (diffG < 0) diffG = -diffG;
            if (diffB < 0) diffB = -diffB;

            return diffR <= tolerance && diffG <= tolerance && diffB <= tolerance;
        }
    }
}
