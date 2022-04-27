using System.Collections.Generic;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.vue;
using System;


namespace Mediatek86.controleur
{
    public class Controle
    {
        private readonly FrmAuthentification frmAuthentification;
        private readonly List<Categorie> lesRayons;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesGenres;

        /// <summary>
        /// Ouverture de la fenêtre
        /// </summary>
        public Controle()
        {           
            lesGenres = Dao.GetAllGenres();
            lesRayons = Dao.GetAllRayons();
            lesPublics = Dao.GetAllPublics();
            frmAuthentification = new FrmAuthentification(this);
            frmAuthentification.ShowDialog();
        }

        /// <summary>
        /// Demande la vérification de l'authentification
        /// Si correct et habillié, alors ouvre la fenêtre principale
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="mdp"></param>
        /// <returns></returns>
        public int ControleAuthentification(string nom, string prenom, string mdp)
        {
            int idService = Dao.ControleAuthentification(nom, prenom, mdp);
            if (idService >1)
            {
                frmAuthentification.Hide();
                (new FrmMediatek(this, idService)).ShowDialog();
            }
            return idService;
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Collection d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return lesGenres;
        }

        /// <summary>
        /// récupérer la liste des livres depuis la bdd
        /// </summary>
        /// <returns>Collection d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
           return Dao.GetAllLivres();
        }

        /// <summary>
        /// récupérer la liste des Dvd depuis la bdd
        /// </summary>
        /// <returns>Collection d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return Dao.GetAllDvd();
        }

        /// <summary>
        /// récupérer la liste des revues depuis la bdd
        /// </summary>
        /// <returns>Collection d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return Dao.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return lesRayons;
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return lesPublics;
        }

        /// <summary>
        /// récupère les exemplaires d'un document
        /// </summary>
        /// <returns>Collection d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesDocument(string idDocuement)
        {
            return Dao.GetExemplairesDocument(idDocuement);
        }

        /// <summary>
        /// récupérer toutes les commandes liées à un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="typeDocument"></param>
        /// <returns></returns>
        public List<Commande> GetCommandes(string idDocument, string typeDocument)
        {
            return Dao.GetCommandes(idDocument, typeDocument);
        }

        /// <summary>
        /// récupérer tous les suivis
        /// </summary>
        /// <returns></returns>
        public List<Suivi> GetAllSuivis()
        {
            return Dao.GetAllSuivis();
        }

        /// <summary>
        /// récupérer tous les etats
        /// </summary>
        /// <returns></returns>
        public List<Etat> GetAllEtats()
        {
            return Dao.GetAllEtats();
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return Dao.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Créer un document dans la bdd
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool CreerDocument(Document document)
        {
            return Dao.CreerDocument(document);
        }

        /// <summary>
        /// modifier un document dans la bdd
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool ModifDocument(Document document)
        {
            return Dao.ModifDocument(document);
        }

        public bool SupprDocument(Document document)
        {
            return Dao.SupprDocument(document);
        }

        /// <summary>
        /// modifier l'étape de suivi d'une commande de livre/dvd
        /// </summary>
        /// <param name="idLivreDvd"></param>
        /// <param name="idSuivi"></param>
        /// <returns></returns>
        public bool UpdateSuiviCommandeDocument(int idLivreDvd, int idSuivi)
        {
            return Dao.UpdateSuiviCommandeDocument(idLivreDvd, idSuivi);
        }

        /// <summary>
        /// supprimer une commandeDocument pas encore livrée
        /// </summary>
        /// <param name="idCommandeDocument"></param>
        /// <returns></returns>
        public bool SupprCommandeDocument(int idCommandeDocument)
        {
            return Dao.SupprCommandeDocument(idCommandeDocument);
        }

        /// <summary>
        /// supprimer un abonnement de revue dans la bdd
        /// </summary>
        /// <param name="idAbonnement"></param>
        /// <returns></returns>
        public bool SuppreAbonnementRevue(int idAbonnement)
        {
            return Dao.SupprAbonnementRevue(idAbonnement);
        }

        /// <summary>
        /// ajouter une commandeDocument dans la bdd
        /// </summary>
        /// <param name="commandeDocument"></param>
        /// <returns></returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            return Dao.CreerCommandeDocument(commandeDocument);
        }

        /// <summary>
        /// ajouter un abonnement dans la bdd
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns></returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return Dao.CreerAbonnement(abonnement);
        }

        /// <summary>
        /// récupérer le dernier id des commandes depuis la bdd
        /// </summary>
        /// <returns></returns>
        public int GetLastIdCommande()
        {
            return Dao.GetLastIdCommande();
        }

        /// <summary>
        /// mettre à jour l'état d'un exemplaire d'un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="idEtat"></param>
        /// <returns></returns>
        public bool UpdateEtatExemplaire(string idDocument, int numero, string idEtat)
        {
            return Dao.UpdateEtatExemplaire(idDocument, numero, idEtat);
        }

        /// <summary>
        /// supprimer un exemplaire dans la bdd
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="numero"></param>
        /// <returns></returns>
        public bool SupprExemplaire(string idDocument, int numero)
        {
            return Dao.SupprExemplaire(idDocument, numero);
        }

        /// <summary>
        /// récupérer la liste des abonnements de revue qui vont expirer dans moins de 30 jours depuis la bdd
        /// </summary>
        /// <returns></returns>
        public List<Abonnement> GetAbonnementsAExpirer()
        {
            return Dao.GetAbonnementsAExpirer();
        }

    }
}

