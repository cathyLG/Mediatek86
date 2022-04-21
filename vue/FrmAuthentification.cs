using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mediatek86.controleur;

namespace Mediatek86.vue
{
    /// <summary>
    /// Fenêtre d'authentification (seuls l'administrateur, les employés de service administratif et de service prêts peuvent accéder à l'application)
    /// </summary>
    public partial class FrmAuthentification : Form
    {
        /// <summary>
        /// instance du controleur
        /// </summary>
        private readonly Controle controle;

        public FrmAuthentification(Controle controle)
        {
            InitializeComponent();
            this.controle = controle;
        }

        /// <summary>
        /// Demande au controleur de controler l'authentification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!txbNom.Text.Equals("") && !txbPrenom.Equals("") && !txbMdp.Text.Equals(""))
            {
                int idService = controle.ControleAuthentification(txbNom.Text, txbPrenom.Text, txbMdp.Text);
                if (idService == 0)
                {
                    MessageBox.Show("Merci de vérifier votre nom/prénom/mot de passe", "Authentification incorrecte");                    
                    txbMdp.Text = "";
                    txbNom.Focus();
                }
                else if (idService == 1)
                {
                    MessageBox.Show("Vous n'avez pas l'accès à l'application", "Alerte");
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("Tous les champs doivent être remplis.", "Information");
            }
        }      

    }
}
