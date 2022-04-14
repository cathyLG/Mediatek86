using Mediatek86.metier;
using System.Collections.Generic;
using Mediatek86.bdd;
using System;
using System.Windows.Forms;

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
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <returns>Liste d'objets Exemplaire</returns>
        public static List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            List<Exemplaire> lesExemplaires = new List<Exemplaire>();
            string req = "Select e.id, e.numero, e.dateAchat, e.photo, e.idEtat ";
            req += "from exemplaire e join document d on e.id=d.id ";
            req += "where e.id = @id ";
            req += "order by e.dateAchat DESC";
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
                Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocuement);
                lesExemplaires.Add(exemplaire);
            }
            curs.Close();

            return lesExemplaires;
        }

        /// <summary>
        /// récupérer toutes les commandes liées à un document
        /// </summary>
        /// <param name="idDocument"></param>
        /// <returns>liste d'abonnements pour revue, liste de commandeDocument pour livre/dvd</returns>
        public static List<Commande> GetCommandes(string idDocument, string typeDocument)
        {
            List<Commande> lesCommandes = new List<Commande>();
            // revue => liste Abonnement
            if (typeDocument.Equals("revue"))
            {
                string req = "Select c.id, c.dateCommande, c.montant, a.dateFinAbonnement, a.idRevue ";
                req += "from commande c join abonnement a using(id)";
                req += "where a.idRevue = @id ";
                req += "order by c.dateCommande DESC";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, parameters);

                while (curs.Read())
                {
                    string id = (string)curs.Field("id");
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
                req += "order by c.dateCommande DESC";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@id", idDocument}
                };

                BddMySql curs = BddMySql.GetInstance(connectionString);
                curs.ReqSelect(req, parameters);

                while (curs.Read())
                {
                    string id = (string)curs.Field("id");
                    DateTime dateCommande = (DateTime)curs.Field("dateCommande");
                    double montant = (double)curs.Field("montant");
                    int nbExemplaire = (int)curs.Field("nbExemplaire");
                    string idLivreDvd = (string)curs.Field("idLivreDvd");
                    string idSuivi = (string)curs.Field("idSuivi");
                    string etapeSuivi = (string)curs.Field("nom");
                    CommandeDocument commandeDocument = new CommandeDocument(id, dateCommande, montant, nbExemplaire, idLivreDvd, idSuivi, etapeSuivi);
                    lesCommandes.Add(commandeDocument);
                }
                curs.Close();
            }
            return lesCommandes;
        }

        /// <summary>
        /// Retourne toutes les étapes de suivi à partir de la BDD
        /// </summary>
        /// <returns></returns>
        public static List<Suivi> GetAllSuivis()
        {
            List<Suivi> lesSuivis = new List<Suivi>();
            string req = "Select * from suivi ;";

            BddMySql curs = BddMySql.GetInstance(connectionString);
            curs.ReqSelect(req, null);

            while (curs.Read())
            {
                Suivi leSuivi = new Suivi((string)curs.Field("idSuivi"), (string)curs.Field("nom"));
                lesSuivis.Add(leSuivi);
            }
            curs.Close();
            return lesSuivis;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire"></param>
        /// <returns>true si l'insertion a pu se faire</returns>
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
        /// ecriture d'un document dans la bdd
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>

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

            switch (document)
            {
                case Livre _:
                    {
                        Livre livre = (Livre)document;

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
                        break;
                    }

                case Dvd _:
                    {
                        Dvd dvd = (Dvd)document;

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
                        break;
                    }

                case Revue _:
                    {
                        Revue revue = (Revue)document;
                        req3 = "insert into revue values (@idRevue, @empruntable,@periodicite,@delaiMiseADispo)";
                        parameters3 = new Dictionary<string, object>
                            {
                                { "@idRevue", revue.Id},
                                { "@empruntable", revue.Empruntable},
                                { "@periodicite", revue.Periodicite},
                                { "@delaiMiseADispo", revue.DelaiMiseADispo}
                            };
                        break;
                    }
            }
            Console.WriteLine("req1 : " + req1 + "\nreq2 : " + req2 + "\nreq3 : " + req3);
            try
            {
                // exécuter les deux requêtes dans une transaction
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
        /// update un document dans la bdd
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
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

            switch (document)
            {
                case Livre _:
                    {
                        Livre livre = (Livre)document;
                        req2 = "UPDATE livre SET isbn=@isbn,auteur=@auteur, collection=@collection WHERE id=@idLivre";
                        parameters2 = new Dictionary<string, object>
                            {
                                { "@idLivre", livre.Id},
                                { "@isbn", livre.Isbn},
                                { "@auteur", livre.Auteur},
                                { "@collection", livre.Collection}
                            };
                        break;
                    }

                case Dvd _:
                    {
                        Dvd dvd = (Dvd)document;
                        req2 = "UPDATE dvd SET synopsis=@synopsis,realisateur=@realisateur,duree=@duree WHERE id=@idDvd";
                        parameters2 = new Dictionary<string, object>
                            {
                                { "@idDvd", dvd.Id},
                                { "@synopsis", dvd.Synopsis},
                                { "@realisateur", dvd.Realisateur},
                                { "@duree", dvd.Duree}
                            };
                        break;
                    }

                case Revue _:
                    {
                        Revue revue = (Revue)document;
                        req2 = "UPDATE revue SET empruntable=@empruntable, periodicite=@periodicite, delaiMiseAdispo=@delaiMiseADispo ";
                        req2 += "WHERE id=@idRevue";
                        parameters2 = new Dictionary<string, object>
                            {
                                { "@idRevue", revue.Id},
                                { "@empruntable", revue.Empruntable},
                                { "@periodicite", revue.Periodicite},
                                { "@delaiMiseADispo", revue.DelaiMiseADispo}
                            };
                        break;
                    }
            }
            Console.WriteLine("req1 : " + req1 + "\nreq2 : " + req2);

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
        /// <returns></returns>
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
                    { req += "dvd";
                        break;
                    }
                case Revue _:
                    {
                        req += "revue";
                        break ;
                    }
            }
            req += " WHERE id=@id";
            Console.WriteLine(req);
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
        /// <returns></returns>
        public static bool UpdateSuiviCommandeDocument(string idLivreDvd, string idSuivi)
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

    }
}
