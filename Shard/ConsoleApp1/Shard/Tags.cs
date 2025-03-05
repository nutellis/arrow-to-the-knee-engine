using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard.Shard
{
    internal class Tags
    {
        private List<string> tags;

        public Tags()
        {
            tags = new List<string>();
        }

        public void addTag(string str)
        {
            if (tags.Contains(str))
            {
                return;
            }

            tags.Add(str);
        }

        public void removeTag(string str)
        {
            tags.Remove(str);
        }

        public bool checkTag(string tag)
        {
            return tags.Contains(tag);
        }

        public String getTags()
        {
            string str = "";

            foreach (string s in tags)
            {
                str += s;
                str += ";";
            }

            return str;
        }
    }
}
