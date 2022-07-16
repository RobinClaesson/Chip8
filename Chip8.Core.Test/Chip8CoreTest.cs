using Chip8.Core.Test.Roms;

namespace Chip8.Core.Test
{
    [TestClass]
    public class Chip8CoreTest
    {
        Chip8Core _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new Chip8Core();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _target = null;
        }

        [TestMethod]
        public void Constructor_Chip8Instantiated_FontFirstInMemory()
        {
            VerifyMemoryData(Font.Characters, 0);
        }

        [TestMethod]
        public void CountdownTimers_TimersAlreadyZero_NoTimerDecreased()
        {
            Assert.AreEqual(0, _target.DelayTimer);
            Assert.AreEqual(0, _target.SoundTimer);

            _target.CountdownTimers();

            Assert.AreEqual(0, _target.DelayTimer);
            Assert.AreEqual(0, _target.SoundTimer);
        }

        [TestMethod]
        public void Beep_SoundTimerIsZero_BeepFalse()
        {
            Assert.AreEqual(0, _target.SoundTimer);
            Assert.AreEqual(false, _target.Beep);
        }

        [TestMethod]
        public void LoadRom_RomFromByteArray_RomLoadedToMemory()
        {
            var rom = TestRoms.FillMemoryWithIncreasingBytes();

            _target.LoadRom(rom);

            VerifyMemoryData(rom, Chip8Core.RomStartLocation);
        }

        [TestMethod]
        public void LoadRom_RomFromFilePath_RomLoadedToMemory()
        {
            var path = Path.Combine("Roms", "IBMLogo.ch8");
            var rom = File.ReadAllBytes(path);

            _target.LoadRom(path);

            VerifyMemoryData(rom, Chip8Core.RomStartLocation);
        }



        private void VerifyMemoryData(byte[] expectedBytes, int memoryStartIndex)
        {
            var memory = _target.Memory;
            for (int i = 0; i < expectedBytes.Length; i++)
                Assert.AreEqual(expectedBytes[i], memory[memoryStartIndex + i]);
        }
    }
}