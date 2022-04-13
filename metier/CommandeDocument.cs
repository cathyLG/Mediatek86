using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class CommandeDocument : Commande
    {
        private readonly int nbExemplaires;
        private readonly string idLivreDvd;
        private readonly string idSuivi;
        private readonly string etapeSuivi;
        public CommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaires, string idLivreDvd, string idSuivi, string etapeSuivi)
            : base(id, dateCommande, montant)
        {
            this.nbExemplaires = nbExemplaires;
            this.idLivreDvd = idLivreDvd;
            this.idSuivi = idSuivi;
            this.etapeSuivi = etapeSuivi;
        }

        public int NbExemplaires => nbExemplaires;
        public string IdLivreDvd => idLivreDvd;
        public string IdSuivi => idSuivi;
        public string EtapeSuivi => etapeSuivi;
    }
}
