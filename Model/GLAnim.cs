﻿using System;
using System.Collections.Generic;
using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    class GLAnim
    {
        public string Magic;
        public int Version;
        public int ElementCount;
        public int FrameCount;
        public int AnimsCount;
        public GLAnimation[] GLAnimations;

        private const string FileMagicBuild = "ANIM";
        private const int VersionRequired = 7;

        public GLAnim(GLReader reader)
        {
            Magic = reader.ReadString(4);
            Version = reader.ReadInt32();
            if (Magic != FileMagicBuild || Version < VersionRequired)
            {
                throw new FormatException("Invalid file format");
            }
            ElementCount = reader.ReadInt32();
            FrameCount = reader.ReadInt32();
            AnimsCount = reader.ReadInt32();

            GLAnimations = new GLAnimation[AnimsCount];
            for (var i = 0; i < AnimsCount; i++)
            {
                GLAnimations[i] = new GLAnimation(reader);
            }
        }
    }
}