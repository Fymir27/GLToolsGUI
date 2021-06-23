using System.IO;

namespace GLToolsGUI.Utils
{
    public class GLWriter : BinaryWriter
    {
        public readonly string Path;
        
        public GLWriter(FileStream output) : base(output)
        {
            Path = output.Name;
        }
        
        public override void Write(string value)
        {
            Write(value.Length);
            foreach (char c in value)
            {
                Write(c);
            }
        }

        public void WriteBytePrefixed(string value)
        {
            Write((byte)value.Length);
            foreach (char c in value)
            {
                Write((byte)c);
            }
        }
    }
}