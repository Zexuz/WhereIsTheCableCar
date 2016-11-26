using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhereIsTheCableCar;

namespace Runner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var x = ApiHelper.GetLivemap();
            var res = x.Result;
            var cableCars = res.Vehicles.Where(v => v.GetType() == Vehicles.Tram).ToList();

            foreach (var cc in cableCars)
            {
                Console.WriteLine($"Nr {cc.Name}, dir {cc.Direction}");
            }
        }
    }
}