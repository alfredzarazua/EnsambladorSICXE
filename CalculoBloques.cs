using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnsambladorSicXE
{
    class CalculoBloques
    {

        public CalculoBloques()
        {
        }

        public int buscaBloques(string nombre, DataGridView tablaBloques)
        {
            int index = -1;

            for(int i = 0; i<tablaBloques.RowCount; i++)
            {
                if ((string)tablaBloques.Rows[i].Cells[1].Value==nombre)
                {
                    index = i;
                }
            }

            return index;
        }

        //Retorna -1 si inserto un bloque, retorna el indice del bloque si ya existia
        public int insertaNuevoBloque(string nombre, int dirIni, int longitud, Seccion sec)
        {
            int flag = buscaBloques(nombre, sec.getTabBloques());
            if (flag == -1)
            {
                string[] row = new string[4];
                row[0] = (sec.getTabBloques().RowCount).ToString();
                row[1] = nombre;
                row[2] = dirIni.ToString("X4");
                row[3] = longitud.ToString("X4");
                sec.tabBloquesAddRow(row);                
            }

            return flag;

        }

       public int regresaLongitudBloque(DataGridView tablaBloques, int index)
        {
            int valor = -1;

            if(index< tablaBloques.RowCount)
            {
                valor = Convert.ToInt32((string)tablaBloques.Rows[index].Cells[3].Value, 16);
            }

            return valor;
        }

        public int regresaCantidadBloques(DataGridView tablaBloques)
        {
            return tablaBloques.RowCount;

        }

        public bool modificaLongitudBloque(DataGridView tablaBloques, int index, int nuevaLongitud)
        {
            bool flag = false;

            if (index < tablaBloques.RowCount)
            {
                tablaBloques.Rows[index].Cells[3].Value = nuevaLongitud.ToString("X4");
                flag = true;
            }

            return flag;
        }

        

        public int regresaDirInicoBloque(DataGridView tablaBloques, int index)
        {
            int valor = -1;

            if (index < tablaBloques.RowCount)
            {
                valor = Convert.ToInt32((string)tablaBloques.Rows[index].Cells[2].Value, 16);
            }

            return valor;
        }

    }
}
