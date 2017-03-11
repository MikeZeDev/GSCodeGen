using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GSCodegen.GUI
{
	public class ImageItem
	{
		#region Champs

		private object m_oValue;
		private Int32 m_iImgIndex;

		#endregion

		#region Constructeurs

		public ImageItem()
		{ 
			this.ImageIndex = 0; 
		}

		public ImageItem(Object Value, Int32 imageIndex)
		{ 
			this.Value = Value; this.ImageIndex = imageIndex; 
		}

		#endregion

		#region Propriétés

		public Object Value
		{
			get { return m_oValue; }
			set { m_oValue = value; }
		}

		public Int32 ImageIndex
		{
			get { return m_iImgIndex; }
			set { m_iImgIndex = value; }
		}

		#endregion

		#region Méthodes

		public override string ToString()
		{ 
			return this.Value.ToString();
		}

		#endregion
	}
}