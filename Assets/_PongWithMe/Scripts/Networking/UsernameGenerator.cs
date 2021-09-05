using PerigonGames;

namespace PongWithMe
{
    public class UsernameGenerator
    {
        private static string[] FirstNames = new[]
        {
            "Johhny",
            "Str8",
            "Death",
            "Ghost",
            "Happy",
            "Delightful",
            "Big"
        };

        private static string[] SecondNames = new[]
        {
            "Walker",
            "Aroma",
            "BrightScreen",
            "Ambition",
            "Table",
            "Steak",
            "Dinner",
            "Lunch"
        };

        public static string GeneratorRandomUserName()
        {
            var random = new RandomUtility();
            var firstNameIndex = random.NextInt(0, FirstNames.Length);
            var secondNameIndex = random.NextInt(0, SecondNames.Length);

            return $"{FirstNames[firstNameIndex]} {SecondNames[secondNameIndex]}";
        }
    }
}

