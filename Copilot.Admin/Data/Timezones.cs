namespace Copilot.Admin.Data;

public record TimezoneItem(string Title, int? Value);

public static class Timezones
{
    public static readonly List<TimezoneItem> Items =
    [
        new TimezoneItem("(GMT) Dublin, Edinburgh, Lisbon, London", 0),
        new TimezoneItem("(GMT +1:00) Brussels, Copenhagen, Madrid, Paris", 1),
        new TimezoneItem("(GMT +2:00) Athens, Bucharest, Istanbul", 2),
        new TimezoneItem("(GMT +3:00) Moscow, St. Petersburg, Volgograd", 3),
        new TimezoneItem("(GMT +4:00) Abu Dhabi, Baku, Tbilisi", 4),
        new TimezoneItem("(GMT +5:00) Astana, Almaty, Yekaterinburg", 5),
        new TimezoneItem("(GMT +6:00) Omsk, Novosibirsk, Dhaka", 6),
        new TimezoneItem("(GMT +7:00) Bangkok, Hanoi, Jakarta", 7),
        new TimezoneItem("(GMT +8:00) Beijing, Hong Kong, Singapore", 8),
        new TimezoneItem("(GMT +9:00) Tokyo, Seoul, Yakutsk", 9),
        new TimezoneItem("(GMT +10:00) Vladivostok, Vladivostok", 10),
        new TimezoneItem("(GMT +11:00) Magadan, Solomon Is., New Caledonia", 11),
        new TimezoneItem("(GMT +12:00) Fiji, Kamchatka, Marshall Is.", 12),
        new TimezoneItem("(GMT -12:00) International Date Line West", -12),
        new TimezoneItem("(GMT -11:00) Midway Island, Samoa", -11),
        new TimezoneItem("(GMT -10:00) Hawaii", -10),
        new TimezoneItem("(GMT -9:00) Alaska", -9),
        new TimezoneItem("(GMT -8:00) Pacific Time (US & Canada)", -8),
        new TimezoneItem("(GMT -7:00) Arizona", -7),
        new TimezoneItem("(GMT -6:00) Central Time (US & Canada)", -6),
        new TimezoneItem("(GMT -5:00) Eastern Time (US & Canada)", -5),
        new TimezoneItem("(GMT -4:00) Atlantic Time (Canada)", -4),
        new TimezoneItem("(GMT -3:00) Brasilia", -3),
        new TimezoneItem("(GMT -2:00) Mid-Atlantic", -2),
        new TimezoneItem("(GMT -1:00) Cape Verde Is.", -1)
    ];   
}
