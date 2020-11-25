using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ByteReader.Lib.Parser {
    public class Tokeniser {
        private Token? previousToken;
        private bool tokenIdentified;

        public string[] PrepareString(string input) {
            string i = Regex.Replace(input, @"(\w)(\W)", @"$1 $2");
            string j = Regex.Replace(i, @"(\W)(\w)", @"$1 $2");
            string k = Regex.Replace(j, @"(\W)(\W)", @"$1 $2");

            // Special case - Ensure braces are surrounded by whitespaces
            string l = k.Replace("{", " { ").Replace("}", " } ");

            string m = Regex.Replace(l, @"\s+", " ");
            return m.Split(" ");
        }

        public bool IsKeyword(string tokenStr, out TokenType typeOverride) {
            typeOverride = TokenType.Keyword;
            switch (tokenStr.ToLower()) {
                case "struct":
                    typeOverride = TokenType.Struct;
                    return true;
                case "class":
                case "enum":
                case "typedef":
                    typeOverride = TokenType.Reserved;
                    return true;
                default:
                    return false;
            }
        }

        private Token MakeToken(string tokenStr, TokenType type) {
            Token t = new(tokenStr, type);
            tokenIdentified = true;
            previousToken = t;
            return t;
        }

        private bool IsPrevTokenType(TokenType type) {
            if (previousToken == null)
                return false;

            return previousToken.Value.type == type;
        }

        private bool IsPrevTokenTypeOneOf(params TokenType[] types) {
            if (previousToken == null)
                return false;

            foreach (TokenType tokenType in types)
                if (IsPrevTokenType(tokenType))
                    return true;

            return false;
        }

        private void Reset() {
            tokenIdentified = false;
        }

        private TokenType MakeTokenFromCharacter(string tokenString) {
            return tokenString switch {
                ";" => TokenType.Terminator,
                "{" => TokenType.BlockStart,
                "}" => TokenType.BlockEnd,
                "*" => TokenType.Pointer,
                _ => TokenType.Unidentified
            };
        }

        public TokenEnumerator GetTokenEnumerator(string input) {
            return new(TokeniseString(input));
        }

        public IEnumerable<Token> TokeniseString(string input) {
            string[] tokenStrings = PrepareString(input);

            foreach (string tokenString in tokenStrings) {
                Reset();
                // Identify tokens in isolation

                if (IsKeyword(tokenString, out TokenType keywordType)) {
                    yield return MakeToken(tokenString, keywordType);
                    continue;
                }

                TokenType type = MakeTokenFromCharacter(tokenString);
                if (type != TokenType.Unidentified) {
                    yield return MakeToken(tokenString, type);
                }


                // Identify contextual tokens

                if (IsPrevTokenType(TokenType.Struct)) {
                    yield return MakeToken(tokenString, TokenType.TypeDef);
                    continue;
                }

                if (Regex.IsMatch(tokenString, @"\w(\w|\d|_)*")) {
                    TokenType t = TokenType.Identifier;
                    if (IsPrevTokenTypeOneOf(TokenType.Terminator, TokenType.BlockStart, TokenType.BlockEnd))
                        t = TokenType.TypeUse;
                    yield return MakeToken(tokenString, t);
                }

                if (!tokenIdentified)
                    yield return MakeToken(tokenString, TokenType.Unidentified);
            }
        }
    }
}