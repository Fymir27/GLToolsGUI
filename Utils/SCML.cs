using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using GLToolsGUI.Model;

namespace GLToolsGUI.Utils
{
    public static class SCML
    {
        private static List<int> _folderRefs;

        public record FrameFile(int ID, string Name, GLFrame Frame);

        public record AnimationFolder(int ID, string Name, List<FrameFile> Files);

        public record Timeline(int ID, string Name, string ObjectType, List<TimelineKey> KeyFrames);

        public record TimelineKey(int FrameIndex, float Time, GLElement Element);

        public record ElementID(int Ref, int Index);

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
                        GetAnimationNodes(animationSet, animationSet.Refs)
                    )
                )
            );

            doc.Save(filename);

            return true;
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

        private static IEnumerable<XElement> GetAnimationNodes(GLAnimationSet animationSet,
            Dictionary<int, string> animationRefs)
        {
            var timelineKeysOfAnimations = animationSet.GLAnimations.Select(GetAllTimelineKeys).ToList();
            var timelinesOfAnimations = timelineKeysOfAnimations.Select(
                keysOfAnimation => ConstructTimelines(keysOfAnimation, animationRefs)
            ).ToList();

            return animationSet.GLAnimations.Select((animation, animationIndex) => new XElement("animation",
                new XAttribute("name", GetFullAnimationName(animation)),
                new XAttribute("id", animationIndex),
                new XAttribute("length", animation.Framerate * animation.FrameCount),
                new XAttribute("interval", 100), // TODO: should this always the same?
                new XElement("mainline", GetMainlineKeys(timelinesOfAnimations[animationIndex])),
                GetTimelineNodes(timelinesOfAnimations[animationIndex].Values)
            ));
        }

        private static string GetFullAnimationName(GLAnimation animation)
        {
            return animation.Name1 == animation.Name2 ? animation.Name1 : $"{animation.Name1}-{animation.Name2}";
        }

        private static List<TimelineKey> GetAllTimelineKeys(GLAnimation animation)
        {
            return animation.GLAnimFrames
                .SelectMany
                (
                    (frame, frameIndex) => frame.GLElements.Select(element =>
                        new TimelineKey(frameIndex, frameIndex * animation.Framerate, element))
                )
                .ToList();
        }

        private static Dictionary<ElementID, Timeline> ConstructTimelines(IEnumerable<TimelineKey> timelineKeys,
            Dictionary<int, string> animationRefs)
        {
            var timelineID = 0;
            return new Dictionary<ElementID, Timeline>
            (
                timelineKeys.GroupBy
                (
                    timelineKey => new ElementID
                    (
                        timelineKey.Element.Ref,
                        (int)timelineKey.Element.index
                    ),
                    timelineKey => timelineKey,
                    (elementID, keysOfTimeline) => KeyValuePair.Create
                    (
                        elementID,
                        new Timeline
                        (
                            timelineID++,
                            $"{animationRefs[elementID.Ref]}-{elementID.Index}",
                            _folderRefs.IndexOf(elementID.Ref) == -1 ? "entity" : "sprite",
                            keysOfTimeline.ToList()
                        )
                    )
                )
            );
        }

        private static IEnumerable<XElement> GetTimelineNodes(IEnumerable<Timeline> timelines)
        {
            return timelines.Select(timeline =>
            {
                return new XElement("timeline",
                    new XAttribute("id", timeline.ID),
                    new XAttribute("name", timeline.Name),
                    new XAttribute("object_type", timeline.ObjectType),
                    timeline.KeyFrames.Select(keyFrame =>
                        new XElement("key",
                            new XAttribute("id", keyFrame.FrameIndex),
                            new XAttribute("time", keyFrame.Time),
                            // TODO: "curve_type"
                            GetSpatialInfo(keyFrame.Element)
                        )
                    )
                );
            });
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

        private static IEnumerable<XElement> GetMainlineKeys(Dictionary<ElementID, Timeline> timelines)
        {
            return timelines
                .Values
                .SelectMany(timeline => timeline.KeyFrames.Select(timelineKey => (timeline.ID, timelineKey)))
                .GroupBy
                (
                    tuple => tuple.timelineKey.FrameIndex,
                    tuple => tuple,
                    (frameIndex, keyTuples) =>
                    {
                        var keyTupleList = keyTuples.ToList();
                        float time = keyTupleList.First().timelineKey.Time;
                        return new XElement("key",
                            new XAttribute("id", frameIndex),
                            new XAttribute("time", time),
                            GetObjectRefs(keyTupleList)
                        );
                    });
        }

        private static IEnumerable<XElement> GetObjectRefs(
            List<(int timelineID, TimelineKey timelineKey)> timelineKeyTuples)
        {
            var zIndex = timelineKeyTuples.Count;
            return timelineKeyTuples.Select((tuple, id) => new XElement("object_ref",
                new XAttribute("id", id),
                new XAttribute("timeline", tuple.timelineID),
                new XAttribute("key", tuple.timelineKey.FrameIndex),
                new XAttribute("z_index", zIndex--)
            ));
        }
    }
}