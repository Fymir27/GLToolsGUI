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
            for (var i = 0; i < ElementCount; i++)
            {
                GLElements[i] = new GLElement(reader);
            }
        }
    }
}