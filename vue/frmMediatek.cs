using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mediatek86.metier;
using Mediatek86.controleur;
using System.Drawing;
using System.Linq;

namespace Mediatek86.vue
{
    /// <summary>
    /// fenêtre de gestion de média
    /// </summary>
    public partial class FrmMediatek : Form
    {

        #region Variables globales

        private readonly Controle controle;

        const string ETATNEUF = "00001";
        const string TYPELIVRE = "livre";
        const string TYPEREVUE = "revue";
        const string TYPEDVD = "dvd";
        const int IDMIN_GESTION = 3;

        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgLivresEtats = new BindingSource();
        private readonly BindingSource bdgDvdEtats = new BindingSource();
        private readonly BindingSource bdgRevuesEtats = new BindingSource();
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private readonly BindingSource bdgRevueExemplairesListe = new BindingSource();
        private readonly BindingSource bdgLivreCommandesListe = new BindingSource();
        private readonly BindingSource bdgAbonnementsRevueListe = new BindingSource();
        private readonly BindingSource bdgLivreExemplaires = new BindingSource();
        private readonly BindingSource bdgDvdExemplaires = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<Exemplaire> lesRevueExemplaires = new List<Exemplaire>();
        private readonly List<Categorie> lesGenres;
        private readonly List<Categorie> lesPublics;
        private readonly List<Categorie> lesRayons;
        private readonly List<Suivi> lesSuivis;
        private readonly List<Etat> lesEtats;
        private List<Livre> lesSelectLivres;
        private List<CommandeDocument> lesLivreCommandes;
        private List<Abonnement> lesAbonnementsRevue;
        private List<Exemplaire> lesLivreExemplaires;
        private List<Exemplaire> lesDvdExemplaires;
        private readonly BindingSource bdgInfoGenres = new BindingSource();
        private readonly BindingSource bdgInfoPublics = new BindingSource();
        private readonly BindingSource bdgInfoRayons = new BindingSource();
        private readonly BindingSource bdgSelectLivre = new BindingSource();
        private readonly BindingSource bdgSelectRevue = new BindingSource();
        private bool livreEnModif = false;
        private bool dvdEnModif = false;
        private bool revueEnModif = false;
        #endregion
        
        /// <summary>
        /// initialiser la forme
        /// </summary>
        /// <param name="controle"></param>
        /// <param name="idService"></param>
        internal FrmMediatek(Controle controle, int idService)
        {
            InitializeComponent();
            this.controle = controle;
            DroitGestionCatalogue(idService >= IDMIN_GESTION);
            if (idService >= IDMIN_GESTION)
            {
                // afficher les abonnements qui vont expirer dans moins de 30 jours
                List<Abonnement> lesAbonnements = controle.GetAbonnementsAExpirer();
                if (lesAbonnements.Count > 0)
                {
                    lesRevues = controle.GetAllRevues();
                    string message = "Attention, ces abonnements ci-dessous vont expirer dans moins de 30 jours : ";
                    foreach (Abonnement abonnement in lesAbonnements)
                    {
                        message += "\n" + lesRevues.Find(x => x.Id.Equals(abonnement.IdRevue)).Titre + " : " + abonnement.DateFinAbonnement.ToString("dd/MM/yyyy");
                    }
                    MessageBox.Show(message, "Alerte");
                }

            }
            lesGenres = this.controle.GetAllGenres();
            lesPublics = this.controle.GetAllPublics();
            lesRayons = this.controle.GetAllRayons();
            lesEtats = this.controle.GetAllEtats();
            lesSuivis = this.controle.GetAllSuivis();
        }


        #region modules communs

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// gérer le droit de mise à jour du catalogue (Livre/dvd/revue)
        /// </summary>
        /// <param name="droit"></param>
        private void DroitGestionCatalogue(bool droit)
        {
            // onglet livre
            btnLivreAjout.Visible = droit;
            btnLivreModif.Visible = droit;
            btnLivreSuppr.Visible = droit;
            btnLivreValider.Visible = droit;
            cbxEtatExemplaireLivre.Visible = droit;
            btnMajEtatExemplaireLivre.Visible = droit;
            btnSupprExemplaireLivre.Visible = (droit);

            // onglet dvd
            btnDvdAjout.Visible = droit;
            btnDvdModif.Visible = droit;
            btnDvdSuppr.Visible = droit;
            btnDvdValider.Visible = droit;
            cbxEtatExemplaireDvd.Visible = droit;
            btnMajEtatExemplaireDvd.Visible = droit;
            btnSupprExemplaireDvd.Visible = (droit);

            // onglet revue
            btnRevueAjout.Visible = droit;
            btnRevueModif.Visible = droit;
            btnRevueSuppr.Visible = droit;
            btnRevueValider.Visible = droit;

            // onglet parution des revues
            cbxEtatExemplaireRevue.Visible = droit;
            btnMajEtatExemplaireRevue.Visible = droit;
            btnSupprExemplaireRevue.Visible = droit;
            grpReceptionExemplaire.Visible = droit;

            // onglet abonnements des revues
            btnSupprAbonnementRevue.Visible = droit;
            grpNouvelAbonnement.Visible = droit;

            // onglet commandes des livres
            cbxSuiviCommandeLivre.Visible = droit;
            btnMajSuiviCommandeLivre.Visible = droit;
            btnSupprCommandeLivre.Visible = droit;
            grbNouvelleCommandeLivre.Visible = droit;
        }

        /// <summary>
        /// accepter uniquement la saisie d'un montant en décimal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaisieMontant(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != ','))
            {
                e.Handled = true;
                return;
            }

