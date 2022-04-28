using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// classe Abonnement, fille de Commande
    /// </summary>
    public class Abonnement : Commande
    {
        private readonly DateTime dateFinAbonnement;
        private readonly string idRevue;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="dateFinAbonnement"></param>
        /// <param name="idRevue"></param>
        public Abonnement(int id, DateTime dateCommande, double montant, DateTime dateFinAbonnement, string idRevue) 
            : base(id, dateCommande, montant)
        {
            this.dateFinAbonnement = dateFinAbonnement;
            this.idRevue = idRevue;
        }

        /// <summary>
        /// date de fin d'abonnement
        /// </summary>
        public DateTime DateFinAbonnement => dateFinAbonnement;

        /// <summary>
        /// id de revue
        /// </summary>
        public string IdRevue => idRevue;

        /// <summary>
        /// contrôler si la date d'un exemplaire de revue est compris entre la date de la commande et la date de fin d'abonnement
        /// </summary>
        /// <param name="dateParution"></param>
        /// <returns>true ou false</returns>
        public bool ParutionDansAbonnement(DateTime dateParution)
        {
            return dateParution >= this.DateCommande && dateParution <= this.DateFinAbonnement;
        }
    }
}
