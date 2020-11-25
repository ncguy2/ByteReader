using System;
using System.Collections.Generic;
using ByteReader.Lib.Parser;
using ByteReader.Lib.Parser.Types;
using NUnit.Framework;

namespace ByteReader_lib_tests {
    public class Tests {
        private string input;

        [SetUp]
        public void Setup() {
            input = "struct TestObject {\n" +
                    "    int a;\n" +
                    "    int b;\n" +
                    "    char c;" +
                    "    bool d;" +
                    "}\nTestObject test;";
        }

        [Test]
        public void TokeniseTest() {
            Tokeniser tokeniser = new();
            IEnumerable<Token> tokens = tokeniser.TokeniseString(input);

            foreach (Token token in tokens) {
                Console.WriteLine(token.ToString());
            }
        }

        [Test]
        public void ParseTest() {
            IEnumerable<ParseNode> parseNodes = new TemplateParser().Parse(input);

            ParseResults parseResults = new();

            foreach (ParseNode parseNode in parseNodes)
                parseResults.AddNode(parseNode);

            parseResults.Dump(Console.WriteLine);

        }

        [Test]
        public void ConvertTest() {
            MockByteProvider provider = new MockByteProvider();

            provider.Add((int) 12345);
            provider.Add((char) 'c');

            IntType intType = new();
            CharType charType = new();

            intType.ReadBytes(provider);
            charType.ReadBytes(provider);

            // Assert.AreEqual(intType.GetValue(), 12345);
            // Assert.AreEqual(charType.GetValue(), 'c');

        }

    }
}