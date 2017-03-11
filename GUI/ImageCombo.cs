using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GSCodegen.GUI
{
	public class ImageCombo : System.Windows.Forms.ComboBox
	{
		public ImageCombo()
		{
			//on veut le dessiner nous même
			this.DrawMode = DrawMode.OwnerDrawFixed;
			//forcement un DropDownList
			this.DropDownStyle = ComboBoxStyle.DropDownList;
			m_oColumns = 1;
			m_oSpecial = ForeColor;
		}

		private Color m_oSpecial;
		public Color SpecialForeColor
		{
			get { return m_oSpecial; }
			set { m_oSpecial = value; }
		}

		private ImageList m_oImageList;
		public ImageList ImageList
		{
			get { return m_oImageList; }
			set	{ m_oImageList = value;	}
		}

		private int m_oColumns;
		public int Columns
		{
			get { return m_oColumns; }
			set 
			{
				if (value > 0)
					m_oColumns = value;
				else
					throw new Exception("Nombre de colonnes incorrect");
			}
		}

		protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
		{
			this.DropDownStyle = ComboBoxStyle.DropDownList;
			base.OnDrawItem(e);
			if (e.Index == -1)
				return;

			System.Drawing.SolidBrush oB;
			if (this.ImageList != null)
			{
				//on laisse la gestion des exceptions chez le client
				//l'élément
				ImageItem oItem = this.Items[e.Index] as ImageItem;
				//l'image associée
				Image oImg = null;

				if (oItem != null && oItem.ImageIndex >= 0)
				{
					oImg = this.ImageList.Images[oItem.ImageIndex];
					//dessin de l'image
					e.Graphics.DrawImage(oImg, e.Bounds.X, e.Bounds.Y);
				}
				//dessin du texte
				StringFormat oSF = new StringFormat();
				oSF.Alignment = StringAlignment.Near;
				oSF.LineAlignment = StringAlignment.Center;

				string text = this.Items[e.Index].ToString();
				string[] itms = text.Split('\t');

				int tW = e.Bounds.Width - this.ImageList.ImageSize.Width;
				int lX = e.Bounds.X + this.ImageList.ImageSize.Width;

				int cC = itms.Length;
				int CW = tW / m_oColumns;

				int[] CX = new int[m_oColumns];

				oB = new System.Drawing.SolidBrush((cC > m_oColumns) ? this.SpecialForeColor : this.ForeColor);

				for (int i = 0; i < m_oColumns; i++)
				{
					CX[i] = (i > 0) ? CX[i - 1] + CW : lX;

					Rectangle oRect = new Rectangle(
						  CX[i],
						  e.Bounds.Y,
						  CW,
						  e.Bounds.Height);
					e.Graphics.DrawString((i < cC) ? itms[i] : "", this.Font, oB, oRect, oSF);
				}

				oB.Dispose();
				return;
			}
			oB = new System.Drawing.SolidBrush(this.ForeColor);
			e.Graphics.DrawString(this.Items[e.Index].ToString(), this.Font, oB, e.Bounds);
			oB.Dispose();
		}
	}
}