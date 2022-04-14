using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Mediatek86.metier;
using Mediatek86.controleur;
using System.Drawing;
using System.Linq;

namespace Mediatek86.vue
{
    public partial class FrmMediatek : Form
    {

        #region Variables globales

        private readonly Controle controle;
        const string ETATNEUF = "00001";
        const string TYPELIVRE = "livre";

        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private readonly BindingSource bdgLivreCommandesListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Revue> lesRevues = new List<Revue>();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        private List<Categorie> lesGenres;
        private List<Categorie> lesPublics;
        private List<Categorie> lesRayons;
        private List<Suivi> lesSuivis;
        private List<CommandeDocument> lesLivreCommandes;
        private readonly BindingSource bdgInfoGenres = new BindingSource();
        private readonly BindingSource bdgInfoPublics = new BindingSource();
        private readonly BindingSource bdgInfoRayons = new BindingSource();
        private bool livreEnModif = false;
        private bool dvdEnModif = false;
        private bool revueEnModif = false;
        #endregion


        internal FrmMediatek(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
            lesGenres = this.controle.GetAllGenres();
            lesPublics = this.controle.GetAllPublics();
            lesRayons = this.controle.GetAllRayons();
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
            OnOffEcritureRevue(false);

            OnOffEcritureRevue(false);
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
            dgvRevuesListe.ClearSelection();
            VideRevuesInfos();
            VideRevuesZones();
            OnOffEcritureRevue(true);
            grpRevuesInfos.Text = "Nouvelle revue";
            // calculer l'id de la nouvelle revue, attention : l'id des revues commence par 1
            txbRevuesNumero.Text = (int.Parse(lesRevues.OrderBy(o => o.Id).Last().Id) + 1).ToString();
        }


