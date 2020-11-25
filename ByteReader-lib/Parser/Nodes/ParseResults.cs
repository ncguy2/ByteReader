using System;
using System.Collections.Generic;
using System.Linq;
using ByteReader.Lib.Parser.Types;

namespace ByteReader.Lib.Parser {
    public class ParseResults {
        private readonly List<ParseNode> topLevelNodes;
        private readonly TypeRegistry typeRegistry;

        public ParseResults() {
            topLevelNodes = new List<ParseNode>();
            typeRegistry = new TypeRegistry();
        }

        public TypeRegistry Types => typeRegistry;

        public void AddNode(ParseNode node) {
            topLevelNodes.Add(node);
        }

        public IEnumerable<ParseNode> FindTopLevelNodesOfType(TokenType type) {
            return topLevelNodes.Where(x => x.token == type);
        }

        public ParseNode FindTypeDefNode(string typeName) {
            // ReSharper disable once PossibleNullReferenceException
            return FindTopLevelNodesOfType(TokenType.TypeDef).Select(x => x as TypeDefNode)
                                                             .FirstOrDefault(x => x.typeName.Equals(typeName));
        }

        public void Finalise() {
            foreach (TypeDefNode def in topLevelNodes.OfType<TypeDefNode>())
                typeRegistry.RegisterType(def);
        }

        public void Dump(Action<string> print) {
            DumpContext context = new() {
                results = this,
                print = print,
                recursive = true,
                prefixStep = "  ",
                prefixTerm = ""
            };

            foreach (ParseNode topLevelNode in topLevelNodes)
                topLevelNode.Dump(context);
        }

        public ParseType GetParseType(string typeName) {
            return typeRegistry.Get(typeName);
        }

        public Dictionary<string, ParseType> ConvertToTypeTree() {
            Dictionary<string, ParseType> typeMap = new();

            IEnumerable<TypeUseNode> uses = FindTopLevelNodesOfType(TokenType.TypeUse).OfType<TypeUseNode>();

            foreach (TypeUseNode use in uses)
                ConvertToTypeTree(use, typeMap);

            return typeMap;
        }

        public void ConvertToTypeTree(TypeUseNode useNode, Dictionary<string, ParseType> typeMap) {
            string identifier = (useNode.FindChildOfType(TokenType.Identifier) as IdentifierNode)?.Identifier;
            ParseType parseType = Types.Get(useNode.TypeName);

            typeMap.Add(identifier ?? string.Empty, parseType);
        }

    }
}