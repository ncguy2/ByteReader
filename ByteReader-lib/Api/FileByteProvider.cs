using System;
using System.IO;

namespace ByteReader.Lib.Api {
    public class FileByteProvider : IByteProvider {
        private readonly string _file;
        private byte[] bytes;

        public FileByteProvider(string file) {
            _file = file;

            // TODO Replace with file mapping in anticipation for larger files
            bytes = File.ReadAllBytes(file);
            ptr = 0;
        }

        public override byte Next() {
            return bytes[ptr++];
        }

        protected override byte[] Get(ulong offset, long size) {
            byte[] tgt = new byte[size];
            Array.Copy(bytes, (long) offset, tgt, 0, size);
            return tgt;
        }
    }
}