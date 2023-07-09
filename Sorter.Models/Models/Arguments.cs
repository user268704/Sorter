using System.ComponentModel;
using Spectre.Console.Cli;

namespace Sorter.Models.Models;

public class Arguments : CommandSettings
{
    
    [Description("Путь к папке которую надо сканировать")]
    [CommandArgument(0, "[path]")]
    public string Path { get; set; }
    
    [Description("Если true то показывает только папки")]
    [CommandOption("-m")]
    public bool IsMinimize { get; set; }

    [Description("Показывать файлы размером больше чем n килобайт")]
    [CommandOption("-l")]
    public int LessThan { get; set; }

    [Description("Если true сортирует по возрастанию")]
    [CommandOption("-s")]
    public bool SortMethod { get; set; }
}