using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnsambladorSicXE
{
    class Cargador
    {
        List<string> files;
        List<string[]> tabse;
        int dirSec;

        public Cargador(int dirOrig)
        {
            files = new List<string>();
            tabse = new List<string[]>();
            dirSec = dirOrig;
        } 

        public void addFile(string path)
        {
            files.Add(path);
        }

        public void cargadorAlgoritmo(DataGridView memoria, string[]programa)
        {
            pasoUno(programa);
            pasoDos(programa, memoria);
        }

        public void pasoUno(string[] programa)
        {
            int dirProg = dirSec;
            
            int dirsc = dirProg;
            int lonsc = 0;
            string nombreSeccion = "";
            bool errorFlag = false;
            
            foreach(string linea in programa)
            {
                if(linea[0] == 'H')
                {
                    lonsc = Convert.ToInt32(linea.Substring(13, 6), 16);
                    nombreSeccion = linea.Substring(1, 6);
                    bool insTABSE = buscaInsertaTabSESeccion(nombreSeccion, dirsc, lonsc);
                    if (!insTABSE)
                    {
                        errorFlag = true;
                        break;
                    }
                }
                else if(linea[0] == 'D')
                {
                    int dLen = linea.Length;
                    int dirPointer = 1;
                    while (dirPointer < dLen)
                    {
                        string simbolo = linea.Substring(dirPointer, 6);
                        dirPointer += 6;
                        bool insSimbolo = buscaTabSESimbolo(simbolo);
                        if (!insSimbolo)
                        {
                            errorFlag = true;
                            break;
                        }
                        int dirSimbolo = Convert.ToInt32(linea.Substring(dirPointer, 6), 16);
                        dirPointer += 6;
                        insertaSimboloTabSe(simbolo, dirSimbolo+dirsc);
                    }
                    if (errorFlag == true) break;
                    
                }
                
            }
            //dirSec = dirsc + lonsc;
        }

        public void pasoDos(string[] programa, DataGridView memoria)
        {
            int dirsc = dirSec;
            int direj = dirsc;
            int lonsc = 0;

            foreach (string linea in programa)
            {
                int dirRow = 0;
                if (linea[0] == 'H')
                {
                    lonsc = Convert.ToInt32(linea.Substring(13, 6), 16);
                }
                else if (linea[0] == 'T')
                {
                    int dirT = Convert.ToInt32(linea.Substring(1, 6),16);
                    dirRow = ((dirT + dirsc)/0x10)*0x10;
                    creaLineaMemoria(memoria, dirRow);
                    cargaRegistroT(memoria, dirT + dirsc, linea, dirRow);
                    //cargaRegistroT(memoria, dirT + dirsc, linea, dirRow);
                }
            }
        }

        public bool buscaInsertaTabSESeccion(string simbolo, int dirIni, int longitud)
        {
            bool found = true;
            for (int i = 0; i<tabse.Count; i++)
            {
                if (tabse[i][0] == simbolo)
                {
                    found = false;
                    break;
                }
            }

            if(found == true)
            {
                string[] row = { simbolo, "", dirIni.ToString("X6"), longitud.ToString("X6") };
                tabse.Add(row);
            }
            return found;
        }

        public bool buscaTabSESimbolo(string simbolo)
        {
            bool found = true;
            for (int i = 0; i < tabse.Count; i++)
            {
                if (tabse[i][1] == simbolo)
                {
                    found = false;
                    break;
                }
            }

            return found;
        }

        public void insertaSimboloTabSe(string simbolo, int dir)
        {
            string[] row = { "", simbolo, dir.ToString("X6"), "" };
            tabse.Add(row);
        }

        public void creaLineaMemoria(DataGridView memoria, int direccion)
        {
            bool creado = true;
            for (int i = 0; i<memoria.RowCount; i++)
            {
                if(Convert.ToInt32((string)memoria.Rows[i].HeaderCell.Value,16) == direccion)
                {
                    creado = false;
                    break;
                }
            }

            if (creado)
            {
                DataGridViewRow rn = new DataGridViewRow();
                rn.HeaderCell.Value = direccion.ToString("X6");
                memoria.Rows.Add(rn);
            }

            /**/
        }

        public void cargaRegistroT(DataGridView memoria, int direccion, string linea, int dirRenglon)
        {
            int offset = direccion - dirRenglon;
            int longitud = Convert.ToInt32(linea.Substring(7, 2), 16);
            int indiceMemoriaRow = getIndexofMemoryRow(dirRenglon, memoria);
            int TPointer = 9;

            for(int i = 0; i<longitud; i++)
            {
                string obByte = linea.Substring(TPointer, 2);
                TPointer += 2;
                memoria.Rows[indiceMemoriaRow].Cells[offset].Value = obByte;
                offset++;
                if (offset > 15)
                {
                    DataGridViewRow rn = new DataGridViewRow();
                    rn.HeaderCell.Value = (dirRenglon + 0x10).ToString("X6");
                    memoria.Rows.Add(rn);
                    indiceMemoriaRow ++;
                    offset = 0;
                }
            }

        }

        public int getIndexofMemoryRow(int dir, DataGridView memoria)
        {
            int index = -1;

            for (int i = 0; i < memoria.RowCount; i++)
            {
                if (Convert.ToInt32((string)memoria.Rows[i].HeaderCell.Value, 16) == dir)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        public List<string[]> Tabse
        {
            get{return tabse;}

            set{tabse = value;}
        }
    }

}
