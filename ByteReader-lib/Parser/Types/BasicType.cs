using System;

namespace ByteReader.Lib.Parser.Types {
    public abstract class BasicType<T> : ParseType {

        public int size;

        public BasicType(string token, int size) {
            this.tokenIdentifier = token;
            this.size = size;
        }

        public override int GetSizeInBytes() {
            return size;
        }

        public override string GetValueAsString() {
            return GetValue().ToString();
        }

        public abstract T GetValue();

    }

    public class BoolType : BasicType<bool> {
        public BoolType() : base("bool", 1) { }
        public override bool GetValue() {
            return bytes[0] != 0;
        }
    }

    public class CharType : BasicType<char> {
        public CharType() : base("char", 1) { }
        public override char GetValue() {
            return (char) bytes[0];
        }
    }

    public class ByteType : BasicType<byte> {
        public ByteType() : base("byte", 1) { }

        public override byte GetValue() {
            return bytes[0];
        }
    }

    public class HexByteType : ByteType {
        public HexByteType() {
            tokenIdentifier = "0x" + tokenIdentifier;
        }

        public override string GetValueAsString() {
            return "0x" + GetValue().ToString("X2");
        }
    }

    public class ShortType : BasicType<short> {
        public ShortType() : base("short", 2) { }

        public override short GetValue() {
            return BitConverter.ToInt16(bytes);
        }
    }

    public class HexShortType : ShortType {
        public HexShortType() {
            tokenIdentifier = "0x" + tokenIdentifier;
        }

        public override string GetValueAsString() {
            return "0x" + GetValue().ToString("X4");
        }
    }

    public class IntType : BasicType<int> {
        public IntType() : base("int", 4) { }

        public override int GetValue() {
            return BitConverter.ToInt32(bytes);
        }
    }

    public class HexIntType : IntType {
        public HexIntType() {
            tokenIdentifier = "0x" + tokenIdentifier;
        }

        public override string GetValueAsString() {
            return "0x" + GetValue().ToString("X8");
        }
    }

    public class LongType : BasicType<long> {
        public LongType() : base("long", 8) { }

        public override long GetValue() {
            return BitConverter.ToInt64(bytes);
        }
    }

    public class HexLongType : LongType {
        public HexLongType() {
            tokenIdentifier = "0x" + tokenIdentifier;
        }

        public override string GetValueAsString() {
            return "0x" + GetValue().ToString("X16");
        }
    }

}