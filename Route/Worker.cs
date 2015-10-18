using System;
using System.Collections.Generic;

namespace Route
{
    public class Worker
    {
        private List<Node> pendingRoutes;
        private List<Node> solvedRoutes;

        private int nodeCount;
        private int routeCount;

        public Worker(string src, string dst)
        {
            pendingRoutes = new List<Node>();
            solvedRoutes = new List<Node>();

            parseData(src);
            handleData();
            printData();
            saveData(dst);
        }

        private void parseData(string src)
        {
            string[] lines;

            try
            {
                lines = System.IO.File.ReadAllLines(src);
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine("File Not Found");
                return;
            }

            if (lines.Length < 2)
            {
                return;
            }

            var firstLine = true;

            foreach (var line in lines)
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] data = line.Split(' ');
                if (firstLine)
                {
                    nodeCount = Int32.Parse(data[0]);
                    routeCount = Int32.Parse(data[1]);

                    for (var i = 0; i < nodeCount; i++)
                    {
                        var node = new Node();
                        node.Name = i;
                        for (var j = 0; j < nodeCount; j++)
                        {
                            if (j == i)
                            {
                                node.Routes.Add(j, 0);
                                var trace = new Trace();
                                trace.Nodes.Add(j);
                                node.Traces.Add(j, trace);
                            }
                            else
                            {
                                node.Routes.Add(j, -1);
                            }
                        }
                        pendingRoutes.Add(node);
                    }

                    firstLine = false;
                    Console.WriteLine("node count {0}, route count: {1}", nodeCount, routeCount);
                }
                else
                {
                    Console.WriteLine("{0} {1} {2}", data[0], data[1], data[2]);
                    int srcNode = Int32.Parse(data[0]);
                    int destNode = Int32.Parse(data[1]);
                    int cost = Int32.Parse(data[2]);

                    Node node = null;

                    foreach (var n in pendingRoutes)
                    {
                        if (n.Name == srcNode)
                        {
                            node = n;
                            break;
                        }
                    }

                    if (node == null)
                    {
                        node = new Node();
                        for (int i = 0; i < nodeCount; i++)
                        {
                            if (i == srcNode)
                            {
                                node.Routes.Add(i, 0);
                            }
                            else
                            {
                                node.Routes.Add(i, -1);
                            }
                        }
                        pendingRoutes.Add(node);
                    }
                    node.Name = srcNode;
                    node.Routes[destNode] = cost;
                    Trace trace = new Trace();
                    trace.Nodes.Add(srcNode);
                    trace.Nodes.Add(destNode);
                    node.Traces.Add(destNode, trace);
                }
            }

            
            Console.WriteLine("----------------");
        }

        private void handleData()
        {
            while (pendingRoutes.Count != 0)
            {
                Node node = pendingRoutes[0];
                pendingRoutes.Remove(node);

                foreach (Node solvedRoute in solvedRoutes)
                {
                    if (!solvedRoute.Routes.ContainsKey(node.Name) || solvedRoute.Routes[node.Name] == -1)
                    {
                        break;
                    }

                    var cost = solvedRoute.Routes[node.Name];

                    foreach (KeyValuePair<int, int> pair in node.Routes)
                    {
                        if (pair.Value == -1)
                        {
                            continue;
                        }

                        int originalCost = solvedRoute.Routes.ContainsKey(pair.Key)
                            ? solvedRoute.Routes[pair.Key]
                            : Int32.MaxValue;
                        if (originalCost == -1 || originalCost > cost + pair.Value)
                        {
                            solvedRoute.Routes[pair.Key] = cost + pair.Value;
                            Trace trace = null;
                            if (!solvedRoute.Traces.ContainsKey(pair.Key))
                            {
                                trace = new Trace();
                                solvedRoute.Traces.Add(pair.Key, trace);
                            }
                            else
                            {
                                trace = solvedRoute.Traces[pair.Key];
                            }
                            trace.Nodes.Clear();
                            trace.Nodes.Add(solvedRoute.Name);
                            trace.AppendTrace(node.Traces[pair.Key]);
                        }
                    }
                }

                solvedRoutes.Add(node);
            }
        }

        private void printData()
        {
            foreach (var solvedRoute in solvedRoutes)
            {
                Console.WriteLine("Node: {0}", solvedRoute.Name);
                foreach (KeyValuePair<int, int> pair in solvedRoute.Routes)
                {
                    if (pair.Value == 0)
                    {
                        Console.WriteLine("{0} 0", pair.Key);
                        continue;
                    }
                    else if (pair.Value == -1)
                    {
                        Console.WriteLine("-1");
                        continue;
                    }
                    Console.Write("{0} ", pair.Value);
                    Trace trace = solvedRoute.Traces[pair.Key];
                    foreach (var n in trace.Nodes)
                    {
                        Console.Write("{0} ", n);
                    }
                    Console.Write("\n");
                }
                Console.Write("\n");
            }
        }

        private void saveData(String dst)
        {
            string NEW_LINE = "\r\n";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(dst, true))
            {
                file.Write(NEW_LINE);
                foreach (var solvedRoute in solvedRoutes)
                {
                    file.WriteLine("Node: " + solvedRoute.Name);
                    foreach (KeyValuePair<int, int> pair in solvedRoute.Routes)
                    {
                        if (pair.Value == 0)
                        {
                            file.WriteLine(pair.Key + " 0");
                            continue;
                        }
                        else if (pair.Value == -1)
                        {
                            file.WriteLine("-1");
                            continue;
                        }
                        file.Write(pair.Value + " ");
                        Trace trace = solvedRoute.Traces[pair.Key];
                        foreach (var n in trace.Nodes)
                        {
                            file.Write(n + " ");
                        }
                        file.Write(NEW_LINE);
                    }
                    file.Write(NEW_LINE);
                }
            }
            Console.WriteLine("\nDone");
        }
    }
}