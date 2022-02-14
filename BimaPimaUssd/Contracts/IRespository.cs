using BimaPimaUssd.Models;
using System.Collections.Generic;

namespace BimaPimaUssd.Contracts
{
    public interface IRepository
    {
        Dictionary<string, User> Users { get; }
        Dictionary<string, LinkedList<string>> Requests { get; }
        public Dictionary<string, Stack<int>> levels { get; }
        Dictionary<string, dynamic> Data { get; }
        //List<InsuranceProduct> ValueChains { get; }
        //List<Categories> ResourceCategories { get; }
        //List<Resource> ResourcePrice { get; }
    }
}
