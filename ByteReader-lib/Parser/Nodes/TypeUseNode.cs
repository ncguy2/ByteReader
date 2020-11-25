using System;
using ByteReader.Lib.Parser.Types;

namespace ByteReader.Lib.Parser {
    public class TypeUseNode : CompositeParseNode {
        public TypeUseNode(Token token) : base(token) { }

        public string TypeName => NodeString;

        public TypeDefNode Resolve(ParseResults results) {
            return results.FindTypeDefNode(TypeName) as TypeDefNode;
        }

        public override void Dump(DumpContext context) {
            base.Dump(context);

            DumpContext childContext = context.Descend();
            childContext.prefixTerm = "->";
            childContext.recursive = false;
            childContext.depth += 1;

            Resolve(context.results)?.Dump(childContext);
        }

        public ParseType GetType(ParseResults results) {
            return results.GetParseType(TypeName);
        }

    }
}