using System.Collections.Generic;

namespace Route
{
    public class Trace
    {
        public List<int> Nodes;

        public Trace()
        {
            Nodes = new List<int>();
        }

        public void AppendTrace(Trace t)
        {
            foreach (var node in t.Nodes)
            {
                Nodes.Add(node);
            }
        }
    }
}