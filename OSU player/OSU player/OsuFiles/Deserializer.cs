using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSUplayer.OsuFiles
{
    static class Deserializer
    {
        private static BinaryFormatter _formatter;

        public static object Deserialize(Stream stream)
        {
            if (_formatter == null)
            {
                _formatter = new BinaryFormatter {AssemblyFormat = FormatterAssemblyStyle.Simple};
            }
            return _formatter.Deserialize(stream);
        }

    }
}
