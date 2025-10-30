using System.Runtime.Serialization;
using static System.Net.WebRequestMethods;

[DataContract]
public enum NameGenre
{
    [EnumMember(Value = "Bass House")]
    BassHouse,
    [EnumMember(Value = "EDM Trap")]
    EDMTrap,
    [EnumMember(Value = "Dubstep")]
    Dubstep,
    [EnumMember(Value = "Drum & Bass")]
    DrumAndBass,
    [EnumMember(Value = "Hardstyle")]
    Hardstyle,
    [EnumMember(Value = "Progressive House")]
    ProgressiveHouse,
    [EnumMember(Value = "Psy Trance")]
    Psytrance,
    [EnumMember(Value = "Tech House")]
    TechHouse,
    [EnumMember(Value = "Techno")]
    Techno,
    [EnumMember(Value = "Hard Techno")]
    HardTechno,
    [EnumMember(Value = "UK House / UKG")]
    UKHouseUKG,
    [EnumMember(Value = "Hyper Techno")]
    HyperTechno,
    [EnumMember(Value = "Stutter House")]
    StutterHouse,
}