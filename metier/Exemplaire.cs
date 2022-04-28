using System;

namespace Mediatek86.metier
{
    /// <summary>
    /// classe Exemplaire
    /// </summary>
    public class Exemplaire
    {
        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="dateAchat"></param>
        /// <param name="photo"></param>
        /// <param name="idEtat"></param>
        /// <param name="etat"></param>
        /// <param name="idDocument"></param>
        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string etat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Etat = etat;
            this.IdDocument = idDocument;
        }

        /// <summary>
        /// numéro séquentiel d'exemplaire
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// photo d'exemplaire
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// date d'achat d'exemplaire
        /// </summary>
        public DateTime DateAchat { get; set; }

        /// <summary>
        /// id d'état d'exemplaire
        /// </summary>
        public string IdEtat { get; set; }

        /// <summary>
        /// libellé d'état d'exemplaire
        /// </summary>
        public string Etat { get; set; }

        /// <summary>
        /// id de document d'exemplaire
        /// </summary>
        public string IdDocument { get; set; }
    }
}
