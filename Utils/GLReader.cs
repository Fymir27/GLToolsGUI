using System.IO;
using System.Text;

namespace GLToolsGUI.Utils
{
    public class GLReader : BinaryReader
    {
        public GLReader(Stream input) : base(input)
        {
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
    }
}