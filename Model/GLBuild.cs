using System;
using System.Collections.Generic;
using System.IO;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Model
{
    public class GLBuild
    {
        /* Serialized Properties */
        public string Magic;
        public int Version;
        public int SymbolCount;
        public int FrameCount;
        public string Root;
        public int TextureCount;
        public string[] Textures;
        public int Flag;
        public GLSymbol[] Symbols;
        public int RefCount;
        public Dictionary<int, string> Refs;
        
        /* Calculated/Loaded Properties */
        public GLTexture RootTexture;
        public Dictionary<string, Tuple<string, MagickImage>[]> Parts;

        public GLBuild(GLReader reader, bool loadTexture = true, bool disassembleTexture = true)
        {
            Magic = reader.ReadString(4);
            Version = reader.ReadInt32();
            SymbolCount = reader.ReadInt32();
            FrameCount = reader.ReadInt32();
            Root = reader.ReadString();
            TextureCount = reader.ReadInt32();
            Textures = new string[TextureCount];
            for (var i = 0; i < TextureCount; i++)
            {
                Textures[i] = reader.ReadString();
            }

            Flag = reader.ReadInt32();

            Symbols = new GLSymbol[SymbolCount];
            for (var i = 0; i < SymbolCount; i++)
            {
                Symbols[i] = new GLSymbol(reader);
            }

            RefCount = reader.ReadInt32();
            Refs = new Dictionary<int, string>();
            for (var i = 0; i < RefCount; i++)
            {
                int hash = reader.ReadInt32();
                string name = reader.ReadString();
                Refs.Add(hash, name);
            }

            if (!loadTexture)
                return;

            string directory = Path.GetDirectoryName(reader.Path) ?? throw new Exception("Invalid Path: " + reader.Path);
            
            // TODO: figure out a better way of loading texture instead of hardcoding
            RootTexture = GLTexture.FromKTexFile(Path.Combine(directory, "atlas0.tex"));

            if (!disassembleTexture)
                return;

            Parts = RootTexture.Disassemble(this);
        }
    }
}