using System;
using System.Collections.Generic;
using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLAnimation
    {
        public string Name1;
        public string Name2;
        public float Framerate;
        public int Flag;
        public int FrameCount;
        public GLAnimFrame[] GLAnimFrames;
        
        public GLAnimation(GLReader reader)
        {
            Name1 = reader.ReadString();
            Name2 = reader.ReadString();
            Framerate = reader.ReadSingle();
            Flag = reader.ReadInt32();
            FrameCount = reader.ReadInt32();

            GLAnimFrames = new GLAnimFrame[FrameCount];
            for (var i = 0; i < FrameCount; i++)
            {
                GLAnimFrames[i] = new GLAnimFrame(reader);
            }
        }
    }
}