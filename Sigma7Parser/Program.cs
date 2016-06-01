using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Sigma7Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            

            var options = new FeatureRemoverOptions()
            {
                FeaturesFile = AskForFileName("Please enter the file name from which you would like to load the features", "Features.txt"),
                FeaturesToKeepFile = AskForFileName("Please enter the file name to which you would like to save the features", "FeaturesToKeep.txt"),
                FeaturesToDeleteFile = AskForFileName("Please enter the file name from which to load deleted features", "FeaturesToDelete.txt"),
                NumberOfLinesToParse = 5000
            };
            var remover = new FeatureRemover(options);
            remover.Run();
            Console.WriteLine("Completed");
            Console.ReadKey();
        }

        private static string AskForFileName(string question, string defaultFileName)
        {
            Console.WriteLine($"{question} (default {defaultFileName}):");
            var result = Console.ReadLine();
            return string.IsNullOrWhiteSpace(result) ? defaultFileName : result;
        }
    }
}
