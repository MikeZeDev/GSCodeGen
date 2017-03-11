using System;
using System.Collections.Generic;
using System.Text;

namespace GSCodegen
{
    class GSPerso
    {

        #region Propriétés

        public static uint OFF2LEVEL = 0x0F; 
        public static uint OFF2ITEMS = 0xD8;
        public static uint OFF2EXP = 0x124;
        public static uint OFF2HP = 0x38;

   /*
         * BASE : nom du perso (max 5 cars)
         * BASE+0F : Level du perso (byte)
         * Base + 0x124 : exp du perso
         * Base + 0x38 : Energie du perso
         * 
         * 
         * 
         */

        public String _Nom;//Le nom du perso
        public uint _ADDR;//Adresse du nom du perso (début)
        public Int32 _ImageIndex; //Index de l'image à afficher dans le menu
        public Boolean NoReverse;

        #endregion


        #region Constructeurs
        public GSPerso(String nom,uint addr,int img)
        {
            _Nom=nom;
            _ADDR = addr;
            _ImageIndex = img;
            NoReverse = false;
        }

 
        #endregion


        #region Méthodes
        public override String ToString()
        {
            return _Nom;
        }

        #endregion
    }
}
