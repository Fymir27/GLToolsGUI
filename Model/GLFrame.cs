using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLFrame
    {
        public int Index; // or Index?
        public int Duration;
        public int BuildIndex;
        public float OffsetX;
        public float OffsetY;
        public float Width;
        public float Height;
        public GLNormalizedBox boundingBox;
        public float PivotX;
        public float PivotY;

        public GLFrame(GLReader reader)
        {
            Index = reader.ReadInt32();
            Duration = reader.ReadInt32();
            BuildIndex = reader.ReadInt32();
            OffsetX = reader.ReadFloat();
            OffsetY = reader.ReadFloat();
            Width = reader.ReadFloat();
            Height = reader.ReadFloat();
            boundingBox = new GLNormalizedBox(reader);
            PivotX = 0 - (OffsetX - Width / 2) / Width;
            PivotY = 1 + (OffsetY - Height / 2) / Height;
        }
    }
}