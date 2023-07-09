using Sorter.Models.Models;

namespace Sorter.Console;

class Printer
{
    private readonly Arguments _arguments;

    public Printer(Arguments arguments)
    {
        _arguments = arguments;
    }

    public void ExpandPrint(List<DirItem> items)
    {
        if (_arguments.LessThan >= 0)
        {
            items = items.Where(x => x.Size > _arguments.LessThan).ToList();
        }

        foreach (var item in Filter(items))
        {

            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(
                $"{item.Key} - {item.Sum(x => x.Size) / 1024}Кб - {item.Sum(x => x.Size) / 1024 / 1024}Мб");
            System.Console.ResetColor();

            foreach (DirItem dirItem in item)
            {
                System.Console.WriteLine($"{dirItem.Name} - {dirItem.Size / 1024}Кб");
            }
        }
    }

    public void MinimizePrint(List<DirItem> items)
    {

        if (_arguments.LessThan >= 0)
        {
            items = items.Where(x => (x.Size / 1024) > (float)_arguments.LessThan / 1024).ToList();
        }


        var result = Filter(items);
        for (int i = 0; i < result.Count; i++)
        {
            var item = result[i];

            System.Console.WriteLine(
                $"{i}) {item.Key} - {item.Sum(x => x.Size) / 1024}Кб - {item.Sum(x => x.Size) / 1024 / 1024}Мб");
        }
    }

    private List<IGrouping<string, DirItem>> Filter(List<DirItem> items)
    {
        if (_arguments.SortMethod)
        {
            return items
                .OrderBy(x => x.Size)
                .GroupBy(x => x.Directory)
                .ToList();
        }

        return items
            .OrderByDescending(x => x.Size)
            .GroupBy(x => x.Directory)
            .ToList();
    }
}