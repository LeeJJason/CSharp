using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Serialize
{
    class Program
    {
        static void Main(string[] args)
        {
            var objectGraph = new List<String> {"Jeff", "Kristin", "Aidan", "Grant" };

            Stream stream = SerializeToMemofy(objectGraph);

            stream.Position = 0;
            objectGraph = null;

            objectGraph = (List<String>)DeserializeFromMemory(stream);
            foreach (var s in objectGraph)
            {
                Console.WriteLine(s);
            }
            Console.ReadKey();
        }

        private static MemoryStream SerializeToMemofy(Object objectGraph)
        {
            MemoryStream stream = new MemoryStream();

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, objectGraph);

            return stream;
        }

        private static Object DeserializeFromMemory(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }
    }
}
