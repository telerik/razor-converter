using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aspx2razor {

    /// <summary>
    /// An object that will handle the traversing of directories and collection of files
    /// </summary>
    public class DirectoryHandler {
        private static IEnumerable<string> extensionFilter = new List<string>() {
            ".aspx", ".ascx"
        };

        private string InputDirectory { get; set; }
        private string OutputDirectory { get; set; }

        /// <summary>
        /// Initializes a new DirectoryHandler instance
        /// </summary>
        /// <param name="inputDirectory">The initial directory to start inspections at</param>
        /// <param name="outputDirectory">The output directory to output to</param>
        public DirectoryHandler(string inputDirectory, string outputDirectory) {
            InputDirectory = GetFullPathOrDefault(inputDirectory);

            if(string.IsNullOrEmpty(outputDirectory)) {
                OutputDirectory = InputDirectory;
            } else {
                OutputDirectory = Path.GetFullPath(outputDirectory);
            }
        }

        public IEnumerable<string> GetFiles(bool includeSubdirectories) {
            return GetFiles(InputDirectory, includeSubdirectories);
        }

        public string GetOutputFileName(string fileName) {
            var fullFileName = Path.GetFullPath(fileName);
            var relativeFileName = fullFileName.Remove(0, InputDirectory.Length + 1);

            return Path.Combine(OutputDirectory, relativeFileName);
        }

        private static List<string> GetFiles(string inputDirectory, bool includeSubdirectories) {
            var files = GetFileRecursive(new List<string>(), inputDirectory, includeSubdirectories);
            return files;
        }

        private static List<string> GetFileRecursive(List<string> list, string directoryPath, bool recursive) {
            var directory = new DirectoryInfo(directoryPath);
            var files = directory.GetFiles().Where(file => extensionFilter.Contains(file.Extension));

            list.AddRange(files.Select(file => file.FullName));

            if(recursive) {
                var subDirectories = directory.GetDirectories();
                foreach(var subDirectory in subDirectories) {
                    GetFileRecursive(list, subDirectory.FullName, recursive);
                }
            }
            return list;
        }

        private static string GetFullPathOrDefault(string directory) {
            if(!Directory.Exists(directory)) {
                directory = Path.GetDirectoryName(directory);
            }

            if(string.IsNullOrEmpty(directory)) {
                directory = Directory.GetCurrentDirectory();
            }

            return Path.GetFullPath(directory);
        }
    }
}