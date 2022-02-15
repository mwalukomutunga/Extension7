using System.Collections.Generic;

namespace BimaPimaUssd
{
    public class Edge
    {
        public int src, dst;
        public Edge(int s, int d)
        {
            src = s;
            dst = d;  
        }
    }
    public class Graph
    {
     
        public Dictionary<int, List<int>> adj;
        public Graph(List<Edge> edges)
        {
            foreach (var item in edges)
            {
               var src = item.src;
               var dst = item.dst;
               if(!adj.ContainsKey(src)) adj.Add(src, new List<int>());
               if(!adj.ContainsKey(dst)) adj.Add(dst, new List<int>());
               adj[src].Add(dst);
               adj[dst].Add(src);
            }
        }
    }
    public class Class
    {
        public void DFS(Graph graph,int v)
        {
            var stack = new Stack<int>();
            stack.Push(v);
            while (stack.Count > 0)
            {
                var value = stack.Pop();
                System.Console.WriteLine(value);
                foreach (var child in graph.adj[value])
                {
                    stack.Push(child);
                }
            }
        }
        //only ilitativelt
        public void BFS(Graph graph, int v)
        {
            var queue = new Queue<int>();   
            queue.Enqueue(v);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                System.Console.WriteLine(current);
                foreach (var child in graph.adj[current]) { queue.Enqueue(child); }
            }
        }
        public void DFS2(Graph graph,int src)
        {
            System.Console.WriteLine(src);
            foreach (var child in graph.adj[src])
            {
                DFS2(graph, child);
            }
        }
        public bool HasTarget(Graph graph, int src,int des, HashSet<int> visited)
        {
            if (visited.Contains(src)) return false;
            if (src == des) return true;           
            foreach (var child in graph.adj[src])
            {
               visited.Contains(child);
              return  HasTarget(graph, child,des, visited);
            }return false;
        }
        public bool Target(Graph graph, int v,int i)
        {
            var queue = new Queue<int>();
            queue.Enqueue(v);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == i) return true;
                foreach (var child in graph.adj[current]) { queue.Enqueue(child); }
            }
            return false;
        }
        public int ConnectedComponent(Graph graph)
        {
            int count =0;
            var visited = new HashSet<int>();
            foreach (var item in graph.adj)
            {
                if (explore(graph, item.Key, visited))
                {
                    count++;
                };
            }
            return count;
        }

        private bool explore(Graph graph,int  item, HashSet<int> visited)
        {
            if(visited.Contains(item)) return false;
            visited.Add(item);
            foreach (var v in graph.adj[item])
            {
                visited.Add(v);
                explore(graph, v, visited);
            }
            return true;
        }
        private int exploreSize(Graph graph, int item, HashSet<int> visited)
        {
            if (visited.Contains(item)) return 0;
            visited.Add(item);
            int size = 1; 
            foreach (var v in graph.adj[item])
            {
                size++; 
                visited.Add(v);
                exploreSize(graph, v, visited);
            }
            return size;
        }
    }
}
