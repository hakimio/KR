using System;
using System.Collections.Generic;
using System.Text;

namespace Dialog_Editor
{
    class IdGenerator
    {
        private static Random rnd = new Random();
        private HashSet<String> ids;

        public IdGenerator()
        {
            ids = new HashSet<String>();
        }

        public IdGenerator(String[] ids): this()
        {
            for (int i = 0; i < ids.Length; i++)
                this.ids.Add(ids[i]);
        }

        public bool addId(String id)
        {
            if (ids.Contains(id))
                return false;

            ids.Add(id);
            
            return true;
        }

        public bool contains(String id)
        {
            return ids.Contains(id);
        }

        public bool removeId(String id)
        {
            if (!ids.Contains(id))
                return false;
            
            ids.Remove(id);
            return true;
        }

        public void setIds(String[] ids)
        {
            this.ids.Clear();

            for (int i = 0; i < ids.Length; i++)
                this.ids.Add(ids[i]);
        }

        public String[] getIds()
        {
            if (ids.Count > 0)
                return ids.ToList().ToArray();
            else return new String[0];
        }

        public static String getNpcId(String msg)
        {
            String id = msg;

            id = id.Replace(" ", "_");
            do
            {
                if (msg.Length >= 16)
                    id = id.Substring(0, 16) + "_" + rnd.Next(999);
                else
                    id = id + "_" + rnd.Next(999);
            }
            while (GUI.NpcIdGenerator.contains(id));

            return id;
        }
    }
}
