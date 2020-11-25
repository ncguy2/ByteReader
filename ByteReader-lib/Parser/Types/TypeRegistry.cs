using System;
using System.Collections.Generic;
using System.Linq;

namespace ByteReader.Lib.Parser.Types {
    public class TypeRegistry {
        private Dictionary<string, ParseType> typeMap = new();

        public TypeRegistry() {
            RegisterDefaultTypes();
        }

        public int Count => typeMap.Count;

        public void RegisterType(ParseType type) {
            typeMap.Add(type.tokenIdentifier, type);
        }

        public void RegisterDefaultTypes() {
            RegisterType(new BoolType());
            RegisterType(new ByteType());
            RegisterType(new CharType());
            RegisterType(new ShortType());
            RegisterType(new IntType());
            RegisterType(new LongType());

            RegisterType(new HexByteType());
            RegisterType(new HexShortType());
            RegisterType(new HexIntType());
            RegisterType(new HexLongType());
        }

        private Tuple<ParseType, string> ConvertUseNode(TypeUseNode node) {
            ParseType type = Get(node.TypeName);
            IdentifierNode identifier = node.FindChildOfType(TokenType.Identifier) as IdentifierNode;

            if (identifier == null) {
                throw new Exception("Type use without identifier");
            }

            if (node is PointerNode)
                type = new PointerType(type);

            return new Tuple<ParseType, string>(type, identifier.Identifier);
        }

        public void RegisterType(TypeDefNode node) {
            CompositeParseType compositeParseType = new(node.typeName);

            // List<Tuple<ParseType, string>> typeNodes = new List<Tuple<ParseType, string>>();

            IEnumerable<Tuple<ParseType, string>> typeNodes = node.ChildNodes.OfType<TypeUseNode>().Select(ConvertUseNode);
            compositeParseType.Add(typeNodes);

            RegisterType(compositeParseType);
        }

        public ParseType Get(string typename) {
            return typeMap[typename].Clone() as ParseType;
        }
    }
}