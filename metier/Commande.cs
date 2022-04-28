using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// classe Commande, classe mère de CommandeDocument et Abonnement
    /// </summary>
    public class Commande
    {
        private readonly int id;
        private readonly DateTime dateCommande;
        private readonly double montant;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        public Commande(int id, DateTime dateCommande, double montant)
        {
            this.id = id;
            this.dateCommande = dateCommande;
            this.montant = montant;
        }

        /// <summary>
        /// id de commande
        /// </summary>
        public int Id => id;

        /// <summary>
        /// date de commande
        /// </summary>
        public DateTime DateCommande => dateCommande;

        /// <summary>
        /// montant de commande
        /// </summary>
        public double Montant => montant;
    }
}