        /// <summary>
        /// événement click sur le bouton "valider"
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
                        lesRevues.Add(revue);
                        lesRevues = lesRevues.OrderBy(x => x.Titre).ToList();
                        RemplirRevuesListeComplete();
                        bdgRevuesListe.Position = lesRevues.FindIndex(x => x.Id.Equals(revue.Id));
                        //AfficheRevuesInfos(revue);
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
                        if (index != -1)
                        {
                            lesRevues[index] = revue;
                            RemplirRevuesListeComplete();
                            bdgRevuesListe.Position = index;
                        }
                        //AfficheRevuesInfos(revue);                        
                    }
                    else
                    {
                        MessageBox.Show("La revue n°" + revue.Id + "n'est pas modifiée !", "Echec");
                    }
                }

            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires");
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
            OnOffEcritureRevue(true);
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
            List<Exemplaire> lesExemplaires = controle.GetExemplairesRevue(revue.Id);
            List<Commande> lesCommandes = controle.GetCommandes(revue.Id, "revue");
            Console.WriteLine(lesExemplaires.Count);
            Console.WriteLine(lesCommandes.Count);
            if (lesExemplaires.Count == 0 && lesCommandes.Count == 0)
            {
                // demander la confirmation de suppression
                if (MessageBox.Show("Etes-vous sûr de supprimer la revue n°" + revue.Id + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controle.SupprDocument(revue))
                    {
                        MessageBox.Show("La revue n°" + revue.Id + " est supprimée !", "Succès");
                        lesRevues.Remove(revue);
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
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
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
        /// <param name="acti"></param>
        private void OnOffEcritureRevue(bool acti)
        {
            chkRevuesEmpruntable.Enabled = acti;
            txbRevuesTitre.ReadOnly = !acti;
            txbRevuesPeriodicite.ReadOnly = !acti;
            nudRevuesDelai.ReadOnly = !acti;
            cbxRevuesInfoGenres.Enabled = acti;
            cbxRevuesInfoPublics.Enabled = acti;
            cbxRevuesInfoRayons.Enabled = acti;
            txbRevuesImage.ReadOnly = !acti;
            btnRevueValider.Enabled = acti;
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
                    btnRevueModif.Enabled = true;
                    btnRevueSuppr.Enabled = true;
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
                btnRevueModif.Enabled = false;
                btnRevueSuppr.Enabled = false;
            }
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
            bdgRevuesListe.Position = 0;
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
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
            VideRevuesZones();
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
            dgvLivresListe.ClearSelection();
            VideLivresInfos();
            VideLivresZones();
            OnOffEcritureLivres(true);
            grpLivresInfos.Text = "Nouveau livre";
            // calculer l'id du nouveau livre, attention : l'id des livres commence par 0
            string lastId = lesLivres.OrderBy(o => o.Id).ToList().Last().Id;
            string newId = (int.Parse(lastId) + 1).ToString();
            newId = new string('0', lastId.Length - newId.Length) + newId;
            txbLivresNumero.Text = newId;
        }

        private void btnLivreModif_Click(object sender, EventArgs e)
        {
            livreEnModif = true;
            OnOffEcritureLivres(true);
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
            List<Exemplaire> lesExemplaires = controle.GetExemplairesRevue(livre.Id);
            List<Commande> lesCommandes = controle.GetCommandes(livre.Id, TYPELIVRE);
            Console.WriteLine(lesExemplaires.Count);
            Console.WriteLine(lesCommandes.Count);
            if (lesExemplaires.Count == 0 && lesCommandes.Count == 0)
            {
                // demander la confirmation de suppression
                if (MessageBox.Show("Etes-vous sûr de supprimer le livre n°" + livre.Id + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controle.SupprDocument(livre))
                    {
                        MessageBox.Show("Le livre n°" + livre.Id + " est supprimé !", "Succès");
                        lesLivres.Remove(livre);
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
        /// valider les informations saisie sur le nouveau livre, et les transmettre dans la bdd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivreValider_Click(object sender, EventArgs e)
        {
            // les champs 'numéro', 'genre', 'public' et 'rayon' sont obligatoires
            if (!txbLivresNumero.Equals("") && cbxLivresInfoGenres.SelectedIndex >= 0 && cbxLivresInfoPublics.SelectedIndex >= 0 && cbxLivresInfoRayons.SelectedIndex >= 0)
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
                        lesLivres.Add(livre);
                        lesLivres = lesLivres.OrderBy(x => x.Titre).ToList();
                        RemplirLivresListeComplete();
                        bdgLivresListe.Position = lesLivres.FindIndex(x => x.Id.Equals(livre.Id));

                        //AfficheLivresInfos(livre);
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
                        if (index != -1)
                        {
                            lesLivres[index] = livre;
                            RemplirLivresListeComplete();
                            bdgLivresListe.Position = index;
                        }
                        AfficheLivresInfos(livre);
                    }
                    else
                    {
                        MessageBox.Show("Le livre n°" + livre.Id + "n'est pas modifié !", "Echec");
                    }

                }

            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires");
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
            OnOffEcritureLivres(false);

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
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
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
                    btnLivreModif.Enabled = true;
                    btnLivreSuppr.Enabled = true;
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
                btnLivreModif.Enabled = false;
                btnLivreSuppr.Enabled = false;
            }
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
            bdgLivresListe.Position = 0;
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
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
            VideLivresZones();
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
        /// activer ou désactiver la saisie dans les champs (sauf id) des informations détaillées
        /// </summary>
        /// <param name="acti"></param>
        private void OnOffEcritureLivres(bool acti)
        {
            txbLivresIsbn.ReadOnly = !acti;
            txbLivresTitre.ReadOnly = !acti;
            txbLivresAuteur.ReadOnly = !acti;
            txbLivresCollection.ReadOnly = !acti;
            txbLivresImage.ReadOnly = !acti;
            cbxLivresInfoGenres.Enabled = acti;
            cbxLivresInfoPublics.Enabled = acti;
            cbxLivresInfoRayons.Enabled = acti;
            btnLivreValider.Enabled = acti;
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
            dgvDvdListe.ClearSelection();
            VideDvdInfos();
            VideDvdZones();
            OnOffEcritureDvd(true);
            grpDvdInfos.Text = "Nouveau Dvd";
            // calculer l'id du nouveau Dvd, attention : l'id des livres commence par 2
            txbDvdNumero.Text = (int.Parse(lesDvd.OrderBy(o => o.Id).ToList().Last().Id) + 1).ToString();
        }


        /// <summary>
        /// événement clic sur le bouton "modifier" dans l'onglet dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdModif_Click(object sender, EventArgs e)
        {
            dvdEnModif = true;
            OnOffEcritureDvd(true);
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
            List<Exemplaire> lesExemplaires = controle.GetExemplairesRevue(dvd.Id);
            List<Commande> lesCommandes = controle.GetCommandes(dvd.Id, "dvd");
            Console.WriteLine(lesExemplaires.Count);
            Console.WriteLine(lesCommandes.Count);
            if (lesExemplaires.Count == 0 && lesCommandes.Count == 0)
            {
                // demander la confirmation de suppression
                if (MessageBox.Show("Etes-vous sûr de supprimer le dvd n°" + dvd.Id + " ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (controle.SupprDocument(dvd))
                    {
                        MessageBox.Show("Le dvd n°" + dvd.Id + " est supprimé !", "Succès");
                        lesDvd.Remove(dvd);
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

            // les champs 'genre', 'public' et 'rayon' sont obligatoires
            if (cbxDvdInfoGenres.SelectedIndex >= 0 && cbxDvdInfoPublics.SelectedIndex >= 0 && cbxDvdInfoRayons.SelectedIndex >= 0)
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
                        lesDvd.Add(dvd);
                        lesDvd = lesDvd.OrderBy(x => x.Titre).ToList();
                        RemplirDvdListeComplete();
                        bdgDvdListe.Position = lesDvd.FindIndex(x => x.Id.Equals(dvd.Id));
                        //AfficheDvdInfos(dvd);
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
                        if (index != -1)
                        {
                            lesDvd[index] = dvd;
                            RemplirDvdListeComplete();
                            bdgDvdListe.Position = index;
                        }
                        // AfficheDvdInfos(dvd);
                    }
                    else
                    {
                        MessageBox.Show("Le dvd n°" + dvd.Id + "n'est pas modifié !", "Echec");
                    }
                }

            }
            else
            {
                MessageBox.Show("Merci de remplir les champs obligatoires");
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
            OnOffEcritureDvd(false);

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
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
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
        /// <param name="acti"></param>
        private void OnOffEcritureDvd(bool acti)
        {
            nudDvdDuree.ReadOnly = !acti;
            txbDvdTitre.ReadOnly = !acti;
            txbDvdRealisateur.ReadOnly = !acti;
            txbDvdSynopsis.ReadOnly = !acti;
            cbxDvdInfoGenres.Enabled = acti;
            cbxDvdInfoPublics.Enabled = acti;
            cbxDvdInfoRayons.Enabled = acti;
            txbDvdImage.ReadOnly = !acti;
            btnDvdValider.Enabled = acti;
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
                    btnDvdModif.Enabled = true;
                    btnDvdSuppr.Enabled = true;
                }
                catch
                {
                    VideDvdZones();
                }
            }
            else
            {
                VideDvdInfos();
                btnDvdModif.Enabled = false;
                btnDvdSuppr.Enabled = false;
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
            bdgDvdListe.Position = 0;
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
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
            VideDvdZones();
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
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            bdgExemplairesListe.DataSource = exemplaires;
            dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
            dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
            dgvReceptionExemplairesListe.Columns["idDocument"].Visible = false;
            dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
            dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
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
                    VideReceptionRevueInfos();
                }
            }
            else
            {
                VideReceptionRevueInfos();
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
            accesReceptionExemplaireGroupBox(false);
            VideReceptionRevueInfos();
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
            afficheReceptionExemplairesRevue();
            // accès à la zone d'ajout d'un exemplaire
            accesReceptionExemplaireGroupBox(true);
        }

        private void afficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = controle.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
        }

        /// <summary>
        /// Vide les zones d'affchage des informations de la revue
        /// </summary>
        private void VideReceptionRevueInfos()
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
            lesExemplaires = new List<Exemplaire>();
            RemplirReceptionExemplairesListe(lesExemplaires);
            accesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de l'exemplaire
        /// </summary>
        private void VideReceptionExemplaireInfos()
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
        private void accesReceptionExemplaireGroupBox(bool acces)
        {
            VideReceptionExemplaireInfos();
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
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (controle.CreerExemplaire(exemplaire))
                    {
                        VideReceptionExemplaireInfos();
                        afficheReceptionExemplairesRevue();
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
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
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
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
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





        #endregion

        #region CommandeLivres

        //-----------------------------------------------------------
        // ONGLET "Commande de Livres"
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
            List<string> idLivres = new List<string>();
            foreach (Livre livre in lesLivres.OrderBy(o => o.Id))
            {
                idLivres.Add(livre.Id);
            }
            BindingSource bdgSelectLivre = new BindingSource();
            BindingSource bdgSelectLivreCommande = new BindingSource();

            remplirComboLivreId(idLivres, bdgSelectLivre, cbxSelectLivre);
            remplirComboLivreId(idLivres, bdgSelectLivreCommande, cbxSelectLivreCommande);
            // récupérer la liste des suivis
            lesSuivis = controle.GetAllSuivis();
            RemplirSelectLivreInformations(null);
            
        }

        /// <summary>
        /// remplir des combobox pour sélectionner un livre par id
        /// </summary>
        /// <param name="livresId"></param>
        /// <param name="bdg"></param>
        /// <param name="cbx"></param>
        private void remplirComboLivreId(List<string> livresId, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = livresId;
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
                Livre livre = lesLivres.Find(x => x.Id.Equals(cbxSelectLivre.SelectedItem.ToString()));
                lesLivreCommandes = controle.GetCommandes(livre.Id, TYPELIVRE).ConvertAll(c => (CommandeDocument)c);
                RemplirSelectLivreInformations(livre);
                RemplirLivreCommandesListe();
            }
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        private void RemplirLivreCommandesListe()
        {
            bdgLivreCommandesListe.DataSource = lesLivreCommandes;
            dgvLivreCommandesListe.DataSource = bdgLivreCommandesListe;
            
            dgvLivreCommandesListe.Columns["idLivreDvd"].Visible = false;
            dgvLivreCommandesListe.Columns["idSuivi"].Visible = false;
            dgvLivreCommandesListe.Columns["id"].Visible = false;
            dgvLivreCommandesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivreCommandesListe.Columns["dateCommande"].DisplayIndex = 0;
            dgvLivreCommandesListe.Columns["montant"].DisplayIndex = 1;
            dgvLivreCommandesListe.ClearSelection();
        }

        private void dgvLivreCommandesListe_SelectionChanged(object sender, EventArgs e)
        {
            cbxMajSuiviCommandeLivre.Items.Clear();
            if (dgvLivreCommandesListe.CurrentCell != null)
            {
                CommandeDocument commandeLivre = (CommandeDocument)bdgLivreCommandesListe.List[bdgLivreCommandesListe.Position];
                int idsuivi = int.Parse(commandeLivre.IdSuivi);
                // si la commande n'est pas réglée, possible de modifier le stade de suivi ou supprimer
                if (idsuivi < lesSuivis.Count)
                {
                    OnOffMajCommandeLivre(true);
                    // remplir le cbx pour mettre à jour le suivi
                    for (int i = (idsuivi - 1); i < lesSuivis.Count; i++)
                    {
                        cbxMajSuiviCommandeLivre.Items.Add(lesSuivis[i].Nom);
                    }
                    cbxMajSuiviCommandeLivre.SelectedIndex = 0;
                }
                else
                {
                    OnOffMajCommandeLivre(false);
                }
            }
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
            string newEtapeSuivi = cbxMajSuiviCommandeLivre.SelectedItem.ToString();
            string newIdSuivi = lesSuivis.Find(x => x.Nom.Equals(newEtapeSuivi)).Id;
            if (controle.UpdateSuiviCommandeDocument(commandeLivre.Id, newIdSuivi))
            {
                MessageBox.Show("L'étape de suivi est mis à jour", "Succès");
                lesLivreCommandes[indexCommande].IdSuivi = newIdSuivi;
                lesLivreCommandes[indexCommande].EtapeSuivi = newEtapeSuivi;
                RemplirLivreCommandesListe();
                dgvLivreCommandesListe.Rows[indexCommande].Selected = true;
            }
            else
            {
                MessageBox.Show("L'étape de suivi n'est pas mise à jour !", "Echec");
            }

        }

        /// <summary>
        /// activer ou désactiver les boutons de modification ou suppression d'une commande livre
        /// </summary>
        /// <param name="acti"></param>
        private void OnOffMajCommandeLivre(bool acti)
        {
            cbxMajSuiviCommandeLivre.Enabled = acti;
            btnMajSuiviCommandeLivre.Enabled = acti;
            btnSupprCommandeLivre.Enabled = acti;
        }

        /// <summary>
        /// remplir les informations détaillées du livre sélectionné
        /// </summary>
        /// <param name="livre"></param>
        private void RemplirSelectLivreInformations(Livre livre)
        {
            if (livre != null)
            {
                txbClNumero.Text = livre.Id;
                txbClIsbn.Text = livre.Isbn;
                txbClTitre.Text = livre.Titre;
                txbClAuteur.Text = livre.Auteur;
                txbClCollection.Text = livre.Collection;
                txbClGenre.Text = livre.Genre;
                txbClPublic.Text = livre.Public;
                txbClRayon.Text = livre.Rayon;
                txbClImage.Text = livre.Image;
            }
            else
            {
                txbClNumero.Text = "";
                txbClIsbn.Text = "";
                txbClTitre.Text = "";
                txbClAuteur.Text = "";
                txbClCollection.Text = "";
                txbClGenre.Text = "";
                txbClPublic.Text = "";
                txbClRayon.Text = "";
                txbClImage.Text = "";
            }
            
        }

        #endregion


    }
}
