using Sorter.Core;
using Sorter.Models.Models;
using Spectre.Console.Cli;

var app = new CommandApp<Sorter.Console.Program>();

try
{
    app.Run(args);
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}


namespace Sorter.Console
{
    public partial class Program : Command<Arguments>
    {
        private Scanner Scanner { get; set; }

        public Program()
        {
            Scanner = new ();
        }
    
        public override int Execute(CommandContext context, Arguments settings)
        {
            var result = Scanner.Scan(settings.Path);

            var printer = new Printer(settings);
        
            if (settings.IsMinimize)
            {
                printer.MinimizePrint(result);
            }
            else
            {
                printer.ExpandPrint(result);
            }

            return 0;
        }
    }
}