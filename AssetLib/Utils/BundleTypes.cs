
namespace AssetLib.Utils
{
    public enum CompressionType
    {
        None,
        Lzma,
        Lz4,
        Lz4HC,
        LzHAM,
        Lz4Mr0k
    }
    public class Header
    {
        public const int HeaderSize = 0x32;

        public string Signature;
        public uint Version;
        public string UnityVersion;
        public string UnityRevision;
        public long Size;
        public int CompressedBlocksInfoSize;
        public int UncompressedBlocksInfoSize;
        public int Flags;
    }

    public class StorageBlock
    {
        public int CompressedSize;
        public int UncompressedSize;
        public short Flags;
    }

    public class Node
    {
        public long Offset;
        public long Size;
        public int Flags;
        public string Path;
        public bool IsAssetFile;
    }
    public class StreamFile
    {
        public string Path;
        public string FileName;
        public Stream Stream;
    }
}
