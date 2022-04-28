

namespace Mediatek86.metier
{
    /// <summary>
    /// classe Etat
    /// </summary>
    public class Etat
    {
        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Etat(string id, string libelle)
        {
            this.Id = id;
            this.Libelle = libelle;
        }

        /// <summary>
        /// id d'état
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// libellé d'état
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Libelle;
        }
    }
}
