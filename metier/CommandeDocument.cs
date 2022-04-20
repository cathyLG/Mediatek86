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
        private int idSuivi;
        private string etapeSuivi;
        public CommandeDocument(int id, DateTime dateCommande, double montant, int nbExemplaires, string idLivreDvd, int idSuivi, string etapeSuivi)
            : base(id, dateCommande, montant)
        {
            this.nbExemplaires = nbExemplaires;
            this.idLivreDvd = idLivreDvd;
            this.idSuivi = idSuivi;
            this.etapeSuivi = etapeSuivi;
        }

        public int NbExemplaires { get => nbExemplaires; }
        public string IdLivreDvd { get => idLivreDvd; }
        public int IdSuivi { get => idSuivi; set => idSuivi = value; }
        public string EtapeSuivi { get => etapeSuivi; set => etapeSuivi = value; }
    }
}
