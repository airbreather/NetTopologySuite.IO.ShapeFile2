using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace NetTopologySuite.IO
{
    internal static class BitTwiddlers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ToOrFromBigEndian(int val) => BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(val) : val;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ToOrFromLittleEndian(int val) => BitConverter.IsLittleEndian ? val : BinaryPrimitives.ReverseEndianness(val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static double ToOrFromLittleEndian(double val) => BitConverter.IsLittleEndian ? val : BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(val)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint WordsToBytes(int value) => unchecked((uint)(value + (long)value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int BytesToWords(uint value)
        {
            if ((value & 1) == 1)
            {
                ThrowArgumentExceptionForOddNumber();
            }

            return checked((int)(value / 2));
        }

#pragma warning disable CA2208 // Instantiate argument exceptions correctly
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForOddNumber() => throw new ArgumentException("Only even numbers are supported.", "value");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
    }
}
