using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _12B_keszletnyilvantarto {
    internal class Termek {
        public int Id { get; set; }
        public string Cikkszam { get; set; }
        public string Megnevezes { get; set; }

        public Termek(int id, string cikkszam, string megnevezes) {
            Id = id;
            Cikkszam = cikkszam;
            Megnevezes = megnevezes;
        }
    }
}
