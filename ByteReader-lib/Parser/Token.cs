using System;

namespace ByteReader.Lib.Parser {
    public struct Token {
        public bool Equals(Token other) {
            return tokenStr == other.tokenStr && type == other.type;
        }

        public override bool Equals(object obj) {
            return obj is Token other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(tokenStr, (int) type);
        }

        public readonly string tokenStr;
        public readonly TokenType type;

        public Token(string tokenStr, TokenType type) {
            this.tokenStr = tokenStr;
            this.type = type;
        }

        public static bool operator ==(Token a, TokenType b) {
            return a.type == b;
        }

        public static bool operator !=(Token a, TokenType b) {
            return a.type != b;
        }

        public static bool operator ==(Token a, string b) {
            return a.tokenStr == b;
        }

        public static bool operator !=(Token a, string b) {
            return a.tokenStr != b;
        }

        public static bool operator ==(Token a, Token b) {
            return a.Equals(b);
        }

        public static bool operator !=(Token a, Token b) {
            return !a.Equals(b);
        }

        public override string ToString() {
            return $"[{type.ToString()}] {tokenStr}";
        }
    }

    public enum TokenType {
        Unidentified = 0,
        Terminator,
        Keyword,
        Whitespace,
        TypeDef,
        TypeUse,
        Identifier,
        BlockStart,
        BlockEnd,
        Struct,
        Pointer,
        Reserved = 0xFFF
    }
}