            // checks to make sure only 1 decimal is allowed
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(e.KeyChar) != -1)
            {
                e.Handled = true;
            }
        }

        #endregion


        #region Revues
        //-----------------------------------------------------------
        // ONGLET "Revues"
        //------------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            RemplirComboCategorie(lesGenres, bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(lesPublics, bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(lesRayons, bdgRayons, cbxRevuesRayons);

            RemplirComboCategorie(lesGenres, bdgInfoGenres, cbxRevuesInfoGenres);
            RemplirComboCategorie(lesPublics, bdgInfoPublics, cbxRevuesInfoPublics);
            RemplirComboCategorie(lesRayons, bdgInfoRayons, cbxRevuesInfoRayons);

            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["empruntable"].Visible = false;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;

            DeselectDgvRevuesListe();
        }

        /// <summary>
        /// nettoyer la sélection dans la datagridview
        /// </summary>
        private void DeselectDgvRevuesListe()
        {
            bdgRevuesListe.Position = -1;
            dgvRevuesListe.ClearSelection();
            AccesModifRevue(false);
            ViderRevuesInfos();
            ViderRevuesZones();
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>();
                    revues.Add(revue);
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheRevuesInfos(Revue revue)
        {
            grpRevuesInfos.Text = "Informations détaillées";
            revueEnModif = false;
            AccesEcritureRevue(false);

            AccesEcritureRevue(false);
            txbRevuesPeriodicite.Text = revue.Periodicite;
            chkRevuesEmpruntable.Checked = revue.Empruntable;
            txbRevuesImage.Text = revue.Image;
            nudRevuesDelai.Value = revue.DelaiMiseADispo;
            txbRevuesNumero.Text = revue.Id;
            cbxRevuesInfoGenres.SelectedIndex = lesGenres.FindIndex(x => x.Id.Equals(revue.IdGenre));
            cbxRevuesInfoPublics.SelectedIndex = lesPublics.FindIndex(x => x.Id.Equals(revue.IdPublic));
            cbxRevuesInfoRayons.SelectedIndex = lesRayons.FindIndex(x => x.Id.Equals(revue.IdRayon));
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);

            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }
        /// <summary>
        /// événement clic sur le bouton "ajouter une nouvelle revue"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevueAjout_Click(object sender, EventArgs e)
        {
            DeselectDgvRevuesListe();
            AccesEcritureRevue(true);
            grpRevuesInfos.Text = "Nouvelle revue";
            // calculer l'id de la nouvelle revue, attention : l'id des revues commence par 1
            txbRevuesNumero.Text = (int.Parse(lesRevues.OrderBy(o => o.Id).Last().Id) + 1).ToString();
        }


        /// <summary>
        /// événement click sur le bouton "valider" dans l'onglet revues
        /// valider la revue nouvellement créée ou modifiée, et le transmettre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevueValider_Click(object sender, EventArgs e)
        {
            // les champs 'numéro', 'genre', 'public' et 'rayon' sont obligatoires
            if (cbxRevuesInfoGenres.SelectedIndex >= 0 && cbxRevuesInfoPublics.SelectedIndex >= 0 && cbxRevuesInfoRayons.SelectedIndex >= 0)
            {
                Categorie LeGenre = lesGenres[cbxRevuesInfoGenres.SelectedIndex];
                Categorie lePublic = lesPublics[cbxRevuesInfoPublics.SelectedIndex];
                Categorie leRayon = lesRayons[cbxRevuesInfoRayons.SelectedIndex];
                Revue revue = new Revue(txbRevuesNumero.Text, txbRevuesTitre.Text, txbLivresImage.Text, LeGenre.Id, LeGenre.Libelle,
                    lePublic.Id, lePublic.Libelle, leRayon.Id, leRayon.Libelle, chkRevuesEmpruntable.Checked, txbReceptionRevuePeriodicite.Text, (int)nudRevuesDelai.Value);

                // mode ajout
                if (!revueEnModif)
                {

                    if (controle.CreerDocument(revue))
                    {
                        MessageBox.Show("La nouvelle revue n°" + revue.Id + " est ajoutée !", "Succès");
                        lesRevues = controle.GetAllRevues();
                        RemplirRevuesListeComplete();
                        bdgRevuesListe.Position = lesRevues.FindIndex(x => x.Id.Equals(revue.Id));
                    }
                    else
                    {
                        MessageBox.Show("La nouvelle revue n°" + revue.Id + " n'est pas ajoutée !", "Echec");
                    }
                }
                // mode modification
                else
                {
                    if (controle.ModifDocument(revue))
                    {
                        MessageBox.Show("La revue n°" + revue.Id + " est modifiée !", "Succès");

                        int index = lesRevues.FindIndex(x => x.Id.Equals(revue.Id));
                        lesRevues[index] = revue;
                        RemplirRevuesListeComplete();
                        bdgRevuesListe.Position = index;
                    }
                    else
                    {
                        MessageBox.Show("La revue n°" + revue.Id + "n'est pas modifiée !", "Echec");
                    }
                }

            }
            else
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires", "Information");
            }
        }

        /// <summary>
        /// événement clic sur le bouton "modifier" dans l'onglet Revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevueModif_Click(object sender, EventArgs e)
        {
            revueEnModif = true;
            AccesEcritureRevue(true);
            grpRevuesInfos.Text = "Modification";
        }

        /// <summary>
        /// événement clic sur le bouton "supprimer" dans l'onglet Revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevueSuppr_Click(object sender, EventArgs e)
        {
            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            // vérifier que le document n'a pas de commande ni d'exemplaires attaché
            if (controle.GetExemplairesDocument(revue.Id).Count == 0 && controle.GetCommandes(revue.Id, TYPEREVUE).Count == 0)
            {
                // demander la confirmation de suppression
                if (MessageBox.Show("Etes-vous sûr de supprimer la revue n°" + revue.Id + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controle.SupprDocument(revue))
                    {
                        MessageBox.Show("La revue n°" + revue.Id + " est supprimée !", "Succès");
                        lesRevues = controle.GetAllRevues();
                        RemplirRevuesListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("La revue n°" + revue.Id + " n'est pas supprimée !", "Echec");
                    }
                }
            }
            else
            {
                MessageBox.Show("Cette revue contient des exemplaires et/ou commandes", "Impossible à supprimer");
            }

        }


        /// <summary>
        /// Vider les zones d'affichage des informations de la reuve
        /// </summary>
        private void ViderRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            chkRevuesEmpruntable.Checked = false;
            txbRevuesImage.Text = "";
            nudRevuesDelai.Value = 0;
            txbRevuesNumero.Text = "";
            cbxRevuesInfoGenres.SelectedIndex = -1;
            cbxRevuesInfoPublics.SelectedIndex = -1;
            cbxRevuesInfoRayons.SelectedIndex = -1;
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// activer ou désactiver la saisie dans les champs (sauf id) des informations détaillées
        /// </summary>
        /// <param name="acces"></param>
        private void AccesEcritureRevue(bool acces)
        {
            chkRevuesEmpruntable.Enabled = acces;
            txbRevuesTitre.ReadOnly = !acces;
            txbRevuesPeriodicite.ReadOnly = !acces;
            nudRevuesDelai.ReadOnly = !acces;
            cbxRevuesInfoGenres.Enabled = acces;
            cbxRevuesInfoPublics.Enabled = acces;
            cbxRevuesInfoRayons.Enabled = acces;
            txbRevuesImage.ReadOnly = !acces;
            btnRevueValider.Enabled = acces;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                    AccesModifRevue(true);
                }
                catch
                {
                    ViderRevuesZones();
                }
            }
        }

        /// <summary>
        /// enable le bouton de modification et celui de suppression pour une revue
        /// </summary>
        /// <param name="acces"></param>
        private void AccesModifRevue(bool acces)
        {
            btnRevueModif.Enabled = acces;
            btnRevueSuppr.Enabled = acces;
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void ViderRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ViderRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }

        #endregion


        #region Livres

        //-----------------------------------------------------------
        // ONGLET "LIVRES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controle.GetAllLivres();
            RemplirComboCategorie(lesGenres, bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(lesPublics, bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(lesRayons, bdgRayons, cbxLivresRayons);

            RemplirComboCategorie(lesGenres, bdgInfoGenres, cbxLivresInfoGenres);
            RemplirComboCategorie(lesPublics, bdgInfoPublics, cbxLivresInfoPublics);
            RemplirComboCategorie(lesRayons, bdgInfoRayons, cbxLivresInfoRayons);

            RemplirLivresListeComplete();
            RemplirComboLivresEtats();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;

            DeselectDgvLivresListe();
        }

        /// <summary>
        /// nettoyer la sélection dans la datagridview
        /// </summary>
        private void DeselectDgvLivresListe()
        {
            bdgLivresListe.Position = -1;
            dgvLivresListe.ClearSelection();
            AccesModifLivre(false);
            ViderLivresZones();
            ViderLivresInfos();
            dgvLivreExemplaires.DataSource = null;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>();
                    livres.Add(livre);
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }
        /// <summary>
        /// click pour ajouter un nouveau livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivreAjout_Click(object sender, EventArgs e)
        {
            DeselectDgvLivresListe();
            AccesEcritureLivres(true);
            grpLivresInfos.Text = "Nouveau livre";
            // calculer l'id du nouveau livre, attention : l'id des livres commence par 0
            string lastId = lesLivres.OrderBy(o => o.Id).Last().Id;
            string newId = (int.Parse(lastId) + 1).ToString();
            newId = new string('0', lastId.Length - newId.Length) + newId;
            txbLivresNumero.Text = newId;
        }

        private void btnLivreModif_Click(object sender, EventArgs e)
        {
            livreEnModif = true;
            AccesEcritureLivres(true);
            grpLivresInfos.Text = "Modification";
        }

        /// <summary>
        /// événement clic sur le bouton "supprimer" dans l'onglet livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivreSuppr_Click(object sender, EventArgs e)
        {
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            // vérifier que le document n'a pas de commande ni d'exemplaire attaché
            if (controle.GetExemplairesDocument(livre.Id).Count == 0 && controle.GetCommandes(livre.Id, TYPELIVRE).Count == 0)
            {
                // demander la confirmation de suppression
                if (MessageBox.Show("Etes-vous sûr de supprimer le livre n°" + livre.Id + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controle.SupprDocument(livre))
                    {
                        MessageBox.Show("Le livre n°" + livre.Id + " est supprimé !", "Succès");
                        lesLivres = controle.GetAllLivres();
                        RemplirLivresListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("Le livre n°" + livre.Id + " n'est pas supprimé !", "Echec");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ce livre contient des exemplaires et/ou commandes", "Impossible à supprimer");
            }

        }

        /// <summary>
        /// événement click sur le btnLivreValider
        /// enregistrer un livre modifié/créé
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivreValider_Click(object sender, EventArgs e)
        {
            // les champs 'numéro', 'titre', genre', 'public' et 'rayon' sont obligatoires
            if (!txbLivresTitre.Text.Equals("") && cbxLivresInfoGenres.SelectedIndex >= 0
                && cbxLivresInfoPublics.SelectedIndex >= 0 && cbxLivresInfoRayons.SelectedIndex >= 0)
            {
                Categorie LeGenre = lesGenres[cbxLivresInfoGenres.SelectedIndex];
                Categorie lePublic = lesPublics[cbxLivresInfoPublics.SelectedIndex];
                Categorie leRayon = lesRayons[cbxLivresInfoRayons.SelectedIndex];
                Livre livre = new Livre(txbLivresNumero.Text, txbLivresTitre.Text, txbLivresImage.Text, txbLivresIsbn.Text, txbLivresAuteur.Text,
                    txbLivresCollection.Text, LeGenre.Id, LeGenre.Libelle, lePublic.Id, lePublic.Libelle, leRayon.Id, leRayon.Libelle);

                // mode ajout
                if (!livreEnModif)
                {
                    if (controle.CreerDocument(livre))
                    {
                        MessageBox.Show("Le nouveau livre n°" + livre.Id + " est ajouté !", "Succès");
                        lesLivres = controle.GetAllLivres();
                        RemplirLivresListeComplete();
                        bdgLivresListe.Position = lesLivres.FindIndex(x => x.Id.Equals(livre.Id));
                    }
                    else
                    {
                        MessageBox.Show("Le nouveau livre n°" + livre.Id + " n'est pas ajouté !", "Echec");
                    }
                }
                // mode modification
                else
                {
                    if (controle.ModifDocument(livre))
                    {
                        MessageBox.Show("Le livre n°" + livre.Id + " est modifié !", "Succès");
                        int index = lesLivres.FindIndex(x => x.Id.Equals(livre.Id));
                        lesLivres[index] = livre;
                        RemplirLivresListeComplete();
                        bdgLivresListe.Position = index;
                    }
                    else
                    {
                        MessageBox.Show("Le livre n°" + livre.Id + "n'est pas modifié !", "Echec");
                    }

                }

            }
            else
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires", "Information");
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficheLivresInfos(Livre livre)
        {
            grpLivresInfos.Text = "Informations détaillées";
            livreEnModif = false;
            AccesEcritureLivres(false);

            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            cbxLivresInfoGenres.SelectedIndex = lesGenres.FindIndex(x => x.Id.Equals(livre.IdGenre));
            cbxLivresInfoPublics.SelectedIndex = lesPublics.FindIndex(x => x.Id.Equals(livre.IdPublic));
            cbxLivresInfoRayons.SelectedIndex = lesRayons.FindIndex(x => x.Id.Equals(livre.IdRayon));
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vider les zones d'affichage des informations du livre
        /// </summary>
        private void ViderLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            cbxLivresInfoGenres.SelectedIndex = -1;
            cbxLivresInfoPublics.SelectedIndex = -1;
            cbxLivresInfoRayons.SelectedIndex = -1;
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                    AccesModifLivre(true);
                    lesLivreExemplaires = controle.GetExemplairesDocument(livre.Id);
                    remplirLivreExemplairesListe(lesLivreExemplaires);
                }
                catch
                {
                    ViderLivresZones();
                }
            }
        }

        /// <summary>
        /// enable le bouton de modification et celui de suppression pour un livre
        /// </summary>
        /// <param name="access"></param>
        private void AccesModifLivre(bool access)
        {
            btnLivreModif.Enabled = access;
            btnLivreSuppr.Enabled = access;
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
        }

        /// <summary>
        /// vider les zones de recherche et de filtre
        /// </summary>
        private void ViderLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ViderLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Tri sur les colonnes de la liste des exemplaires d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivreExemplaires_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvLivreExemplaires.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "DateAchat":
                    sortedList = lesLivreExemplaires.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "Numero":
                    sortedList = lesLivreExemplaires.OrderBy(o => o.Numero).ToList();
                    break;
                case "Etat":
                    sortedList = lesLivreExemplaires.OrderBy(o => o.IdEtat).ToList();
                    break;
            }
            remplirLivreExemplairesListe(sortedList);
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivreExemplaires_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivreExemplaires.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgLivreExemplaires.List[bdgLivreExemplaires.Position];
                AccesModifEtatExemplaireLivre(true);
                cbxEtatExemplaireLivre.SelectedIndex = lesEtats.FindIndex(x => x.Id.Equals(exemplaire.IdEtat));
            }
        }

        /// <summary>
        /// énénement clic sur le bonton "mettre à jour l'état" pour un exemplaire d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMajEtatLivreExemplaire_Click(object sender, EventArgs e)
        {
            if (cbxEtatExemplaireLivre.SelectedIndex >= 0)
            {
                Exemplaire exemplaire = (Exemplaire)bdgLivreExemplaires.List[bdgLivreExemplaires.Position];
                if (controle.UpdateEtatExemplaire(exemplaire.IdDocument, exemplaire.Numero, ((Etat)cbxEtatExemplaireLivre.SelectedItem).Id))
                {
                    MessageBox.Show("Mis à jour réussi!", "Succès");
                    lesLivreExemplaires = controle.GetExemplairesDocument(exemplaire.IdDocument);
                    remplirLivreExemplairesListe(lesLivreExemplaires);
                    dgvLivreExemplaires.Rows[lesLivreExemplaires.FindIndex(x => x.Numero.Equals(exemplaire.Numero))].Selected = true;
                }
                else
                {
                    MessageBox.Show("Mis à jour échoué !", "Echec");
                }
            }
        }

        /// <summary>
        /// énénement clic sur le bouton "supprimer" pour supprimer un exemplaire de livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprExemplaireLivre_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de supprimer cet exemplaire ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                Exemplaire exemplaire = (Exemplaire)bdgLivreExemplaires.List[bdgLivreExemplaires.Position];
                if (controle.SupprExemplaire(exemplaire.IdDocument, exemplaire.Numero))
                {
                    MessageBox.Show("Suppression réussie!", "Succès");
                    lesLivreExemplaires = controle.GetExemplairesDocument(exemplaire.IdDocument);
                    remplirLivreExemplairesListe(lesLivreExemplaires);
                }
                else
                {
                    MessageBox.Show("Suppression échouée !", "Echec");
                }
            }
        }

        /// <summary>
        /// activer ou désactiver la saisie dans les champs (sauf id) des informations détaillées
        /// </summary>
        /// <param name="acces"></param>
        private void AccesEcritureLivres(bool acces)
        {
            txbLivresIsbn.ReadOnly = !acces;
            txbLivresTitre.ReadOnly = !acces;
            txbLivresAuteur.ReadOnly = !acces;
            txbLivresCollection.ReadOnly = !acces;
            txbLivresImage.ReadOnly = !acces;
            cbxLivresInfoGenres.Enabled = acces;
            cbxLivresInfoPublics.Enabled = acces;
            cbxLivresInfoRayons.Enabled = acces;
            btnLivreValider.Enabled = acces;
        }

        /// <summary>
        /// gérer l'accès à la modification d'un exemplaire livre 
        /// </summary>
        /// <param name="acces"></param>
        private void AccesModifEtatExemplaireLivre(bool acces)
        {
            cbxEtatExemplaireLivre.Enabled = acces;
            btnMajEtatExemplaireLivre.Enabled = acces;
            btnSupprExemplaireLivre.Enabled = acces;
            if (!acces)
            {
                cbxEtatExemplaireLivre.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// remplir la datagridview avec la liste des exemplaires du livre
        /// </summary>
        /// <param name="lesLivreExemplaires"></param>
        private void remplirLivreExemplairesListe(List<Exemplaire> lesLivreExemplaires)
        {
            bdgLivreExemplaires.DataSource = lesLivreExemplaires;
            dgvLivreExemplaires.DataSource = bdgLivreExemplaires;

            dgvLivreExemplaires.Columns["Photo"].Visible = false;
            dgvLivreExemplaires.Columns["IdEtat"].Visible = false;
            dgvLivreExemplaires.Columns["IdDocument"].Visible = false;
            dgvLivreExemplaires.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivreExemplaires.Columns["DateAchat"].DisplayIndex = 0;

            dgvLivreExemplaires.ClearSelection();
            AccesModifEtatExemplaireLivre(false);
        }

        private void RemplirComboLivresEtats()
        {
            bdgLivresEtats.DataSource = lesEtats;
            cbxEtatExemplaireLivre.DataSource = bdgLivresEtats;
            cbxEtatExemplaireLivre.SelectedIndex = -1;
        }



        #endregion


        #region Dvd
        //-----------------------------------------------------------
        // ONGLET "DVD"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controle.GetAllDvd();
            RemplirComboCategorie(lesGenres, bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(lesPublics, bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(lesRayons, bdgRayons, cbxDvdRayons);

            RemplirComboCategorie(lesGenres, bdgInfoGenres, cbxDvdInfoGenres);
            RemplirComboCategorie(lesPublics, bdgInfoPublics, cbxDvdInfoPublics);
            RemplirComboCategorie(lesRayons, bdgInfoRayons, cbxDvdInfoRayons);

            RemplirComboDvdEtats();
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;

            DeselectDgvDvdListe();
        }

        /// <summary>
        /// nettoyer la sélection dans la datagridview
        /// </summary>
        private void DeselectDgvDvdListe()
        {
            bdgDvdListe.Position = -1;
            dgvDvdListe.ClearSelection();
            AccesModifDvd(false);
            ViderDvdInfos();
            ViderDvdZones();
            dgvDvdExemplaires.DataSource = null;
        }

        /// <summary>
        /// remplir le combobox des états d'exemplaire de dvd
        /// </summary>
        private void RemplirComboDvdEtats()
        {
            bdgDvdEtats.DataSource = lesEtats;
            cbxEtatExemplaireDvd.DataSource = bdgDvdEtats;
            cbxEtatExemplaireDvd.SelectedIndex = -1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>();
                    Dvd.Add(dvd);
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// événement clic sur le bouton "ajouter un nouveau dvd"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAjout_Click(object sender, EventArgs e)
        {
            DeselectDgvDvdListe();
            AccesEcritureDvd(true);
            grpDvdInfos.Text = "Nouveau Dvd";
            // calculer l'id du nouveau Dvd, attention : l'id des livres commence par 2
            txbDvdNumero.Text = (int.Parse(lesDvd.OrderBy(o => o.Id).Last().Id) + 1).ToString();
        }

        /// <summary>
        /// événement clic sur le bouton "modifier" dans l'onglet dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdModif_Click(object sender, EventArgs e)
        {
            dvdEnModif = true;
            AccesEcritureDvd(true);
            grpDvdInfos.Text = "Modification";
        }

        /// <summary>
        /// événement clic sur le bouton "supprimer" dans l'onglet dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdSuppr_Click(object sender, EventArgs e)
        {
            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            // vérifier que le document n'a pas de commande ni d'exemplaire attaché
            if (controle.GetExemplairesDocument(dvd.Id).Count == 0 && controle.GetCommandes(dvd.Id, TYPEDVD).Count == 0)
            {
                // demander la confirmation de suppression
                if (MessageBox.Show("Etes-vous sûr de supprimer le dvd n°" + dvd.Id + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controle.SupprDocument(dvd))
                    {
                        MessageBox.Show("Le dvd n°" + dvd.Id + " est supprimé !", "Succès");
                        lesDvd = controle.GetAllDvd();
                        RemplirDvdListeComplete();
                    }
                    else
                    {
                        MessageBox.Show("Le dvd n°" + dvd.Id + " n'est pas supprimé !", "Echec");
                    }
                }
            }
            else
            {
                MessageBox.Show("Ce dvd contient des exemplaires et/ou commandes", "Impossible à supprimer");
            }

        }

        /// <summary>
        /// événement click sur le bouton "valider"
        /// valider la modification ou le nouveau dvd , et le transmettre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdValider_Click(object sender, EventArgs e)
        {

            // les champs 'genre', 'titre', 'duree', public' et 'rayon' sont obligatoires
            if (!txbDvdTitre.Text.Equals("") && nudDvdDuree.Value > 0 && cbxDvdInfoGenres.SelectedIndex >= 0
                && cbxDvdInfoPublics.SelectedIndex >= 0 && cbxDvdInfoRayons.SelectedIndex >= 0)
            {
                Categorie LeGenre = lesGenres[cbxDvdInfoGenres.SelectedIndex];
                Categorie lePublic = lesPublics[cbxDvdInfoPublics.SelectedIndex];
                Categorie leRayon = lesRayons[cbxDvdInfoRayons.SelectedIndex];
                Dvd dvd = new Dvd(txbDvdNumero.Text, txbDvdTitre.Text, txbDvdImage.Text, (int)nudDvdDuree.Value, txbDvdRealisateur.Text,
                    txbDvdSynopsis.Text, LeGenre.Id, LeGenre.Libelle, lePublic.Id, lePublic.Libelle, leRayon.Id, leRayon.Libelle);

                // mode ajout
                if (!dvdEnModif)
                {

                    if (controle.CreerDocument(dvd))
                    {
                        MessageBox.Show("Le nouveau Dvd n°" + dvd.Id + " est ajouté !", "Succès");
                        lesDvd = controle.GetAllDvd();
                        RemplirDvdListeComplete();
                        bdgDvdListe.Position = lesDvd.FindIndex(x => x.Id.Equals(dvd.Id));
                    }
                    else
                    {
                        MessageBox.Show("Le nouveau Dvd n°" + dvd.Id + " n'est pas ajouté !", "Echec");
                    }
                }

                // mode modification
                else
                {
                    if (controle.ModifDocument(dvd))
                    {
                        MessageBox.Show("Le dvd n°" + dvd.Id + " est modifié !", "Succès");

                        int index = lesDvd.FindIndex(x => x.Id.Equals(dvd.Id));
                        lesDvd[index] = dvd;
                        RemplirDvdListeComplete();
                        bdgDvdListe.Position = index;
                    }
                    else
                    {
                        MessageBox.Show("Le dvd n°" + dvd.Id + "n'est pas modifié !", "Echec");
                    }
                }

            }
            else
            {
                MessageBox.Show("Merci de remplir tous les champs obligatoires", "Information");
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd"></param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            grpDvdInfos.Text = "Informations détaillées";
            dvdEnModif = false;
            AccesEcritureDvd(false);

            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            nudDvdDuree.Value = dvd.Duree;
            txbDvdNumero.Text = dvd.Id;
            cbxDvdInfoGenres.SelectedIndex = lesGenres.FindIndex(x => x.Id.Equals(dvd.IdGenre));
            cbxDvdInfoPublics.SelectedIndex = lesPublics.FindIndex(x => x.Id.Equals(dvd.IdPublic));
            cbxDvdInfoRayons.SelectedIndex = lesRayons.FindIndex(x => x.Id.Equals(dvd.IdRayon));
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }

        /// <summary>
        /// Vider les zones d'affichage des informations du dvd
        /// </summary>
        private void ViderDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            nudDvdDuree.Value = 0;
            txbDvdNumero.Text = "";
            cbxDvdInfoGenres.SelectedIndex = -1;
            cbxDvdInfoPublics.SelectedIndex = -1;
            cbxDvdInfoRayons.SelectedIndex = -1;
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }
        /// <summary>
        /// activer ou désactiver la saisie dans les champs (sauf id) des informations détaillées
        /// </summary>
        /// <param name="acces"></param>
        private void AccesEcritureDvd(bool acces)
        {
            nudDvdDuree.ReadOnly = !acces;
            txbDvdTitre.ReadOnly = !acces;
            txbDvdRealisateur.ReadOnly = !acces;
            txbDvdSynopsis.ReadOnly = !acces;
            cbxDvdInfoGenres.Enabled = acces;
            cbxDvdInfoPublics.Enabled = acces;
            cbxDvdInfoRayons.Enabled = acces;
            txbDvdImage.ReadOnly = !acces;
            btnDvdValider.Enabled = acces;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                    AccesModifDvd(true);
                    lesDvdExemplaires = controle.GetExemplairesDocument(dvd.Id);
                    RemplirDvdExemplairesListe(lesDvdExemplaires);
                }
                catch
                {
                    ViderDvdZones();
                }
            }
        }

        /// <summary>
        /// enable le bouton de modification et celui de suppresion pour un dvd 
        /// </summary>
        /// <param name="access"></param>
        private void AccesModifDvd(bool access)
        {
            btnDvdModif.Enabled = access;
            btnDvdSuppr.Enabled = access;
        }

        /// <summary>
        /// Tri sur les colonnes de la liste des exemplaires d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdExemplaires_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvDvdExemplaires.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "DateAchat":
                    sortedList = lesDvdExemplaires.OrderBy(o => o.DateAchat).ToList();
                    break;
                case "Numero":
                    sortedList = lesDvdExemplaires.OrderBy(o => o.Numero).ToList();
                    break;
                case "Etat":
                    sortedList = lesDvdExemplaires.OrderBy(o => o.IdEtat).ToList();
                    break;
            }
            RemplirDvdExemplairesListe(sortedList);
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdExemplaires_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdExemplaires.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgDvdExemplaires.List[bdgDvdExemplaires.Position];
                AccesModifExemplaireDvd(true);
                cbxEtatExemplaireDvd.SelectedIndex = lesEtats.FindIndex(x => x.Id.Equals(exemplaire.IdEtat));
            }
        }


        /// <summary>
        /// énénement clic sur le bonton "mettre à jour l'état" pour un exemplaire d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMajEtatDvdExemplaire_Click(object sender, EventArgs e)
        {
            if (cbxEtatExemplaireDvd.SelectedIndex >= 0)
            {
                Exemplaire exemplaire = (Exemplaire)bdgDvdExemplaires.List[bdgDvdExemplaires.Position];
                if (controle.UpdateEtatExemplaire(exemplaire.IdDocument, exemplaire.Numero, ((Etat)cbxEtatExemplaireDvd.SelectedItem).Id))
                {
                    MessageBox.Show("Mis à jour réussi!", "Succès");
                    lesDvdExemplaires = controle.GetExemplairesDocument(exemplaire.IdDocument);
                    RemplirDvdExemplairesListe(lesDvdExemplaires);
                    dgvDvdExemplaires.Rows[lesDvdExemplaires.FindIndex(x => x.Numero.Equals(exemplaire.Numero))].Selected = true;
                }
                else
                {
                    MessageBox.Show("Mis à jour échoué !", "Echec");
                }
            }
        }

        /// <summary>
        /// énénement clic sur le bouton "supprimer" pour supprimer un exemplaire de dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprExemplaireDvd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de supprimer cet exemplaire ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Exemplaire exemplaire = (Exemplaire)bdgDvdExemplaires.List[bdgDvdExemplaires.Position];
                if (controle.SupprExemplaire(exemplaire.IdDocument, exemplaire.Numero))
                {
                    MessageBox.Show("Suppression réussie!", "Succès");
                    lesDvdExemplaires = controle.GetExemplairesDocument(exemplaire.IdDocument);
                    RemplirDvdExemplairesListe(lesDvdExemplaires);
                }
                else
                {
                    MessageBox.Show("Suppression échouée !", "Echec");
                }
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
        }

        /// <summary>
        /// remplir la datagridview avec la liste des exemplaires de dvd
        /// </summary>
        /// <param name="lesDvdExemplaires"></param>
        private void RemplirDvdExemplairesListe(List<Exemplaire> lesDvdExemplaires)
        {
            bdgDvdExemplaires.DataSource = lesDvdExemplaires;
            dgvDvdExemplaires.DataSource = bdgDvdExemplaires;

            dgvDvdExemplaires.Columns["Photo"].Visible = false;
            dgvDvdExemplaires.Columns["IdEtat"].Visible = false;
            dgvDvdExemplaires.Columns["IdDocument"].Visible = false;
            dgvDvdExemplaires.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdExemplaires.Columns["DateAchat"].DisplayIndex = 0;

            dgvDvdExemplaires.ClearSelection();
            AccesModifExemplaireDvd(false);
        }

        /// <summary>
        /// vider les zones de recherche et de filtre
        /// </summary>
        private void ViderDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ViderDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }

        /// <summary>
        /// activer ou désactiver la possibilité de mise à jour de l'état d'un exemplaire dvd 
        /// </summary>
        /// <param name="acces"></param>
        private void AccesModifExemplaireDvd(bool acces)
        {
            cbxEtatExemplaireDvd.Enabled = acces;
            btnMajEtatExemplaireDvd.Enabled = acces;
            btnSupprExemplaireDvd.Enabled = acces;
            if (!acces)
            {
                cbxEtatExemplaireDvd.SelectedIndex = -1;
            }
        }

        #endregion


        #region Réception Exemplaire de presse
        //-----------------------------------------------------------
        // ONGLET "RECEPTION DE REVUES"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet : blocage en saisie des champs de saisie des infos de l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues();
            RemplirComboRevuesEtats();
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            bdgRevueExemplairesListe.DataSource = exemplaires;
            dgvReceptionExemplairesListe.DataSource = bdgRevueExemplairesListe;
            dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
            dgvReceptionExemplairesListe.Columns["idDocument"].Visible = false;
            dgvReceptionExemplairesListe.Columns["photo"].Visible = false;
            dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
            dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;

            dgvReceptionExemplairesListe.ClearSelection();
            AccesModifExemplaireRevue(false);
            cbxEtatExemplaireRevue.SelectedIndex = -1;
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    ViderReceptionRevueInfos();
                }
            }
            else
            {
                ViderReceptionRevueInfos();
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            AccesReceptionExemplaireGroupBox(false);
            ViderReceptionRevueInfos();
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue"></param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            chkReceptionRevueEmpruntable.Checked = revue.Empruntable;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficherReceptionExemplairesRevue();
            // accès à la zone d'ajout d'un exemplaire
            AccesReceptionExemplaireGroupBox(true);
        }

        private void AfficherReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesRevueExemplaires = controle.GetExemplairesDocument(idDocuement);
            RemplirReceptionExemplairesListe(lesRevueExemplaires);
        }

        /// <summary>
        /// Vider les zones d'affchage des informations de la revue
        /// </summary>
        private void ViderReceptionRevueInfos()
        {
            txbReceptionRevuePeriodicite.Text = "";
            chkReceptionRevueEmpruntable.Checked = false;
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            lesRevueExemplaires = new List<Exemplaire>();
            RemplirReceptionExemplairesListe(lesRevueExemplaires);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Vider les zones d'affichage des informations de l'exemplaire
        /// </summary>
        private void ViderReceptionExemplaireInfos()
        {
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces"></param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            ViderReceptionExemplaireInfos();
            grpReceptionExemplaire.Enabled = acces;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, "neuf", idDocument);
                    if (controle.CreerExemplaire(exemplaire))
                    {
                        ViderReceptionExemplaireInfos();
                        AfficherReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesRevueExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesRevueExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Etat":
                    sortedList = lesRevueExemplaires.OrderBy(o => o.IdEtat).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// Sélection d'une ligne complète et affichage de l'image sz l'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgRevueExemplairesListe.List[bdgRevueExemplairesListe.Position];
                AccesModifExemplaireRevue(true);
                cbxEtatExemplaireRevue.SelectedIndex = lesEtats.FindIndex(x => x.Id.Equals(exemplaire.IdEtat));
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }

        /// <summary>
        /// énénement clic sur le bonton "mettre à jour l'état" pour un exemplaire d'un dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMajEtatRevueExemplaire_Click(object sender, EventArgs e)
        {
            if (cbxEtatExemplaireRevue.SelectedIndex >= 0)
            {
                Exemplaire exemplaire = (Exemplaire)bdgRevueExemplairesListe.List[bdgRevueExemplairesListe.Position];
                if (controle.UpdateEtatExemplaire(exemplaire.IdDocument, exemplaire.Numero, ((Etat)cbxEtatExemplaireRevue.SelectedItem).Id))
                {
                    MessageBox.Show("Mis à jour réussi!", "Succès");
                    AfficherReceptionExemplairesRevue();
                    dgvReceptionExemplairesListe.Rows[lesRevueExemplaires.FindIndex(x => x.Numero.Equals(exemplaire.Numero))].Selected = true;
                }
                else
                {
                    MessageBox.Show("Mis à jour échoué !", "Echec");
                }
            }
        }

        /// <summary>
        /// énénement clic sur le bouton "supprimer" pour supprimer un exemplaire de revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprExemplaireRevue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Êtes-vous sûr de supprimer cet exemplaire ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                Exemplaire exemplaire = (Exemplaire)bdgRevueExemplairesListe.List[bdgRevueExemplairesListe.Position];
                if (controle.SupprExemplaire(exemplaire.IdDocument, exemplaire.Numero))
                {
                    MessageBox.Show("Suppression réussie!", "Succès");
                    AfficherReceptionExemplairesRevue();
                }
                else
                {
                    MessageBox.Show("Suppression échouée !", "Echec");
                }
            }
        }

        /// <summary>
        /// gérer l'accès à la modification d'une parution de revue
        /// </summary>
        /// <param name="acces"></param>
        private void AccesModifExemplaireRevue(bool acces)
        {
            cbxEtatExemplaireRevue.Enabled = acces;
            btnMajEtatExemplaireRevue.Enabled = acces;
            btnSupprExemplaireRevue.Enabled = acces;
            if (!acces)
            {
                cbxEtatExemplaireRevue.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// remplir le combobox des états d'exemplaire de revue
        /// </summary>
        private void RemplirComboRevuesEtats()
        {
            bdgRevuesEtats.DataSource = lesEtats;
            cbxEtatExemplaireRevue.DataSource = bdgRevuesEtats;
            cbxEtatExemplaireRevue.SelectedIndex = -1;
        }

        #endregion

        #region AbonnementsRevues
        //-----------------------------------------------------------
        // ONGLET "Abonnements des revues"
        //-----------------------------------------------------------

        /// <summary>
        /// A l'entrée de l'onglet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabAbonnementRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controle.GetAllRevues().OrderBy(x => x.Id).ToList();
            RemplirComboSelectRevue(lesRevues, bdgSelectRevue, cbxSelectRevue);
            RemplirComboSelectRevue(lesRevues, bdgSelectRevue, cbxSelectRevueNouvelAbonnement);
            ViderSelectRevueInformations();
        }

        /// <summary>
        /// remplir des combobox pour sélectionner un livre par id
        /// </summary>
        /// <param name="revues"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        private void RemplirComboSelectRevue(List<Revue> revues, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = revues;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// remplir les informations détaillées et les commandes d'un livre sélectionnné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSelectRevue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSelectRevue.SelectedIndex >= 0)
            {
                Revue revue = (Revue)cbxSelectRevue.SelectedItem;
                AfficherSelectRevueInformations(revue);
                lesAbonnementsRevue = controle.GetCommandes(revue.Id, TYPEREVUE).ConvertAll(x => (Abonnement)x);
                RemplirAbonnementsRevueListe(lesAbonnementsRevue);
            }
        }

        /// <summary>
        /// afficher les informations d'une revue sélectionnée
        /// </summary>
        /// <param name="revue"></param>
        private void AfficherSelectRevueInformations(Revue revue)
        {
            ckbArEmpruntable.Checked = revue.Empruntable;
            txbArTitre.Text = revue.Titre;
            txbArPeriodicite.Text = revue.Periodicite;
            txbArDelai.Text = revue.DelaiMiseADispo.ToString();
            txbArGenre.Text = revue.Genre;
            txbArPublic.Text = revue.Public;
            txbArRayon.Text = revue.Rayon;
            txbArImage.Text = revue.Image;
        }

        /// <summary>
        /// vider les informations de revue
        /// </summary>
        private void ViderSelectRevueInformations()
        {
            ckbArEmpruntable.Checked = false;
            txbArTitre.Text = "";
            txbArPeriodicite.Text = "";
            txbArDelai.Text = "";
            txbArGenre.Text = "";
            txbArPublic.Text = "";
            txbArRayon.Text = "";
            txbArImage.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirAbonnementsRevueListe(List<Abonnement> abonnements)
        {
            bdgAbonnementsRevueListe.DataSource = abonnements;
            dgvAbonnementsRevue.DataSource = bdgAbonnementsRevueListe;

            dgvAbonnementsRevue.Columns["id"].Visible = false;
            dgvAbonnementsRevue.Columns["idRevue"].Visible = false;
            dgvAbonnementsRevue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgvAbonnementsRevue.ClearSelection();
            btnSupprAbonnementRevue.Enabled = false;
        }

        /// <summary>
        /// enable le bouton de suppression quand un abonnement est sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAbonnementsRevue_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAbonnementsRevue.CurrentCell != null)
            {
                Abonnement abonnement = (Abonnement)bdgAbonnementsRevueListe.List[bdgAbonnementsRevueListe.Position];
                bool supprimable = true;
                List<Exemplaire> lesExemplairesRevue = controle.GetExemplairesDocument(abonnement.IdRevue);
                if (lesExemplairesRevue.Count > 0)
                {
                    foreach (Exemplaire exemplaire in lesExemplairesRevue)
                    {
                        if (abonnement.ParutionDansAbonnement(exemplaire.DateAchat))
                        {
                            supprimable = false;
                            break;
                        }
                    }
                }
                btnSupprAbonnementRevue.Enabled = supprimable;
            }
        }

        /// <summary>
        /// supprimer un abonnement sans aucun exemplaire attaché
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprAbonnementRevue_Click(object sender, EventArgs e)
        {
            Abonnement abonnement = (Abonnement)bdgAbonnementsRevueListe.List[bdgAbonnementsRevueListe.Position];
            if (MessageBox.Show("Etes-vous sûr de supprimer cet abonnement ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (controle.SuppreAbonnementRevue(abonnement.Id))
                {
                    MessageBox.Show("Suppression réussie", "Succès");
                    lesAbonnementsRevue = controle.GetCommandes(abonnement.IdRevue, TYPEREVUE).ConvertAll(x => (Abonnement)x);
                    RemplirAbonnementsRevueListe(lesAbonnementsRevue);
                }
                else
                {
                    MessageBox.Show("Suppression échouée", "Echec");
                }
            }
        }

        /// <summary>
        /// Accepter uniquement la saisie d'un entier ou décimal dans le champs du montant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbMontantAbonnement_KeyPress(object sender, KeyPressEventArgs e)
        {
            SaisieMontant(sender, e);
        }

        /// <summary>
        /// événement clic sur le bouton "valider l'abonnement"
        /// créer un abonnement dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValiderAbonnement_Click(object sender, EventArgs e)
        {
            if (cbxSelectRevueNouvelAbonnement.SelectedIndex >= 0 && txbMontantAbonnement.Text != "")
            {
                Abonnement abonnement = new Abonnement(controle.GetLastIdCommande() + 1, dtpDateCommandeAbonnement.Value,
                    double.Parse(txbMontantAbonnement.Text), dtpDateFinAbonnement.Value, cbxSelectRevueNouvelAbonnement.SelectedValue.ToString());
                if (controle.CreerAbonnement(abonnement))
                {
                    MessageBox.Show("Abonnement enregistré", "Succès");
                    lesAbonnementsRevue = controle.GetCommandes(abonnement.IdRevue, TYPEREVUE).ConvertAll(x => (Abonnement)x);
                    RemplirAbonnementsRevueListe(lesAbonnementsRevue);
                    ViderInfosNouvelAbonnement();
                }
                else
                {
                    MessageBox.Show("Abonnement non enregistré", "Echec");

                }
            }
            else
            {
                MessageBox.Show("Merci de remplir tous les champs", "Information");
            }
        }

        /// <summary>
        /// vider la zone de saisie du nouvel abonnement
        /// </summary>
        private void ViderInfosNouvelAbonnement()
        {
            dtpDateCommandeAbonnement.Value = DateTime.Now;
            dtpDateFinAbonnement.Value = DateTime.Now;
            txbMontantAbonnement.Text = "";
        }

        #endregion


        #region CommandesLivres

        //-----------------------------------------------------------
        // ONGLET "Commandes des Livres"
        //-----------------------------------------------------------

        /// <summary>
        /// Ouverture de l'onglet Commande de livres : 
        /// remplir le combobox des ids des livres 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCommandeLivre_Enter(object sender, EventArgs e)
        {
            // remplir les deux combobox pour sélectionner l'id d'un livre         
            lesSelectLivres = lesLivres.OrderBy(x => x.Id).ToList();
            RemplirComboLivreId(lesSelectLivres, bdgSelectLivre, cbxSelectLivre);
            RemplirComboLivreId(lesSelectLivres, bdgSelectLivre, cbxSelectLivreCommande);
            ViderSelectLivreInformations();
        }

        /// <summary>
        /// remplir des combobox pour sélectionner un livre par id
        /// </summary>
        /// <param name="lesLivres"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        private void RemplirComboLivreId(List<Livre> lesLivres, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesLivres;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// remplir les informations détaillées et les commandes d'un livre sélectionnné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSelectLivre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxSelectLivre.SelectedIndex >= 0)
            {
                Livre livre = (Livre)cbxSelectLivre.SelectedItem;
                lesLivreCommandes = controle.GetCommandes(livre.Id, TYPELIVRE).ConvertAll(x => (CommandeDocument)x);
                AfficherSelectLivreInformations(livre);
                RemplirLivreCommandesListe(lesLivreCommandes);
            }
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirLivreCommandesListe(List<CommandeDocument> commandes)
        {
            bdgLivreCommandesListe.DataSource = commandes;
            dgvLivreCommandesListe.DataSource = bdgLivreCommandesListe;

            dgvLivreCommandesListe.Columns["idLivreDvd"].Visible = false;
            dgvLivreCommandesListe.Columns["idSuivi"].Visible = false;
            dgvLivreCommandesListe.Columns["id"].Visible = false;
            dgvLivreCommandesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivreCommandesListe.Columns["dateCommande"].DisplayIndex = 0;
            dgvLivreCommandesListe.Columns["montant"].DisplayIndex = 1;

            AccesModifCommandeLivre(false);
            cbxSuiviCommandeLivre.SelectedIndex = -1;
            dgvLivreCommandesListe.ClearSelection();
        }

        /// <summary>
        /// permettre le mise à jour de suivi et la suppression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivreCommandesListe_SelectionChanged(object sender, EventArgs e)
        {
            cbxSuiviCommandeLivre.Items.Clear();
            if (dgvLivreCommandesListe.CurrentCell != null)
            {
                CommandeDocument commandeLivre = (CommandeDocument)bdgLivreCommandesListe.List[bdgLivreCommandesListe.Position];
                int idsuivi = commandeLivre.IdSuivi;
                // remplir le cbx pour mettre à jour le suivi
                for (int i = (idsuivi - 1); i < 4; i++)
                {
                    cbxSuiviCommandeLivre.Items.Add(lesSuivis[i]);
                }
                cbxSuiviCommandeLivre.SelectedIndex = 0;
                // si la commande n'est pas réglée, possible de modifier le stade de suivi 
                if (idsuivi < 4)
                {
                    AccesModifCommandeLivre(true);
                    // impossible de supprimer une commande livrée
                    if (idsuivi >= 3)
                    {
                        btnSupprCommandeLivre.Enabled = false;
                    }
                }
                else
                {
                    AccesModifCommandeLivre(false);
                }
            }
        }

        /// <summary>
        /// Tri sur les colonnes de la liste des commandes d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivreCommandesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvLivreCommandesListe.Columns[e.ColumnIndex].HeaderText;
            List<CommandeDocument> sortedList = new List<CommandeDocument>();
            switch (titreColonne)
            {
                case "DateCommande":
                    sortedList = lesLivreCommandes.OrderBy(o => o.DateCommande).Reverse().ToList();
                    break;
                case "Montant":
                    sortedList = lesLivreCommandes.OrderBy(o => o.Montant).ToList();
                    break;
                case "NbExemplaires":
                    sortedList = lesLivreCommandes.OrderBy(o => o.NbExemplaires).ToList();
                    break;
                case "EtapeSuivi":
                    sortedList = lesLivreCommandes.OrderBy(o => o.IdSuivi).ToList();
                    break;
            }
            RemplirLivreCommandesListe(sortedList);
        }

        /// <summary>
        /// événement clic sur le bouton "mettre à jour le suivi" dans l'onglet "commande de livres"
        /// Mettre à jour l'étape de suivi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMajSuiviCommandeLivre_Click(object sender, EventArgs e)
        {
            int indexCommande = bdgLivreCommandesListe.Position;
            CommandeDocument commandeLivre = (CommandeDocument)bdgLivreCommandesListe.List[indexCommande];
            Suivi newSuivi = (Suivi)cbxSuiviCommandeLivre.SelectedItem;
            if (controle.UpdateSuiviCommandeDocument(commandeLivre.Id, newSuivi.Id))
            {
                MessageBox.Show("L'étape de suivi est mis à jour", "Succès");
                lesLivreCommandes[indexCommande].IdSuivi = newSuivi.Id;
                lesLivreCommandes[indexCommande].EtapeSuivi = newSuivi.Nom;
                RemplirLivreCommandesListe(lesLivreCommandes);
                dgvLivreCommandesListe.Rows[indexCommande].Selected = true;
            }
            else
            {
                MessageBox.Show("L'étape de suivi n'est pas mise à jour !", "Echec");
            }
        }

        /// <summary>
        /// supprimer une commande pas encore livrée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprCommandeLivre_Click(object sender, EventArgs e)
        {
            CommandeDocument commandeLivre = (CommandeDocument)bdgLivreCommandesListe.List[bdgLivreCommandesListe.Position];
            if (MessageBox.Show("Etes-vous sûr de supprimer cette commande ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (controle.SupprCommandeDocument(commandeLivre.Id))
                {
                    MessageBox.Show("Suppression réussie", "Succès");
                    lesLivreCommandes = controle.GetCommandes(commandeLivre.IdLivreDvd, TYPELIVRE).ConvertAll(x => (CommandeDocument)x);
                    RemplirLivreCommandesListe(lesLivreCommandes);
                }
                else
                {
                    MessageBox.Show("Suppression échouée", "Echec");
                }
            }
        }

        /// <summary>
        /// événement clic sur le bouton "enregistrer"
        /// créer la commande dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnregLivreCommande_Click(object sender, EventArgs e)
        {
            // vérifier que tous les champs sont remplis
            if (cbxSelectLivreCommande.SelectedIndex >= 0 && txbMontantCommandeLivre.Text != "")
            {
                double montant = Convert.ToDouble(txbMontantCommandeLivre.Text);
                int idCommande = controle.GetLastIdCommande() + 1;
                string idLivre = cbxSelectLivreCommande.SelectedValue.ToString();
                CommandeDocument commandeDocument = new CommandeDocument(idCommande, dtpDateCommandeLivre.Value, montant,
                   (int)nudNbExemplairesCommandeLivre.Value, idLivre, 1, "en cours");

                if (controle.CreerCommandeDocument(commandeDocument))
                {
                    MessageBox.Show("Nouvelle commande créée !", "Succès");
                    lesLivreCommandes = controle.GetCommandes(idLivre, TYPELIVRE).ConvertAll(x => (CommandeDocument)x);
                    RemplirLivreCommandesListe(lesLivreCommandes);
                    bdgLivreCommandesListe.Position = 0;
                    ViderInfosLivreCommande();
                }
                else
                {
                    MessageBox.Show("Nouvelle commande pas créée !", "Echec");
                }
            }
            else
            {
                MessageBox.Show("Merci de remplir tous les champs", "Information");
            }
        }

        /// <summary>
        /// Accepter uniquement la saisie d'un entier ou décimal dans le champs du montant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbMontantCommandeLivre_KeyPress(object sender, KeyPressEventArgs e)
        {
            SaisieMontant(sender, e);
        }

        /// <summary>
        /// activer ou désactiver les boutons de modification ou suppression d'une commande livre
        /// </summary>
        /// <param name="acces"></param>
        private void AccesModifCommandeLivre(bool acces)
        {
            cbxSuiviCommandeLivre.Enabled = acces;
            btnMajSuiviCommandeLivre.Enabled = acces;
            btnSupprCommandeLivre.Enabled = acces;
        }

        /// <summary>
        /// remplir les informations détaillées du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void AfficherSelectLivreInformations(Livre livre)
        {
            txbClIsbn.Text = livre.Isbn;
            txbClTitre.Text = livre.Titre;
            txbClAuteur.Text = livre.Auteur;
            txbClCollection.Text = livre.Collection;
            txbClGenre.Text = livre.Genre;
            txbClPublic.Text = livre.Public;
            txbClRayon.Text = livre.Rayon;
            txbClImage.Text = livre.Image;
        }

        /// <summary>
        /// remplir les informations détaillées du livre sélectionné
        /// </summary>
        private void ViderSelectLivreInformations()
        {
            txbClIsbn.Text = "";
            txbClTitre.Text = "";
            txbClAuteur.Text = "";
            txbClCollection.Text = "";
            txbClGenre.Text = "";
            txbClPublic.Text = "";
            txbClRayon.Text = "";
            txbClImage.Text = "";
        }

        /// <summary>
        /// vider la zone de saisie d'une nouvelle commande
        /// </summary>
        private void ViderInfosLivreCommande()
        {
            nudNbExemplairesCommandeLivre.Value = 1;
            dtpDateCommandeLivre.Value = DateTime.Now;
            txbMontantCommandeLivre.Text = "";
        }

        #endregion

    }
}
