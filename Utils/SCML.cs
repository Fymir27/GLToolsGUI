using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Xml.Linq;
using GLToolsGUI.Model;

namespace GLToolsGUI.Utils
{
    public static class SCML
    {
        public static bool createFile(string filename, GLBuild build, GLAnimationSet animationSet, string frameFormat = "png")
        {
            var doc = new XDocument
            (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("spriter_data", 
                    GetFolders(build.Symbols, build.Refs, frameFormat),
                    new XElement("entity",
                           GetAnimations(animationSet)
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
        
        private static IEnumerable<XElement> GetAnimations(GLAnimationSet animationSet)
        {
            return animationSet.GLAnimations.Select((animation, animationIndex) => new XElement("animation",
                new XAttribute("name", GetFullAnimationName(animation)),
                new XAttribute("id", animationIndex),
                new XAttribute("length", animation.Framerate * animation.FrameCount),
                new XAttribute("interval", 100), // TODO: should this always the same?
                new XElement("mainline", GetMainlineKeys(animation)),
                new XElement("timeline", GetTimelineKeys(animation))
            ));
        }

        private static IEnumerable<XElement> GetTimelineKeys(GLAnimation animation)
        {
            return animation.GLAnimFrames.Select((frame, frameIndex) => new XElement("key",
                new XAttribute("id", frameIndex),
                new XAttribute("time", frameIndex * animation.Framerate),
                new XAttribute("curve_type", "instant"), // TODO: always the same?
                GetObjectRefs(frame, frameIndex)
            ));
        }

        private static IEnumerable<XElement> GetObjectRefs(GLAnimFrame frame, int frameIndex)
        {
            // TODO: no element info in this at all?
            return frame.GLElements.Select((element, elementIndex) => new XElement("object_ref",
                new XAttribute("id", elementIndex),
                new XAttribute("timeline", 0), // TODO: always the same? What does it mean?
                new XAttribute("key", frameIndex),
                new XAttribute("z_index", frame.ElementCount - elementIndex)
            ));
        }

        private static IEnumerable<XElement> GetMainlineKeys(GLAnimation animation)
        {
            return Array.Empty<XElement>();
        }

        private static IEnumerable<XElement> GetFolders(IEnumerable<GLSymbol> symbols, IReadOnlyDictionary<int, string> refs, string frameFormat)
        {
            return symbols.Select((symbol, symbolIndex) => new XElement("folder",
                new XAttribute("name", refs[symbol.Ref1]), // TODO: what about Ref2?
                new XAttribute("id", symbolIndex),
                GetFrames(symbol.Frames, frameFormat)
            ));
        }

        private static IEnumerable<XElement> GetFrames(IEnumerable<GLFrame> frames, string frameFormat)
        {
            return frames
                .Where(frame => !(float.IsNaN(frame.PivotX) || float.IsNaN(frame.PivotY)))    
                .Select(frame => new XElement("file",
                new XAttribute("name", $"{frame.Index}.{frameFormat}"),
                new XAttribute("width", frame.Width),
                new XAttribute("height", frame.Height),
                new XAttribute("pivot_x", frame.PivotX),
                new XAttribute("pivot_y", frame.PivotY)
            ));
        }
    }
}