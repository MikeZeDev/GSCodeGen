using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GSCodegen.GUI;
using System.Collections.ObjectModel;

namespace GSCodegen
{
    public partial class Form1 : Form
    {
        Collection<GSItem> LesItems = new Collection<GSItem>();
        Collection<GSPerso> LesPersos = new Collection<GSPerso>();

 

/*************************************************************/
#region GUI
        
        public Form1()
        {
            InitializeComponent();

            GSI_Loaditems();
            UpdateCombo();

            GSI_LoadPersos();
            UpdatePersoCombo();


        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                ClearCodes();
                GSI_Loaditems();
                UpdateCombo();
                GSI_LoadPersos();
                UpdatePersoCombo();
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                ClearCodes();
                GSII_Loaditems();
                UpdateCombo();
                GSII_LoadPersos();
                UpdatePersoCombo();
            
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClearCodes();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (CHK_DESC.Checked)
                            TXT_CODES.Text += GenItemCodeText()+Environment.NewLine;
            TXT_CODES.Text += GenItemCode() + Environment.NewLine + Environment.NewLine;
           
        }
        private void button4_Click(object sender, EventArgs e)
          {
            ToClipboard();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (CHK_DESC.Checked)
                TXT_CODES.Text += GenEXPCodeText() + Environment.NewLine;
            TXT_CODES.Text += GenEXPCode() + Environment.NewLine + Environment.NewLine;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (CHK_DESC.Checked)
                TXT_CODES.Text += GenHPCodeText() + Environment.NewLine;
            TXT_CODES.Text += GenHPCode() + Environment.NewLine + Environment.NewLine;
            }

        public void UpdatePersoCombo()
        {
            CB_PERSO.Items.Clear();
            CB_PERSO2.Items.Clear();
            for (int i = 0; i < LesPersos.Count; i++)
            {
                GSPerso Perso = LesPersos[i];
                CB_PERSO.Items.Add(new ImageItem(Perso, Perso._ImageIndex));
                CB_PERSO2.Items.Add(new ImageItem(Perso, Perso._ImageIndex));
            }
            CB_PERSO.SelectedIndex = 0;
            CB_PERSO2.SelectedIndex = 0;

        }
        public void UpdateCombo()
        {
            CB_ITEMS.Items.Clear();
            for (int i = 0; i < LesItems.Count; i++)
            {
                GSItem Item  = LesItems[i];
                CB_ITEMS.Items.Add(new ImageItem(Item, Item._ImageIndex));
            }
            CB_ITEMS.SelectedIndex = 0;
        }
        public void ClearCodes()
        {
            TXT_CODES.Clear();
        }
        public void ToClipboard()
        {
            TXT_CODES.SelectAll();
            TXT_CODES.Copy();
            TXT_CODES.SelectionLength = 0;
        }


#endregion


/**************************************************************/
#region Procédures de génération des codes

        /// <summary>
        /// Crée le text du code d'objet
        /// </summary>
        /// <returns>Description du code d'objet</returns>
        public String GenItemCodeText()
        {
            String resultat;
            GSItem Objet = LesItems[CB_ITEMS.SelectedIndex];
            GSPerso Perso = LesPersos[CB_PERSO.SelectedIndex];

            if (radioButton1.Checked)
                resultat = "GS1:";
            else resultat = "GS2:";
            resultat += "Objet n° " + CB_EMPLACEMENT.Value.ToString() + " de " + Perso._Nom;
            resultat += " = " + CB_QUANTITE.Value.ToString() + " " + Objet._Nom;  

            return resultat;

        }


        /// <summary>
        /// Crée le code d'objet lui-même
        /// </summary>
        /// <returns>Code Action Replay sous forme XXXXXXXX:YYZZ</returns>
        public String GenItemCode()
        {
            String resultat;
            GSItem Objet = LesItems[CB_ITEMS.SelectedIndex];
            GSPerso Perso = LesPersos[CB_PERSO.SelectedIndex];
            
            //addresse de l'item
            int emplacement = (int) CB_EMPLACEMENT.Value - 1;
            uint adresse = Perso._ADDR + GSPerso.OFF2ITEMS + (uint)(emplacement * 2);
            resultat = Utilities.toHex(adresse)+ ':';

            //byte de quantité
            byte quantite = (byte)CB_QUANTITE.Value;
            String QT = Utilities.toHex(Objet.RealQTtoGameQT(quantite));

            if (Perso.NoReverse)
                        resultat += Utilities.toHex(Objet._Code) + QT;
                    else resultat += QT + Utilities.toHex(Objet._Code);
            return resultat;

        }

        public String GenEXPCodeText()
        {
            GSPerso Perso = LesPersos[CB_PERSO2.SelectedIndex];
            return "Exp MAX pour " + Perso._Nom;


        }

        public String GenEXPCode()
        {
            String resultat;
            GSPerso Perso = LesPersos[CB_PERSO2.SelectedIndex];

            uint adresse = Perso._ADDR + GSPerso.OFF2EXP;
            uint exp = 0x00895440;
            resultat = Utilities.toHex(adresse) + ':' + Utilities.toHex(exp);
            return resultat;

        }

        public String GenHPCodeText()
        {
            GSPerso Perso = LesPersos[CB_PERSO2.SelectedIndex];
            return "Hp infinis pour " + Perso._Nom;
        }

        public String GenHPCode()
        {
            String resultat;
            GSPerso Perso = LesPersos[CB_PERSO2.SelectedIndex];

            uint adresse = Perso._ADDR + GSPerso.OFF2HP;
            UInt16 hp = 0x270f;
            resultat = Utilities.toHex(adresse) + ':' + Utilities.toHex(hp);
            return resultat;

        }




        #endregion



/*************************************************************/
    
#region Initialisation des Personnages

        /// <summary>
        /// Charge les persos de GS1
        /// </summary>
        public void GSI_LoadPersos()
        {
            //Adresses de GS1 !!
            LesPersos.Clear();
            LesPersos.Add(new GSPerso("Vlad", 0x2000500, 0));
            LesPersos.Add(new GSPerso("Garet", 0x0200064C,1));
            LesPersos.Add(new GSPerso("Ivan",  0x02000798,2));
            LesPersos.Add(new GSPerso("Sofia", 0x020008E4, 3));
        }

        /// <summary>
        /// Charge les persos de GSII
        /// </summary>
        public void GSII_LoadPersos()
        {
            //Adresses de GS2 !!
            LesPersos.Clear();
            LesPersos.Add(new GSPerso("Vlad",  0x02000520, 0));
            LesPersos.Add(new GSPerso("Garet",  0x0200066C, 1));
            LesPersos.Add(new GSPerso("Ivan", 0x020007B8,2));
            LesPersos.Add(new GSPerso("Sofia",0x02000904,3));
            LesPersos.Add(new GSPerso("Pavel",0x02000A50,4));
            LesPersos.Add(new GSPerso("Lina", 0x02000B9C,5));
            LesPersos.Add(new GSPerso("Cylia",0x02000CE8,6));
            LesPersos.Add(new GSPerso("Piers", 0x02000E34, 7));

        }

#endregion

#region Initialisation des Items


