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
       
        public CommandeDocument(int id, DateTime dateCommande, double montant, int nbExemplaires, string idLivreDvd, int idSuivi, string etapeSuivi)
            : base(id, dateCommande, montant)
        {
            this.nbExemplaires = nbExemplaires;
            this.idLivreDvd = idLivreDvd;
            this.IdSuivi = idSuivi;
            this.EtapeSuivi = etapeSuivi;
        }

        public int NbExemplaires { get => nbExemplaires; }
        public string IdLivreDvd { get => idLivreDvd; }
        public int IdSuivi { get; set; }
        public string EtapeSuivi { get; set; }
    }
}
