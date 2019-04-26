using System;
using System.IO;
using System.Text;

namespace Ark
{
	public static class BinaryRWExtensions
	{
		public static uint ReadVLUInt32(this BinaryReader reader)
		{
			return (uint)reader.ReadVLInt32();
		}

		public static int ReadVLInt32(this BinaryReader reader)
		{
			// Read out an Int32 7 bits at a time.
			// The high bit of the byte when on means to continue reading more bytes.
			int count = 0;
			int shift = 0;
			byte b;
			do
			{
				// Check for a corrupted stream.  Read a max of 5 bytes.
				// In a future version, add a DataFormatException.
				if (shift == 5 * 7)  // 5 bytes max per Int32, shift += 7
					throw new FormatException("Format_Bad7BitInt32");

				// ReadByte handles end of stream cases for us.
				b = reader.ReadByte();
				count |= (b & 0x7F) << shift;
				shift += 7;
			}
			while ((b & 0x80) != 0);

			return count;
		}

		public static void WriteVL(this BinaryWriter writer, int val)
		{
			writer.WriteVL((uint)val);
		}

		public static void WriteVL(this BinaryWriter writer, uint val)
		{
			// Write out an int 7 bits at a time.
			// The high bit of the byte, when on, tells reader to continue reading more bytes.
			while (val >= 0x80)
			{
				writer.Write((byte)(val | 0x80));
				val >>= 7;
			}

			writer.Write((byte)val);
		}
	}
}
