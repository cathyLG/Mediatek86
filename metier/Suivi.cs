using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class Suivi
    {
        private readonly int id;
        private readonly string nom;

        public Suivi(int id, string nom)
        {
            this.id = id;
            this.nom = nom;
        }

        public int Id { get => id; }
        public string Nom { get => nom; }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Nom;
        }
    }
}
