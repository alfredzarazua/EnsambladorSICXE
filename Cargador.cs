using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnsambladorSicXE
{
    class Cargador
    {
        List<string> files;
        List<string[]> tabse;
        public int dirSec { get; set; }

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

        public string pasoUno(string[] programa)
        {
            int dirProg = dirSec;
            
            int dirsc = dirProg;
            int lonsc = 0;
            string nombreSeccion = "";
            bool errorFlag = false;
            string msg = "";            
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
                        msg += "Error: Símbolo externo duplicado ["+nombreSeccion+"]"+Environment.NewLine;
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
                            msg += "Error: Símbolo externo duplicado [" + simbolo + "]" + Environment.NewLine;
                            break;
                        }
                        int dirSimbolo = Convert.ToInt32(linea.Substring(dirPointer, 6), 16);
                        dirPointer += 6;
                        insertaSimboloTabSe(simbolo, dirSimbolo+dirsc);
                    }
                    if (errorFlag == true) break;
                    
                }
                
            }
            dirSec = dirsc + lonsc;
            return msg;
        }

        public string pasoDos(string[] programa, DataGridView memoria)
        {
            int dirsc = dirSec;
            int direj = dirsc;
            int lonsc = 0;
            bool errorFlag = false;
            string msg = "";
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
                }else if (linea[0] == 'M')
                {
                    string simbolo = linea.Substring(10, 6);
                    int valor = buscarSimboloTABSE(simbolo);
                    if (valor != -1)
                    {
                        int direcc = Convert.ToInt32(linea.Substring(1, 6),16);
                        int mObjetivo = dirsc + direcc;
                        dirRow = (mObjetivo / 0x10) * 0x10;
                        modificarBytes(linea, mObjetivo, dirRow, memoria, valor);
                    }
                    else
                    {
                        //Activa la bandera de error (símbolo externo indefinido)
                        msg += "Error: Símbolo externo indefinido [" + simbolo + "]" + Environment.NewLine;
                        errorFlag = true;
                        break;
                    }

                }
                else if (linea[0] == 'E')
                {
                    if(linea.Length > 1)
                    {
                        direj = dirsc + Convert.ToInt32(linea.Substring(1,6), 16);
                    }
                    dirSec = dirsc + lonsc;
                }
            }
            return msg;
        }
        private void modificarBytes(string regM, int direcObjetivo, int dirRow, DataGridView memoria, int val)
        {
            int halfBytes = Convert.ToInt32(regM.Substring(7, 2), 16);
            int rIndex = getIndexofMemoryRow(dirRow, memoria);
            int cIndex = direcObjetivo - dirRow;
            string valor = "";
            int cont = 0;
            for (int i = 0; i < 3; i++)
            {
                if ((cIndex + i) > 15)
                {
                    rIndex++;
                    cIndex = 0;
                    cont = 0;                    
                }
                valor += memoria.Rows[rIndex].Cells[cIndex + cont].Value;
                cont++;
            }
            switch (halfBytes)
            {
                case 5:
                    int mx = Convert.ToInt32(valor.Substring(1, 5), 16);
                    if (regM[9] == '+')
                    {
                        mx = mx + val;
                    }
                    else{
                        mx = mx - val;
                    }
                    valor = valor.Substring(0, 1) + mx.ToString("X5");
                    break;
                case 6:
                    int mx1 = Convert.ToInt32(valor, 16);
                    if (regM[9] == '+')
                    {
                        mx1 = mx1 + val;
                    }
                    else
                    {
                        mx1 = mx1 - val;
                    }
                    valor = mx1.ToString("X6");
                    break;
            }
            rIndex = getIndexofMemoryRow(dirRow, memoria);
            cIndex = direcObjetivo - dirRow;
            cont = 0;
            for (int i = 0; i < 3; i++)
            {
                if ((cIndex + i) > 15)
                {
                    rIndex++;
                    cIndex = 0;
                    cont = 0;
                }
                memoria.Rows[rIndex].Cells[cIndex + cont].Value = valor.Substring(i*2, 2);
                cont++;
            }
        }
        private int buscarSimboloTABSE(string simbolo)
        {
            int dir = -1;
            foreach (string[] row in tabse)
            {
                if (row[0] == simbolo || row[1] == simbolo)
                {
                    dir = Convert.ToInt32(row[2],16);
                }
            }
            return dir;
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
