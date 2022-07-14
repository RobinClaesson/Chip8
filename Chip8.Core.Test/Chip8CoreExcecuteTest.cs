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
        const ushort InstructionPcIncrease = 2;

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

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease, _target.PC);
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

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease, _target.PC);
            Assert.AreEqual(stackCountBefore, _target.Stack.Count());
        }

        [TestMethod]
        public void CompareSkipEqualsImmediateInstruction_RegisterEqualsOpcodeNNN_PCSkipIstruction()
        {
            _target.Execute(0x3000);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void CompareSkipEqualsImmediateInstruction_RegisterDontEqualsOpcodeNNN_PCDontSkipIstruction()
        {
            _target.Execute(0x6013);
            _target.Execute(0x3000);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void CompareSkipNotEqualsImmediateInstruction_RegisterEqualsOpcodeNNN_PCDontSkipIstruction()
        {
            _target.Execute(0x4000);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease, _target.PC);
        }

        [TestMethod]
        public void CompareSkipNotEqualsImmediateInstruction_RegisterDontEqualsOpcodeNNN_PCSkipIstruction()
        {
            _target.Execute(0x6013);
            _target.Execute(0x4000);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 3, _target.PC);
        }

        [TestMethod]
        public void CompareSkipEqualsRegisterInstruction_RegistersEqual_PCSkipInstruction()
        {
            _target.Execute(0x5010);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void CompareSkipEqualsRegisterInstruction_RegistersDontEqual_PCDontSkipInstruction()
        {
            _target.Execute(0x6013);
            _target.Execute(0x5010);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void CompareSkipNotEqualsRegisterInstruction_RegistersEqual_PCDontSkipInstruction()
        {
            _target.Execute(0x9010);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease, _target.PC);
        }

        [TestMethod]
        public void CompareSkipNotEqualsRegisterInstruction_RegistersDontEqual_PCSkipInstruction()
        {
            _target.Execute(0x6013);
            _target.Execute(0x9010);

            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 3, _target.PC);
        }

        [TestMethod]
        public void SetRegisterInstruction_RegisterSetToOpcodeNN()
        {
            _target.Execute(0x6513);

            Assert.AreEqual(0x13, _target.VariableRegisters[5]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease, _target.PC);
        }

        [TestMethod]
        public void AddImmediateInstruction_NoAdditionOverFlow_ImmediateAddedToRegister()
        {
            _target.Execute(0x6913);
            _target.Execute(0x7945);

            Assert.AreEqual(0x58, _target.VariableRegisters[9]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void AddImmediateInstruction_AdditionOverFlow_RegisterIsAdditionResultMinus256()
        {
            _target.Execute(0x6913);
            _target.Execute(0x79FF);

            var addition = 0x13 + 0xFF;

            Assert.AreEqual(addition - 256, _target.VariableRegisters[9]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void CopyRegisterInstruction_RegieterYCopiedToX()
        {
            _target.Execute(0x6D89);
            _target.Execute(0x8AD0);

            Assert.AreEqual(_target.VariableRegisters[0xD], _target.VariableRegisters[0xA]);
            Assert.AreEqual(0x89, _target.VariableRegisters[0xD]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 2, _target.PC);
        }

        [TestMethod]
        public void BinaryOrInstruction_RegisterXSetToBiteWiseOrWithY()
        {
            _target.Execute(0x6D23);
            _target.Execute(0x6746);
            _target.Execute(0x87D1);

            Assert.AreEqual(0x67, _target.VariableRegisters[7]);
            Assert.AreEqual(0x23, _target.VariableRegisters[0xD]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 3, _target.PC);
        }

        [TestMethod]
        public void BinaryAndInstruction_RegisterXSetToBitewiseAndWithY()
        {
            _target.Execute(0x6DFE);
            _target.Execute(0x6035);
            _target.Execute(0x8D02);

            Assert.AreEqual(0x34, _target.VariableRegisters[0xD]);
            Assert.AreEqual(0x35, _target.VariableRegisters[0]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 3, _target.PC);
        }

        [TestMethod]
        public void LogicalXorInstruction_RegisterXSetToLogicalXorWithY()
        {
            _target.Execute(0x6D26);
            _target.Execute(0x6743);
            _target.Execute(0x8D73);

            Assert.AreEqual(0x43, _target.VariableRegisters[7]);
            Assert.AreEqual(0x65, _target.VariableRegisters[0xD]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 3, _target.PC);
        }

        [TestMethod]
        public void AddRegirsterInstruction_NoOverflow_RegisterYAddedToX()
        {
            _target.Execute(0x6D26);
            _target.Execute(0x6743);
            _target.Execute(0x8D74);

            Assert.AreEqual(0x43, _target.VariableRegisters[7]);
            Assert.AreEqual(0x69, _target.VariableRegisters[0xD]);
            Assert.AreEqual(0, _target.VariableRegisters[0xF]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 3, _target.PC);
        }

        [TestMethod]
        public void AddRegirsterInstruction_NoOverflow_RegisterIsAdditionResultMinus256AndRegisterFSetTo1()
        {
            _target.Execute(0x6DFF);
            _target.Execute(0x6705);
            _target.Execute(0x8D74);

            Assert.AreEqual(0x05, _target.VariableRegisters[7]);
            Assert.AreEqual(0x4, _target.VariableRegisters[0xD]);
            Assert.AreEqual(1, _target.VariableRegisters[0xF]);
            Assert.AreEqual(_programCounterAtStart + InstructionPcIncrease * 3, _target.PC);
        }
    }
}
