

namespace Mediatek86.metier
{
    /// <summary>
    /// classe abstraite Categorie, classe mère de Genre, Public et Rayon
    /// </summary>
    public abstract class Categorie
    {
        private readonly string id;
        private readonly string libelle;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        protected Categorie(string id, string libelle)
        {
            this.id = id;
            this.libelle = libelle;
        }

        /// <summary>
        /// id de catégorie
        /// </summary>
        public string Id { get => id; }

        /// <summary>
        /// libellé de catégorie
        /// </summary>
        public string Libelle { get => libelle; }

        /// <summary>
        /// Récupération du libellé pour l'affichage dans les combos
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.libelle;
        }

    }
}
