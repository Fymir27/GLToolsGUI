﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Model
{
    /// <summary>
    /// Griftlands Texture files are special .dds files with dxt5 compression
    /// They usually start with "KTEX" followed by 5 more "magic" bytes
    /// that I don't know the meaning of but are probably necessary
    /// </summary>
    public class GLTexture
    {
        public MagickImage Image;
        public readonly byte[] MagicBytes;
        public const int MagicByteCount = 5;
        private const string FileMagicKtex = "KTEX";
        private const string FileMagicDds = "DDS ";
        private const string Compression = "dxt5";

        public GLTexture(GLReader reader)
        {
            string fileMagic = reader.ReadString(4);
            switch (fileMagic)
            {
                case FileMagicKtex:
                    MagicBytes = reader.ReadBytes(MagicByteCount);
                    break;
                case FileMagicDds:
                    MagicBytes = null;
                    reader.Reset(); // just a plain DDS file
                    break;
                default:
                    throw new FormatException("Invalid file format");
            }

            Image = new MagickImage(reader.ReadAllRemainingBytes());
        }

        public GLTexture(MagickImage image, byte[] magicBytes)
        {
            ValidateMagicBytes(magicBytes);
            Image = new MagickImage(image) {Format = MagickFormat.Dds};
            Image.Settings.SetDefine(MagickFormat.Dds, "compression", Compression);
            MagicBytes = magicBytes;
        }
        
        public static GLTexture FromKTexFile(string path)
        {
            var textureFile = File.OpenRead(path); // reader will dispose of file automatically
            using var reader = new GLReader(textureFile);
            try
            {
                return new GLTexture(reader);
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to read Texture from file: " + path, exception);
            }
        }

        /// <summary>
        /// Writes Texture file to .tex format
        /// </summary>
        /// <param name="path">path of file to write to (will be created if it doesn't exist)</param>
        /// <exception cref="ArgumentException">if directory in path doesn't exist</exception>
        /// <exception cref="MagickException">if writing the image fails</exception>
        public void Write(string path)
        {
            if (!File.Exists(path))
            {
                throw new ArgumentException("Invalid path: " + path);
            }

            using var outputFile = File.OpenWrite(path);
            Write(outputFile);
        }

        public void Write(Stream outputStream)
        {
            // put everything back in its place
            // "KTEX" + 5 magic bytes from original klei texture + rest of DDS file
            outputStream.Write(Encoding.ASCII.GetBytes(FileMagicKtex), 0, FileMagicKtex.Length);
            outputStream.Write(MagicBytes, 0, MagicBytes.Length);
            Image.Write(outputStream);
        }

        private static void ValidateMagicBytes(IReadOnlyCollection<byte> magicBytes)
        {
            if (magicBytes == null)
            {
                throw new ArgumentException("GLTexture magicBytes cannot be null");
            }

            if (magicBytes.Count != MagicByteCount)
            {
                throw new ArgumentException($"GLTexture invalid magicBytes length: {magicBytes.Count}! (Must be: {MagicByteCount})");
            }
        }
    }
}