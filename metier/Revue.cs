
namespace Mediatek86.metier
{
    /// <summary>
    /// classe Revue, fille de Document
    /// </summary>
    public class Revue : Document
    {
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
        /// <param name="empruntable"></param>
        /// <param name="periodicite"></param>
        /// <param name="delaiMiseADispo"></param>
        public Revue(string id, string titre, string image, string idGenre, string genre,
            string idPublic, string lePublic, string idRayon, string rayon, 
            bool empruntable, string periodicite, int delaiMiseADispo)
             : base(id, titre, image, idGenre, genre, idPublic, lePublic, idRayon, rayon)
        {
            Periodicite = periodicite;
            Empruntable = empruntable;
            DelaiMiseADispo = delaiMiseADispo;
        }

        /// <summary>
        /// periodicite de revue
        /// </summary>
        public string Periodicite { get; set; }

        /// <summary>
        /// si une revue est empruntable
        /// </summary>
        public bool Empruntable { get; set; }

        /// <summary>
        /// délai de mise à disposition
        /// </summary>
        public int DelaiMiseADispo { get; set; }
    }
}
