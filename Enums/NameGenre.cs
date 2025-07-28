using System.Runtime.Serialization;

[DataContract]
public enum NameGenre
{
    [EnumMember(Value = "Bass House")]
    BassHouse,
    [EnumMember(Value = "Trap")]
    Trap,
    [EnumMember(Value = "Dubstep")]
    Dubstep,
    [EnumMember(Value = "Drum'N'Bass")]
    DrumAndBass,
    [EnumMember(Value = "Hardstyle")]
    Hardstyle,
    [EnumMember(Value = "House")]
    House,
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