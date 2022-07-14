using Chip8.Core;

Chip8Core chip8Core = new Chip8Core();

chip8Core.Execute(0x71CC);
chip8Core.Execute(0x7215);


while (true)
{
    chip8Core.Execute(0x8124);

    Console.Clear();
    Console.WriteLine($"Register 1: {chip8Core.VariableRegisters[1]}");
    Console.WriteLine($"Register 2: {chip8Core.VariableRegisters[2]}");
    Console.WriteLine($"Register F: {chip8Core.VariableRegisters[0xF]}");
    Console.ReadKey();
}