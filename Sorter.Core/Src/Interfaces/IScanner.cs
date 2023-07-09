using Sorter.Models.Models;
using Sorter.Models.Models.Events;

namespace Sorter.Core.Interfaces;

public interface IScanner
{
    List<DirItem> Scan(string path);
    
    public delegate void FileIncrementDelegate(FileIncrementArgs e);

    public event FileIncrementDelegate OnFileIncrement;
    public event FileIncrementDelegate OnCoreDirectoryIncrement;

}