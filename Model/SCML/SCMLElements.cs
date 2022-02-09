using System.Collections.Generic;

namespace GLToolsGUI.Model.SCML
{
    public record TimelineKey(int FrameIndex, float Time, GLElement Element);

    public record ElementID(int Ref, int Index);

    public record Timeline(int ID, string Name, string ObjectType, List<TimelineKey> KeyFrames);

    public record AnimationFolder(int ID, string Name, List<FrameFile> Files);

    public record FrameFile(int ID, string Name, GLFrame Frame);
}