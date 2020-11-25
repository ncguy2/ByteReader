using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteReader.Lib.Parser {
    public class TokenEnumerator : IDisposable {

        private readonly IEnumerator<Token> tokens;

        public TokenEnumerator(IEnumerable<Token> tokens) {
            this.tokens = tokens.GetEnumerator();
        }

        public Token Current => tokens.Current;
        public Token Next => GetNext();

        public IEnumerable<Token> LookAhead(TokenType type, bool includeCurrent = false) {
            if (includeCurrent)
                yield return Current;

            while (tokens.MoveNext()) {
                yield return Current;

                if (Current == type) {
                    break;
                }
            }
        }

        public IEnumerable<T> ForEach<T>(Func<Token, TokenEnumerator, T> task) {
            while (MoveNext()) {
                yield return task(Current, this);
            }
        }

        public Token GetNext() {
            if (MoveNext()) {
                return Current;
            }

            throw new IndexOutOfRangeException("Enumerator has no more elements");
        }

        public bool MoveNext() {
            return tokens.MoveNext();
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            tokens.Dispose();
        }
    }
}