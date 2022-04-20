﻿using System.Collections.Generic;
using Mediatek86.modele;
using Mediatek86.metier;
using Mediatek86.vue;


namespace Mediatek86.controleur
{
    internal class Controle
    {
        private readonly List<Livre> lesLivres;
        private readonly List<Dvd> lesDvd;
        private readonly List<Revue> lesRevues;
        private readonly List<Categorie> lesRayons;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesGenres;

        /// <summary>
        /// Ouverture de la fenêtre
        /// </summary>
        public Controle()
        {
            lesLivres = Dao.GetAllLivres();
            lesDvd = Dao.GetAllDvd();
            lesRevues = Dao.GetAllRevues();
            lesGenres = Dao.GetAllGenres();
            lesRayons = Dao.GetAllRayons();
            lesPublics = Dao.GetAllPublics();
            FrmMediatek frmMediatek = new FrmMediatek(this);
            frmMediatek.ShowDialog();
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
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Collection d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return lesLivres;
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Collection d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return lesDvd;
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Collection d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return lesRevues;
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
        /// ajouter une commandeDocument dans la bdd
        /// </summary>
        /// <param name="commandeDocument"></param>
        /// <returns></returns>
        public bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            return Dao.CreerCommandeDocument(commandeDocument);
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

    }
}

