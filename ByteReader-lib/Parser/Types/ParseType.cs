using System;
using ByteReader.Lib.Api;

namespace ByteReader.Lib.Parser.Types {
    public abstract class ParseType : ICloneable {

        public string tokenIdentifier;
        public byte[] bytes;

        public int Size => GetSizeInBytes();

        public abstract int GetSizeInBytes();

        public virtual int ReadBytes(IByteProvider provider) {
            int sizeInBytes = GetSizeInBytes();
            bytes = provider.Next(sizeInBytes);
            return sizeInBytes;
        }

        public virtual string GetValueAsString() {
            return bytes.ToString();
        }

        public object Clone() {
            return this.MemberwiseClone();
        }

        public static byte[] Flip(byte[] arr) {
            byte[] newArr = new byte[arr.Length];
            Array.Copy(arr, newArr, newArr.Length);
            Array.Reverse(newArr);
            return newArr;
        }

    }
}