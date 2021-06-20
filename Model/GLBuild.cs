using System.Collections.Generic;
using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLBuild
    {
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

        public GLBuild(GLReader reader)
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
        }
    }
}