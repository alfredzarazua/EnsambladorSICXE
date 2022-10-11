using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsambladorSicXE
{
    class CalculaCodigoObjeto
    {
        System.Windows.Forms.DataGridView tablaArchivo, tabSim;
        string nombProg;
        public CalculaCodigoObjeto(System.Windows.Forms.DataGridView gridArchivo, System.Windows.Forms.DataGridView gridTabSim)
        {
            tablaArchivo = gridArchivo;
            tabSim = gridTabSim;
            nombProg = "      ";
        }

        public List<string> obtenArchivoObjeto()
        {
            List<string> arrCodObj = new List<string>();
            string line = calculaHeader();
            arrCodObj.Add(line);
            List<string> arrRegT = calculaT();

            for(int i=0; i<arrRegT.Count; i++)
            {
                arrCodObj.Add(arrRegT[i]);
            }

            List<string> arrRegM = calculaM();

            for (int i = 0; i < arrRegM.Count; i++)
            {
                arrCodObj.Add(arrRegM[i]);
            }

            string regE = calculaE();
            arrCodObj.Add(regE);

            return arrCodObj;
        }

        public string calculaHeader()
        {
            string line = "";
            int CPInit = Convert.ToInt32(((string)tablaArchivo.Rows[0].Cells[2].Value), 16);
            int CPLast = Convert.ToInt32(((string)tablaArchivo.Rows[tablaArchivo.RowCount - 1].Cells[2].Value), 16);
            string name = "      ";
            if (((string)tablaArchivo.Rows[0].Cells[3].Value).Length < 6)
            {
                int i = 6 - ((string)tablaArchivo.Rows[0].Cells[3].Value).Length;
                name = (string)tablaArchivo.Rows[0].Cells[3].Value;
                for (; i > 0; i--)
                {
                    name += " ";
                }
            }
            else
            {
                name = ((string)tablaArchivo.Rows[0].Cells[3].Value).Substring(0, 6);
            }
            nombProg = name;
            line = "H" + name + CPInit.ToString("X6") + CPLast.ToString("X6");
            return line;
        }

        public List<string> calculaT()
        {
            List<string> registrosT = new List<string>();
            int linea = 0;

            while (linea < tablaArchivo.RowCount-1)
            {
                linea = buscaLineaCodRegT(linea+1);
                string ins = (string)tablaArchivo.Rows[linea].Cells[4].Value;
                int iniCP = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[2].Value, 16);
                int size=0;
                string TRen = "T"+iniCP.ToString("X6");
                string tempT = "";
                while(ins != "RESB" && ins != "RESW")
                {
                    int currentCP = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[2].Value, 16);
                    int nextCP = Convert.ToInt32((string)tablaArchivo.Rows[linea+1].Cells[2].Value, 16);
                    size += (nextCP - currentCP);
                    if(size > 30)
                    {
                        size -= (nextCP - currentCP);
                        linea--;
                        break;
                    }
                    if ((string)tablaArchivo.Rows[linea].Cells[7].Value != "---")
                    {
                        if (((string)tablaArchivo.Rows[linea].Cells[7].Value).Contains("*"))
                        {
                            tempT += ((string)tablaArchivo.Rows[linea].Cells[7].Value).Remove(((string)tablaArchivo.Rows[linea].Cells[7].Value).Length - 1, 1);
                        }
                        else tempT += (string)tablaArchivo.Rows[linea].Cells[7].Value;
                    }
                    linea++;
                    if (linea == tablaArchivo.RowCount - 1) break;
                    ins = (string)tablaArchivo.Rows[linea].Cells[4].Value;
                }
                TRen += size.ToString("X2") + tempT;
                registrosT.Add(TRen);
            }


            return registrosT;
        }

        public int buscaLineaCodRegT(int start)
        {
            int linea = start;

            for(; linea<tablaArchivo.RowCount; linea++)
            {
                if ((string)tablaArchivo.Rows[linea].Cells[7].Value != "---")
                {
                    //linea = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[2].Value, 16);
                    break;
                }
            }

            return linea;
        }

        public List<string> calculaM()
        {
            List<string> regM = new List<string>();
            for(int i = 0; i<tablaArchivo.RowCount-1; i++)
            {
                if (((string)tablaArchivo.Rows[i].Cells[7].Value).Contains("*"))
                {
                    string rengRM = "M";
                    int currentCP = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[2].Value, 16) +1;
                    rengRM += currentCP.ToString("X6") + "05" + "+" + nombProg;
                    regM.Add(rengRM);
                }
            }

            return regM;
        }

        public string calculaE()
        {
            string regE = "E";
            string simbolo = (string)tablaArchivo.Rows[tablaArchivo.RowCount - 1].Cells[5].Value;
            if (simbolo.Contains("\r\n"))
            {
                simbolo = simbolo.Remove(simbolo.Length - 2);
            }
            if (simbolo == "")
            {
                int dir = buscaPrimerIns();
                regE += dir.ToString("X6");
            }
            else
            {
                int dir = buscaTabSim(simbolo);
                if (dir == -1)
                {
                    regE += "FFFFFF";
                }
                else
                {
                    regE += dir.ToString("X6");
                }
            }
            
            return regE;
        }

        public int buscaTabSim(string simbolo)
        {
            int dir = -1;
            for(int i = 0; i<tabSim.RowCount; i++)
            {
                if((string)tabSim.Rows[i].Cells[0].Value == simbolo)
                {
                    dir = Convert.ToInt32((string)tabSim.Rows[i].Cells[1].Value, 16);
                    break;
                }
            }

            return dir;
        }

        public int buscaPrimerIns()
        {
            int dir = 0;

            for(int i=0; i<tablaArchivo.RowCount; i++)
            {
                if ((string)tablaArchivo.Rows[i].Cells[1].Value != "---")
                {
                    dir = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[2].Value, 16);
                    break;
                }
            }

            return dir;
        }
    }
}
