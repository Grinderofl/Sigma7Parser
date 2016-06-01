using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sigma7Parser
{
    public class FeatureRemover
    {
        private readonly FeatureRemoverOptions _options;

        public FeatureRemover(FeatureRemoverOptions options)
        {
            _options = options;
        }

        public void Run()
        {
            // First, read in the file as a list of feature id's as a hashset for more efficient Contains()
            var featuresToDelete = GetFeaturesToDelete(_options.FeaturesToDeleteFile);

            // StreamReader for efficient file access
            using (var reader = File.OpenText(_options.FeaturesFile))
            {
                // Features to keep can never exceed the amount of features originally buffered, so limit the size
                var featuresToKeep = new List<string>(_options.NumberOfLinesToParse);

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var currentFeatureId = GetCurrentLineId(line);
                    if (!featuresToDelete.Contains(currentFeatureId))
                        featuresToKeep.Add(line);

                    // Save periodically to reduce file open-close latency
                    if (ReachedMaximumBufferSize(featuresToKeep))
                    {
                        Save(featuresToKeep);
                        featuresToKeep.Clear();
                    }

                }
                Save(featuresToKeep);
            }
        }

        private bool ReachedMaximumBufferSize(List<string> featuresToKeep)
        {
            return featuresToKeep.Count == _options.NumberOfLinesToParse;
        }

        private void Save(IEnumerable<string> featuresToKeep)
        {
            File.AppendAllLines(_options.FeaturesToKeepFile, featuresToKeep);
        }

        private static string GetCurrentLineId(string currentLine)
        {
            return currentLine.Substring(0, currentLine.IndexOf(",", StringComparison.OrdinalIgnoreCase));
        }

        private ISet<string> GetFeaturesToDelete(string featuresToDeleteFile)
        {
            var lines = File.ReadAllLines(featuresToDeleteFile);
            return new HashSet<string>(lines);
        }
    }
}