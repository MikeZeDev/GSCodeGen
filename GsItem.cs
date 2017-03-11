using System;
using System.Collections.Generic;
using System.Text;

namespace GSCodegen
{
    class GSItem
    {
        public String _Nom;//Le nom de l'objet
        public Byte _Code;//La valeur magique du byte à mettre (identifie l'objet de manière unique)
        public Boolean _ListeSecondaire; //Indique si l'objet fait partie de la liste secondaire
        public Int32 _ImageIndex; //Index de l'image à afficher dans le menu

      
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


            #region Méthodes

        public override String ToString()
        {
            return _Nom;
        }
        /// <summary>
        /// Donne la valeur du byte de quantité à mettre pour avoir "qt" objets dans le jeu 
        /// </summary>
        /// <param name="qt"></param>
        /// <returns></returns>
        /// !!!!!! Ne tient pas compte de l'état de l'objet (cassé, équipé ou non) !!!!!!!!!!
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
