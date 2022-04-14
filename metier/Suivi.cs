using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class Suivi
    {
        private readonly string id;
        private readonly string nom;

        public Suivi(string id, string nom)
        {
            this.id = id;
            this.nom = nom;
        }

        public string Id { get => id; }
        public string Nom { get => nom; }
    }
}
