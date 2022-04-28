

namespace Mediatek86.metier
{
    /// <summary>
    /// classe Livre, fille de LivresDvd
    /// </summary>
    public class Livre : LivreDvd
    {

        private readonly string isbn;
        private readonly string auteur;
        private readonly string collection;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="isbn"></param>
        /// <param name="auteur"></param>
        /// <param name="collection"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Livre(string id, string titre, string image, string isbn, string auteur, string collection,
            string idGenre, string genre, string idPublic, string lePublic, string idRayon, string rayon)
            : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            this.isbn = isbn;
            this.auteur = auteur;
            this.collection = collection;
        }

        /// <summary>
        /// isbn
        /// </summary>
        public string Isbn { get => isbn; }

        /// <summary>
        /// auteur
        /// </summary>
        public string Auteur { get => auteur; }

        /// <summary>
        /// collection
        /// </summary>
        public string Collection { get => collection; }
    }
}
