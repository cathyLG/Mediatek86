using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    /// <summary>
    /// classe CommandeDocument, fille de Commande
    /// </summary>
    public class CommandeDocument : Commande
    {

        private readonly int nbExemplaires;
        private readonly string idLivreDvd;
       
        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dateCommande"></param>
        /// <param name="montant"></param>
        /// <param name="nbExemplaires"></param>
        /// <param name="idLivreDvd"></param>
        /// <param name="idSuivi"></param>
        /// <param name="etapeSuivi"></param>
        public CommandeDocument(int id, DateTime dateCommande, double montant, int nbExemplaires, string idLivreDvd, int idSuivi, string etapeSuivi)
            : base(id, dateCommande, montant)
        {
            this.nbExemplaires = nbExemplaires;
            this.idLivreDvd = idLivreDvd;
            this.IdSuivi = idSuivi;
            this.EtapeSuivi = etapeSuivi;
        }

        /// <summary>
        /// nombre d'exemplaire d'une commandeDocument
        /// </summary>
        public int NbExemplaires { get => nbExemplaires; }

        /// <summary>
        /// id de livre/dvd concerné
        /// </summary>
        public string IdLivreDvd { get => idLivreDvd; }

        /// <summary>
        /// id de suivi
        /// </summary>
        public int IdSuivi { get; set; }

        /// <summary>
        /// libellé de suivi
        /// </summary>
        public string EtapeSuivi { get; set; }
    }
}
