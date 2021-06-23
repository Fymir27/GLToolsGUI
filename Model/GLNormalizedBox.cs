using System;
using System.IO;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Model
{
    public struct GLNormalizedBox
    {
        public float MinX;
        public float MinY;
        public float MaxX;
        public float MaxY;
        public float Width => Math.Abs(MaxX - MinX);
        public float Height => Math.Abs(MaxY - MinY);

        private bool _vertically_inverted;
        
        public GLNormalizedBox(GLReader reader, bool verticallyInverted = false)
        {
            MinX = reader.ReadFloat();
            MinY = reader.ReadFloat();
            MaxX = reader.ReadFloat();
            MaxY = reader.ReadFloat();
            _vertically_inverted = verticallyInverted;
            if (verticallyInverted)
            {
                MinY = 1 - MinY;
                MaxY = 1 - MaxY;
            }
        }

        public MagickGeometry GetScaledGeometry(int totalWidth, int totalHeight, bool floor = false)
        {
            if (floor)
            {
                return new MagickGeometry(
                    (int) (MinX * totalWidth - 0.5),
                    (int) (MinY * totalHeight - 0.5),
                    (int) (MaxX * totalWidth - 0.5) - (int)(MinX * totalWidth - 0.5),
                    (int) (MaxY * totalHeight - 0.5) -(int)(MinY * totalHeight - 0.5));
            }
                
            return new MagickGeometry(
                (int) Math.Round((double)MinX * totalWidth),
                (int) Math.Round((double)MinY * totalHeight),
                (int) Math.Round((double)Width * totalWidth),
                (int) Math.Round((double)Height * totalHeight));
        }

        public void Write(GLWriter writer)
        {
            writer.Write(MinX);
            writer.Write(_vertically_inverted ? 1 - MinY : MinY);
            writer.Write(MaxX);
            writer.Write(_vertically_inverted ? 1 - MaxY : MaxY);
        }
    }
}