using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteReader.Lib.Parser {
    public class TemplateParser {

        private Token FindToken(List<Token> tokens, TokenType type, out int index) {
            for (int i = 0; i < tokens.Count; i++) {
                Token token = tokens[i];
                if (token == type) {
                    index = i;
                    return token;
                }
            }

            index = -1;
            return new Token();
        }

        private IEnumerable<List<Token>> SeparateTokens(List<Token> tokens, TokenType typeToSplit) {
            int idx = 0;
            int max = tokens.Count;

            for (int index = 0; index < tokens.Count; index++) {
                Token token = tokens[index];
                if (token != typeToSplit)
                    continue;

                yield return tokens.GetRange(idx, (index - idx) + 1);
                idx = index;

            }
        }

        private List<List<Token>> SegmentTokens(List<Token> tokens, Pattern<TokenType> pattern) {
            List<List<Token>> segments = new();

            IEnumerable<List<Token>> separateTokens = SeparateTokens(tokens, pattern.Back.Last);

            foreach (List<Token> ts in separateTokens) {
                int max = ts.Count - pattern.Length;

                for (int i = 0; i <= max; i++) {
                    List<Token> range = ts.GetRange(i, ts.Count - i);
                    if (!pattern.Matches(range.Select(x => x.type).ToList()))
                        continue;
                    segments.Add(range);
                    i = max;
                }
            }

            return segments;
        }

        public string StringifyTokens(IEnumerable<Token> tokens) {
            return tokens.Aggregate("", (current, token) => current + token);
        }

        public IEnumerable<ParseNode> Parse(string input) {

            Tokeniser t = new ();
            using (TokenEnumerator tokens = t.GetTokenEnumerator(input)) {
                while (tokens.MoveNext()) {
                    Token token = tokens.Current;
                    if (token == TokenType.Struct) {
                        IEnumerable<Token> structBlock = tokens.LookAhead(TokenType.BlockEnd);
                        yield return ParseTypeDef(structBlock.ToList());
                    }else if (token == TokenType.TypeUse) {
                        IEnumerable<Token> useBlock = tokens.LookAhead(TokenType.Terminator, includeCurrent: true);
                        yield return ParseTypeUse(useBlock.ToList());
                    }
                }
            }
        }

        private ParseNode ParseTypeUse(List<Token> tokens) {
            Token typeUseToken = FindToken(tokens, TokenType.TypeUse, out int _);
            Token ptrToken = FindToken(tokens, TokenType.Pointer, out int _);
            Token identifierToken = FindToken(tokens, TokenType.Identifier, out int _);

            TypeUseNode useNode = ptrToken == TokenType.Pointer ? new PointerNode(typeUseToken)
                                                                : new TypeUseNode(typeUseToken);
            IdentifierNode identNode = new(identifierToken);

            useNode.AddChild(identNode);
            return useNode;
        }


        private ParseNode ParseTypeDef(List<Token> tokens) {
            Token typeDefToken = FindToken(tokens, TokenType.TypeDef, out int _);
            TypeDefNode typeDefNode = new(typeDefToken);

            Pattern<TokenType> pattern = new();
            pattern.SetFrontPattern(TokenType.TypeUse);
            pattern.SetBackPattern(TokenType.Identifier, TokenType.Terminator);

            List<List<Token>> segmentTokens = SegmentTokens(tokens, pattern);

            Console.WriteLine(segmentTokens.Count);

            foreach (List<Token> segment in segmentTokens) {
                typeDefNode.AddChild(ParseTypeUse(segment));
            }

            return typeDefNode;
        }

    }

    public class Pattern<T> {
        private PatternItem<T> frontPattern;
        private PatternItem<T> backPattern;

        public int Length => (frontPattern != null ? Front.Length : 0) + (backPattern != null ? Back.Length : 0);

        public PatternItem<T> Front => frontPattern;
        public PatternItem<T> Back => backPattern;

        public void SetFrontPattern(params T[] pattern) {
            SetFrontPattern(new PatternItem<T>(pattern));
        }

        public void SetBackPattern(params T[] pattern) {
            SetBackPattern(new PatternItem<T>(pattern));
        }

        public void SetFrontPattern(PatternItem<T> frontPattern) {
            this.frontPattern = frontPattern;
        }

        public void SetBackPattern(PatternItem<T> backPattern) {
            this.backPattern = backPattern;
        }

        public bool MatchesFront(List<T> items) {
            if (frontPattern == null)
                return true;

            List<T> frontRange = items.GetRange(0, frontPattern.types.Count);
            return Front.Matches(frontRange);
        }

        public bool MatchesBack(List<T> items) {
            if (backPattern == null)
                return true;

            List<T> backRange = items.GetRange(items.Count - backPattern.types.Count, backPattern.types.Count);
            return Back.Matches(backRange);
        }

        public bool Matches(List<T> items) {
            return MatchesFront(items) && MatchesBack(items);
        }

    }

    public class PatternItem<T> {
        public readonly List<T> types;

        public PatternItem(params T[] pattern) {
            types = new List<T>(pattern);
        }

        public T this[in int index] => types[index];

        public T First => this[0];
        public T Last => this[types.Count - 1];
        public int Length => types.Count;

        public bool Matches(IEnumerable<T> sublist) {
            return sublist.Select((o, index) => Equals(o, this[index])).All(x => x);
        }

    }

}