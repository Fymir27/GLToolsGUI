using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using GLToolsGUI.Model;
using GLToolsGUI.Model.SCML;
using SpriterDotNet;

namespace GLToolsGUI.Utils
{
    /**
     * based on https://github.com/instant-noodles/gltools
     */
    public static class SCMLWriter
    {
        public static bool WriteGLAnimation(string path, GLBuild build, GLAnimationSet animationSet,
            string frameFormat = "png")
        {
            #region debug output

            Debug.WriteLine("Build Refs: \n" + string.Join("\n", build.Refs.Select((key, value) =>
                $"{key}:{value}")));

            Debug.WriteLine("Animation Refs: \n" + string.Join("\n", animationSet.Refs.Select((key, value) =>
                $"{key}:{value}")));

            #endregion

            if (build.Parts is null || build.Parts.Count == 0)
            {
                build.DisassembleRootTexture();
            }

            var animationFolders = new Dictionary<int, SpriterFolder>();
            var nextFolderID = 0;
            foreach (var symbol in build.Symbols)
            {
                var folder = new SpriterFolder { Id = nextFolderID++, Name = build.Refs[symbol.Ref1] };
                folder.Files = symbol
                    .GetValidFrames()
                    .Select(frame => GLFrameToSpriterFile(folder, frame, frameFormat))
                    .ToArray();

                animationFolders[symbol.Ref1] = folder;
                animationFolders[symbol.Ref2] = folder;
            }

            var spriterData = new GLSpriterData
            {
                Folders = build.Symbols.Select(symbol => animationFolders[symbol.Ref1]).ToArray(),
                Entities = new SpriterEntity[]
                {
                    new()
                    {
                        Id = 0,
                        Name = build.Root,
                        Animations = animationSet.GLAnimations
                            .Select((animation, id) =>
                                GLAnimationToSpriterAnimation(animation, id, animationSet.Refs, animationFolders))
                            .ToArray()
                    }
                }
            };

            var ser = new XmlSerializer(typeof(GLSpriterData));
            using var fs = new FileStream(path, FileMode.Create);
            ser.Serialize(fs, spriterData);

            return true;
        }

        private static SpriterAnimation GLAnimationToSpriterAnimation(GLAnimation glAnimation, int animationID,
            Dictionary<int, string> refs, Dictionary<int, SpriterFolder> folders)
        {
            var timelineKeysWithElementID = GetTimelineKeysWithElementID(glAnimation, animationID, folders);

            var timelines = GetTimelines(timelineKeysWithElementID, folders, refs);

            SpriterMainlineKey ConstructMainlineKey(int frameIndex,
                IEnumerable<(ElementID ID, SpriterTimelineKey TimelineKey)> keysWithID)
            {
                var listOfTimelineKeys = keysWithID.ToList();
                var firstKey = listOfTimelineKeys.First().TimelineKey;
                float time = firstKey.Time;
                int zIndex = listOfTimelineKeys.Count;
                return new SpriterMainlineKey
                {
                    Id = frameIndex,
                    Time = time,
                    CurveType = SpriterCurveType.Instant,
                    ObjectRefs = listOfTimelineKeys.Select((tuple, objectRefID) => new SpriterObjectRef
                    {
                        Id = objectRefID,
                        TimelineId = timelines[tuple.ID].Id,
                        KeyId = frameIndex,
                        ZIndex = zIndex--
                    }).ToArray()
                };
            }

            var mainlineKeys = timelineKeysWithElementID.GroupBy
            (
                x => x.TimelineKey.Id, // == frameIndex
                x => x,
                ConstructMainlineKey
            );

            return new SpriterAnimation
            {
                Id = animationID,
                Name = GetAnimationName(glAnimation),
                Length = glAnimation.Framerate * glAnimation.FrameCount,
                MainlineKeys = mainlineKeys.ToArray(),
                Timelines = timelines.Values.ToArray()
            };
        }

        private static Dictionary<ElementID, SpriterTimeline> GetTimelines(
            List<(ElementID ID, SpriterTimelineKey TimelineKey)> timelineKeysWithElementID,
            Dictionary<int, SpriterFolder> folders,
            Dictionary<int, string> refs)
        {
            var nextTimelineID = 0;

            KeyValuePair<ElementID, SpriterTimeline> ConstructTimeline(
                ElementID elementID,
                IEnumerable<SpriterTimelineKey> keysOfTimeline)
            {
                return KeyValuePair.Create(elementID, new SpriterTimeline
                {
                    Id = nextTimelineID++,
                    Name = $"{refs[elementID.Ref]}-{elementID.Index}",
                    ObjectType = folders.ContainsKey(elementID.Ref)
                        ? SpriterObjectType.Sprite
                        : SpriterObjectType.Entity,
                    Keys = keysOfTimeline.ToArray()
                });
            }

            var timelines = new Dictionary<ElementID, SpriterTimeline>(timelineKeysWithElementID.GroupBy
            (
                tuple => tuple.ID,
                tuple => tuple.TimelineKey,
                ConstructTimeline
            ));
            return timelines;
        }

        private static List<(ElementID ID, SpriterTimelineKey TimelineKey)> GetTimelineKeysWithElementID(
            GLAnimation glAnimation,
            int animationID,
            Dictionary<int, SpriterFolder> folders)
        {
            var timelineKeysWithElementID = glAnimation.GLAnimFrames.SelectMany
            (
                (frame, frameIndex) => frame.GLElements.Select(element =>
                {
                    var objInfo = new SpriterObject
                    {
                        X = element.X,
                        Y = element.Y,
                        Angle = (float)element.Angle,
                        ScaleX = element.ScaleX,
                        ScaleY = element.ScaleY,
                        Alpha = element.a,
                        AnimationId = 5 // TODO: using animationID here doesn't work, no idea why
                    };

                    if (folders.TryGetValue(element.Ref, out var folder))
                    {
                        objInfo.FolderId = folder.Id;
                        SpriterFile file = null;
                        for (int i = 0; i < folder.Files.Length; i++)
                        {
                            if (folder.Files[i].Id > element.Ndx)
                                break;
                            file = folder.Files[i];
                        }

                        if (file is not null)
                        {
                            objInfo.FileId = file.Id;
                            objInfo.PivotX = file.PivotX;
                            objInfo.PivotY = file.PivotY;
                        }
                    }

                    return (
                        ID: new ElementID(element.Ref, (int)element.index),
                        TimelineKey: new SpriterTimelineKey
                        {
                            Id = frameIndex,
                            Time = frameIndex * glAnimation.Framerate,
                            CurveType = SpriterCurveType.Instant,
                            ObjectInfo = objInfo,
                            Spin = (int)element.Spin,
                        });
                })
            ).ToList();
            return timelineKeysWithElementID;
        }

        private static SpriterFile GLFrameToSpriterFile(SpriterFolder folder, GLFrame frame, string frameFormat)
        {
            return new SpriterFile
            {
                Id = frame.Index,
                Name = $"{folder.Name}/{frame.Index}.{frameFormat}",
                Type = SpriterFileType.Image,
                PivotX = frame.PivotX,
                PivotY = frame.PivotY,
                Width = (int)Math.Round(frame.Width),
                Height = (int)Math.Round(frame.Height)
            };
        }

        private static string GetAnimationName(GLAnimation animation)
        {
            return animation.Name1 == animation.Name2 ? animation.Name1 : $"{animation.Name1}-{animation.Name2}";
        }
    }
}