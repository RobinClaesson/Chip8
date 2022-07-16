using System.Collections.Immutable;

namespace Chip8.Core
{
    public enum KeypadKeys { Num0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, A, B, C, D, E, F }

    public class Chip8Core
    {
        public byte[] Font { get; private init; } = new byte[] { 0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                                            0x20, 0x60, 0x20, 0x20, 0x70, // 1
                                            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                                            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                                            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                                            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                                            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                                            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                                            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                                            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                                            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                                            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                                            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                                            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                                            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                                            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
                                           };

        public byte[] Memory { get; private set; } = new byte[4096];

        public ushort IndexRegister { get; private set; } = 0;

        private readonly byte[] _variableRegisters = new byte[16];
        public ImmutableArray<byte> VariableRegisters => ImmutableArray.Create(_variableRegisters);

        private readonly Stack<ushort> _stack = new();
        public ImmutableStack<ushort> Stack => ImmutableStack.Create(_stack.ToArray());

        public byte DelayTimer { get; private set; } = 0;
        public byte SoundTimer { get; private set; } = 0;

        public const int DisplayWidth = 64;
        public const int DisplayHeight = 32;

        public ushort PC { get; private set; } = 0;

        /// <summary>
        /// Pixels to draw to the display are true, order by [Y, X]
        /// </summary>
        public bool[,] Display { get; private set; } = new bool[DisplayHeight, DisplayWidth];

        public Chip8Core()
        {
            for (int i = 0; i < Font.Length; i++)
                Memory[i] = Font[i];
        }

        //TODO: Unit Test CountdownTimers() when timers > 0
        /// <summary>
        /// Decrease delay and sound timers, should be called 60 times per second
        /// </summary>
        public void CountdownTimers()
        {
            if (DelayTimer > 0)
                DelayTimer--;
            if (SoundTimer > 0)
                SoundTimer--;
        }

        //TODO: Unit Test Beep when SoundTimer > 0
        /// <summary>
        /// Is true if the chip8 console should beep
        /// </summary>
        public bool Beep => SoundTimer > 0;


        //TODO: Unit test Fetch
        public short Fetch()
        {
            return (short)((Memory[PC << 8]) | Memory[PC + 1]);
        }

        public void Execute(ushort opcode)
        {
            Execute(new Opcode(opcode));
        }

        //TODO: Unit test Execute with all opcodes
        public void Execute(Opcode opcode)
        {
            switch (opcode.First)
            {
                case 0:
                    SpecialInstructions(opcode);
                    break;
                case 1:
                    JumpInstructíon(opcode);
                    break;
                case 2:
                    SubroutineInstruction(opcode);
                    break;
                case 3:
                    CompareSkipEqualsImmediateInstruction(opcode);
                    break;
                case 4:
                    CompareSkipNotEqualsImmediateInstruction(opcode);
                    break;
                case 5:
                    CompareSkipEqualsRegisterInstruction(opcode);
                    break;
                case 6:
                    SetRegisterInstruction(opcode);
                    break;
                case 7:
                    AddImmediateInstruction(opcode);
                    break;
                case 8:
                    LogicalAndArithmeticInstructions(opcode);
                    break;
                case 9:
                    CompareSkipNotEqualsRegisterInstruction(opcode);
                    break;
                case 0xA:
                    SetIndexRegiesterInstruction(opcode);
                    break;
                case 0xD:
                    PrintToDisplayInstruction(opcode);
                    break;
            }
        }

        //TODO: Instructions that throws exceptions should throw custom Chip8 exception with relevant messages
        private void SpecialInstructions(Opcode opcode) //0___
        {
            switch (opcode.All)
            {
                case 0x00E0:
                    ClearScreenInstruction();
                    break;

                case 0x00EE:
                    ReturnInstruction();
                    break;
            }
        }

        private void ClearScreenInstruction() //00E0
        {
            Display = new bool[DisplayHeight, DisplayWidth];
            PC += 2;
        }

        private void ReturnInstruction() //00EE
        {
            PC = _stack.Pop();
            PC += 2;
        }


        private void JumpInstructíon(Opcode opcode) //1NNN
        {
            PC = opcode.NNN;
        }

        private void SubroutineInstruction(Opcode opcode) //2NNN
        {
            _stack.Push(PC);
            PC = opcode.NNN;
        }

