using System;
using System.IO;
using System.Text;

namespace GLToolsGUI.Utils
{
    public class GLReader : BinaryReader
    {
        public const int FileMagicLength = 4;
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

        /**
         * Peeks next 4 characters and returns them as a string
         */
        public string PeekFileMagic()
        {
            long position = BaseStream.Position;
            Reset();
            string magic = ReadString(FileMagicLength);
            BaseStream.Position = position;
            return magic;
        }

        public bool HasFileMagic(string expectedMagic)
        {
            if (expectedMagic.Length != FileMagicLength)
            {
                throw new ArgumentException($"File magic is expected to be {FileMagicLength} characters long!");
            }

            return expectedMagic.ToUpper() == PeekFileMagic();
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
        
        /// <param name="directory"></param>
        /// <param name="filename"></param>
        /// <returns>full path of file</returns>
        /// <exception cref="Exception">when filename doesn't exist</exception>
        public static string RequireFile(string directory, string filename)
        {
            var fileInfo = new FileInfo(System.IO.Path.Combine(directory, filename));
            if (!fileInfo.Exists)
            {
                throw new Exception($"File missing: {fileInfo.FullName}");
            }

            return fileInfo.FullName;
        }
    }
}