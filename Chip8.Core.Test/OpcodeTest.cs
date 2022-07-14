namespace Chip8.Core.Test
{
    [TestClass]
    public class OpcodeTest
    {
        [TestMethod]
        public void Constructor_Opcode0xABCD_InterpretsCorrectly()
        {
            var opcode = new Opcode(0xABCD);

            Assert.IsNotNull(opcode);

            Assert.AreEqual(0xABCD, opcode.All);
            Assert.AreEqual(0xA, opcode.First);
            Assert.AreEqual(0xB, opcode.X);
            Assert.AreEqual(0xC, opcode.Y);
            Assert.AreEqual(0xD, opcode.N);
            Assert.AreEqual(0xCD, opcode.NN);
            Assert.AreEqual(0xBCD, opcode.NNN);
        }

    }
}
