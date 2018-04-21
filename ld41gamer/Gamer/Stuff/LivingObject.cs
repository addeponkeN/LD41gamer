using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ld41gamer.Gamer
{
    public class LivingObject : GameObject
    {
        public string Name { get; set; }
        public int HealthPoints { get; set; }
        public int Damage { get; set; }

        public bool IsAlive { get; set; }
    }
}
