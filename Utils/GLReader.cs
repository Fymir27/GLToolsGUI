using System;
using System.IO;
using System.Text;

namespace GLToolsGUI.Utils
{
    public class GLReader : BinaryReader
    {
        public readonly string Path;
        
        public GLReader(FileStream input) : base(input)
        {
            Path = input.Name;
        }

        public void Reset()
        {
            BaseStream.Position = 0;
        }

        public byte[] ReadAllRemainingBytes()
        {
            return ReadBytes((int)BaseStream.Length - (int)BaseStream.Position);
        }

        public override string ReadString()
        {
            return ReadString(ReadInt32());
        }

        public string ReadBytePrefixedString()
        {
            return ReadString(ReadByte());
        }

        public string ReadString(int length)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                sb.Append(ReadChar());
            }

            return sb.ToString();
        }

        public float ReadFloat()
        {
            return ReadSingle();
        }
        
        public static int RoundToInt(float f)
        {
            return (int)Math.Round(f);
        }
    }
}