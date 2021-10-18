using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using GLToolsGUI.Utils;

namespace GLToolsGUI.Model
{
    public class GLAnimationSet
    {
        public string Magic;
        public int Version;
        public int ElementCount;
        public int FrameCount;
        public int AnimsCount;
        public int RefCount;
        public GLAnimation[] GLAnimations;
        public Dictionary<int, string> Refs;

        private const string FileMagicBuild = "ANIM";
        private const int VersionRequired = 7;

        public GLAnimationSet(GLReader reader)
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

            //Anims have four "layers" of data to process
            //For reference:
            //GLAnimationSet -> GLAnimation -> GLAnimFrame -> GLElement
            //TODO: create lists for these layers so they can be accessed
            //when writing to scml later
            GLAnimations = new GLAnimation[AnimsCount];
            for (var i = 0; i < AnimsCount; i++)
            {
                GLAnimations[i] = new GLAnimation(reader);
            }

            RefCount = reader.ReadInt32();
            System.Diagnostics.Debug.WriteLine(RefCount);
            Refs = new Dictionary<int, string>();
            for (var i = 0; i < RefCount; i++)
            {
                int hash = reader.ReadInt32();
                string name = reader.ReadString();
                Refs.Add(hash, name);
            }
        }

        public XDocument ToXml()
        {
            var doc = new XDocument
            (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement
                (
                    "spriter_data",
                    GLAnimations.Select
                    (
                        anim => new XElement("folder", new XAttribute("name", anim.Name1))
                    )
                )
            );

            return doc;
        }
    }
}