        private void CompareSkipEqualsImmediateInstruction(Opcode opcode) //3XNN
        {
            if (_variableRegisters[opcode.X] == opcode.NN)
                PC += 4;
            else
                PC += 2;
        }

        private void CompareSkipNotEqualsImmediateInstruction(Opcode opcode) //4XNN
        {
            if (_variableRegisters[opcode.X] != opcode.NN)
                PC += 4;
            else
                PC += 2;
        }

        private void CompareSkipEqualsRegisterInstruction(Opcode opcode) //5XY0
        {
            if (_variableRegisters[opcode.X] == _variableRegisters[opcode.Y])
                PC += 4;
            else
                PC += 2;
        }

        private void CompareSkipNotEqualsRegisterInstruction(Opcode opcode) //9XY0
        {
            if (_variableRegisters[opcode.X] != _variableRegisters[opcode.Y])
                PC += 4;
            else
                PC += 2;
        }

        private void SetRegisterInstruction(Opcode opcode)//6XNN
        {
            _variableRegisters[opcode.X] = opcode.NN;
            PC += 2;
        }

        private void AddImmediateInstruction(Opcode opcode)//7XNN
        {
            _variableRegisters[opcode.X] += opcode.NN;
            PC += 2;
        }


        private void LogicalAndArithmeticInstructions(Opcode opcode) //8___
        {
            switch (opcode.N)
            {
                case 0:
                    CopyRegisterInstruction(opcode);
                    break;
                case 1:
                    BinaryOrInstruction(opcode);
                    break;
                case 2:
                    BinaryAndInstruction(opcode);
                    break;
                case 3:
                    LogicalXorInstruction(opcode);
                    break;
                case 4:
                    AddRegirsterInstruction(opcode);
                    break;
                case 5:
                    SubtractYFromXInstruction(opcode);
                    break;

                case 6:
                    break;

                case 7:
                    SubtractXFromYInstruction(opcode);
                    break;

                case 8:
                    break;

                case 9:
                    break;

                case 0xA:
                    break;

                case 0xB:

                    break;
                case 0xC:
                    break;

                case 0xD:
                    break;

                case 0xE:
                    break;

                case 0xF:
                    break;
            }
        }

        private void CopyRegisterInstruction(Opcode opcode) //8XY0
        {
            _variableRegisters[opcode.X] = _variableRegisters[opcode.Y];
            PC += 2;
        }

        private void BinaryOrInstruction(Opcode opcode) //8XY1
        {
            _variableRegisters[opcode.X] |= _variableRegisters[opcode.Y];
            PC += 2;
        }

        private void BinaryAndInstruction(Opcode opcode) //8XY2
        {
            _variableRegisters[opcode.X] &= _variableRegisters[opcode.Y];
            PC += 2;
        }

        private void LogicalXorInstruction(Opcode opcode) //8XY3
        {
            _variableRegisters[opcode.X] ^= _variableRegisters[opcode.Y];
            PC += 2;
        }

        private void AddRegirsterInstruction(Opcode opcode) //8XY4
        {
            if (_variableRegisters[opcode.X] + _variableRegisters[opcode.Y] > 255)
                _variableRegisters[0xF] = 1;
            else
                _variableRegisters[0xF] = 0;

            _variableRegisters[opcode.X] += _variableRegisters[opcode.Y];
            PC += 2;
        }

        private void SubtractYFromXInstruction(Opcode opcode) //8XY5
        {
            if (_variableRegisters[opcode.X] > _variableRegisters[opcode.Y])
                _variableRegisters[0xF] = 1;
            else
                _variableRegisters[0xF] = 0;


            _variableRegisters[opcode.X] -= _variableRegisters[opcode.Y];
            PC += 2;
        }

        private void SubtractXFromYInstruction(Opcode opcode) //8XY7
        {
            if (_variableRegisters[opcode.X] < _variableRegisters[opcode.Y])
                _variableRegisters[0xF] = 1;
            else
                _variableRegisters[0xF] = 0;

            _variableRegisters[opcode.X] = (byte)(_variableRegisters[opcode.Y] - VariableRegisters[opcode.X]);
            PC += 2;
        }


        private void SetIndexRegiesterInstruction(Opcode opcode) //ANNN
        {
            IndexRegister = opcode.NNN;
            PC += 2;
        }

        private void PrintToDisplayInstruction(Opcode opcode)
        {

        }
    }
}