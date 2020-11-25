using System;
using System.Collections.Generic;

namespace ByteReader.Lib.Api {
    public abstract class IByteProvider {

        protected ulong ptr;
        public ulong Pointer => ptr;

        public abstract byte Next();
        protected abstract byte[] Get(ulong offset, long size);

        public byte[] Next(int n) {
            byte[] bytes = Get(Pointer, n);
            Increment(n);
            return bytes;
        }

        public void SetPointer(ulong offset) {
            ptr = offset;
        }

        private Stack<ulong> ptrStack = new();

        public void Push(ulong newPtr) {
            ptrStack.Push(ptr);
            ptr = newPtr;
        }

        public void Pop() {
            ptr = ptrStack.Pop();
        }

        public void Increment(int step) {
            ptr += (ulong) step;
        }
    }
}