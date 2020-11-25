using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ByteReader.Lib.Api;
using ByteReader.Lib.Parser;
using ByteReader.Lib.Parser.Types;
using NUnit.Framework;

namespace ByteReader_lib_tests {
    public class ElfTest {

        [Test]
        public void RunElfTest() {
            string template = File.ReadAllText(TestContext.CurrentContext.TestDirectory + "\\ElfTemplate.txt");

            TemplateParser parser = new();

            IEnumerable<ParseNode> parseNodes = parser.Parse(template);

            ParseResults parseResults = new();

            foreach (ParseNode parseNode in parseNodes)
                parseResults.AddNode(parseNode);

            parseResults.Dump(Console.WriteLine);

            parseResults.Finalise();

            TypeRegistry resultsTypes = parseResults.Types;
            Console.WriteLine(resultsTypes.Count);

            IByteProvider byteProvider = new FileByteProvider(TestContext.CurrentContext.TestDirectory + "\\sample.elf");

            Dictionary<string,ParseType> convertToTypeTree = parseResults.ConvertToTypeTree();

            Console.WriteLine(convertToTypeTree.Count);

            foreach ((string ident, ParseType type) in convertToTypeTree) {
                type.ReadBytes(byteProvider);
                Console.WriteLine($"{ident}: \"{type.GetValueAsString()}\"");
            }

        }

    }
}
