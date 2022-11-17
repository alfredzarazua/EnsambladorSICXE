using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnsambladorSicXE
{
    internal class Seccion
    {
        private DataGridView tabSim;
        private DataGridView tabBloques;
        public string nombre { get; set; }
        public string tam { get; set; }//tamaño de la seccion

        public Seccion()
        {
            tabSim = new DataGridView();
            tabBloques = new DataGridView();
            iniciarGridTabSim();
            iniciarGridTabBloq();
            setStyle();
        }
        public DataGridView getTabSim()
        {
            return tabSim;
        }
        public DataGridView getTabBloques()
        {
            return tabBloques;
        }
        public void tabSimAddRow(string[] row)
        {
            tabSim.Rows.Add(row);
        }
        public void tabBloquesAddRow(string[] row)
        {
            tabBloques.Rows.Add(row);
        }

        public void calculaTamSeccion()
        {
            int a = Convert.ToInt32((string)tabBloques.Rows[tabBloques.Rows.Count - 1].Cells[2].Value, 16);
            int b = Convert.ToInt32((string)tabBloques.Rows[tabBloques.Rows.Count - 1].Cells[3].Value, 16);
            tam = (a+b).ToString("X");

        }
        private void setStyle()
        {
            tabSim.EditMode = DataGridViewEditMode.EditProgrammatically;
            tabSim.Location = new Point(11, 250);
            tabSim.Size = new Size(282, 287);            
            tabSim.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabSim.AllowUserToAddRows = false;
            tabSim.RowHeadersVisible = false;
            tabSim.BorderStyle = BorderStyle.Fixed3D;
            tabSim.BackgroundColor = SystemColors.ButtonHighlight;
            tabSim.AllowUserToResizeRows = false;
            tabSim.ScrollBars = System.Windows.Forms.ScrollBars.None;
            tabSim.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabSim.DefaultCellStyle.SelectionBackColor = Color.Teal;
            tabSim.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;

            tabBloques.EditMode = DataGridViewEditMode.EditProgrammatically;
            tabBloques.Location = new Point(11, 250);
            tabBloques.Size = new Size(282, 157);            
            tabBloques.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabBloques.AllowUserToAddRows = false;
            tabBloques.RowHeadersVisible = false;
            tabBloques.BorderStyle = BorderStyle.Fixed3D;
            tabBloques.BackgroundColor = SystemColors.ButtonHighlight;
            tabBloques.AllowUserToResizeRows = false;
            tabBloques.ScrollBars = System.Windows.Forms.ScrollBars.None;
            tabBloques.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabBloques.DefaultCellStyle.SelectionBackColor = Color.Teal;
            tabBloques.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
        }
                
        private void iniciarGridTabSim()
        {
            tabSim.ColumnCount = 5;
            tabSim.Columns[0].Name = "Simbolo";
            tabSim.Columns[1].Name = "Dirección";
            tabSim.Columns[2].Name = "Tipo";
            tabSim.Columns[3].Name = "Bloque";
            tabSim.Columns[4].Name = "S.E.";
        }

        private void iniciarGridTabBloq()
        {
            tabBloques.ColumnCount = 4;
            tabBloques.Columns[0].Name = "No.";
            tabBloques.Columns[1].Name = "Nombre";
            tabBloques.Columns[2].Name = "Dir Inicio";
            tabBloques.Columns[3].Name = "Longitud";
        }
    }
}
