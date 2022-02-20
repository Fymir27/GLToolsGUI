using System.Collections.Generic;
using System.Xml.Serialization;
using SpriterDotNet;

namespace GLToolsGUI.Model.SCML
{   
    /**
     * uniquely identifies an element inside a GL animation
     */
    public record ElementID(int Ref, int Index);

    /**
     * extended Spriter to include XML attributes in root
     */
    [XmlRoot("spriter_data")]
    public class GLSpriterData : Spriter
    {
        [XmlAttribute("scml_version")] public string SCMLVersion = "1.0";
        [XmlAttribute("generator")] public string Generator = "BrashMonkey Spriter";
        [XmlAttribute("generator_version")] public string GeneratorVersion = "r11";
    }
}