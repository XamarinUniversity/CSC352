using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace CatsDinner.Models
{
    public class DiningRoom
    {
        static readonly string[] Names = {
          "Garfield", "Sylvester", "Tom", "Muffin", "Bob"  
        };

        public List<Cat> Cats { get; private set; }
        public event Action<Cat,Cat,Sensor> CatFightDetected = delegate {};

        public DiningRoom()
        {
            int count = Names.Length;

            Sensor[] sensors = new Sensor[count];
            for (int n = 0; n < count; n++)
                sensors[n] = new Sensor(n, this);

            Cats = Enumerable.Range(0, count)
                .Select(n => new Cat(Names[n], sensors[n], sensors[(n + 1) % count]))
                .ToList();
        }

        public void StartDinnerParty()
        {
            foreach (var c in Cats)
            {
                Task.Run(() => c.Run());
            }
        }

        public void RaiseCatfight(Cat currentOwner, Cat otherCat, Sensor sensor)
        {
            CatFightDetected(currentOwner, otherCat, sensor);
        }
    }
}
