using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using GLToolsGUI.Model;

namespace GLToolsGUI.Utils
{
    public static class SCML
    {

        private static Dictionary<int, int> _refToFolderID;
        private static Dictionary<int, string> _animationRefs;
        private static List<int> _folderRefs;

        public record FrameFile(int ID, string Name, GLFrame Frame);
        public record AnimationFolder(int ID, string Name, List<FrameFile> Files);

        
        
        public static bool CreateFile(string filename, GLBuild build, GLAnimationSet animationSet,
            string frameFormat = "png")
        {
            Debug.WriteLine("Build Refs: \n" + string.Join("\n", build.Refs.Select((key, value) =>
                $"{key}:{value}")));
            
            Debug.WriteLine("Animation Refs: \n" + string.Join("\n", animationSet.Refs.Select((key, value) =>
                $"{key}:{value}")));

            _folderRefs = build.Symbols.Select(s => s.Ref1).ToList();

            if (build.Parts is null || build.Parts.Count == 0)
            {
                build.DisassembleRootTexture();
            }
            
            var animationFolders = new Dictionary<int, AnimationFolder>();
            int nextFolderID = 0;
            foreach (var symbol in build.Symbols)
            {
                var files = symbol
                    .GetValidFrames()
                    .Select(frame => new FrameFile(frame.Index, $"{frame.Index}.{frameFormat}", frame))
                    .ToList();
                
                var folder = new AnimationFolder(nextFolderID++, build.Refs[symbol.Ref1], files);
                animationFolders[symbol.Ref1] = folder;
                animationFolders[symbol.Ref2] = folder;
            }

            var doc = new XDocument
            (
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("spriter_data",
                    new XAttribute("scml_version", "1.0"),
                    new XAttribute("generator", "BrashMonkey Spriter"),
                    new XAttribute("generator_version", "r11"),
                    build.Symbols.Select(symbol => GetFolderNode(animationFolders[symbol.Ref1])),
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
                new XElement("mainline", GetMainlineKeys(animation)),
                GetTimelines(animation, animationRefs)
            ));
        }

        private static IEnumerable<XElement> GetTimelines(GLAnimation animation,
            Dictionary<int, string> animationRefs)
        {
            // TODO: separate grouping to separate function to use for mainline + here
            int timelineID = 0;
            return animation.GLAnimFrames
                .SelectMany
                (
                    (frame, frameIndex) => frame.GLElements.Select
                    (
                        element => (FrameIndex: frameIndex, Element: element) // TODO: check if this works 
                    )
                )
                .GroupBy
                (
                    x => (x.Element.Ref,(int)x.Element.index),
                    x => x,
                    (elementIdentification, elementStates) =>
                    {
                        (int @ref, int index) = elementIdentification;
                        //Debug.Assert(elementStates.Select(e => e.Element.index).All(i => i == elementStates.First().Element.index));
                        return new XElement("timeline",
                            new XAttribute("id", timelineID++), // TODO: actually create element lookup with indexes and shit
                            new XAttribute("name",
                                $"{animationRefs[@ref]}-{index}"),
                            new XAttribute("object_type",
                                _folderRefs.IndexOf(@ref) == -1 ? "entity" : "sprite"),
                            GetTimelineKeys(elementStates, animation.Framerate)
                        );
                    });
        }

        private static IEnumerable<XElement> GetTimelineKeys(IEnumerable<(int FrameIndex, GLElement Element)> elementStates, float framerate)
        {
            return elementStates.Select((elementState) => new XElement("key",
                new XAttribute("id", elementState.FrameIndex),
                new XAttribute("time", elementState.FrameIndex * framerate),
                //new XAttribute("curve_type", "INSTANT"),
                GetSpatialInfo(elementState.Element)
            ));
        }

        private static IEnumerable<XElement> GetMainlineKeys(GLAnimation animation)
        { 
            return animation.GLAnimFrames
                .Select((frame, frameIndex) => new XElement("key",
                new XAttribute("id", frameIndex),
                new XAttribute("time", frameIndex * animation.Framerate),
                GetObjectRefs(frame, frameIndex)
            ));
        }
        
        private static IEnumerable<XElement> GetObjectRefs(GLAnimFrame frame, int frameIndex)
        {
            // TODO: no element info in this at all?
            return frame.GLElements.Select((element, elementIndex) => new XElement("object_ref",
                new XAttribute("id", elementIndex),
                new XAttribute("timeline", element.index), // TODO: get index of ref instead?
                new XAttribute("key", frameIndex),
                new XAttribute("z_index", frame.ElementCount - elementIndex)
            ));
        }

        private static XElement GetFolderNode(AnimationFolder animationFolder)
        {
            return new XElement("folder",
                new XAttribute("id", animationFolder.ID),
                new XAttribute("name", animationFolder.Name),
                animationFolder.Files.Select(file => new XElement("file",
                    new XAttribute("id", file.ID),
                    new XAttribute("name", $"{animationFolder.Name}/{file.Name}"),
                    new XAttribute("width", file.Frame.Width),
                    new XAttribute("height", file.Frame.Height),
                    new XAttribute("pivot_x", file.Frame.PivotX),
                    new XAttribute("pivot_y", file.Frame.PivotY)
                ))
            );
        }

        private static XElement GetSpatialInfo(GLElement element)
        {
            var objectNode = new XElement("object",
                new XAttribute("x", element.X),
                new XAttribute("y", element.Y),
                new XAttribute("angle", element.Angle),
                new XAttribute("scale_x", element.ScaleX),
                new XAttribute("scale_y", element.ScaleY),
                new XAttribute("a", element.a),
                new XAttribute("spin", element.Spin)
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