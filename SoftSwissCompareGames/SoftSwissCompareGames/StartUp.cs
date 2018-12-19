using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SoftSwissCompareGames
{
    class StartUp
    {
        static void Main(string[] args)
        {
            FileStream fileStreamJson = new FileStream(@"C:\MyDir\SomeJsonFile.json", FileMode.Open, FileAccess.Read);

            // returns:
            // "c:\MyDir\my-car.yml"
            // "c:\MyDir\Friends\james.yml"
            // MyDir - here we put the directory we want to look into and we are looking only for .YML files.
            string path = @"c:\MyDir\";
            IEnumerable<string> allFilesWithProviders = Directory.GetFiles(path, "*.yml", SearchOption.AllDirectories);

            List<string> listOfJackpotGames = new List<string>();
            List<string> listOfGamesInCasino = new List<string>();

            string tempStr = string.Empty;
            string line;
            string patternJackpot = $"(\")(.+)(\")";
            string patternGames = $"(identifier: )(.+)";


            using (var streamReader = new StreamReader(fileStreamJson, Encoding.UTF8))
            {
                Regex regexJackpot = new Regex(patternJackpot);

                while ((line = streamReader.ReadLine()) != null)
                {
                    // process the line
                    if (line.Contains(":["))
                    {
                        Match match = regexJackpot.Match(line);
                        Group group = match.Groups[2];

                        tempStr = group.ToString();
                        listOfJackpotGames.Add(tempStr);
                    }
                }
            }

            foreach (var provider in allFilesWithProviders)
            {
                using (var streamReader = new StreamReader(provider, Encoding.UTF8))
                {
                    Regex regexGames = new Regex(patternGames);

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        // process the line
                        if (line.Contains("identifier:"))
                        {
                            Match match = regexGames.Match(line);
                            Group group = match.Groups[2];

                            tempStr = group.ToString();
                            listOfGamesInCasino.Add(tempStr);
                        }
                    }
                }
            }

            using (var streamWriter = new StreamWriter(@"C:\MyDir\ComparedNames.txt"))
            {
                streamWriter.WriteLine("Jacpots Games Names".PadRight(60) + "Casiono Games");

                foreach (var item in listOfJackpotGames)
                {
                    if (listOfGamesInCasino.Contains(item))
                    {
                        streamWriter.WriteLine($"{item}".PadRight(60) + "+");
                    }
                    else
                    {
                        streamWriter.WriteLine($"{item}");
                    }
                }
            }

        }
    }
}
