using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediatek86.metier;
using System;


namespace Mediatek86.metier.Tests
{
    [TestClass()]
    public class AbonnementTests
    {
        readonly Abonnement abonnement = new Abonnement(0, new DateTime(2022, 4, 1), 0, new DateTime(2022, 5, 1), null);

        [TestMethod()]
        public void ParutionDansAbonnementTest()
        {
            Assert.AreEqual(true, abonnement.ParutionDansAbonnement(new DateTime(2022, 4, 1)), "devrait être vrai : date de commande");
            Assert.AreEqual(true,abonnement.ParutionDansAbonnement(new DateTime(2022, 5, 1)), "devrait être vrai : date de fin d'abonnement") ;
            Assert.AreEqual(true,abonnement.ParutionDansAbonnement(new DateTime(2022, 4, 15)), "devrait être vrai : date comprise entre ces deux dates");
            Assert.AreEqual(false, abonnement.ParutionDansAbonnement(new DateTime(2022, 3, 7)), "devrait être faux : date avant la date de commande");
            Assert.AreEqual(false, abonnement.ParutionDansAbonnement(new DateTime(2023, 1, 1)), "devrait être faux : date après la date de commande");
        }
    }
}