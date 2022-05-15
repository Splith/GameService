using GameService.Host.Models;
using GameService.Host.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace GameService.Host.Repos
{
    public class InstanceFileRepo : IInstanceFileService
    {
        public InstanceFileRepo(string backupDirectoryPath, string storeDirectoryPath)
        {
            BackupDirectoryPath = backupDirectoryPath;
            StoreDirectoryPath = storeDirectoryPath;
        }

        public string BackupDirectoryPath {get; set; }

        public string StoreDirectoryPath { get; set; }

        public async Task LoadInstanceFilesAsync(InstanceData instanceData, PortData portData)
        {
            await Task.Run(() => 
            {
                Directory.CreateDirectory(portData.DirectoryPath);
                var instancePath = Path.Combine(StoreDirectoryPath, instanceData.InstanceGuid.ToString());
                CopyDirectory(portData.DirectoryPath, instancePath, recursive: true);
            });
        }

        public async Task BackupInstanceFilesAsync(InstanceData instanceData, PortData portData)
        {
            await Task.Run(() =>
            {
                var backupPath = Path.Combine
                (
                    BackupDirectoryPath, 
                    instanceData.InstanceGuid.ToString(), 
                    DateTime.UtcNow.ToString("yyyyMMddTHHmmss")
                );
                Directory.CreateDirectory(backupPath);
                CopyDirectory(portData.DirectoryPath, backupPath, recursive: true);
                var binaryFiles = Directory.GetFiles(backupPath)
                    .Where(IsBinaryExecutable)
                    .ToList();

                foreach (var file in binaryFiles)
                {
                    File.Delete(file);
                }
            });
        }

        private static bool IsBinaryExecutable(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            var comparer = StringComparer.InvariantCultureIgnoreCase;
            return comparer.Compare(extension, "exe") == 0
                || comparer.Compare(extension, "dll") == 0
                || comparer.Compare(extension, "jar") == 0;
        }

        public Task ShutdownInstanceFilesAsync(InstanceData instanceData, PortData portData)
        {
        }

        private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}