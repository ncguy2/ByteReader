using System;

namespace ByteReader.Lib.Parser {
    public abstract class ParseNode {

        public readonly Token token;
        public string NodeString => token.tokenStr;
        public CompositeParseNode Parent => parent;

        protected CompositeParseNode parent;

        protected ParseNode(Token token) {
            this.token = token;
        }

        public void SetParent(CompositeParseNode parent) {
            this.parent = parent;
        }

        public override string ToString() {
            return $"[{this.GetType().FullName}] {NodeString}";
        }

        public virtual void Dump(DumpContext context) {
            context.Print(this.ToString());
        }

    }
}