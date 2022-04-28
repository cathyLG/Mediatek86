using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// classe Suivi, pour suivre une commandeDocument
    /// </summary>
    public class Suivi
    {
        private readonly int id;
        private readonly string nom;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nom"></param>
        public Suivi(int id, string nom)
        {
            this.id = id;
            this.nom = nom;
        }

        /// <summary>
        /// id de suivi
        /// </summary>
        public int Id { get => id; }

        /// <summary>
        /// libellé de suivi
        /// </summary>
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
