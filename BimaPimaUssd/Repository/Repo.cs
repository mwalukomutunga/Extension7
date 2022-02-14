using BimaPimaUssd.Contracts;
using BimaPimaUssd.Models;
using System.Collections.Generic;

namespace BimaPimaUssd.Repository
{
    public class Repo : IRepository
    {
        public Dictionary<string, User> Users { get; }
        public Dictionary<string, Stack<int>> levels { get; }
        public Dictionary<string, LinkedList<string>> Requests { get; }
        public Dictionary<string, dynamic> Data { get; }
        

        public Repo()
        {
            Users = new Dictionary<string, User>();
            levels = new Dictionary<string, Stack<int>>();
            Data = new Dictionary<string, dynamic>();
            Requests = new Dictionary<string, LinkedList<string>>();
           
        }

    }
}
