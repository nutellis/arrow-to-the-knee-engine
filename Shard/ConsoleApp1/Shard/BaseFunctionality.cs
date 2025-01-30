/*
*
*   Generally useful functions that might be useful everywhere
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;
using System.IO;

namespace Shard
{
    class BaseFunctionality
    {
        private static BaseFunctionality me;

        private BaseFunctionality()
        {
        }

        public static BaseFunctionality getInstance()
        {
            if (me == null)
            {
                me = new BaseFunctionality();
            }

            return me;
        }

        public string readFileAsString(string file)
        {
            string text;

            text = File.ReadAllText(file);

            return text;
        }

        public Dictionary<string, string> readConfigFile(string file)
        {
            Dictionary<string, string> configEntries = new Dictionary<string, string>();
            string text = readFileAsString(file);
            string[] lines = text.Split("\n");
            string[] bits;
            string key, value;

            foreach (string line in lines)
            {
                // Don't read blank lines.
                if (line.Length == 0) {
                    continue;
                }

                // Don't read comments.
                if (line[0] == '#') {
                    continue;
                }

                bits = line.Split(":");

                key = bits[0].Trim();
                value = bits[1].Trim();

                value = value.Replace ("%BASE_DIR%", Bootstrap.getBaseDir());

                configEntries[key] = value;

                Console.WriteLine("Reading " + key + " and " + value);
            }

            return configEntries;
        }
    }
}
