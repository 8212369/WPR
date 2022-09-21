using System.Buffers;
using System.IO;

using K4os.Compression.LZ4;

namespace Xamarin.Android.AssemblyStore
{
    // https://github.com/xamarin/xamarin-android/blob/main/tools/decompress-assemblies/main.cs
    public static class AssemblyDecompressor
    {
        const uint CompressedDataMagic = 0x5A4C4158; // 'XALZ', little-endian
        static private ArrayPool<byte> BytePool = ArrayPool<byte>.Shared;

        public static bool IsCompressed(Stream inputStream)
        {
            return (new BinaryReader(inputStream, System.Text.Encoding.Default, true).ReadUInt32() == CompressedDataMagic);
        }

        public static bool Work(Stream inputStream, Stream outputStream)
        {
            bool retVal = true;

            //
            // LZ4 compressed assembly header format:
            //   uint magic;                 // 0x5A4C4158; 'XALZ', little-endian
            //   uint descriptor_index;      // Index into an internal assembly descriptor table
            //   uint uncompressed_length;   // Size of assembly, uncompressed
            //
            using (var reader = new BinaryReader(inputStream))
            {
                uint magic = reader.ReadUInt32();
                if (magic == CompressedDataMagic)
                {
                    reader.ReadUInt32(); // descriptor index, ignore
                    uint decompressedLength = reader.ReadUInt32();

                    int inputLength = (int)(inputStream.Length - 12);
                    byte[] sourceBytes = BytePool.Rent((int)inputLength);
                    reader.Read(sourceBytes, 0, inputLength);

                    byte[] assemblyBytes = BytePool.Rent((int)decompressedLength);
                    int decoded = LZ4Codec.Decode(sourceBytes, 0, inputLength, assemblyBytes, 0, (int)decompressedLength);
                    if (decoded != (int)decompressedLength)
                    {
                        retVal = false;
                    }
                    else
                    {
                        outputStream.Write(assemblyBytes, 0, decoded);
                        outputStream.Flush();
                    }

                    BytePool.Return(sourceBytes);
                    BytePool.Return(assemblyBytes);
                }
                else
                {
                    retVal = false;
                }
            }

            return retVal;
        }
    }
}
