
namespace Mediatek86.metier
{
    /// <summary>
    /// Classe Document, mère de LivreDvd et Revue
    /// </summary>
    public class Document
    {

        private readonly string id;
        private readonly string titre;
        private readonly string image;
        private readonly string idGenre;
        private readonly string genre;
        private readonly string idPublic;
        private readonly string lePublic;
        private readonly string idRayon;
        private readonly string rayon;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="id"></param>
        /// <param name="titre"></param>
        /// <param name="image"></param>
        /// <param name="idGenre"></param>
        /// <param name="genre"></param>
        /// <param name="idPublic"></param>
        /// <param name="lePublic"></param>
        /// <param name="idRayon"></param>
        /// <param name="rayon"></param>
        public Document(string id, string titre, string image, string idGenre, string genre, 
            string idPublic, string lePublic, string idRayon, string rayon)
        {
            this.id = id;
            this.titre = titre;
            this.image = image;
            this.idGenre = idGenre;
            this.genre = genre;
            this.idPublic = idPublic;
            this.lePublic = lePublic;
            this.idRayon = idRayon;
            this.rayon = rayon;
        }

        /// <summary>
        /// id de document
        /// </summary>
        public string Id { get => id; }

        /// <summary>
        /// titre de document
        /// </summary>
        public string Titre { get => titre; }

        /// <summary>
        /// image de document
        /// </summary>
        public string Image { get => image; }

        /// <summary>
        /// id de genre de document
        /// </summary>
        public string IdGenre { get => idGenre; }

        /// <summary>
        /// libellé de genre de document
        /// </summary>
        public string Genre { get => genre; }

        /// <summary>
        /// id de public de document
        /// </summary>
        public string IdPublic { get => idPublic; }

        /// <summary>
        /// libellé de public de document
        /// </summary>
        public string Public { get => lePublic; }

        /// <summary>
        /// id de rayon de document
        /// </summary>
        public string IdRayon { get => idRayon; }

        /// <summary>
        /// libellé de rayon de document
        /// </summary>
        public string Rayon { get => rayon; }

        /// <summary>
        /// Récupération de l'id de document pour l'affichage dans les combos
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Id;
        }

    }


}
