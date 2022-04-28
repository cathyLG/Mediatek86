

namespace Mediatek86.metier
{
    /// <summary>
    /// classe Genre, fille de Categorie
    /// </summary>
    public class Genre : Categorie
    {
        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="libelle"></param>
        public Genre(string id, string libelle) : base(id, libelle)
        {
        }

    }
}
