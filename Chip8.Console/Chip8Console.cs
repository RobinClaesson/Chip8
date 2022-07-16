using Chip8.Core;
using System.Text;
using System.Runtime.InteropServices;

Chip8Core chip8Core = new Chip8Core();
chip8Core.DisplayChanged += PrintDisplay;

if (args.Length == 0)
{
    Console.WriteLine("No rom loaded...");
    PrintUsage();
    return;
}

chip8Core.LoadRom(args[0]);

while (true)
{
    Thread.Sleep(2);
    chip8Core.Update();
}

void PrintDisplay(object sender, EventArgs e)
{
    Console.Clear();

    var sb = new StringBuilder();
    for (int y = 0; y < Chip8Core.DisplayHeight; y++)
    {
        for (int x = 0; x < Chip8Core.DisplayWidth; x++)
        {
            sb.Append(chip8Core.Display[y, x] ? "#" : " ");
        }
        sb.AppendLine();
    }

    Console.WriteLine(sb.ToString());
}

void PrintUsage()
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        Console.WriteLine("Usage: Chip8Console.exe <path-to-rom>");
    else
        Console.WriteLine("Usage: dotnet run <path-to-rom>");
}