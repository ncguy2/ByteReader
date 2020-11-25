using System;
using ByteReader.Lib.Api;

namespace ByteReader.Lib.Parser.Types {
    public class PointerType : ParseType {

        public ulong Address;
        public ParseType ActualType;

        public PointerType(ParseType actualType) {
            ActualType = actualType;
        }

        public override int GetSizeInBytes() {
            return ActualType.GetSizeInBytes();
        }

        public override int ReadBytes(IByteProvider provider) {
            Address = BitConverter.ToUInt32(provider.Next(4), 0);
            provider.Push(Address);
            ActualType.ReadBytes(provider);
            provider.Pop();
            return 4;
        }

        public override string GetValueAsString() {
            return ActualType.GetValueAsString();
        }
    }
}