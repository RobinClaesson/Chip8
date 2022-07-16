namespace Chip8.Core.Test.Roms
{
    public class TestRoms
    {
        public static byte[] FillMemoryWithIncreasingBytes()
        {
            byte[] testRom = new byte[Chip8Core.MemorySize - Chip8Core.RomStartLocation];
            for (int i = 0; i < testRom.Length; i++)
                testRom[i] = (byte)(i % 256);

            return testRom;
        }
    }
}
