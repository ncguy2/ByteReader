using System;

namespace ByteReader.Lib.Parser {
    public struct DumpContext {
        public int depth;
        public string prefixStep;
        public string prefixTerm;
        public bool recursive;
        public ParseResults results;
        public Action<string> print;

        private string prefix;

        public string Prefix {
            get {
                if (prefix == null)
                    buildPrefix();
                return prefix;
            }
        }

        private string buildPrefix() {
            prefix = "";
            for (int i = 0; i < depth; i++)
                prefix += prefixStep;

            return prefix = prefix.Substring(0, prefix.Length - prefixTerm.Length) + prefixTerm;
        }

        public void Print(string s) {
            print(Prefix + s);
        }

        public DumpContext Descend() {
            return new() {
                depth = depth + 1,
                prefixStep = prefixStep,
                prefixTerm = prefixTerm,
                recursive = recursive,
                results = results,
                print = print
            };
        }
    }
}