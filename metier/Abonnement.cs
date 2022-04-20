﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.metier
{
    public class Abonnement : Commande
    {
        private readonly DateTime dateFinAbonnement;
        private readonly string idRevue;

        public Abonnement(int id, DateTime dateCommande, double montant, DateTime dateFinAbonnement, string idRevue) 
            : base(id, dateCommande, montant)
        {
            this.dateFinAbonnement = dateFinAbonnement;
            this.idRevue = idRevue;
        }

        public DateTime DateFinAbonnement => dateFinAbonnement;
        public string IdRevue => idRevue;
    }
}
