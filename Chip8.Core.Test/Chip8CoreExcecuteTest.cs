using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chip8.Core.Test
{
    [TestClass]
    public class Chip8CoreExcecuteTest
    {
        Chip8Core _target;
        ushort _programCounterAtStart;
        ushort _instructionPcIncrease = 2;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new Chip8Core();
            _programCounterAtStart = _target.PC;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _target = null;
            _programCounterAtStart = 0xFFFF;
            _instructionPcIncrease = 0xFFFF;
        }

        [TestMethod]
        public void ClearScreenInstruction_DisplayHasData_DisplayisCleared()
        {
            for (int i = 0; i < Chip8Core.DisplayHeight; i++)
                for (int j = i; j < Chip8Core.DisplayWidth; j++)
                    _target.Display[i, j] = true;

            _target.Execute(0x00E0);

            for (int x = 0; x < Chip8Core.DisplayWidth; x++)
                for (int y = 0; y < Chip8Core.DisplayHeight; y++)
                    Assert.IsFalse(_target.Display[y, x]);

            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease, _target.PC);
        }

        [TestMethod]
        public void JumpInstruction_PCSetToOpcodeNNN()
        {
            _target.Execute(0x1ABC);

            Assert.AreEqual(0xABC, _target.PC);
        }

        [TestMethod]
        public void SubrutineInstruction_CurrentPCStackedAndPCSetToOpcodeNNN()
        {
            _target.Execute(0x2FBA);

            var stack = _target.Stack;

            Assert.AreEqual(0xFBA, _target.PC);
            Assert.AreEqual(_programCounterAtStart, stack.Peek());
        }

        [TestMethod]
        public void ReturnInstruction_SubrutineInstructionExcecuted_ReturnsToPCBeforeSubrutine()
        {
            var stackCountBefore = _target.Stack.Count();

            _target.Execute(0x2FBA);
            _target.Execute(0x00EE);

            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease, _target.PC);
            Assert.AreEqual(stackCountBefore, _target.Stack.Count());
        }

        [TestMethod]
        public void CompareSkipEqualsImmediateInstruction_RegisterEqualsOpcodeNNN_PCSkipIstruction()
        {
            _target.Execute(0x3000);

            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void CompareSkipEqualsImmediateInstruction_RegisterDontEqualsOpcodeNNN_PCDontSkipIstruction()
        {
            _target.Execute(0x6013);
            _target.Execute(0x3000);

            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void CompareSkipNotEqualsImmediateInstruction_RegisterEqualsOpcodeNNN_PCDontSkipIstruction()
        {
            _target.Execute(0x4000);

            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease, _target.PC);
        }

        [TestMethod]
        public void CompareSkipNotEqualsImmediateInstruction_RegisterDontEqualsOpcodeNNN_PCSkipIstruction()
        {
            _target.Execute(0x6013);
            _target.Execute(0x4000);

            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease * 3, _target.PC);
        }

        [TestMethod]
        public void SetRegisterInstruction_RegisterSetToOpcodeNN()
        {
            _target.Execute(0x6513);

            Assert.AreEqual(0x13, _target.VariableRegisters[5]);
            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease, _target.PC);
        }

        [TestMethod]
        public void AddImmediateInstruction_NoAdditionOverFlow_ImmediateAddedToRegister()
        {
            _target.Execute(0x6913);
            _target.Execute(0x7945);

            Assert.AreEqual(0x58, _target.VariableRegisters[9]);
            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void AddImmediateInstruction_AdditionOverFlow_RegisterIsAdditionResultMinus256()
        {
            _target.Execute(0x6913);
            _target.Execute(0x79FF);

            var addition = 0x13 + 0xFF;

            Assert.AreEqual(addition - 256, _target.VariableRegisters[9]);
            Assert.AreEqual(_programCounterAtStart + _instructionPcIncrease * 2, _target.PC);
        }

    }
}
