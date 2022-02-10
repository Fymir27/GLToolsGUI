using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLAnimFrame
    {
        public float x;
        public float y;
        public float Width;
        public float Height;
        public int ElementCount;
        public GLElement[] GLElements;
        public GLAnimFrame(GLReader reader)
        {
            x = reader.ReadFloat();
            y = reader.ReadFloat();
            Width = reader.ReadFloat();
            Height = reader.ReadFloat();
            ElementCount = reader.ReadInt32();

            GLElements = new GLElement[ElementCount];
            double prevAngle = 0;
            float prevScaleX = 0;
            float prevScaleY = 0;
            for (var i = 0; i < ElementCount; i++)
            {
                var el = new GLElement(reader, prevAngle, prevScaleX, prevScaleY);
                prevAngle = el.Angle;
                prevScaleX = el.ScaleX;
                prevScaleY = el.ScaleY;
                GLElements[i] = el;
            }
        }
    }
}