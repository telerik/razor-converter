using System.IO;
using System.Linq;

namespace aspx2razor
{
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
        public DirectoryHandler(string inputDirectory, string outputDirectory)
        {
            InputFilter = Path.GetFileName(inputDirectory) ?? "";

            InputDirectory = Path.GetDirectoryName(Path.GetFullPath(inputDirectory));
            OutputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputDirectory));
        }

        public string[] GetFiles(bool includeSubdirectories)
        {
            return GetFiles(InputDirectory, InputFilter, includeSubdirectories);
        }

        public string CalculateOutputfilename(string filename, string newExtension)
        {
            var fullFileName = Path.GetFullPath(filename);
            var relativeFileName = fullFileName.Remove(0, InputDirectory.Length + 1);

            var targetFolder = Path.GetDirectoryName(Path.Combine(OutputDirectory, relativeFileName));
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            return Path.Combine(targetFolder, Path.GetFileNameWithoutExtension(filename) + newExtension);
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
                outFiles = directories.Aggregate(outFiles, (current, subdirectory) => current.Union(GetFiles(subdirectory.FullName, inputFilter, true)).ToArray());
            }

            return outFiles;
        }
    }
}