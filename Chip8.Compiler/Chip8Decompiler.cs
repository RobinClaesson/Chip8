namespace Chip8.Compiler
{
    public class Chip8Decompiler
    {
        public static byte[] DecompileToByteArray(string romPath)
        {
            return File.ReadAllBytes(romPath);
        }

        public static void DecompileToFile(string romPath, string savePath)
        {
            var rom = File.ReadAllBytes(romPath);
            var byteStrings = BitConverter.ToString(rom).Split('-');

            using (var writer = new StreamWriter(savePath))
            {
                for (int i = 0; i < rom.Length; i += 2)
                {
                    writer.WriteLine($"0x{byteStrings[i]}{byteStrings[i + 1]}");
                }
            }
        }
    }
}