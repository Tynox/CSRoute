using System.Collections.Generic;

namespace Route
{
    public class Node
    {
        public int Name { get; set; }

        public Dictionary<int, int> Routes { get; set; }

        public Dictionary<int, Trace> Traces { get; set; }

        public Node()
        {
            Routes = new Dictionary<int, int>();
            Traces = new Dictionary<int, Trace>();
        }
    }
}