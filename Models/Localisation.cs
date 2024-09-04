using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMV.Models
{
    public static class Localisation
    {
        public static List<Tuple<string, string>> Read(StreamReader reader)
        {
            List<Tuple<string, string>> localisation = new List<Tuple<string, string>>();

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();

                if (String.IsNullOrWhiteSpace(line))
                    continue;

                line = line.Trim();
                // Ignore comment line
                if (line.StartsWith("#"))
                {
                    continue;
                }

                if (line.IndexOf(' ') == -1)
                {
                    continue;
                }

                string[] split = line.Split(new char[] { ' ' }, 2);
                    
                if (split[0].Length == 0 || split[1].Length <= 2) 
                {
                    continue;
                }

                string[] key = split[0].Split(':');
                string value = split[1].Substring(1, split[1].Length - 2);

                localisation.Add(new Tuple<string, string>(key[0], value));
            }

            return localisation;
        }
    }
}
