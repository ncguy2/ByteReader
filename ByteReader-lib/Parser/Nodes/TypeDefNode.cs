namespace ByteReader.Lib.Parser {
    public class TypeDefNode : CompositeParseNode {
        public TypeDefNode(Token token) : base(token) { }

        public string typeName => NodeString;

    }
}