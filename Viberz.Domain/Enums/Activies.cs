using System.ComponentModel;
using System.Runtime.Serialization;

namespace Viberz.Domain.Enums;

public enum Activies
{
    [EnumMember(Value = "Guess Genre")]
    GuessGenre = 0,
    [EnumMember(Value = "Guess Song")]
    GuessSong = 1,
    [EnumMember(Value = "Discover Genre")]
    DiscoverGenre = 2
}

public enum XpGames
{
    GuessGenre = 5,
    GuessSong = 5,
    DiscoverGenre = 15
}
