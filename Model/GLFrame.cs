using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLFrame
    {
        public int Ndx; // or Index?
        public int Duration;
        public int BuildIndex;
        public float OffsetX;
        public float OffsetY;
        public float Width;
        public float Height;
        public float U1;
        public float V1;
        public float U2;
        public float V2;
        public float PivotX;
        public float PivotY;

        public GLFrame(GLReader reader)
        {
            Ndx = reader.ReadInt32();
            Duration = reader.ReadInt32();
            BuildIndex = reader.ReadInt32();
            OffsetX = reader.ReadFloat();
            OffsetY = reader.ReadFloat();
            Width = reader.ReadFloat();
            Height = reader.ReadFloat();
            U1 = reader.ReadFloat();
            V1 = reader.ReadFloat();
            U2 = reader.ReadFloat();
            V2 = reader.ReadFloat();
            PivotX = 0 - (OffsetX - Width / 2) / Width;
            PivotY = 1 + (OffsetY - Height / 2) / Height;
        }
    }
}