using Mediatek86.metier;
using System.Collections.Generic;
using Mediatek86.bdd;
using System;
using Serilog;

namespace Mediatek86.modele
{
    public static class Dao
    {

        private static readonly string server = "localhost";
        private static readonly string userid = "root";
        private static readonly string password = "";
        private static readonly string database = "mediatek86";
        private static readonly string connectionString = "server=" + server + ";user id=" + userid + ";password=" + password + ";database=" + database + ";SslMode=none";

        /// <summary>
        /// contrôler l'authentification des utilisateurs
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="mdp"></param>
        /// <returns>0 si authentification échouée, idService de l'utilisateur si authentification réussie</returns>
        public static int ControleAuthentification(string nom, string prenom, string mdp)
        {
            int idService = 0;
            string req = "select * from utilisateur ";
            req += "where nom=@nom and prenom=@prenom and mdp=SHA2(@mdp, 256) ;";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@nom", nom },
                { "@prenom", prenom },
                { "@mdp", mdp }
            };
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);
            if (curs.Read())
            {
                idService = (int)curs.Field("idService");
            }
            curs.Close();

            return idService;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public static List<Categorie> GetAllGenres()
        {
            List<Categorie> lesGenres = new List<Categorie>();
            string req = "Select * from genre order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Genre genre = new Genre((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesGenres.Add(genre);
            }
            curs.Close();
            return lesGenres;
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Rayon</returns>
        public static List<Categorie> GetAllRayons()
        {
            List<Categorie> lesRayons = new List<Categorie>();
            string req = "Select * from rayon order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Rayon rayon = new Rayon((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesRayons.Add(rayon);
            }
            curs.Close();
            return lesRayons;
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Collection d'objets Public</returns>
        public static List<Categorie> GetAllPublics()
        {
            List<Categorie> lesPublics = new List<Categorie>();
            string req = "Select * from public order by libelle";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Public lePublic = new Public((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesPublics.Add(lePublic);
            }
            curs.Close();
            return lesPublics;
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public static List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = new List<Livre>();
            string req = "Select l.id, l.ISBN, l.auteur, d.titre, d.image, l.collection, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from livre l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                string isbn = (string)curs.Field("ISBN");
                string auteur = (string)curs.Field("auteur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string collection = (string)curs.Field("collection");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Livre livre = new Livre(id, titre, image, isbn, auteur, collection, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon);
                lesLivres.Add(livre);
            }
            curs.Close();

            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public static List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = new List<Dvd>();
            string req = "Select l.id, l.duree, l.realisateur, d.titre, d.image, l.synopsis, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from dvd l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                int duree = (int)curs.Field("duree");
                string realisateur = (string)curs.Field("realisateur");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                string synopsis = (string)curs.Field("synopsis");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Dvd dvd = new Dvd(id, titre, image, duree, realisateur, synopsis, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon);
                lesDvd.Add(dvd);
            }
            curs.Close();

            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public static List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = new List<Revue>();
            string req = "Select l.id, l.empruntable, l.periodicite, d.titre, d.image, l.delaiMiseADispo, ";
            req += "d.idrayon, d.idpublic, d.idgenre, g.libelle as genre, p.libelle as public, r.libelle as rayon ";
            req += "from revue l join document d on l.id=d.id ";
            req += "join genre g on g.id=d.idGenre ";
            req += "join public p on p.id=d.idPublic ";
            req += "join rayon r on r.id=d.idRayon ";
            req += "order by titre ";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                string id = (string)curs.Field("id");
                bool empruntable = (bool)curs.Field("empruntable");
                string periodicite = (string)curs.Field("periodicite");
                string titre = (string)curs.Field("titre");
                string image = (string)curs.Field("image");
                int delaiMiseADispo = (int)curs.Field("delaimiseadispo");
                string idgenre = (string)curs.Field("idgenre");
                string idrayon = (string)curs.Field("idrayon");
                string idpublic = (string)curs.Field("idpublic");
                string genre = (string)curs.Field("genre");
                string lepublic = (string)curs.Field("public");
                string rayon = (string)curs.Field("rayon");
                Revue revue = new Revue(id, titre, image, idgenre, genre,
                    idpublic, lepublic, idrayon, rayon, empruntable, periodicite, delaiMiseADispo);
                lesRevues.Add(revue);
            }
            curs.Close();

            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'un document
        /// </summary>
        /// <returns>Liste d'objets Exemplaire</returns>
        public static List<Exemplaire> GetExemplairesDocument(string idDocument)
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            string req = "Select ex.id, ex.numero, ex.dateAchat, ex.photo, ex.idEtat, et.libelle ";
            req += "from exemplaire ex join etat et on ex.idEtat = et.id ";
            req += "where ex.id=@id ";
            req += "order by ex.dateAchat DESC";
            Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, parameters);

            while (curs.Read())
            {
                string idDocuement = (string)curs.Field("id");
                int numero = (int)curs.Field("numero");
                DateTime dateAchat = (DateTime)curs.Field("dateAchat");
                string photo = (string)curs.Field("photo");
                string idEtat = (string)curs.Field("idEtat");
                string etat = (string)curs.Field("libelle");
                Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, etat, idDocuement);
                lesExemplaires.Add(exemplaire);
            }
            curs.Close();

            return lesExemplaires;
        }


        /// <summary>
        /// récupérer toutes les commandes liées à un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="typeDocument"></param>
        /// <returns>liste d'objets Abonnement pour une revue, liste d'objet CommandeDocument pour un livre/dvd</returns>
        public static List<Commande> GetCommandes(string idDocument, string typeDocument)
        {
            List<Commande> lesCommandes = new List<Commande>();
            // revue => liste Abonnement
            if (typeDocument.Equals("revue"))
            {
                string req = "Select c.id, c.dateCommande, c.montant, a.dateFinAbonnement, a.idRevue ";
                req += "from commande c join abonnement a using(id)";
                req += "where a.idRevue = @id ";
                req += "order by c.dateCommande DESC, c.id DESC";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, parameters);

                while (curs.Read())
                {
                    int id = (int)curs.Field("id");
                    DateTime dateCommande = (DateTime)curs.Field("dateCommande");
                    double montant = (double)curs.Field("montant");
                    DateTime dateFinAbonnement = (DateTime)curs.Field("dateFinAbonnement");
                    string idRevue = (string)curs.Field("idRevue");
                    Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
                    lesCommandes.Add(abonnement);
                }
                curs.Close();
            }
            // livres ou dvd => liste CommandeDocument
            else
            {
                string req = "Select c.id, c.dateCommande, c.montant, cd.nbExemplaire, cd.idLivreDvd, cd.idSuivi, s.nom ";
                req += "from commande c join commandeDocument cd using(id) join suivi s using(idSuivi)";
                req += "where cd.idLivreDvd = @id ";
                req += "order by c.dateCommande DESC, c.id DESC";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, parameters);

                while (curs.Read())
                {
                    int id = (int)curs.Field("id");
                    DateTime dateCommande = (DateTime)curs.Field("dateCommande");
                    double montant = (double)curs.Field("montant");
                    int nbExemplaire = (int)curs.Field("nbExemplaire");
                    string idLivreDvd = (string)curs.Field("idLivreDvd");
                    int idSuivi = (int)curs.Field("idSuivi");
                    string etapeSuivi = (string)curs.Field("nom");
                    CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, etapeSuivi);
                    lesCommandes.Add(commandeDocument);
                }
                curs.Close();
            }
            return lesCommandes;
        }

        /// <summary>
        /// récupérer toutes les étapes de suivi à partir de la bdd
        /// </summary>
        /// <returns>liste d'objets Suivi</returns>
        public static List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = new List<Suivi>();
            string req = "Select * from suivi ;";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Suivi leSuivi = new Suivi((int)curs.Field("idSuivi"), (string)curs.Field("nom"));
                lesSuivis.Add(leSuivi);
            }
            curs.Close();
            return lesSuivis;
        }


        /// <summary>
        /// récupérer tous les états à partir de la bdd
        /// </summary>
        /// <returns>liste d'objets Etat</returns>
        public static List<Etat> GetAllEtats()
        {
            List<Etat> lesEtats = new List<Etat>();
            string req = "Select * from etat ;";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Etat etat = new Etat((string)curs.Field("id"), (string)curs.Field("libelle"));
                lesEtats.Add(etat);
            }
            curs.Close();
            return lesEtats;
        }

        /// <summary>
        /// insérer un exemplaire dans la bdd
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool CreerExemplaire(Exemplaire exemplaire)
        {
            try
            {
                string req = "insert into exemplaire values (@idDocument,@numero,@dateAchat,@photo,@idEtat)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@idDocument", exemplaire.IdDocument},
                    { "@numero", exemplaire.Numero},
                    { "@dateAchat", exemplaire.DateAchat},
                    { "@photo", exemplaire.Photo},
                    { "@idEtat",exemplaire.IdEtat}
                };
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// insérer un document dans la bdd
        /// </summary>
        /// <param name="document"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool CreerDocument(Document document)
        {
            // req 1 : insert into table document
            string req1 = "insert into document values (@idDocument,@titre,@image,@idRayon,@idPublic, @idGenre)";
            Dictionary<string, object> parameters1 = new Dictionary<string, object>
                {
                    { "@idDocument", document.Id},
                    { "@titre", document.Titre},
                    { "@image", document.Image},
                    { "@idRayon", document.IdRayon},
                    { "@idPublic",document.IdPublic},
                    {"@idGenre", document.IdGenre }
            };
            // req 2 : insert into table livres_dvd si c'est un livre ou dvd
            string req2 = null;
            Dictionary<string, object> parameters2 = null;
            // req 3 : insert into table livre, revue ou dvd selon le type
            string req3 = null;
            Dictionary<string, object> parameters3 = null;

            if (document is Livre livre)
            {
                req2 = "insert into livres_dvd values (@id)";
                parameters2 = new Dictionary<string, object>
                        {
                            {"@id", livre.Id }
                        };

                req3 = "insert into livre values (@idLivre,@isbn,@auteur,@collection)";
                parameters3 = new Dictionary<string, object>
                            {
                                { "@idLivre", livre.Id},
                                { "@isbn", livre.Isbn},
                                { "@auteur", livre.Auteur},
                                { "@collection", livre.Collection}
                            };
            }
            else if (document is Dvd dvd)
            {
                req2 = "insert into livres_dvd values (@id)";
                parameters2 = new Dictionary<string, object>
                        {
                            {"@id", dvd.Id }
                        };

                req3 = "insert into dvd values (@idDvd,@synopsis,@realisateur,@duree)";
                parameters3 = new Dictionary<string, object>
                            {
                                { "@idDvd", dvd.Id},
                                { "@synopsis", dvd.Synopsis},
                                { "@realisateur", dvd.Realisateur},
                                { "@duree", dvd.Duree}
                            };
            }
            else if (document is Revue revue)
            {
                req3 = "insert into revue values (@idRevue, @empruntable,@periodicite,@delaiMiseADispo)";
                parameters3 = new Dictionary<string, object>
                            {
                                { "@idRevue", revue.Id},
                                { "@empruntable", revue.Empruntable},
                                { "@periodicite", revue.Periodicite},
                                { "@delaiMiseADispo", revue.DelaiMiseADispo}
                            };
            }

            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);

                curs.ReqUpdate(req1, parameters1);
                if (document is Livre || document is Dvd)
                {
                    curs.ReqUpdate(req2, parameters2);
                }
                curs.ReqUpdate(req3, parameters3);

                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// modifier un document dans la bdd
        /// </summary>
        /// <param name="document"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool ModifDocument(Document document)
        {
            // req 1 : update dans la table document
            string req1 = "UPDATE document SET titre=@titre, image=@image, idrayon=@idRayon, idpublic=@idPublic, idgenre=@idGenre ";
            req1 += "WHERE id = @idDocument";
            Dictionary<string, object> parameters1 = new Dictionary<string, object>
                {
                    { "@idDocument", document.Id},
                    { "@titre", document.Titre},
                    { "@image", document.Image},
                    { "@idRayon", document.IdRayon},
                    { "@idPublic",document.IdPublic},
                    { "@idGenre", document.IdGenre }
            };
            // inutile d'update dans la table livres_dvd qui ne contient que l'id
            // req 2 : update dans la table livre, revue ou dvd selon le type
            string req2 = null;
            Dictionary<string, object> parameters2 = null;

            if (document is Livre livre)
            {
                req2 = "UPDATE livre SET isbn=@isbn,auteur=@auteur, collection=@collection WHERE id=@idLivre";
                parameters2 = new Dictionary<string, object>
                            {
                                { "@idLivre", livre.Id},
                                { "@isbn", livre.Isbn},
                                { "@auteur", livre.Auteur},
                                { "@collection", livre.Collection}
                            };
            }
            else if (document is Dvd dvd)
            {
                req2 = "UPDATE dvd SET synopsis=@synopsis,realisateur=@realisateur,duree=@duree WHERE id=@idDvd";
                parameters2 = new Dictionary<string, object>
                            {
                                { "@idDvd", dvd.Id},
                                { "@synopsis", dvd.Synopsis},
                                { "@realisateur", dvd.Realisateur},
                                { "@duree", dvd.Duree}
                            };
            }
            else if (document is Revue revue)
            {
                req2 = "UPDATE revue SET empruntable=@empruntable, periodicite=@periodicite, delaiMiseAdispo=@delaiMiseADispo ";
                req2 += "WHERE id=@idRevue";
                parameters2 = new Dictionary<string, object>
                            {
                                { "@idRevue", revue.Id},
                                { "@empruntable", revue.Empruntable},
                                { "@periodicite", revue.Periodicite},
                                { "@delaiMiseADispo", revue.DelaiMiseADispo}
                            };
            }

            //exécuter ces deux requêtes
            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);

                curs.ReqUpdate(req1, parameters1);
                curs.ReqUpdate(req2, parameters2);

                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// supprimer un document dans la bdd
        /// </summary>
        /// <param name="document"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool SupprDocument(Document document)
        {
            string req = "DELETE FROM ";
            switch (document)
            {
                case Livre _:
                    {
                        req += "livre";
                        break;
                    }
                case Dvd _:
                    {
                        req += "dvd";
                        break;
                    }
                case Revue _:
                    {
                        req += "revue";
                        break;
                    }
            }
            req += " WHERE id=@id";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@id", document.Id}
            };
            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// modifier l'étape de suivi d'une commande de livre/dvd dans la bdd
        /// </summary>
        /// <param name="idLivreDvd"></param>
        /// <param name="idSuivi"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool UpdateSuiviCommandeDocument(int idLivreDvd, int idSuivi)
        {
            string req = "UPDATE commandeDocument SET idSuivi=@idSuivi WHERE id=@idLivreDvd";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@idSuivi", idSuivi},
                { "@idLivreDvd", idLivreDvd }
            };
            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// supprimer une commandedocument dans la bdd
        /// </summary>
        /// <param name="idCommandeDocument"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool SupprCommandeDocument(int idCommandeDocument)
        {
            string req = "DELETE FROM commandeDocument WHERE id=@id";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@id", idCommandeDocument }
            };

            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// supprimer un abonnement de revue dans la bdd
        /// </summary>
        /// <param name="idAbonnement"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool SupprAbonnementRevue(int idAbonnement)
        {
            string req = "DELETE FROM abonnement WHERE id=@id";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@id", idAbonnement }
            };

            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// créer une commandeDocument dans la bdd
        /// </summary>
        /// <param name="commandeDocument"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool CreerCommandeDocument(CommandeDocument commandeDocument)
        {
            string req1 = "INSERT INTO commande VALUES(@id, @dateCommande, @montant)";
            Dictionary<string, object> parameters1 = new Dictionary<string, object>
            {
                { "@id", commandeDocument.Id },
                { "@dateCommande", commandeDocument.DateCommande },
                { "@montant", commandeDocument.Montant }
            };
            string req2 = "INSERT INTO commandedocument VALUES(@id, @nbExemplaires, @idLivreDvd, @idSuivi)";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>
            {
                { "@id", commandeDocument.Id },
                { "@nbExemplaires", commandeDocument.NbExemplaires },
                { "@idLivreDvd", commandeDocument.IdLivreDvd },
                { "@idSuivi", commandeDocument.IdSuivi }
            };

            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req1, parameters1);
                curs.ReqUpdate(req2, parameters2);
                curs.Close();
                return true;
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// insérer un abonnement dans la bdd
        /// </summary>
        /// <param name="abonnement"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool CreerAbonnement(Abonnement abonnement)
        {
            string req1 = "INSERT INTO commande VALUES(@id, @dateCommande, @montant)";
            Dictionary<string, object> parameters1 = new Dictionary<string, object>
            {
                { "@id", abonnement.Id },
                { "@dateCommande", abonnement.DateCommande },
                { "@montant", abonnement.Montant }
            };
            string req2 = "INSERT INTO abonnement VALUES(@id, @dateFinAbonnement, @idRevue)";
            Dictionary<string, object> parameters2 = new Dictionary<string, object>
            {
                { "@id", abonnement.Id },
                { "@dateFinAbonnement", abonnement.DateFinAbonnement },
                { "@idRevue", abonnement.IdRevue }
            };

            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req1, parameters1);
                curs.ReqUpdate(req2, parameters2);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// récupérer le dernier id des commandes depuis la bdd
        /// </summary>
        /// <returns>max(id) dans la table commande</returns>
        public static int GetLastIdCommande()
        {
            string req = "SELECT MAX(id) FROM commande;";
            int lastIdCommande = 0;
            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, null);
                if (curs.Read())
                {
                    lastIdCommande = (int)curs.Field("MAX(id)");
                }
                curs.Close();
                return lastIdCommande;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// mettre à jour l'état d'un exemplaire d'un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="numero"></param>
        /// <param name="idEtat"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool UpdateEtatExemplaire(string idDocument, int numero, string idEtat)
        {
            string req = "UPDATE exemplaire SET idEtat=@idEtat WHERE id=@idDocument AND numero=@numero";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@idDocument", idDocument },
                { "@numero", numero },
                { "@idEtat", idEtat }
            };
            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// supprimer un exemplaire dans la bdd
        /// </summary>
        /// <param name="idDocument"></param>
        /// <param name="numero"></param>
        /// <returns>true si réussi, false si échoué</returns>
        public static bool SupprExemplaire(string idDocument, int numero)
        {
            string req = "DELETE FROM exemplaire WHERE id=@idDocument AND numero=@numero";
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@idDocument", idDocument },
                { "@numero", numero }
            };
            try
            {
                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqUpdate(req, parameters);
                curs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// retourner la liste des abonnements de revue qui vont expirer dans moins de 30 jours depuis la bdd
        /// </summary>
        /// <returns>liste d'objets Abonnement</returns>
        public static List<Abonnement> GetAbonnementsAExpirer()
        {
            List<Abonnement> lesAbonnements = new List<Abonnement>();
            string req = "CALL abonnementsAExpirer(); ";
            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);
            while (curs.Read())
            {
                int id = (int)curs.Field("id");
                DateTime dateCommande = (DateTime)curs.Field("dateCommande");
                double montant = (double)curs.Field("montant");
                DateTime dateFinAbonnement = (DateTime)curs.Field("dateFinAbonnement");
                string idRevue = (string)curs.Field("idRevue");
                Abonnement abonnement = new Abonnement(id, dateCommande, montant, dateFinAbonnement, idRevue);
                lesAbonnements.Add(abonnement);
            }
            curs.Close();
            return lesAbonnements;
        }

    }
}
