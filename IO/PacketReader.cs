using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OpenMaple.IO
{
    class PacketReader
    {
        private BinaryReader reader;

        public PacketReader(Stream stream)
        {
            this.reader = new BinaryReader(stream, Encoding.UTF8);
        }

        public string ReadMapleAsciiString()
        {
            int length = reader.ReadInt16();
            string str = this.ReadAsciiString(length);
            return str;
        }

        private string ReadAsciiString(int length)
        {
            string str = new string(reader.ReadChars(length));
            return str;
        }
    }
}
