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
        public CommandeDocument(string id, DateTime dateCommande, double montant, int nbExemplaires, string idLivreDvd) 
            : base(id, dateCommande, montant)
        {
            this.nbExemplaires = nbExemplaires;
            this.idLivreDvd = idLivreDvd;
        }

        public int NbExemplaires => nbExemplaires;
        public string IdLivreDvd => idLivreDvd;
    }
}
