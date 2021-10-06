using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Dictionary<string, Dictionary<string, MagickImage>> Parts;

        public GLBuild(GLReader reader, bool loadRootTexture = true, bool disassembleIntoParts = true)
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

            if (!loadRootTexture)
                return;

            string directory = Path.GetDirectoryName(reader.Path) ?? throw new Exception("Invalid Path: " + reader.Path);
            
            // TODO: figure out a better way of loading texture instead of hardcoding
            string rootTexturePath = GLReader.RequireFile(directory, "atlas0.tex");
            
            RootTexture = GLTexture.FromKTexFile(rootTexturePath);

            if (disassembleIntoParts)
            {
                DisassembleRootTexture();   
            }
        }

        public void UpdatePartsFromDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new ArgumentException($"Directory doesn't exist: {directory}");
            }

            var newParts = new Dictionary<string, Dictionary<string, MagickImage>>();
            foreach (var symbol in Symbols)
            {
                var frames = new Dictionary<string, MagickImage>();
                
                string symbolName = Refs[symbol.Ref1];
                string symbolDirectoryPath = Path.Combine(directory, symbolName);
                
                if (!Directory.Exists(symbolDirectoryPath))
                {
                    throw new Exception($"Symbol directory not found: {symbolDirectoryPath}");
                }
                
                foreach (var frame in symbol.Frames)
                {
                    string frameFileName = frame.Index + ".png";
                    string frameFilePath = Path.Combine(symbolDirectoryPath, frameFileName);
                    if (!File.Exists(frameFilePath))
                    {
                        throw new Exception($"Frame file not found: {frameFilePath}");
                    }

                    var frameImage = new MagickImage(frameFilePath);
                    frames.Add(frame.Index.ToString(), frameImage);
                }
                
                newParts.Add(symbolName, frames);
            }

            Parts = newParts;
        }

        public void Write(GLWriter writer, bool updateRootTexture = true, bool assembleFromParts = true)
        {
            writer.WriteWithoutLength(Magic);
            writer.Write(Version);
            writer.Write(SymbolCount);
            writer.Write(FrameCount);
            writer.Write(Root);
            writer.Write(TextureCount);

            foreach (string textureName in Textures)
            {
                writer.Write(textureName);
            }
            
            writer.Write(Flag);

            foreach (var symbol in Symbols)
            {
                symbol.Write(writer);
            }
            
            writer.Write(RefCount);

            foreach ((int hash, string name) in Refs)
            {
                writer.Write(hash);
                writer.Write(name);
            }

            if (!updateRootTexture)
                return;

            if (RootTexture == null)
            {
                throw new ArgumentException("Root Texture cannot be updated if original wasn't loaded first!");
            }
            
            if (assembleFromParts)
            {
                UpdateRootTextureFromParts();
            }
            
            // TODO: figure out a better way of saving texture instead of hardcoding filename
            string directory = Path.GetDirectoryName(writer.Path) ?? throw new Exception("Invalid Path: " + writer.Path);
            RootTexture.Write(Path.Combine(directory, "atlas0.tex"));
        }
        
        public void DisassembleRootTexture()
        {
            var image = RootTexture.Image;
            Parts = new Dictionary<string, Dictionary<string, MagickImage>>();

            foreach (var symbol in Symbols)
            {
                var frames = new Dictionary<string, MagickImage>();
                foreach (var frame in symbol.Frames)
                {
                    if (frame.BuildIndex == -1)
                    {
                        continue;
                    }
                    var frameGeometry = frame.BoundingBox.GetScaledGeometry(image.Width, image.Height);
                    var frameImage = new MagickImage(image);
                    frameImage.Crop(frameGeometry);
                    frames.Add(frame.Index.ToString(), frameImage);
                }
                string symbolName = Refs[symbol.Ref1];
                Parts.TryAdd(symbolName, frames);
                string symbolName2 = Refs[symbol.Ref2];
                if (symbolName2.Length > 0)
                {
                    Parts.TryAdd(symbolName2, frames);                        
                }
                Debug.WriteLine($"Ref1: {symbolName}, Ref2: {symbolName2}");
            }
        }

        public void UpdateRootTextureFromParts()
        {
            foreach (var symbol in Symbols)
            {
                foreach (var frame in symbol.Frames)
                {
                    string symbolName = Refs[symbol.Ref1];
                    var tile = Parts[symbolName][frame.Index.ToString()];
                    var geometry = frame.BoundingBox.GetScaledGeometry(RootTexture.Image.Width, RootTexture.Image.Height);
                    RootTexture.Image.Composite(tile, Gravity.Northwest, geometry.X, geometry.Y, CompositeOperator.Copy);
                }
            }
        }
    }
}