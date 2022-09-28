namespace Chip8.Core
{
    /// <summary>
    /// A 2 byte Chip8 Opcode ordered FXYN
    /// </summary>
    public record Opcode
    {
        /// <summary>
        /// First nibble in the opcode 
        /// </summary>
        public byte First { get; private init; }
        /// <summary>
        /// Second nibble in the opcode
        /// </summary>
        public byte X { get; private init; }     
        /// <summary>
        /// Third nibble in the opcode 
        /// </summary>
        public byte Y { get; private init; }   
        /// <summary>
        /// Fourth (last) nibble in the opcode
        /// </summary>
        public byte N { get; private init; }
        /// <summary>
        /// The second byte (third and fourth nibbles) in the opcode
        /// </summary>
        public byte NN { get; private init; }
        /// <summary>
        /// The second, third and fourth nibbles in the opcode
        /// </summary>
        public ushort NNN { get; private init; }
        /// <summary>
        /// All the four nibbles of the opcodes
        /// </summary>
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
