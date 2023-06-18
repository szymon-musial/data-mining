namespace airly_data_fetch;

public class Urls
{
    public static Dictionary<int, string> GetFriendlyStationName()
    {
        return new Dictionary<int, string>()
        {
            {42665,  "Franciszka Bujaka"},
            {19,     "Bulwarowa"},
            {101557, "Świętego Wawrzyńca"},
            {17,     "Aleja Zygmunta Krasińskiego"},
            {87029,  "Emaus"},
            {82510,  "Krakowska - Zabierzów"},

            //Skawiny ?,
            {64,     "Skawina"},

            //Węgrzce,
            {87014,  "Węgrzce"},

            // koło lotniska,
            {9106,   "Kaszów, no2"},
            {100427, " liski, szkolna"},

            // Tylko pm1, 2.5, 10,
            {18622,  " Rynek mogilany"},
            {18506,  " Wieliczka - Karola Kuczkiewicza"},
            {6074,   " Niepołomice szkolna"},
            {11337,  " Miętowa, prusy"},
            {96253,  " Skała, rynek"},
            
        };
    }

}
