using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public List<string> obtenArchivoObjeto(int numSeccion, ref List<List<string>> simbExt)
        {
            List<string> arrCodObj = new List<string>();
            int numlinea = buscarInicioSeccion(numSeccion);
            string line = calculaHeader(numlinea);
            arrCodObj.Add(line);

            List<string> arrRegRef = calculaRegRef(numlinea + 1);
            for (int i = 0; i < arrRegRef.Count; i++)
            {
                arrCodObj.Add(arrRegRef[i]);
            }

            List<string> arrRegT = calculaT(numlinea+1);

            for(int i=0; i<arrRegT.Count; i++)
            {
                arrCodObj.Add(arrRegT[i]);
            }

            List<string> arrRegM = calculaM(numlinea+1, ref simbExt);

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

            while (linea < tablaArchivo.RowCount - 1)
            {
                int bloq = 0;
                linea = buscaLineaCodRegT(linea, ref bloq);
                if (linea == -1) break;                
                string ins = (string)tablaArchivo.Rows[linea].Cells[5].Value;
                int offset = Convert.ToInt32((string)tabBloq.Rows[bloq].Cells[2].Value, 16);
                int iniCP = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[2].Value, 16) + offset;
                int size = 0;
                string TRen = "T" + iniCP.ToString("X6");
                string tempT = "";
                while (ins != "RESB" && ins != "RESW" && ins != "USE" && ins != "ORG" && ins != "CSECT")
                {
                    string formato = (string)tablaArchivo.Rows[linea].Cells[1].Value;                    
                    string operando = (string)tablaArchivo.Rows[linea].Cells[6].Value;
                    
                    size += getInstrLenght(formato, ins, operando,linea);
                    if (size > 30)
                    {                        
                        size -= getInstrLenght(formato, ins, operando,linea);
                        linea--;
                        break;
                    }
                    if ((string)tablaArchivo.Rows[linea].Cells[8].Value != "---")
                    {
                        if (((string)tablaArchivo.Rows[linea].Cells[8].Value).Contains("*"))
                        {
                            string[] sep = { "*" };
                            string[] simbolosSe = ((string)tablaArchivo.Rows[linea].Cells[8].Value).Split(sep, StringSplitOptions.RemoveEmptyEntries);
                            tempT += simbolosSe[0];                            
                        }
                        else tempT += (string)tablaArchivo.Rows[linea].Cells[8].Value;
                    }
                    linea++;
                    if (linea == tablaArchivo.RowCount - 1) break;
                    ins = (string)tablaArchivo.Rows[linea].Cells[5].Value;

                }
                TRen += size.ToString("X2") + tempT;
                registrosT.Add(TRen);
                /*if (ins == "CSECT" || (linea + 1) < tablaArchivo.RowCount && (string)tablaArchivo.Rows[linea + 1].Cells[5].Value == "CSECT")
                {
                    break;
                }*/
            }


            return registrosT;
        }
        private int getInstrLenght(string format, string directiva, string operando, int linea)
        {
            int len = 0;
            if(!tablaArchivo.Rows[linea].Cells[7].Value.ToString().Contains("Error de Sintaxis"))
            {
                if (format == "---")
                {
                    if (!Regex.IsMatch(directiva, "(BASE|EQU|EXTDEF|EXTREF|END)+"))
                    {
                        switch (directiva)
                        {
                            case "RESB":
                                len = int.Parse(operando);
                                break;
                            case "RESW":
                                len = int.Parse(operando) * 3;
                                break;
                            case "WORD":
                                len = 3;
                                break;
                            case "BYTE":
                                if (operando.StartsWith("C")) { len = operando.Length - 5; }
                                if (operando.StartsWith("X"))
                                {
                                    double div = 2;
                                    double aux = (operando.Length - 5) / div;
                                    len = (int)Math.Ceiling(aux);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    len = int.Parse(format);
                }
            }
            return len;
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
                if((string)tablaArchivo.Rows[linea].Cells[5].Value == "CSECT")
                {
                    linea = -1;
                    break;
                }
            }
            if(linea != -1)
            {
                bloq = Convert.ToInt32((string)tablaArchivo.Rows[linea].Cells[3].Value, 16);
            }
            
            return linea;
        }

        public List<string> calculaM(int numLinea, ref List<List<string>> simbExt)
        {
            List<string> regM = new List<string>();
            for(int i = numLinea; i<tablaArchivo.RowCount-1 && (string)tablaArchivo.Rows[i].Cells[5].Value!="CSECT"; i++)
            {
                if (((string)tablaArchivo.Rows[i].Cells[8].Value).Contains("*"))
                {
                    string[] sep = { "*" };
                    string[] simbolorRegM = ((string)tablaArchivo.Rows[i].Cells[8].Value).Split(sep, StringSplitOptions.RemoveEmptyEntries);

                    bool haySE = false;
                    List<string> renglonSE = new List<string>();
                    for (int j = 1; j< simbolorRegM.Length; j++)
                    {
                        string rengRM = "M";
                        
                        int bloq = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[3].Value, 16);
                        int offset = Convert.ToInt32((string)tabBloq.Rows[bloq].Cells[2].Value, 16);
                        string nomSimb = "";
                        
                        if (simbolorRegM[j] == "SE")
                        {
                            if (haySE == false)
                            {
                                haySE = true;
                                renglonSE = simbExt[0];
                                simbExt.RemoveAt(0);
                            }
                            nomSimb = renglonSE[0];
                            renglonSE.RemoveAt(0);
                        }
                        else
                        {
                            nomSimb = nombProg;
                        }                        
                        if ((string)tablaArchivo.Rows[i].Cells[5].Value == "WORD")
                        {
                            int currentCP = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[2].Value, 16) + offset;
                            string signo = obtenerSigno(nomSimb, tablaArchivo.Rows[i].Cells[6].Value.ToString());
                            if (nomSimb.Length < 6)
                            {
                                while (nomSimb.Length < 6)
                                {
                                    nomSimb = nomSimb + " ";
                                }
                            }
                            rengRM += currentCP.ToString("X6") + "06" + signo + nomSimb;
                        }
                        else
                        {
                            int currentCP = Convert.ToInt32((string)tablaArchivo.Rows[i].Cells[2].Value, 16) + 1 + offset;
                            string signo = obtenerSigno(nomSimb, tablaArchivo.Rows[i].Cells[6].Value.ToString());
                            if (nomSimb.Length < 6)
                            {
                                while (nomSimb.Length < 6)
                                {
                                    nomSimb = nomSimb + " ";
                                }
                            }
                            rengRM += currentCP.ToString("X6") + "05" + signo + nomSimb;
                        }
                        regM.Add(rengRM);
                    }
                  
                    
                }
            }

            return regM;
        }
        private string[] tokenizeExp(string exp)
        {
            string separator = "+-()*/";
            for(int i = 0; i < exp.Length-1; i++)
            {
                string xe = exp[i].ToString();
                if (separator.Contains(xe))
                {                                        
                    exp = exp.Remove(i,1);
                    exp = exp.Insert(i, " "+xe+" ");                    
                    i+=2;
                }
            }
            string[] sep = { " ","\n","\r" };
            string[] tokens = exp.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            return tokens;
        }
        private string obtenerSigno(string simb, string exp)
        {
            string[] tokens = tokenizeExp(exp);            
            Stack<string> stack = new Stack<string>();
            int i = 0;
            string lastOp = "", currToken = tokens[0];
            while (currToken != simb && i < tokens.Length)
            {
                currToken = tokens[i];
                switch (currToken)
                {
                    case "+":
                        lastOp = currToken;
                        break;
                    case "-":
                        lastOp = currToken;
                        break;
                    case "(":
                        stack.Push(currToken);
                        stack.Push(lastOp);
                        lastOp = "";
                        break;
                    case ")":
                        string aux;
                        do
                        {
                            aux = stack.Pop();
                        }while (aux != "(" && stack.Count > 0);
                        break;
                } 
                if(currToken == simb && tokens[i-1] == "-")
                {
                    stack.Push(lastOp);
                }
                i++;
            }
            string a, signo;
            i=0;
            while (stack.Count > 0)
            {
                a = stack.Pop();
                if (a == "-")
                i++;
            }
            if (i % 2 != 0)
                signo = "-";
            else
                signo = "+";
            return signo;
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

        public List<string> calculaRegRef(int numLinea)
        {
            List<string> lineaRef = new List<string>();

            for (int i = numLinea; i < tablaArchivo.RowCount - 1 && (string)tablaArchivo.Rows[i].Cells[5].Value != "CSECT"; i++)
            {
                if((string)tablaArchivo.Rows[i].Cells[5].Value == "EXTDEF")
                {
                    List<string> regD = calculaD(i);

                    foreach(string rD in regD)
                    {
                        lineaRef.Add(rD);
                    }
                }

                if ((string)tablaArchivo.Rows[i].Cells[5].Value == "EXTREF")
                {
                    List<string> regR = calculaR(i);

                    foreach (string rR in regR)
                    {
                        lineaRef.Add(rR);
                    }
                }
            }

            return lineaRef;
        }

        public List<string> calculaD(int numLinea)
        {
            string expotOp = (string)tablaArchivo.Rows[numLinea].Cells[6].Value;
            string[] esp = { "," };
            string[] simbD = expotOp.Split(esp, StringSplitOptions.RemoveEmptyEntries);
            List<string> lineasD = new List<string>();
            int limit = 6;
            string renglonD = "D";
            for(int i = 0; i< simbD.Length; i++)
            {
                simbD[i] = simbD[i].Replace("\r\n", string.Empty);
                int bk = 0;
                string simName = "";
                int dir = buscaTabSim(simbD[i], ref bk);
                if (simbD[i].Length < 6)
                {
                    int conta = 6 - simbD[i].Length;
                    simName = simbD[i];
                    for(; conta > 0; conta--)
                    {
                        simName += " ";
                    }
                }
                else
                {
                    simName = simbD[i].Substring(0, 6);
                }

                renglonD += simName + dir.ToString("X6");

                if (i == limit - 1)
                {
                    lineasD.Add(renglonD);
                    renglonD = "D";
                }
            }
            lineasD.Add(renglonD);

            return lineasD;

        }

        public List<string> calculaR(int numLinea)
        {
            string expotOp = (string)tablaArchivo.Rows[numLinea].Cells[6].Value;
            string[] esp = { "," };
            string[] simbR = expotOp.Split(esp, StringSplitOptions.RemoveEmptyEntries);
            List<string> lineasR = new List<string>();
            int limit = 6;
            string renglonR = "R";
            for (int i = 0; i < simbR.Length; i++)
            {
                simbR[i] = simbR[i].Replace("\r\n", string.Empty);
                int bk = 0;
                string simName = "";
                if (simbR[i].Length < 6)
                {
                    int conta = 6 - simbR[i].Length;
                    simName = simbR[i];
                    for (; conta > 0; conta--)
                    {
                        simName += " ";
                    }
                }
                else
                {
                    simName = simbR[i].Substring(0, 6);
                }

                renglonR += simName;

                if (i == limit - 1)
                {
                    lineasR.Add(renglonR);
                    renglonR = "R";
                }
            }
            lineasR.Add(renglonR);

            return lineasR;

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
                    int dirIniBloque = Convert.ToInt32((string)tabBloq.Rows[bloque].Cells[2].Value, 16);
                    dir += dirIniBloque;
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

        /*private int buscaSiguienteCPBloque(int linea)
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
        }*/
    }
}
