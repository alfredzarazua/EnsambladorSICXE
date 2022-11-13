using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsambladorSicXE
{
    class CalculaCodigoObjeto
    {
        System.Windows.Forms.DataGridView tablaArchivo, tabSim, tabBloq;
        string nombProg;
        public CalculaCodigoObjeto(System.Windows.Forms.DataGridView gridArchivo, System.Windows.Forms.DataGridView gridTabSim,
            System.Windows.Forms.DataGridView gridBloques)
        {
            tablaArchivo = gridArchivo;
            tabSim = gridTabSim;
            tabBloq = gridBloques;
            nombProg = "      ";
        }
        //busca el numero de linea donde empieza la seccion
        private int buscarInicioSeccion(int numSeccion)
        {
            int cont = 0, linea = 0;            
            for(int i=0; i < tablaArchivo.RowCount; i++)
            {
                if ((string)tablaArchivo.Rows[i].Cells[5].Value == "CSECT")
                {
                    cont++;
                }
                if (cont != numSeccion)
                {
                    linea++;
                }
                else break;               
            }
            return linea;
        }
        public List<string> obtenArchivoObjeto(int numSeccion)
        {
            List<string> arrCodObj = new List<string>();
            int numlinea = buscarInicioSeccion(numSeccion);
            string line = calculaHeader(numlinea);
            arrCodObj.Add(line);
            //Registros T
            //Error en rango de valores de numLinea
           // List<string> arrRegT = calculaT(numlinea+1);//numlinea + 1 para que inicie despues de CSECT checa rango

            //for(int i=0; i<arrRegT.Count; i++)
            //{
            //    arrCodObj.Add(arrRegT[i]);
            //}

            List<string> arrRegM = calculaM(numlinea+1);

            for (int i = 0; i < arrRegM.Count; i++)
            {
                arrCodObj.Add(arrRegM[i]);
            }

            string regE = calculaE(numSeccion);
            arrCodObj.Add(regE);

            return arrCodObj;
        }

        public string calculaHeader(int numLinea)
        {
            string line = "";
            int CPInit = Convert.ToInt32(((string)tablaArchivo.Rows[numLinea].Cells[2].Value), 16);//0
            //int CPLast = Convert.ToInt32(((string)tablaArchivo.Rows[tablaArchivo.RowCount - 1].Cells[2].Value), 16);
            int CPLast = Convert.ToInt32((string)tabBloq.Rows[tabBloq.RowCount-1].Cells[2].Value, 16) 
                + Convert.ToInt32((string)tabBloq.Rows[tabBloq.RowCount - 1].Cells[3].Value, 16);
            string name = "      ";
            if (((string)tablaArchivo.Rows[numLinea].Cells[4].Value).Length < 6)
            {
                int i = 6 - ((string)tablaArchivo.Rows[numLinea].Cells[4].Value).Length;
                name = (string)tablaArchivo.Rows[numLinea].Cells[4].Value;
                for (; i > 0; i--)
                {
                    name += " ";
                }
            }
            else
            {
                name = ((string)tablaArchivo.Rows[numLinea].Cells[4].Value).Substring(0, 6);
            }
            nombProg = name;
            line = "H" + name + CPInit.ToString("X6") + CPLast.ToString("X6");
            return line;
        }

        public List<string> calculaT(int numLinea)
        {
            List<string> registrosT = new List<string>();
            int linea = numLinea;

            while (linea < tablaArchivo.RowCount-1 && (string)tablaArchivo.Rows[linea].Cells[5].Value!="CSECT")
            {
                int bloq = 0;
                linea = buscaLineaCodRegT(linea+1, ref bloq);
                string ins = (string)tablaArchivo.Rows[linea].Cells[5].Value;
                int offset = Convert.ToInt32((string)tabBloq.Rows[bloq].Cells[2].Value, 16);
                int iniCP = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[2].Value, 16)+offset;
                int size=0;
                string TRen = "T"+iniCP.ToString("X6");
                string tempT = "";
                while(ins != "RESB" && ins != "RESW" && ins != "USE")
                {
                    int currentCP = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[2].Value, 16);
                    int nextCP = 0;
                    if(tablaArchivo.Rows[linea].Cells[3].Value == tablaArchivo.Rows[linea + 1].Cells[3].Value)
                    {
                        nextCP = Convert.ToInt32((string)tablaArchivo.Rows[linea + 1].Cells[2].Value, 16);
                    }
                    else
                    {
                        nextCP = buscaSiguienteCPBloque(linea);
                    }
                    
                    size += (nextCP - currentCP);
                    if(size > 30)
                    {
                        size -= (nextCP - currentCP);
                        linea--;
                        break;
                    }
                    if ((string)tablaArchivo.Rows[linea].Cells[8].Value != "---")
                    {
                        if (((string)tablaArchivo.Rows[linea].Cells[8].Value).Contains("*"))
                        {
                            tempT += ((string)tablaArchivo.Rows[linea].Cells[8].Value).Remove(((string)tablaArchivo.Rows[linea].Cells[8].Value).Length - 1, 1);
                        }
                        else tempT += (string)tablaArchivo.Rows[linea].Cells[8].Value;
                    }
                    linea++;
                    if (linea == tablaArchivo.RowCount - 1) break;
                    ins = (string)tablaArchivo.Rows[linea].Cells[5].Value;
                }
                TRen += size.ToString("X2") + tempT;
                registrosT.Add(TRen);
            }


            return registrosT;
        }

        public int buscaLineaCodRegT(int start, ref int bloq)
        {
            int linea = start;

            for(; linea<tablaArchivo.RowCount; linea++)
            {
                if ((string)tablaArchivo.Rows[linea].Cells[8].Value != "---")
                {
                    //linea = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[2].Value, 16);
                    break;
                }
            }
            bloq = Convert.ToInt32((string) tablaArchivo.Rows[linea].Cells[3].Value, 16);
            return linea;
        }

        public List<string> calculaM(int numLinea)
        {
            List<string> regM = new List<string>();
            for(int i = numLinea; i<tablaArchivo.RowCount-1 && (string)tablaArchivo.Rows[i].Cells[5].Value!="CSECT"; i++)
            {
                if (((string)tablaArchivo.Rows[i].Cells[8].Value).Contains("*"))
                {
                    string rengRM = "M";
                    int bloq = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[3].Value, 16);
                    int offset = Convert.ToInt32((string)tabBloq.Rows[bloq].Cells[2].Value, 16);
                    if (((string)tablaArchivo.Rows[i].Cells[8].Value).Length==7)//registro para WORD
                    {
                        int currentCP = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[2].Value, 16)+offset;
                        rengRM += currentCP.ToString("X6") + "06" + "+" + nombProg;
                    }
                    else//registro para formato 4
                    {
                        int currentCP = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[2].Value, 16) + 1 + offset;
                        rengRM += currentCP.ToString("X6") + "05" + "+" + nombProg;
                    }                    
                    regM.Add(rengRM);
                }
            }

            return regM;
        }

        public string calculaE(int numSeccion)
        {
            string regE = "E";            
            //int bloq = Convert.ToInt32((string)tablaArchivo.Rows[tablaArchivo.RowCount - 1].Cells[3].Value, 16);
            //int offset = Convert.ToInt32((string)tabBloq.Rows[bloq].Cells[2].Value, 16);

            if(numSeccion == 0)
            {
                string simbolo = (string)tablaArchivo.Rows[tablaArchivo.RowCount - 1].Cells[6].Value;
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
                    int bloque = 0;
                    int dir = buscaTabSim(simbolo, ref bloque);
                    int offset = Convert.ToInt32((string)tabBloq.Rows[bloque].Cells[2].Value, 16);
                    if (dir == -1)
                    {
                        regE += "FFFFFF";
                        tablaArchivo.Rows[tablaArchivo.RowCount - 1].Cells[7].Value += "-Simbolo no encontrado";
                    }
                    else
                    {
                        dir += offset;
                        regE += dir.ToString("X6");
                    }
                }
            }
            
            return regE;
        }

        public int buscaTabSim(string simbolo, ref int bloque)
        {
            int dir = -1;
            for(int i = 0; i<tabSim.RowCount; i++)
            {
                if((string)tabSim.Rows[i].Cells[0].Value == simbolo)
                {
                    dir = Convert.ToInt32((string)tabSim.Rows[i].Cells[1].Value, 16);
                    bloque = Convert.ToInt32((string)tabSim.Rows[i].Cells[3].Value, 16);
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

        private int buscaSiguienteCPBloque(int linea)
        {
            int conta = -1;
            string BloqueActual = (string)tablaArchivo.Rows[linea].Cells[3].Value;
            for (int i = linea + 1; i < tablaArchivo.Rows.Count - 1; i++)
            {
                if ((string)tablaArchivo.Rows[i].Cells[3].Value == BloqueActual)
                {
                    conta = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[2].Value, 16);
                    break;
                }
            }

            if(conta == -1)
            {
                int bA = Convert.ToInt32(BloqueActual);
                conta = Convert.ToInt32((string)tabBloq.Rows[bA].Cells[3].Value, 16);
            }

            return conta;
        }
    }
}
