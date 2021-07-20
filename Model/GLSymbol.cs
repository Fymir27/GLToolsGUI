using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLSymbol
    {
        public int Ref1;
        public int Ref2;
        public int Flag;
        public int FrameCount;
        public GLFrame[] Frames;

        public GLSymbol(GLReader reader)
        {
            Ref1 = reader.ReadInt32();
            Ref2 = reader.ReadInt32();
            Flag = reader.ReadInt32();
            FrameCount = reader.ReadInt32();
            Frames = new GLFrame[FrameCount];
            for (int i = 0; i < FrameCount; i++)
            {
                Frames[i] = new GLFrame(reader);
            }
        }

        public void Write(GLWriter writer)
        {
            writer.Write(Ref1);
            writer.Write(Ref2);
            writer.Write(Flag);
            writer.Write(FrameCount);
            foreach (var frame in Frames)
            {
                frame.Write(writer);
            }
        }
    }
}