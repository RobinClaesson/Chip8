namespace Chip8.Core
{
    public record Opcode
    {
        public byte First { get; private init; }
        public byte X { get; private init; }
        public byte Y { get; private init; }
        public byte N { get; private init; }
        public byte NN { get; private init; }
        public ushort NNN { get; private init; }
        public ushort All { get; private init; }

        public Opcode(ushort opcode)
        {
            All = opcode;
            First = (byte)((opcode & 0xF000) >> 12);
            X = (byte)((opcode & 0x0F00) >> 8);
            Y = (byte)((opcode & 0x00F0) >> 4);
            N = (byte)(opcode & 0x000F);
            NN = (byte)(opcode & 0x00FF);
            NNN = (ushort)(opcode & 0x0FFF);
        }
    }
}
