using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mediatek86.modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mediatek86.modele.Tests
{
    [TestClass()]
    public class DaoTests
    {
        [TestMethod()]
        public void ControleAuthentificationTest()
        {
            Assert.AreEqual(0, Dao.ControleAuthentification("Lambert", "Porter", "bert"), "0:mot de passe incorrect");
            Assert.AreEqual(1, Dao.ControleAuthentification("Lambert", "Porter", "Lambert"), " l'id de service de Porter Lambert est 1");
            Assert.AreEqual(2, Dao.ControleAuthentification("Bouvier", "Guerin", "Bouvier"), " l'id de service de Bouvier Guerin est 2");
            Assert.AreEqual(3, Dao.ControleAuthentification("Bizier", "Laurent", "Bizier"), " l'id de service de Laurent Bizier est 3");
            Assert.AreEqual(4, Dao.ControleAuthentification("Liu", "X", "Liu"), " l'id de service de Liu X est 4");
        }
    }
}