namespace ByteReader.Lib.Parser {
    public class IdentifierNode : ParseNode {
        public IdentifierNode(Token token) : base(token) { }

        public string Identifier => NodeString;

    }
}