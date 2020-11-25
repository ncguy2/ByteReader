using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteReader.Lib.Parser {
    public abstract class CompositeParseNode : ParseNode {
        protected CompositeParseNode(Token token) : base(token) {
            ChildNodes = new List<ParseNode>();
        }

        public readonly List<ParseNode> ChildNodes;

        public void AddChild(ParseNode node) {
            ChildNodes.Add(node);
            node.SetParent(this);
        }

        public override void Dump(DumpContext context) {
            base.Dump(context);

            if (!context.recursive)
                return;

            foreach (ParseNode child in ChildNodes)
                child.Dump(context.Descend());
        }

        public IEnumerable<ParseNode> FindChildrenOfType(TokenType type) {
            return ChildNodes.Where(x => x.token == type);
        }

        public ParseNode FindChildOfType(TokenType type) {
            return FindChildrenOfType(type).FirstOrDefault();
        }



    }
}