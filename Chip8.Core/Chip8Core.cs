using System.Collections.Immutable;

namespace Chip8.Core
{
    public enum KeypadKeys { Num0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, A, B, C, D, E, F }

    public class Chip8Core
    {
        public const int MemorySize = 0x1000;
        public const int RomStartLocation = 0x200;

        private readonly byte[] _memory = new byte[MemorySize];
        public ImmutableArray<byte> Memory => ImmutableArray.Create(_memory);

        public ushort IndexRegister { get; private set; } = 0;

        private readonly byte[] _variableRegisters = new byte[16];
        public ImmutableArray<byte> VariableRegisters => ImmutableArray.Create(_variableRegisters);

        private readonly Stack<ushort> _stack = new();
        public ImmutableStack<ushort> Stack => ImmutableStack.Create(_stack.ToArray());

        public byte DelayTimer { get; private set; } = 0;
        public byte SoundTimer { get; private set; } = 0;

        public const int DisplayWidth = 64;
        public const int DisplayHeight = 32;

        public ushort PC { get; private set; } = RomStartLocation;

        /// <summary>
        /// Pixels to draw to the display are true, order by [Y, X]
        /// </summary>
        public bool[,] Display { get; private set; } = new bool[DisplayHeight, DisplayWidth];

        /// <summary>
        /// Raised when display is changed from 0xDXYN instruction
        /// </summary>
        public event EventHandler DisplayChanged;


        public Chip8Core()
        {
            for (int i = 0; i < Font.Characters.Length; i++)
                _memory[i] = Font.Characters[i];
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

        public void LoadRom(byte[] romData)
        {
            //TODO: Throw custom exception of romdata wont fit
            for (int i = 0; i < romData.Length; i++)
            {
                _memory[RomStartLocation + i] = romData[i];
            }

            PC = RomStartLocation;
        }

        public void LoadRom(string filePath)
        {
            LoadRom(File.ReadAllBytes(filePath));
        }

        /// <summary>
        /// Goes through the Fetch-Decode-Execute stages once
        /// </summary>
        public void Update()
        {
            Execute(Fetch());
        }

        //TODO: Unit test Fetch
        public ushort Fetch()
        {
            return (ushort)((_memory[PC] << 8) | _memory[PC + 1]);
        }

        public void Execute(ushort opcode)
        {
            Execute(new Opcode(opcode));
        }

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
                    DrawToDisplayInstruction(opcode);
                    break;
            }
        }

        //TODO: Instructions that throws exceptions should throw custom Chip8 exception with relevant messages
        private void SpecialInstructions(Opcode opcode) //0___
        {
            switch (opcode.All)
            {
                case 0:
                    PC += 2;
                    break;
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

        //TODO: Unit test DrawToDisplayInstruction
        private void DrawToDisplayInstruction(Opcode opcode) //DXYN
        {
            var x = _variableRegisters[opcode.X] % DisplayWidth;
            var y = _variableRegisters[opcode.Y] % DisplayHeight;

            _variableRegisters[0xF] = 0;

            for (int i = 0; i < opcode.N && y < DisplayHeight; i++)
            {
                var spriteIndex = IndexRegister + i;
                var spriteData = _memory[spriteIndex];

                var spriteRow = Convert.ToString(spriteData, 2).PadLeft(8, '0');

                var rowIndex = -1;
                while (++rowIndex < spriteRow.Length && x + rowIndex < DisplayWidth)
                {
                    if (spriteRow[rowIndex] == '1')
                    {
                        if (Display[y, x + rowIndex])
                            _variableRegisters[0xF] = 1;

                        Display[y, x + rowIndex] = !Display[y, x + rowIndex];
                    }
                }

                y++;
            }

            DisplayChanged?.Invoke(this, EventArgs.Empty);
            PC += 2;
        }
    }
}