        /// <summary>
        /// Charge les Items de Golden Sun 1
        /// </summary>
        public void GSI_Loaditems()
        {
            LesItems.Clear();
            LesItems.Add(new GSItem("Hyper Bottes",0,true));
            LesItems.Add(new GSItem("Epée Longue", 1,1));
            LesItems.Add(new GSItem("Glaive", 2, 2));
            LesItems.Add(new GSItem("Claymore",3,3));
            LesItems.Add(new GSItem("Espadon", 4,  4));
            LesItems.Add(new GSItem("Shamshir", 5,  5));
            LesItems.Add(new GSItem("Lame d'argent", 6, 6));
            LesItems.Add(new GSItem("Masamune", 7, 7));
            LesItems.Add(new GSItem("Lame artique", 8, 8));
            LesItems.Add(new GSItem("Lame Gaia", 9, 9));
            LesItems.Add(new GSItem("Lame Solaire", 0xA, 10));
            LesItems.Add(new GSItem("Muramasa", 0xB, 11));
/***********************************************************/
            LesItems.Add(new GSItem("Machette", 0xF, 12));
            LesItems.Add(new GSItem("Epée courte", 0x10, 12));
            LesItems.Add(new GSItem("Coutelas", 0x11, 13));
            LesItems.Add(new GSItem("Rapière", 0x12, 14));
            LesItems.Add(new GSItem("Rapière effilée", 0x13, 15));
            LesItems.Add(new GSItem("Ninja-to", 0x14, 16));
            LesItems.Add(new GSItem("Epée de vitesse", 0x15, 17));
            LesItems.Add(new GSItem("Rapière elfique", 0x16, 18));
            LesItems.Add(new GSItem("Lame tueuse", 0x17, 19));
            LesItems.Add(new GSItem("Lame mystère", 0x18, 20));
            LesItems.Add(new GSItem("Kikuichimonji", 0x19, 21));
            LesItems.Add(new GSItem("Kusanagi", 0x1A, 22));
            LesItems.Add(new GSItem("Cimeterre", 0x1B, 23));
/*************************************************************/
            LesItems.Add(new GSItem("Hache", 0x1F, 24));
            LesItems.Add(new GSItem("Hache large", 0x20, 25));
            LesItems.Add(new GSItem("Lochabre", 0x21, 26));
            LesItems.Add(new GSItem("Draco-hache", 0x22, 27));
            LesItems.Add(new GSItem("Hache de géant", 0x23, 28));
            LesItems.Add(new GSItem("Hache Aspoc", 0x24, 29));
            LesItems.Add(new GSItem("Hache de feu", 0x25, 30));
            LesItems.Add(new GSItem("Hache d'Achéron", 0x26, 31));
/******************************************************************/
            LesItems.Add(new GSItem("Masse", 0x2B, 32));
            LesItems.Add(new GSItem("Masse", 0x2C, 33));
            LesItems.Add(new GSItem("Bec de corbin", 0x2D, 34));
            LesItems.Add(new GSItem("Masse cloutée", 0x2E, 35));
            LesItems.Add(new GSItem("Masse bénie", 0x2F, 36));
            LesItems.Add(new GSItem("Masse gravité", 0x30, 37));
            LesItems.Add(new GSItem("Masse sacrée", 0x31, 38));
            LesItems.Add(new GSItem("Masse du chaos", 0x32, 39));
/******************************************************************/
            LesItems.Add(new GSItem("Bâton", 0x37, 40));
            LesItems.Add(new GSItem("Bâguette", 0x38, 41));
            LesItems.Add(new GSItem("Bâguette bénie", 0x39, 42));
            LesItems.Add(new GSItem("Ankh béni", 0x3A, 43));
            LesItems.Add(new GSItem("Bâtonnet psy", 0x3B, 44));
            LesItems.Add(new GSItem("Baguette gel", 0x3C, 45));
            LesItems.Add(new GSItem("Ankh sacré", 0x3D, 46));
            LesItems.Add(new GSItem("Bâton abyssal", 0x3E, 47));
            LesItems.Add(new GSItem("Bâton cristal", 0x3F, 48));
            LesItems.Add(new GSItem("Bâton zodiaque", 0x40, 49));
            LesItems.Add(new GSItem("Bâton chamane", 0x41, 50));
/******************************************************************/
            LesItems.Add(new GSItem("Armure de cuir", 0x4B, 51));
            LesItems.Add(new GSItem("Armure psy", 0x4C, 52));
            LesItems.Add(new GSItem("Cotte de maille", 0x4D, 53));
            LesItems.Add(new GSItem("Cuirasse", 0x4E, 54));
            LesItems.Add(new GSItem("Plate", 0x4F, 55));
            LesItems.Add(new GSItem("Plate complète", 0x50, 56));
            LesItems.Add(new GSItem("Armure esprit", 0x51, 57));
            LesItems.Add(new GSItem("Ecaille dragon", 0x52, 58));
            LesItems.Add(new GSItem("Plate abyssale", 0x53, 59));
            LesItems.Add(new GSItem("Plate d'Ashura", 0x54, 60));
            LesItems.Add(new GSItem("Harnois", 0x55, 61));
/******************************************************************/
            LesItems.Add(new GSItem("Chemise", 0x59, 62));
            LesItems.Add(new GSItem("Gilet de voyage",0x5A, 63));
            LesItems.Add(new GSItem("Manteau", 0x5B, 64));
            LesItems.Add(new GSItem("Robe mystique", 0x5C, 65));
            LesItems.Add(new GSItem("Chemise elfique", 0x5D, 66));
            LesItems.Add(new GSItem("Gilet en argent", 0x5E, 67));
            LesItems.Add(new GSItem("Aqua-veste", 0x5F, 68));
            LesItems.Add(new GSItem("Para-tonnerre", 0x60, 69));
            LesItems.Add(new GSItem("Kimono", 0x61, 70));
            LesItems.Add(new GSItem("Shinobi shojoku", 0x62, 71));
/******************************************************************/
            LesItems.Add(new GSItem("Robe", 0x67, 72));
            LesItems.Add(new GSItem("Robe de voyage", 0x68, 73));
            LesItems.Add(new GSItem("Robe de soie", 0x69, 74));
            LesItems.Add(new GSItem("Robe chinoise", 0x6A, 75));
            LesItems.Add(new GSItem("Pourpoint", 0x6B, 76));
            LesItems.Add(new GSItem("Robe de soirée", 0x6C, 77));
            LesItems.Add(new GSItem("Robe bénie", 0x6D, 78));
            LesItems.Add(new GSItem("Soutane magique ", 0x6E, 79));
            LesItems.Add(new GSItem("Robe enchantée", 0x6F, 80));
            LesItems.Add(new GSItem("Robe à plumes", 0x70, 81));
            LesItems.Add(new GSItem("Robe de l'oracle", 0x71, 82));
/******************************************************************/
            LesItems.Add(new GSItem("Bouclier", 0x77, 83));
            LesItems.Add(new GSItem("Bouclier de fer", 0x78, 84));
            LesItems.Add(new GSItem("Ecu ", 0x79, 85));
            LesItems.Add(new GSItem("Bouclier miroir", 0x7A, 86));
            LesItems.Add(new GSItem("Draco-bouclier", 0x7B, 87));
            LesItems.Add(new GSItem("Bouclier Gaia", 0x7C, 88));
/******************************************************************/
            LesItems.Add(new GSItem("Gants", 0x7F, 89));
            LesItems.Add(new GSItem("Gants de cuir", 0x80, 90));
            LesItems.Add(new GSItem("Gantelets ", 0x81, 91));
            LesItems.Add(new GSItem("Vambrace", 0x82, 92));
            LesItems.Add(new GSItem("Gants de fer", 0x83, 93));
            LesItems.Add(new GSItem("Gants mystère", 0x84, 94));
            LesItems.Add(new GSItem("Gants cloutés", 0x85, 95));
            LesItems.Add(new GSItem("Gants vertueux", 0x86, 96));
/*******************************************************************/
            LesItems.Add(new GSItem("Bracelet cuir", 0x88, 97));
            LesItems.Add(new GSItem("Bracelet", 0x89, 98));
            LesItems.Add(new GSItem("Bracelet lourd", 0x8A, 99));
            LesItems.Add(new GSItem("Bracelet argent", 0x8B, 100));
            LesItems.Add(new GSItem("Bracelet éthéré", 0x8C, 101));
            LesItems.Add(new GSItem("Bracelet saint", 0x8D, 102));
            LesItems.Add(new GSItem("Bracelet gardien ", 0x8E, 103));
/*******************************************************************/
            LesItems.Add(new GSItem("Casque", 0x91, 104));
            LesItems.Add(new GSItem("Heaume", 0x92, 105));
            LesItems.Add(new GSItem("Chapel de fer ", 0x93, 106));
            LesItems.Add(new GSItem("Heaume d'acier ", 0x94, 107));
            LesItems.Add(new GSItem("Heaume d'argent ", 0x95, 108));
            LesItems.Add(new GSItem("Bassinet ", 0x96, 109));
            LesItems.Add(new GSItem("Casque à cornes", 0x97, 110));
            LesItems.Add(new GSItem("Heaume psy", 0x98, 111));
/******************************************************************/
            LesItems.Add(new GSItem("Chapeau en cuir", 0x9C, 112));
            LesItems.Add(new GSItem("Chapeau de bois", 0x9D, 113));
            LesItems.Add(new GSItem("Cagoule", 0x9E, 114));
            LesItems.Add(new GSItem("Couronne ornée", 0x9F, 115));
            LesItems.Add(new GSItem("Cagoule de ninja", 0xA0, 116));
            LesItems.Add(new GSItem("Chapeau-chance", 0xA1, 117));
            LesItems.Add(new GSItem("Couronne foudre", 0xA2, 118));
            LesItems.Add(new GSItem("Mitre", 0xA3, 119));
            LesItems.Add(new GSItem("Chapeau-leurre", 0xA4, 120));
/*******************************************************************/
            LesItems.Add(new GSItem("Tiare", 0xA6, 121));
            LesItems.Add(new GSItem("Tiare d'argent", 0xA7, 122));
            LesItems.Add(new GSItem("Tiare gardienne", 0xA8, 123));
            LesItems.Add(new GSItem("Tiare de platine", 0xA9, 124));
            LesItems.Add(new GSItem("Tiare mythril", 0xAA, 125));
            LesItems.Add(new GSItem("Tiare soleil", 0xAB, 126));
/********************************************************************
 *    ITEMS DE BASE 
 * ******************************************************************/
            LesItems.Add(new GSItem("Herbe", 0xB4, 127));
            LesItems.Add(new GSItem("Noisette", 0xB5, 128));
            LesItems.Add(new GSItem("Fiole", 0xB6, 129));
            LesItems.Add(new GSItem("Potion", 0xB7, 130));
            LesItems.Add(new GSItem("Eau Miracle", 0xB8, 131));
            LesItems.Add(new GSItem("Bouteille vide", 0xB9, 132));
            LesItems.Add(new GSItem("Cristal psy", 0xBA, 133));
            LesItems.Add(new GSItem("Antidote", 0xBB, 134));
            LesItems.Add(new GSItem("Elixir", 0xBC, 135));
            LesItems.Add(new GSItem("Eau jouvence", 0xBD, 136));

            LesItems.Add(new GSItem("Pain", 0xBF, 137));
            LesItems.Add(new GSItem("Cookie", 0xC0, 138));
            LesItems.Add(new GSItem("Pomme", 0xC1, 139));
            LesItems.Add(new GSItem("Noix", 0xC2, 140));
            LesItems.Add(new GSItem("Menthe", 0xC3, 141));
            LesItems.Add(new GSItem("Poivre", 0xC4, 142));
/**********************************************************************/
/*      OBJETS DE POUVOIR
 * ********************************************************************/
            LesItems.Add(new GSItem("Orbe de force", 0xC8, 143));
            LesItems.Add(new GSItem("Goutte", 0xC9, 144));
            LesItems.Add(new GSItem("Joyau de gel", 0xCA, 145));
            LesItems.Add(new GSItem("Joyau kynétique", 0xCB, 146));
            LesItems.Add(new GSItem("Gemme de stase", 0xCC, 147));
            LesItems.Add(new GSItem("Balle d'ombre", 0xCD, 148));
            LesItems.Add(new GSItem("Pierre de prise", 0xCE, 149));
            LesItems.Add(new GSItem("Perle larcin ", 0xCF, 150));
/************************************************************************/
            LesItems.Add(new GSItem("Etoile Vénus", 0xDC, 151));
            LesItems.Add(new GSItem("Etoile Mercure  ", 0xDD, 152));
            LesItems.Add(new GSItem("Etoile Mars", 0xDE, 153));
            LesItems.Add(new GSItem("Etoile Jupiter", 0xDF, 154));
            LesItems.Add(new GSItem("Sac Mythril ", 0xE0, 155));
            LesItems.Add(new GSItem("Petit joyau", 0xE1, 156));
            LesItems.Add(new GSItem("Fumigène", 0xE2, 157));
            LesItems.Add(new GSItem("Somnifère", 0xE3, 158));
            LesItems.Add(new GSItem("Ticket de jeu", 0xE4, 159));
            LesItems.Add(new GSItem("Porte bonheur", 0xE5, 160));
            LesItems.Add(new GSItem("Oeil du dragon", 0xE6, 161));
            LesItems.Add(new GSItem("Os", 0xE7, 162));
            LesItems.Add(new GSItem("Ancre enchantée ", 0xE8, 163));
            LesItems.Add(new GSItem("Maïs", 0xE9, 164));
            LesItems.Add(new GSItem("Clef des geôles", 0xEA, 165));
            LesItems.Add(new GSItem("Ticket bateau", 0xEB, 166));
            LesItems.Add(new GSItem("Plume sacrée", 0xEC, 167));
            LesItems.Add(new GSItem("Potion Lémuria", 0xED, 168));
            LesItems.Add(new GSItem("Goutte d'huile", 0xEE, 169));
            LesItems.Add(new GSItem("Griffe de fouine", 0xEF, 170));
            LesItems.Add(new GSItem("Graine roncière", 0xF0, 171));
            LesItems.Add(new GSItem("Poudre cristal", 0xF1, 172));
            LesItems.Add(new GSItem("Orbe noir", 0xF2, 173));
            LesItems.Add(new GSItem("Clef rouge", 0xF3, 174));
            LesItems.Add(new GSItem("Clef bleue", 0xF4, 175));
/***********************************************************************/
            LesItems.Add(new GSItem("Gilet mythril", 0xFA, 176));
            LesItems.Add(new GSItem("Chemise de soie", 0xFB, 177));
            LesItems.Add(new GSItem("Chemise sport", 0xFC, 178));
/************************************************************************/
/*         LISTE SECONDAIRE
************************************************************************/
            LesItems.Add(new GSItem("Bottes de hâte", 0x01, true, 179));
            LesItems.Add(new GSItem("Bottes fourrées", 0x02, true, 180));
            LesItems.Add(new GSItem("Bottes Tortue", 0x03, true, 181));
            LesItems.Add(new GSItem("Anneau Mystique", 0x06, true, 182));
            LesItems.Add(new GSItem("Anneau Guerrier", 0x07, true, 183));
            LesItems.Add(new GSItem("Anneau Repos", 0x08, true, 184));
            LesItems.Add(new GSItem("Anneau de soin", 0x09, true, 185));
            LesItems.Add(new GSItem("Anneau licorne", 0x0A, true, 186));
            LesItems.Add(new GSItem("Anneau féerique", 0x0B, true, 187));
            LesItems.Add(new GSItem("Anneau Clérical", 0x0C, true, 188));


        } 
        
        
        /// <summary>
        /// Charge les Items de Golden Sun 2
        /// </summary>
        public void GSII_Loaditems()
        {           
            LesItems.Clear();
            LesItems.Add(new GSItem("Hyper Bottes", 0, true));
            LesItems.Add(new GSItem("Epée Longue", 1, 1));
            LesItems.Add(new GSItem("Glaive", 2, 2));
            LesItems.Add(new GSItem("Claymore", 3, 3));
            LesItems.Add(new GSItem("Espadon", 4, 4));
            LesItems.Add(new GSItem("Shamshir", 5, 5));
            LesItems.Add(new GSItem("Lame d'argent", 6, 6));

            LesItems.Add(new GSItem("Lame Rouge", 7, 189));//GS2

            LesItems.Add(new GSItem("Lame artique", 8, 8));
            LesItems.Add(new GSItem("Lame Gaia", 9, 9));
            LesItems.Add(new GSItem("Lame Solaire", 0xA, 10));
            LesItems.Add(new GSItem("Muramasa", 0xB, 11));
            /***********************************************************/
            LesItems.Add(new GSItem("Machette", 0xF, 12));
            LesItems.Add(new GSItem("Epée courte", 0x10, 12));
            LesItems.Add(new GSItem("Coutelas", 0x11, 13));
            LesItems.Add(new GSItem("Rapière", 0x12, 14));
            LesItems.Add(new GSItem("Rapière effilée", 0x13, 15));
            LesItems.Add(new GSItem("Ninja-to", 0x14, 16));
            LesItems.Add(new GSItem("Epée de vitesse", 0x15, 17));
            LesItems.Add(new GSItem("Rapière elfique", 0x16, 18));
            LesItems.Add(new GSItem("Lame tueuse", 0x17, 19));
            LesItems.Add(new GSItem("Lame mystère", 0x18, 20));
            LesItems.Add(new GSItem("Kikuichimonji", 0x19, 21));

            LesItems.Add(new GSItem("Masamune", 0x1A, 22));//GS2

            LesItems.Add(new GSItem("Cimeterre", 0x1B, 23));
            /*************************************************************/
            LesItems.Add(new GSItem("Hache", 0x1F, 24));
            LesItems.Add(new GSItem("Hache large", 0x20, 25));
            LesItems.Add(new GSItem("Lochabre", 0x21, 26));
            LesItems.Add(new GSItem("Draco-hache", 0x22, 27));
            LesItems.Add(new GSItem("Hache de géant", 0x23, 28));
            LesItems.Add(new GSItem("Hache Aspoc", 0x24, 29));
            LesItems.Add(new GSItem("Hache de feu", 0x25, 30));
            LesItems.Add(new GSItem("Hache d'Achéron", 0x26, 31));
            /******************************************************************/
            LesItems.Add(new GSItem("Masse", 0x2B, 32));
            LesItems.Add(new GSItem("Masse", 0x2C, 33));
            LesItems.Add(new GSItem("Bec de corbin", 0x2D, 34));
            LesItems.Add(new GSItem("Masse cloutée", 0x2E, 35));
            LesItems.Add(new GSItem("Masse bénie", 0x2F, 36));
            LesItems.Add(new GSItem("Masse gravité", 0x30, 37));
            LesItems.Add(new GSItem("Masse sacrée", 0x31, 38));
            LesItems.Add(new GSItem("Masse du chaos", 0x32, 39));
            /******************************************************************/
            LesItems.Add(new GSItem("Bâton", 0x37, 40));
            LesItems.Add(new GSItem("Bâguette", 0x38, 41));
            LesItems.Add(new GSItem("Bâguette bénie", 0x39, 42));
            LesItems.Add(new GSItem("Ankh béni", 0x3A, 43));
            LesItems.Add(new GSItem("Bâtonnet psy", 0x3B, 44));
            LesItems.Add(new GSItem("Baguette gel", 0x3C, 45));
            LesItems.Add(new GSItem("Ankh sacré", 0x3D, 46));
            LesItems.Add(new GSItem("Bâton abyssal", 0x3E, 47));
            LesItems.Add(new GSItem("Bâton cristal", 0x3F, 48));
            LesItems.Add(new GSItem("Bâton zodiaque", 0x40, 49));
            LesItems.Add(new GSItem("Bâton chamane", 0x41, 50));
            /******************************************************************/
            LesItems.Add(new GSItem("Armure de cuir", 0x4B, 51));
            LesItems.Add(new GSItem("Armure psy", 0x4C, 52));
            LesItems.Add(new GSItem("Cotte de maille", 0x4D, 53));
            LesItems.Add(new GSItem("Cuirasse", 0x4E, 54));
            LesItems.Add(new GSItem("Plate", 0x4F, 55));
            LesItems.Add(new GSItem("Plate complète", 0x50, 56));
            LesItems.Add(new GSItem("Armure esprit", 0x51, 57));
            LesItems.Add(new GSItem("Ecaille dragon", 0x52, 58));
            LesItems.Add(new GSItem("Plate abyssale", 0x53, 59));
            LesItems.Add(new GSItem("Plate d'Ashura", 0x54, 60));
            LesItems.Add(new GSItem("Harnois", 0x55, 61));
            /******************************************************************/
            LesItems.Add(new GSItem("Chemise", 0x59, 62));
            LesItems.Add(new GSItem("Gilet de voyage", 0x5A, 63));
            LesItems.Add(new GSItem("Manteau", 0x5B, 64));
            LesItems.Add(new GSItem("Robe mystique", 0x5C, 65));
            LesItems.Add(new GSItem("Chemise elfique", 0x5D, 66));
            LesItems.Add(new GSItem("Gilet en argent", 0x5E, 67));
            LesItems.Add(new GSItem("Aqua-veste", 0x5F, 68));
            LesItems.Add(new GSItem("Para-tonnerre", 0x60, 69));
            LesItems.Add(new GSItem("Kimono", 0x61, 70));
            LesItems.Add(new GSItem("Shinobi shojoku", 0x62, 71));
            /******************************************************************/
            LesItems.Add(new GSItem("Robe", 0x67, 72));
            LesItems.Add(new GSItem("Robe de voyage", 0x68, 73));
            LesItems.Add(new GSItem("Robe de soie", 0x69, 74));
            LesItems.Add(new GSItem("Robe chinoise", 0x6A, 75));
            LesItems.Add(new GSItem("Pourpoint", 0x6B, 76));
            LesItems.Add(new GSItem("Robe de soirée", 0x6C, 77));
            LesItems.Add(new GSItem("Robe bénie", 0x6D, 78));
            LesItems.Add(new GSItem("Soutane magique ", 0x6E, 79));
            LesItems.Add(new GSItem("Robe enchantée", 0x6F, 80));
            LesItems.Add(new GSItem("Robe à plumes", 0x70, 81));
            LesItems.Add(new GSItem("Robe de l'oracle", 0x71, 82));
            /******************************************************************/
            LesItems.Add(new GSItem("Bouclier", 0x77, 83));
            LesItems.Add(new GSItem("Bouclier de fer", 0x78, 84));
            LesItems.Add(new GSItem("Ecu ", 0x79, 85));
            LesItems.Add(new GSItem("Bouclier miroir", 0x7A, 86));
            LesItems.Add(new GSItem("Draco-bouclier", 0x7B, 87));
            LesItems.Add(new GSItem("Bouclier Gaia", 0x7C, 88));
            /******************************************************************/
            LesItems.Add(new GSItem("Gants", 0x7F, 89));
            LesItems.Add(new GSItem("Gants de cuir", 0x80, 90));
            LesItems.Add(new GSItem("Gantelets ", 0x81, 91));
            LesItems.Add(new GSItem("Vambrace", 0x82, 92));
            LesItems.Add(new GSItem("Gants de fer", 0x83, 93));
            LesItems.Add(new GSItem("Gants mystère", 0x84, 94));
            LesItems.Add(new GSItem("Gants cloutés", 0x85, 95));
            LesItems.Add(new GSItem("Gants vertueux", 0x86, 96));
            /*******************************************************************/
            LesItems.Add(new GSItem("Bracelet cuir", 0x88, 97));
            LesItems.Add(new GSItem("Bracelet", 0x89, 98));
            LesItems.Add(new GSItem("Bracelet lourd", 0x8A, 99));
            LesItems.Add(new GSItem("Bracelet argent", 0x8B, 100));
            LesItems.Add(new GSItem("Bracelet éthéré", 0x8C, 101));
            LesItems.Add(new GSItem("Bracelet saint", 0x8D, 102));
            LesItems.Add(new GSItem("Bracelet gardien ", 0x8E, 103));
            /*******************************************************************/
            LesItems.Add(new GSItem("Casque", 0x91, 104));
            LesItems.Add(new GSItem("Heaume", 0x92, 105));
            LesItems.Add(new GSItem("Chapel de fer ", 0x93, 106));
            LesItems.Add(new GSItem("Heaume d'acier ", 0x94, 107));
            LesItems.Add(new GSItem("Heaume d'argent ", 0x95, 108));
            LesItems.Add(new GSItem("Bassinet ", 0x96, 109));
            LesItems.Add(new GSItem("Casque à cornes", 0x97, 110));
            LesItems.Add(new GSItem("Heaume psy", 0x98, 111));
            /******************************************************************/
            LesItems.Add(new GSItem("Chapeau en cuir", 0x9C, 112));
            LesItems.Add(new GSItem("Chapeau de bois", 0x9D, 113));
            LesItems.Add(new GSItem("Cagoule", 0x9E, 114));
            LesItems.Add(new GSItem("Couronne ornée", 0x9F, 115));
            LesItems.Add(new GSItem("Cagoule de ninja", 0xA0, 116));
            LesItems.Add(new GSItem("Chapeau-chance", 0xA1, 117));
            LesItems.Add(new GSItem("Couronne foudre", 0xA2, 118));
            LesItems.Add(new GSItem("Mitre", 0xA3, 119));
            LesItems.Add(new GSItem("Chapeau-leurre", 0xA4, 120));
            /*******************************************************************/
            LesItems.Add(new GSItem("Tiare", 0xA6, 121));
            LesItems.Add(new GSItem("Tiare d'argent", 0xA7, 122));
            LesItems.Add(new GSItem("Tiare gardienne", 0xA8, 123));
            LesItems.Add(new GSItem("Tiare de platine", 0xA9, 124));
            LesItems.Add(new GSItem("Tiare mythril", 0xAA, 125));
            LesItems.Add(new GSItem("Tiare soleil", 0xAB, 126));
            /********************************************************************
             *    ITEMS DE BASE 
             * ******************************************************************/
            LesItems.Add(new GSItem("Herbe", 0xB4, 127));
            LesItems.Add(new GSItem("Noisette", 0xB5, 128));
            LesItems.Add(new GSItem("Fiole", 0xB6, 129));
            LesItems.Add(new GSItem("Potion", 0xB7, 130));
            LesItems.Add(new GSItem("Eau Miracle", 0xB8, 131));
            LesItems.Add(new GSItem("Bouteille vide", 0xB9, 132));
            LesItems.Add(new GSItem("Cristal psy", 0xBA, 133));
            LesItems.Add(new GSItem("Antidote", 0xBB, 134));
            LesItems.Add(new GSItem("Elixir", 0xBC, 135));
            LesItems.Add(new GSItem("Eau jouvence", 0xBD, 136));

            LesItems.Add(new GSItem("Potion Voilée", 0xBE, 190));//GS2

            LesItems.Add(new GSItem("Pain", 0xBF, 137));
            LesItems.Add(new GSItem("Cookie", 0xC0, 138));
            LesItems.Add(new GSItem("Pomme", 0xC1, 139));
            LesItems.Add(new GSItem("Noix", 0xC2, 140));
            LesItems.Add(new GSItem("Menthe", 0xC3, 141));
            LesItems.Add(new GSItem("Poivre", 0xC4, 142));
            /**********************************************************************/
            /*      OBJETS DE POUVOIR
             * ********************************************************************/
     
            LesItems.Add(new GSItem("Piquet d'attache", 0xC6, 191));//GS2
            LesItems.Add(new GSItem("Presse cubique", 0xC7, 192));//GS2

            LesItems.Add(new GSItem("Orbe de force", 0xC8, 143));
            LesItems.Add(new GSItem("Goutte", 0xC9, 144));
            LesItems.Add(new GSItem("Joyau de gel", 0xCA, 145));
            LesItems.Add(new GSItem("Joyau kynétique", 0xCB, 146));
            LesItems.Add(new GSItem("Gemme de stase", 0xCC, 147));
            LesItems.Add(new GSItem("Balle d'ombre", 0xCD, 148));
            LesItems.Add(new GSItem("Pierre de prise", 0xCE, 149));
            LesItems.Add(new GSItem("Perle larcin ", 0xCF, 150));

            LesItems.Add(new GSItem("Eclat sismique", 0xD0, 193));//GS2
            LesItems.Add(new GSItem("Joyau de fouille", 0xD1, 194));//GS2
            LesItems.Add(new GSItem("Eclat cyclonique", 0xD2, 195));//GS2
            LesItems.Add(new GSItem("Broche dynamite", 0xD5, 196));//GS2
            LesItems.Add(new GSItem("Meule", 0xD6, 197));//GS2
            LesItems.Add(new GSItem("Jade flottant", 0xD7, 198));//GS2
            LesItems.Add(new GSItem("Kinésilapis", 0xD9, 199));//GS2

            /************************************************************************/
            LesItems.Add(new GSItem("Etoile Vénus", 0xDC, 151));
            LesItems.Add(new GSItem("Etoile Mercure  ", 0xDD, 152));

            LesItems.Add(new GSItem("Sac Mythril (1)", 0xDE, 155));//GS2
            LesItems.Add(new GSItem("Sac Mythril (2)", 0xDF, 155));//GS2
            LesItems.Add(new GSItem("Sac Mythril (3) ", 0xE0, 155));//GS2

            LesItems.Add(new GSItem("Petit joyau", 0xE1, 156));
            LesItems.Add(new GSItem("Fumigène", 0xE2, 157));
            LesItems.Add(new GSItem("Somnifère", 0xE3, 158));
            LesItems.Add(new GSItem("Ticket de jeu", 0xE4, 159));
            LesItems.Add(new GSItem("Porte bonheur", 0xE5, 160));
            LesItems.Add(new GSItem("Oeil du dragon", 0xE6, 161));
            LesItems.Add(new GSItem("Os", 0xE7, 162));
            LesItems.Add(new GSItem("Ancre enchantée ", 0xE8, 163));
            LesItems.Add(new GSItem("Maïs", 0xE9, 164));
            LesItems.Add(new GSItem("Clef des geôles", 0xEA, 165));
            LesItems.Add(new GSItem("Ticket bateau", 0xEB, 166));
            LesItems.Add(new GSItem("Plume sacrée", 0xEC, 167));
            LesItems.Add(new GSItem("Potion Lémuria", 0xED, 168));
            LesItems.Add(new GSItem("Goutte d'huile", 0xEE, 169));
            LesItems.Add(new GSItem("Griffe de fouine", 0xEF, 170));
            LesItems.Add(new GSItem("Graine roncière", 0xF0, 171));
            LesItems.Add(new GSItem("Poudre cristal", 0xF1, 172));
            LesItems.Add(new GSItem("Orbe noir", 0xF2, 173));
            LesItems.Add(new GSItem("Clef rouge", 0xF3, 174));
            LesItems.Add(new GSItem("Clef bleue", 0xF4, 175));

            LesItems.Add(new GSItem("Sac mythril (4)", 0xF5, 155));//GS2
            LesItems.Add(new GSItem("Etoile Jupiter", 0xF6, 154));//GS2
            LesItems.Add(new GSItem("Etoile Mars", 0xF7, 153));//GS2

            /***********************************************************************/
            LesItems.Add(new GSItem("Gilet mythril", 0xFA, 176));
            LesItems.Add(new GSItem("Chemise de soie", 0xFB, 177));
            LesItems.Add(new GSItem("Chemise sport", 0xFC, 178));

            /************************************************************************/
            /*         LISTE SECONDAIRE COMMUNE
            ************************************************************************/
            LesItems.Add(new GSItem("Bottes de hâte", 0x01, true, 179));
            LesItems.Add(new GSItem("Bottes fourrées", 0x02, true, 180));
            LesItems.Add(new GSItem("Bottes Tortue", 0x03, true, 181));
            LesItems.Add(new GSItem("Anneau Mystique", 0x06, true, 182));
            LesItems.Add(new GSItem("Anneau Guerrier", 0x07, true, 183));
            LesItems.Add(new GSItem("Anneau Repos", 0x08, true, 184));
            LesItems.Add(new GSItem("Anneau de soin", 0x09, true, 185));
            LesItems.Add(new GSItem("Anneau licorne", 0x0A, true, 186));
            LesItems.Add(new GSItem("Anneau féerique", 0x0B, true, 187));
            LesItems.Add(new GSItem("Anneau Clérical", 0x0C, true, 188));

            /**************************************************************
            * ITEMS DE LA LISTE SECONDAIRE AJOUTES DANS GOLDEN SUN II
            * ***********************************************************/
            LesItems.Add(new GSItem("Epée géante", 0x10, true, 200));
            LesItems.Add(new GSItem("Lame mythril", 0x11, true, 201));
            LesItems.Add(new GSItem("Levatine", 0x12, true, 202));
            LesItems.Add(new GSItem("Epée obscure", 0x13, true, 203));
            LesItems.Add(new GSItem("Excalibur", 0x14, true, 204));
            LesItems.Add(new GSItem("Poignard", 0x15, true, 205));
            LesItems.Add(new GSItem("Fer de l'âme", 0x16, true, 206));
            LesItems.Add(new GSItem("Fer fougueux", 0x17, true, 207));
            LesItems.Add(new GSItem("Lame Hestia", 0x18, true, 208));
            LesItems.Add(new GSItem("Epée lumineuse", 0x19, true, 209));
            LesItems.Add(new GSItem("Epée runique", 0x1A, true, 210));
            LesItems.Add(new GSItem("Fer vaporeux", 0x1B, true, 211));

            LesItems.Add(new GSItem("Rapière sylphe", 0x1D, true, 212));
            LesItems.Add(new GSItem("Epée embrasée", 0x1E, true, 213));
            LesItems.Add(new GSItem("Epée pirate", 0x1F, true, 214));
            LesItems.Add(new GSItem("Epée corsaire", 0x20, true, 215));
            LesItems.Add(new GSItem("Sabre pirate", 0x21, true, 216));
            LesItems.Add(new GSItem("Epée hypnotique", 0x22, true, 217));
            LesItems.Add(new GSItem("Sabre brumeux", 0x23, true, 218));
            LesItems.Add(new GSItem("Lame Phaéton", 0x24, true, 219));
            LesItems.Add(new GSItem("Epée Tisiphone", 0x25, true, 220));

            LesItems.Add(new GSItem("Hache d'Apollon", 0x27, true, 221));
            LesItems.Add(new GSItem("Hache de Gaia", 0x28, true, 222));
            LesItems.Add(new GSItem("Hache Stellaire", 0x29, true, 223));
            LesItems.Add(new GSItem("Hache capitaine", 0x2A, true, 224));
            LesItems.Add(new GSItem("Hache viking", 0x2B, true, 225));
            LesItems.Add(new GSItem("Faux", 0x2C, true, 226));
            LesItems.Add(new GSItem("Hache Thémis", 0x2D, true, 227));
            LesItems.Add(new GSItem("Hache puissante", 0x2E, true, 228));
            LesItems.Add(new GSItem("Hache Tartare", 0x2F, true, 229));

            LesItems.Add(new GSItem("Masse comète", 0x31, true, 230));
            LesItems.Add(new GSItem("Masse atomique", 0x32, true, 231));
            LesItems.Add(new GSItem("Masse perfide", 0x33, true, 232));
            LesItems.Add(new GSItem("Masse maudite", 0x34, true, 233));
            LesItems.Add(new GSItem("Masse d'air", 0x35, true, 234));
            LesItems.Add(new GSItem("Masse montante", 0x36, true, 235));
            LesItems.Add(new GSItem("Masse Thanatos", 0x37, true, 236));

            LesItems.Add(new GSItem("Bâton vaporeux", 0x39, true, 237));
            LesItems.Add(new GSItem("Bâton de braise", 0x3A, true, 238));
            LesItems.Add(new GSItem("Baguette noire", 0x3B, true, 239));
            LesItems.Add(new GSItem("Dracomasse", 0x3C, true, 240));
            LesItems.Add(new GSItem("Bâton ardent", 0x3D, true, 241));
            LesItems.Add(new GSItem("Bâton maléfique", 0x3E, true, 242));
            LesItems.Add(new GSItem("Bâton d'étude", 0x3F, true, 243));
            LesItems.Add(new GSItem("Pole coupe-feu", 0x40, true, 244));
            LesItems.Add(new GSItem("Bâton Atropos", 0x41, true, 245));
            LesItems.Add(new GSItem("Règle Lachiesis", 0x42, true, 246));
            LesItems.Add(new GSItem("Ankh Clotho", 0x43, true, 247));
            LesItems.Add(new GSItem("Bâton Anubis", 0x44, true, 248));

            LesItems.Add(new GSItem("Trident", 0x46, true, 249));

            LesItems.Add(new GSItem("Armure astrale", 0x48, true, 250));
            LesItems.Add(new GSItem("Draco-haubert", 0x49, true, 251));
            LesItems.Add(new GSItem("Haubert Chronos", 0x4A, true, 252));
            LesItems.Add(new GSItem("Armure furtive", 0x4B, true, 253));
            LesItems.Add(new GSItem("Armure Xylion", 0x4C, true, 254));
            LesItems.Add(new GSItem("Haubert Ixion", 0x4D, true, 255));
            LesItems.Add(new GSItem("Haubert fantôme", 0x4E, true, 256));
            LesItems.Add(new GSItem("Armure Erebus", 0x4F, true, 257));
            LesItems.Add(new GSItem("Mailles Walkyrie", 0x50, true, 258));

            LesItems.Add(new GSItem("Gilet Magique", 0x52, true, 259));
            LesItems.Add(new GSItem("Habit mythril", 0x53, true, 260));
            LesItems.Add(new GSItem("Gilet Métallique", 0x54, true, 261));
            LesItems.Add(new GSItem("Veste sauvage", 0x55, true, 262));
            LesItems.Add(new GSItem("Robe florale", 0x56, true, 263));
            LesItems.Add(new GSItem("Veste de fête", 0x57, true, 264));
            LesItems.Add(new GSItem("Tunique Erinyes", 0x58, true, 265));
            LesItems.Add(new GSItem("Habit Triton", 0x59, true, 266));

            LesItems.Add(new GSItem("Draco-robe", 0x5B, true, 267));
            LesItems.Add(new GSItem("Robe Ardagh", 0x5C, true, 268));
            LesItems.Add(new GSItem("Robe sainte", 0x5D, true, 269));
            LesItems.Add(new GSItem("Soutane d'air", 0x5E, true, 270));
            LesItems.Add(new GSItem("Robe Iris", 0x5F, true, 271));

            LesItems.Add(new GSItem("Bouclier lunaire", 0x61, true, 272));
            LesItems.Add(new GSItem("Draco-bouclier", 0x62, true, 273));
            LesItems.Add(new GSItem("Bouclier de feu", 0x63, true, 274));
            LesItems.Add(new GSItem("Terra-bouclier", 0x64, true, 275));
            LesItems.Add(new GSItem("Bouclier Cosmos", 0x65, true, 276));
            LesItems.Add(new GSItem("Bouclier Fujin", 0x66, true, 277));
            LesItems.Add(new GSItem("Bouclier égide", 0x67, true, 278));

            LesItems.Add(new GSItem("Aéro-gants", 0x69, true, 279));
            LesItems.Add(new GSItem("Gants de titan", 0x6A, true, 280));
            LesItems.Add(new GSItem("Gants big bang", 0x6B, true, 281));
            LesItems.Add(new GSItem("Gants d'artisan", 0x6C, true, 282));
            LesItems.Add(new GSItem("Gants d'émeute", 0x6D, true, 283));
            LesItems.Add(new GSItem("Gants esprit", 0x6E, true, 284));

            LesItems.Add(new GSItem("Bracelet clair", 0x70, true, 285));
            LesItems.Add(new GSItem("Brac. mythril", 0x71, true, 286));
            LesItems.Add(new GSItem("Bracelet osseux", 0x72, true, 287));
            LesItems.Add(new GSItem("Brac. joker", 0x73, true, 288));
            LesItems.Add(new GSItem("Bracelet Leda", 0x74, true, 289));

            LesItems.Add(new GSItem("Draco-heaume", 0x76, true, 290));
            LesItems.Add(new GSItem("Heaume mythril", 0x77, true, 291));
            LesItems.Add(new GSItem("Heaume terrible", 0x78, true, 292));
            LesItems.Add(new GSItem("Milléniheaume", 0x79, true, 293));
            LesItems.Add(new GSItem("Heaume viking", 0x7A, true, 294));
            LesItems.Add(new GSItem("Grand heaume", 0x7B, true, 295));
            LesItems.Add(new GSItem("Heaume Minerve", 0x7C, true, 296));

            LesItems.Add(new GSItem("Coiffe flottante", 0x7E, true, 297));
            LesItems.Add(new GSItem("Coiffe curative", 0x7F, true, 298));
            LesItems.Add(new GSItem("Couronne piquée", 0x80, true, 299));
            LesItems.Add(new GSItem("Masque Otafuku", 0x81, true, 300));
            LesItems.Add(new GSItem("Masque Hiotoko", 0x82, true, 301));
            LesItems.Add(new GSItem("Diadème", 0x83, true, 302));
            LesItems.Add(new GSItem("Cagoule Alastor", 0x84, true, 303));

            LesItems.Add(new GSItem("Tiare pure", 0x86, true, 304));
            LesItems.Add(new GSItem("Tiare astrale", 0x87, true, 305));
            LesItems.Add(new GSItem("Tiare psychique", 0x88, true, 306));
            LesItems.Add(new GSItem("Tiare perfide", 0x89, true, 307));
            LesItems.Add(new GSItem("Tiare claire", 0x8A, true, 308));
            LesItems.Add(new GSItem("Tiare brillante", 0x8B, true, 309));
            LesItems.Add(new GSItem("Bande Berserker", 0x8C, true, 310));

            LesItems.Add(new GSItem("Caraco divin", 0x8E, true, 311));
            LesItems.Add(new GSItem("Chemise florale", 0x8F, true, 312));
            LesItems.Add(new GSItem("Chemise d'or", 0x90, true, 313));
            LesItems.Add(new GSItem("Chemise", 0x91, true, 314));
            LesItems.Add(new GSItem("Bottes de cuir", 0x92, true, 315));
            LesItems.Add(new GSItem("Draco-bottes", 0x93, true, 316));
            LesItems.Add(new GSItem("Bottes-sureté", 0x94, true, 317));
            LesItems.Add(new GSItem("Grèves", 0x95, true, 318));
            LesItems.Add(new GSItem("Grèves d'argent", 0x96, true, 319));
            LesItems.Add(new GSItem("Sandales ninja", 0x97, true, 320));
            LesItems.Add(new GSItem("Bottes d'or", 0x98, true, 321));

            LesItems.Add(new GSItem("Anneau mystique", 0x99, true, 322));
            LesItems.Add(new GSItem("Anneau stellaire", 0x9A, true, 323));
            LesItems.Add(new GSItem("Anneau-arôme", 0x9B, true, 324));
            LesItems.Add(new GSItem("Anneau coloré", 0x9C, true, 325));
            LesItems.Add(new GSItem("Anneau-ame", 0x9D, true, 326));
            LesItems.Add(new GSItem("Anneau-gardien", 0x9E, true, 327));
            LesItems.Add(new GSItem("Anneau d'or", 0x9F, true, 328));

            /****************FORGEABLES***********************************/
            LesItems.Add(new GSItem("Epée rouillée", 0xA1, true, 329));
            LesItems.Add(new GSItem("Epée rouillée", 0xA2, true, 329));
            LesItems.Add(new GSItem("Epée rouillée", 0xA3, true, 329));
            LesItems.Add(new GSItem("Epée rouillée", 0xA4, true, 329));
            LesItems.Add(new GSItem("Hache rouillée", 0xA5, true, 330));
            LesItems.Add(new GSItem("Hache rouillée", 0xA6, true, 330));     
            LesItems.Add(new GSItem("Masse rouillée", 0xA7, true, 331));
            LesItems.Add(new GSItem("Masse rouillée", 0xA8, true, 331));
            LesItems.Add(new GSItem("Barre rouillée", 0xA9, true, 332));
            LesItems.Add(new GSItem("Barre rouillée", 0xAA, true, 332));
            LesItems.Add(new GSItem("Barre rouillée", 0xAB, true, 332));

            LesItems.Add(new GSItem("Larme de pierre", 0xAD, true, 333));
            LesItems.Add(new GSItem("Poudre magique", 0xAE, true, 334));
            LesItems.Add(new GSItem("Plume sylphe", 0xAF, true, 335));
            LesItems.Add(new GSItem("Peau de dragon", 0xB0, true, 336));
            LesItems.Add(new GSItem("Coda de braise", 0xB1, true, 337));
            LesItems.Add(new GSItem("Coeur golem", 0xB2, true, 338));
            LesItems.Add(new GSItem("Argent mythril", 0xB3, true, 339));
            LesItems.Add(new GSItem("Matière obscure", 0xB4, true, 340));
            LesItems.Add(new GSItem("Orihalcon", 0xB5, true, 341));

            /* Morceaux du trident*/
            LesItems.Add(new GSItem("Dent droite", 0xB7, true, 342));
            LesItems.Add(new GSItem("Dent gauche", 0xB8, true, 343));
            LesItems.Add(new GSItem("Dent centrale", 0xB9, true, 344));

            /*Items de classe*/
            LesItems.Add(new GSItem("Carte mystère", 0xBB, true, 345));
            LesItems.Add(new GSItem("Fouet de maitre", 0xBC, true, 346));
            LesItems.Add(new GSItem("Tomegathericon", 0xBD, true, 347));

            /*Champis (hallucinogènes?)*/
            LesItems.Add(new GSItem("Fongus curatif", 0xC0, true, 348));
            LesItems.Add(new GSItem("Fongus riant", 0xC1, true, 349));

            /*Objets de l'intrigue et de l'énigme des animaux*/
            LesItems.Add(new GSItem("Idole dansante", 0xC3, true, 350));
            LesItems.Add(new GSItem("Belle pierre", 0xC4, true, 351));
            LesItems.Add(new GSItem("Etoffe rouge", 0xC5, true, 352));
            LesItems.Add(new GSItem("Lait", 0xC6, true, 353));
            LesItems.Add(new GSItem("Petite tortue", 0xC7, true, 354));
            LesItems.Add(new GSItem("Pierre verseau", 0xC8, true, 355));
            LesItems.Add(new GSItem("Miche de pain", 0xC9, true, 356));
            LesItems.Add(new GSItem("Larme divine", 0xCA, true, 357));
            LesItems.Add(new GSItem("Clé des ruines", 0xCB, true, 358));
            LesItems.Add(new GSItem("Boule magma", 0xCC, true, 359));

        }



        #endregion

     
      

        

       
        











      
      

    }


      
}