using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteReader.Lib.Api;

namespace ByteReader.Lib.Parser.Types {
    public class CompositeParseType : ParseType {

        protected Dictionary<string, ParseType> subtypes;

        public CompositeParseType(string tokenIdentifier) {
            this.tokenIdentifier = tokenIdentifier;
            subtypes = new Dictionary<string, ParseType>();
        }

        public CompositeParseType Add(IEnumerable<Tuple<ParseType, string>> nodes) {
            foreach ((ParseType type, string identifier) in nodes) {
                subtypes.Add(identifier, type);
            }
            return this;
        }

        public override int GetSizeInBytes() {
            return subtypes.Values.Select(x => x.GetSizeInBytes()).Sum(x => x);
        }

        public override int ReadBytes(IByteProvider provider) {
            int bytesRead = 0;
            foreach ((string _, ParseType type) in subtypes)
                bytesRead += type.ReadBytes(provider);

            return bytesRead;
        }

        public override string GetValueAsString() {
            StringBuilder sb = new();

            sb.Append('{');

            int idx = 0;
            foreach ((string key, ParseType value) in subtypes) {
                bool isLast = idx == (subtypes.Count - 1);

                sb.Append('"').Append(key).Append('"').Append(": ");

                if (!(value is CompositeParseType)) {
                    sb.Append('"');
                }
                sb.Append(value.GetValueAsString());
                if (!(value is CompositeParseType)) {
                    sb.Append('"');
                }

                if (!isLast)
                    sb.Append(", ");

                idx++;
            }

            sb.Append('}');

            return sb.ToString();
        }
    }
}