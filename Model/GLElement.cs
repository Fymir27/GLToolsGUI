using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLElement
    {
        //Read from file
        public int Ref;
        public int Ndx;
        public uint Layer;
        public float a;
        public float b;
        public float g;
        public float r;
        public float uk1;
        public float uk2;
        public float uk3;
        public float uk4;
        public float m1;
        public float m2;
        public float m3;
        public float m4;
        public float mX;
        public float mY;
        public float index;

        //Calculated values
        public float X;
        public float Y;
        public float ScaleX;
        public float ScaleY;
        public float Angle;
        public float Spin;
        public GLElement(GLReader reader)
        {
            Ref = reader.ReadInt32();
            Ndx = reader.ReadInt32();
            Layer = reader.ReadUInt32();
            a = reader.ReadFloat();
            b = reader.ReadFloat();
            g = reader.ReadFloat();
            r = reader.ReadFloat();
            uk1 = reader.ReadFloat();
            uk2 = reader.ReadFloat();
            uk3 = reader.ReadFloat();
            uk4 = reader.ReadFloat();
            m1 = reader.ReadFloat();
            m2 = reader.ReadFloat();
            m3 = reader.ReadFloat();
            m4 = reader.ReadFloat();
            mX = reader.ReadFloat();
            mY = reader.ReadFloat();
            index = reader.ReadFloat();
        }
    }
}