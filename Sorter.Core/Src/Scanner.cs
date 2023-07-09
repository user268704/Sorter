using Sorter.Core.Interfaces;
using Sorter.Models.Models;
using Sorter.Models.Models.Events;

namespace Sorter.Core;

public class Scanner : IScanner
{
    public event IScanner.FileIncrementDelegate OnFileIncrement;
    public event IScanner.FileIncrementDelegate OnCoreDirectoryIncrement;

    private List<string> _rootDirectories = new();
    private string _rootDirectory;
    private int _doneCount;

    private void Increment()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(_rootDirectory);
        
        
    }
    
    public List<DirItem> Scan(string path)
    {
        if (_doneCount == 0)
        {
            _rootDirectory = path;
            
            _rootDirectories.AddRange(Directory.GetDirectories(path));
        }

        List<DirItem> result = new();

        try
        {
            var directories = Directory.GetDirectories(path);
            
            foreach (string directory in directories)
            {
                result.AddRange(Scan(directory));
            }
        
            var files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                var fileInfo = new FileInfo(file);
        
                result.Add(new DirItem
                {
                    Name = fileInfo.Name,
                    FullPath = fileInfo.FullName,
                    Size = fileInfo.Length,
                    Directory = fileInfo.Directory.FullName
                });
                _doneCount += 1;
                
                OnFileIncrement.Invoke(new FileIncrementArgs
                {
                    FileName = fileInfo.Name,
                    IndexedFilesCount = _doneCount
                });
            }

            return result;
        }
        catch (UnauthorizedAccessException)
        {
            return result;
        }
    }
}