namespace aspx2razor
{
    using System.IO;
    using System.Linq;

    /// <summary>
    /// An object that will handle the traversing of directories and collection of files
    /// </summary>
    public class DirectoryHandler
    {
        private string InputDirectory { get; set; }
        private string InputFilter { get; set; }
        private string OutputDirectory { get; set; }
        
        /// <summary>
        /// Initializes a new DirectoryHandler instance
        /// </summary>
        /// <param name="inputDirectory">The initial directory to start inspections at</param>
        /// <param name="outputDirectory">The output directory to output to</param>
        public DirectoryHandler(string inputFile, string outputDirectory)
        {
            InputFilter = Path.GetFileName(inputFile) ?? "";
            InputDirectory = GetFullPathOrDefault(inputFile);

            if (string.IsNullOrEmpty(outputDirectory))
            {
                OutputDirectory = InputDirectory;
            }
            else
            {
                OutputDirectory = Path.GetFullPath(outputDirectory);
            }
        }

        public string[] GetFiles(bool includeSubdirectories)
        {
            return GetFiles(InputDirectory, InputFilter, includeSubdirectories);
        }

        public string GetOutputFileName(string fileName)
        {
            var fullFileName = Path.GetFullPath(fileName);
            var relativeFileName = fullFileName.Remove(0, InputDirectory.Length + 1);

            return Path.Combine(OutputDirectory, relativeFileName);
        }

        private static string[] GetFiles(string inputDirectory, string inputFilter, bool includeSubdirectories)
        {
            if (!includeSubdirectories)
            {
                return Directory.GetFiles(inputDirectory, inputFilter);
            }

            string[] outFiles = Directory.GetFiles(inputDirectory, inputFilter);
            var di = new DirectoryInfo(inputDirectory);
            if (di.GetDirectories().Length > 0)
            {
                var directories = di.GetDirectories();
                outFiles =
                    directories.Aggregate(outFiles,
                        (current, subdirectory) => current.Union(GetFiles(subdirectory.FullName, inputFilter, true)).ToArray()
                    );
            }

            return outFiles;
        }

        private static string GetFullPathOrDefault(string directory)
        {
            directory = Path.GetDirectoryName(directory);

            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            return Path.GetFullPath(directory);
        }
    }
}