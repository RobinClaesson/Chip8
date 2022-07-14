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
            for(int i = 0; i < _target.Font.Length; i++)
            {
                Assert.AreEqual(_target.Font[i], _target.Memory[i]);
            }
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
    }
}