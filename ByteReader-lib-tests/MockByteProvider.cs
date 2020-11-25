using System;
using System.Collections.Generic;
using ByteReader.Lib.Api;

namespace ByteReader_lib_tests {
    public class MockByteProvider : IByteProvider {
        private Queue<byte> bytes;

        public MockByteProvider() {
            bytes = new Queue<byte>();
        }

        public void Add(bool b) {
            bytes.Enqueue(b ? 0xFF : 0x0);
        }

        public void Add(char c) {
            bytes.Enqueue((byte) c);
        }

        public void Add(byte b) {
            bytes.Enqueue(b);
        }

        public void Add(short s) {
            Span<byte> span = new(new byte[2]);
            BitConverter.TryWriteBytes(span, s);
            foreach (byte t in span) {
                bytes.Enqueue(t);
            }
        }

        public void Add(int i) {
            Span<byte> span = new(new byte[4]);
            BitConverter.TryWriteBytes(span, i);
            foreach (byte t in span) {
                bytes.Enqueue(t);
            }
        }

        public void Add(long l) {
            Span<byte> span = new(new byte[8]);
            BitConverter.TryWriteBytes(span, l);
            foreach (byte t in span) {
                bytes.Enqueue(t);
            }
        }

        public override byte Next() {
            return bytes.Dequeue();
        }

        protected override byte[] Get(ulong offset, long size) {
            throw new NotImplementedException();
        }
    }
}