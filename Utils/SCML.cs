using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Documents;
using System.Xaml;
using System.Xml.Linq;
using GLToolsGUI.Model;

namespace GLToolsGUI.Utils
{
    public static class SCML
    {

        private static Dictionary<int, int> _refToFolderID;
        private static Dictionary<int, string> _animationRefs;
        private static List<int> _folderRefs;

        public static bool CreateFile(string filename, GLBuild build, GLAnimationSet animationSet,
            string frameFormat = "png")
        {
            
            Debug.WriteLine("Build Refs: \n" + string.Join("\n", build.Refs.Select((key, value) =>
                $"{key}:{value}")));
            
            Debug.WriteLine("Animation Refs: \n" + string.Join("\n", animationSet.Refs.Select((key, value) =>
                $"{key}:{value}")));

            _folderRefs = build.Symbols.Select(s => s.Ref1).ToList();

            var doc = new XDocument
            (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("spriter_data",
                    new XAttribute("scml_version", "1.0"),
                    new XAttribute("generator", "BrashMonkey Spriter"),
                    new XAttribute("generator_version", "r11"),
                    GetFolders(build.Symbols, build.Refs, frameFormat),
                    new XElement("entity",
                        new XAttribute("id", 0),
                        new XAttribute("name", build.Root),
                        GetAnimations(animationSet, animationSet.Refs)
                    )
                )
            );

            doc.Save(filename);

            return true;
        }

        private static string GetFullAnimationName(GLAnimation animation)
        {
            return animation.Name1 == animation.Name2 ? animation.Name1 : $"{animation.Name1}-{animation.Name2}";
        }

        private static IEnumerable<XElement> GetAnimations(GLAnimationSet animationSet,
            Dictionary<int, string> animationRefs)
        {
            return animationSet.GLAnimations.Select((animation, animationIndex) => new XElement("animation",
                new XAttribute("name", GetFullAnimationName(animation)),
                new XAttribute("id", animationIndex),
                new XAttribute("length", animation.Framerate * animation.FrameCount),
                new XAttribute("interval", 100), // TODO: should this always the same?
                new XElement("mainline", GetMainlineKeys(animation, animationIndex, animationRefs)),
                GetTimelines(animation, animationIndex, animationRefs)
            ));
        }

        private static IEnumerable<XElement> GetTimelines(GLAnimation animation, int animationIndex,
            Dictionary<int, string> animationRefs)
        {
            int timelineID = 0;
            return animation.GLAnimFrames
                .SelectMany
                (
                    frame => frame.GLElements.Select
                    (
                        element => KeyValuePair.Create((element.Ref, element.Layer), element) // TODO: check if this works
                    )
                )
                .GroupBy
                (
                    x => x.Key,
                    x => x.Value,
                    (elementIdentification, elementStates) => new XElement("timeline",
                        new XAttribute("id", elementStates.First().index), // TODO: actually create element lookup with indexes and shit
                        new XAttribute("name", $"{animationRefs[elementIdentification.Ref]}-{animationRefs[(int)elementIdentification.Layer]}"), // TODO: uint/int??
                        new XAttribute("object_type", _folderRefs.IndexOf(elementIdentification.Ref) == -1 ? "entity" : "sprite"),
                        GetTimelineKeys(elementStates, animation.Framerate, animationRefs, animationIndex)
                    )
                );
        }

        private static IEnumerable<XElement> GetTimelineKeys(IEnumerable<GLElement> elementStates, float framerate,
            Dictionary<int, string> animationRefs, int folderIndex)
        {
            return elementStates.Select((elementState, frameIndex) => new XElement("key",
                new XAttribute("id", frameIndex),
                new XAttribute("time", frameIndex * framerate),
                new XAttribute("curve_type", "INSTANT"),
                new XAttribute("spin", 1), // TODO: ?
                GetSpatialInfo(elementState, folderIndex)
            ));
        }

        private static IEnumerable<XElement> GetObjectRefs(GLAnimFrame frame, int frameIndex, int folderIndex,
            Dictionary<int, string> animationRefs)
        {
            // TODO: no element info in this at all?
            return frame.GLElements.Select((element, elementIndex) => new XElement("object_ref",
                new XAttribute("id", elementIndex),
                new XAttribute("timeline", element.index), // TODO: get index of ref instead?
                new XAttribute("key", frameIndex),
                //new XAttribute("folder", folderIndex),
                //new XAttribute("file", animationRefs[element.Ref]),
                new XAttribute("z_index", frame.ElementCount - elementIndex)
            ));
        }

        private static IEnumerable<XElement> GetMainlineKeys(GLAnimation animation, int folderIndex,
            Dictionary<int, string> animationRefs)
        { 
            return animation.GLAnimFrames
                .Select((frame, frameIndex) => new XElement("key",
                new XAttribute("id", frameIndex),
                new XAttribute("time", frameIndex * animation.Framerate),
                GetObjectRefs(frame, frameIndex, folderIndex, animationRefs)
            ));
        }

        private static IEnumerable<XElement> GetFolders(IEnumerable<GLSymbol> symbols,
            IReadOnlyDictionary<int, string> refs, string frameFormat)
        {
            return symbols.Select((symbol, symbolIndex) => new XElement("folder",
                new XAttribute("name", refs[symbol.Ref1]), // TODO: what about Ref2?
                new XAttribute("id", symbolIndex),
                GetFrames(symbol.Frames, refs[symbol.Ref1], frameFormat)
            ));
        }

        private static IEnumerable<XElement> GetFrames(IEnumerable<GLFrame> frames, string dir, string frameFormat)
        {
            return frames
                .Where(frame => !(float.IsNaN(frame.PivotX) || float.IsNaN(frame.PivotY)))
                .Select((frame, frameIndex) => new XElement("file",
                    new XAttribute("id", frame.Index),
                    new XAttribute("name", $"{dir}/{frame.Index}.{frameFormat}"),
                    new XAttribute("width", frame.Width),
                    new XAttribute("height", frame.Height),
                    new XAttribute("pivot_x", frame.PivotX),
                    new XAttribute("pivot_y", frame.PivotY)
                ));
        }

        private static XElement GetSpatialInfo(GLElement element, int _)
        {
            var objectNode = new XElement("object",
                //new XAttribute("entity", 0),
                new XAttribute("x", element.X),
                new XAttribute("y", element.Y),
                new XAttribute("angle", element.Angle),
                new XAttribute("scale_x", element.ScaleX),
                new XAttribute("scale_y", element.ScaleY),
                new XAttribute("a", element.a)
                //new XAttribute("spin", element.Spin)
            );

            int folderIndex = _folderRefs.IndexOf(element.Ref);
            if (folderIndex >= 0)
            {
                objectNode.Add
                (
                    new XAttribute("folder", folderIndex),
                    new XAttribute("file", element.Ndx)
                );
            }
            else
            {
                objectNode.Add
                (
                    new XAttribute("entity", 0),
                    new XAttribute("t", 0), // TODO
                    new XAttribute("animation", 5) // TODO
                );
            }

            return objectNode;
        }
    }
}