using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using GLToolsGUI.Utils;
using ImageMagick;

namespace GLToolsGUI.Model
{
    class GLPlax
    {
        public byte RootCount;
        public int Width;
        public int Height;
        public string Root;
        public int SpliceCount;

        public GLNormalizedBox[] SrcBoxes;
        public GLNormalizedBox[] DstBoxes;

        private MagickGeometry[] SrcGeometries;
        private MagickGeometry[] DstGeometries;
        
        public MagickImage Combined;

        public string RootPath;
        public GLTexture RootTexture;

        private const string FileMagicPlax = "PLAX";
        private const char RootSeparator = '/';

        public GLPlax(GLReader reader)
        {
            string fileMagic = reader.ReadString(4);
            if (fileMagic != FileMagicPlax)
            {
                throw new FormatException("Invalid file format");
            }

            RootCount = reader.ReadByte();
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
            Root = reader.ReadBytePrefixedString();
            SpliceCount = reader.ReadInt32();

            // TODO: seems bad to hardcode folder levels like this
            // TODO: but it at least allows for referencing of textures from different folders (no idea if necessary)
            RootPath = Path.GetFullPath(Path.Combine(reader.Path, "..", "..", "..", Root));
            RootTexture = ReadRootTexture(RootPath);
            var rootImage = RootTexture.Image;

            SrcBoxes = new GLNormalizedBox[SpliceCount];
            DstBoxes = new GLNormalizedBox[SpliceCount];
            Combined = new MagickImage(MagickColors.Transparent, Width, Height);

            SrcGeometries = new MagickGeometry[SpliceCount];
            DstGeometries = new MagickGeometry[SpliceCount];
            for (var i = 0; i < SpliceCount; i++)
            {
                // Console.WriteLine($"Processing tile nr {i}");

                var srcBox = new GLNormalizedBox(reader);
                SrcBoxes[i] = srcBox;
                var srcGeometry = srcBox.GetScaledGeometry(rootImage.Width, rootImage.Height);
                SrcGeometries[i] = srcGeometry;

                var dstBox = new GLNormalizedBox(reader, true);
                DstBoxes[i] = dstBox;
                var dstGeometry = dstBox.GetScaledGeometry(Width, Height);
                DstGeometries[i] = dstGeometry;
                // Console.WriteLine($"{i} [{dstGeometry.X},{dstGeometry.Y}, {dstGeometry.Width + dstGeometry.X}, {dstGeometry.Height + dstGeometry.Y}]");

                var tile = rootImage.Clone(srcGeometry);
                tile.Resize(dstGeometry.Width, dstGeometry.Height);
                Combined.Composite(tile, Gravity.Northwest, new PointD(dstGeometry.X, dstGeometry.Y),
                    CompositeOperator.Copy);
                // Console.WriteLine($"Cloning from {srcX1},{srcY1} to {dstX1},{dstY1}");
            }

            /*
            var geometriesByY = DstGeometries.GroupBy(g => g.Y).OrderBy(x => x.Key);
            var yValues = geometriesByY.Select(x => x.Key);

            var widths = DstGeometries.GroupBy(g => g.Width).OrderBy(x => x.Key).Select(x => x.Key);
            var heigths = DstGeometries.GroupBy(g => g.Height).OrderBy(x => x.Key).Select(x => x.Key);
            */
        }

        public void Write(GLWriter writer, bool updateOriginalTexture)
        {
            writer.Write(Encoding.ASCII.GetBytes(FileMagicPlax));
            writer.Write(RootCount);
            writer.Write(Width);
            writer.Write(Height);
            writer.WriteBytePrefixed(Root);
            writer.Write(SpliceCount);

            for (var i = 0; i < SpliceCount; i++)
            {
                SrcBoxes[i].Write(writer);
                DstBoxes[i].Write(writer);
            }

            if (!updateOriginalTexture)
                return;

            var newRootImage = RootTexture.Image;
            for (var i = 0; i < SpliceCount; i++)
            {
                var tile = Combined.Clone(DstGeometries[i]);
                tile.Resize(SrcGeometries[i].Width, SrcGeometries[i].Height);
                newRootImage.Composite(tile, Gravity.Northwest, SrcGeometries[i].X, SrcGeometries[i].Y,
                    CompositeOperator.Copy);
            }

            RootTexture.Image = newRootImage;
            RootTexture.Write(RootPath);
        }

        private static GLTexture ReadRootTexture(string realRoot)
        {
            var rootFile = File.OpenRead(realRoot);
            using (var textureReader = new GLReader(rootFile))
            {
                try
                {
                    return new GLTexture(textureReader);
                }
                catch (Exception exception)
                {
                    throw new Exception("Failed to read root texture of PLAX: " + realRoot, exception);
                }
            }
        }
    }
}