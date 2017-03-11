using System;
using System.Collections.Generic;
using System.Text;

namespace GSCodegen
{
    class GSItem
    {
        public String _Nom;//Le nom de l'objet
        public Byte _Code;//La valeur magique du byte � mettre (identifie l'objet de mani�re unique)
        public Boolean _ListeSecondaire; //Indique si l'objet fait partie de la liste secondaire
        public Int32 _ImageIndex; //Index de l'image � afficher dans le menu

      
            #region Constructeurs
            public GSItem(String nom, Byte Code, Int32 img)
            {
                _Nom = nom;
                _Code = Code;
                _ListeSecondaire = false;
                _ImageIndex = img;
            }

             public GSItem(String nom,Byte Code,Boolean sndlst)
            {
                _Nom = nom;
                _Code = Code;
                _ListeSecondaire = sndlst;
                _ImageIndex = 0;
            }

            public GSItem(String nom, Byte Code, Boolean sndlst,Int32 img)
            {
                _Nom = nom;
                _Code = Code;
                _ListeSecondaire = sndlst;
                _ImageIndex = img;

            }
    #endregion


            #region M�thodes

        public override String ToString()
        {
            return _Nom;
        }
        /// <summary>
        /// Donne la valeur du byte de quantit� � mettre pour avoir "qt" objets dans le jeu 
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        /// !!!!!! Ne tient pas compte de l'�tat de l'objet (cass�, �quip� ou non) !!!!!!!!!!
        public byte RealQTtoGameQT(byte qt)
        {
            qt--;

            if (_ListeSecondaire)
                return (byte)((qt * 8)+ 1);
            else return (byte)(qt * 8);


        }





            #endregion

        }
}
