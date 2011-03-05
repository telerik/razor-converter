using System;
using System.IO;
using System.Linq;

namespace aspx2razor
{
 
    /// <summary>
    /// An object that will handle the traversing of directories and collection of files
    /// </summary>
    public class DirectoryHandler
    {

        #region Fields

        private readonly string _InputDirectory;
        private readonly string _InputFilter;
        private readonly string _OutputDirectory;

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputDirectory">The initial directory to start inspections at</param>
        /// <param name="outputDirectory">The output directory to output to</param>
        public DirectoryHandler(string inputDirectory, string outputDirectory)
        {

            _InputFilter = Path.GetFileName(inputDirectory) ?? "";

            _InputDirectory = ScrubDirectoryName(inputDirectory);
            _OutputDirectory = (inputDirectory == outputDirectory) ? _InputDirectory : outputDirectory;

        }

        public string[] GetFiles(bool includeSubdirectories)
        {

            return GetFiles(_InputDirectory, _InputFilter, includeSubdirectories);

        }

        private static string[] GetFiles(string inputDirectory, string inputFilter, bool includeSubdirectories)
        {

            if (!includeSubdirectories) return Directory.GetFiles(inputDirectory, inputFilter);

            string[] outFiles = Directory.GetFiles(inputDirectory, inputFilter);
            var di = new DirectoryInfo(inputDirectory);
            if (di.GetDirectories().Length > 0)
            {
                var directories = di.GetDirectories();
                outFiles = directories.Aggregate(outFiles, (current, subdirectory) => current.Union(GetFiles(subdirectory.FullName, inputFilter, true)).ToArray());
            }

            return outFiles;

        }

        public string CalculateOutputfilename(string filename, string newExtension)
        {

            var targetFolder = _OutputDirectory + CalculateOutputSubfolder(filename);
            if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);

            return Path.Combine(targetFolder, Path.GetFileNameWithoutExtension(filename) + newExtension);

        }

        public string CalculateOutputSubfolder(string file)
        {

            var outFilename = file.Remove(0, _InputDirectory.Length);
            outFilename = outFilename.Replace(Path.GetFileName(file), "");
            return outFilename;

        }

        private static string ScrubDirectoryName(string inputDirectory)
        {

            inputDirectory = Path.GetDirectoryName(inputDirectory);
            
            if (string.IsNullOrEmpty(inputDirectory))
            {
                inputDirectory = Directory.GetCurrentDirectory();
            }

            return inputDirectory;
        }

    }

}