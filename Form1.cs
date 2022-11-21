using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using Antlr4;
using Antlr4.Runtime;

namespace EnsambladorSicXE
{
    public partial class Form1 : Form
    {
        private string filePath, fileName, filePathNofName, fileNameNoExt;
        private bool fileIsOnDisk;
        IList<string> codigo;
        List<Seccion> secciones;
        int currentSect;
        int CP;
        int currentBloqu;
        bool fase;
        List<string> SEporRenglon;
        List<List<string>> SEregi;

        private Style blue_style;
        private Style green_style;
        private Style purple_style;
        private Style orange_style;
        private Style white_style;
        private Style red_style;
        private Style cyan_style;
        private String regex_inst = "([ \t]|[\n]|['+'])+(ADD|ADDF|AND|COMP|COMPF|DIV|DIVF|J|JEQ|JGT|JLT|JSUB|LDA|LDB|LDCH|LDF|LDL|LDS|LDX|LPS|MUL|MULF|OR|RD|RSUB|SSK|STA|STB|STCH|STF|STI|STL|STS|STSW|STT|STX|SUB|SUBF|TD|TIX|WD|ADDR|SUBR|COMPR|DIVR|MULR|RMO|FIX|FLOAT|HIO|NORM|SIO|TIO|CLEAR|SHIFTR|SHIFTL|TIXR|SVC)([ \t]+|(\r\n))";
        private String regex_number = "([ \t]|[\n]|['@']|['#'])+(([a-fA-F0-9]+(h|H))|([0-9]+))([ \t]+|(\r\n))";
        private String regex_directive = "([ \t]|[\n])+(START|END)([ \t]+|(\r\n))";
        private String regex_directive2 = "([ \t]|[\n])+(BYTE|WORD|RESB|RESW|BASE|EQU|ORG|CSECT|EXTDEF|EXTREF|USE)([ \t]+|(\r\n))";
        public Form1()
        {
            InitializeComponent();
            setEditorStyle();
            iniciarGridInter();
            //iniciarGridTabSim();
            //iniciarGridTabBloq();
            CP = 0x00;
            currentBloqu = 0;
            fase = false;
            SEregi = new List<List<string>>();
            secciones=new List<Seccion>();
            SEporRenglon = new List<string>();
        }
        private void setEditorStyle()
        {
            SolidBrush blue_brush = new SolidBrush(Color.FromArgb(31, 97, 141));
            SolidBrush green_brush = new SolidBrush(Color.FromArgb(0xA6, 0xE2, 0x2E));
            SolidBrush purple_brush = new SolidBrush(Color.FromArgb(0xA3, 0x81, 0xFF));
            SolidBrush orange_brush = new SolidBrush(Color.FromArgb(244, 208, 63));
            SolidBrush red_brush = new SolidBrush(Color.FromArgb(245, 176, 65));
            SolidBrush darkCyan_brush = new SolidBrush(Color.FromArgb(0, 198, 168));
            SolidBrush lightblue_brush = new SolidBrush(Color.FromArgb(93, 173, 226));

            blue_style = new TextStyle(blue_brush, null, FontStyle.Bold);
            green_style = new TextStyle(green_brush, null, FontStyle.Bold);
            orange_style = new TextStyle(orange_brush, null, FontStyle.Bold);
            cyan_style = new TextStyle(darkCyan_brush, null, FontStyle.Bold);
            purple_style = new TextStyle(purple_brush, null, FontStyle.Bold);
            red_style = new TextStyle(red_brush, null, FontStyle.Bold);
            white_style = new TextStyle(lightblue_brush, null, FontStyle.Italic);

        }
        private void editorCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(white_style);
            //comment highlighting
            e.ChangedRange.SetStyle(cyan_style, regex_inst, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(green_style, regex_number, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(red_style, regex_directive, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(red_style, regex_directive2, RegexOptions.Multiline);
            e.ChangedRange.SetStyle(orange_style, "[ \t]*(((c|C)'.+')|((x|X)'[a-fA-F0-9]+'))[ \t]*", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(purple_style, "[ \t]*,[ \t]*(X|x)[ \t]*", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(blue_style, "[ \t]*(?!')[a-zA-Z0-9_]+[a-zA-Z0-9_]*(?!')[ \t]*", RegexOptions.Multiline);
        }
        private void guardarToolStripButton_Click(object sender, EventArgs e)
        {
            if (!fileIsOnDisk && filePath != "")
            {
                OpenFileDialog open = new OpenFileDialog
                {
                    FileName = filePath
                };

                if (!open.CheckFileExists)
                {
                    guardarArchivoXE();
                }
                else
                {
                    File.WriteAllText(filePath, editorCodigo.Text);
                }
            }
            
            else if (filePath == "")
            {
                guardarArchivoXE();
            }
        }

        private void nuevoToolStripButton_Click(object sender, EventArgs e)
        {            
            editorCodigo.Clear();
            fileName = "Archivo 1";
            filePath = "";
            //filePath = Application.StartupPath + "\\" + fileName + ".xe";
            fileIsOnDisk = false;
            Text = "Ensamblador["+fileName+".xe]";
            editorCodigo.Enabled = true;
            clearForm();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fileName = "Archivo 1";
            filePath = "";
            fileIsOnDisk = false;
            editorCodigo.Enabled = false;
        }
        private void guardarArchivoXE()
        {
            SaveFileDialog guardarArchivo = new SaveFileDialog
            {
                FileName = fileName,
                DefaultExt = ".xe",
                Filter = "Archivos SicXE (*.xe) | *.xe"
            };            
            if (guardarArchivo.ShowDialog() == DialogResult.OK)
            { 
                filePath = guardarArchivo.FileName;
                filePathNofName = Path.GetDirectoryName(guardarArchivo.FileName);
                fileNameNoExt = Path.GetFileNameWithoutExtension(guardarArchivo.FileName);
                File.WriteAllText(filePath, editorCodigo.Text);
                //string extesion = Path.GetExtension(fileName);
                fileIsOnDisk = true;
                Text = "Ensamblador[" + fileName + ".xe]";
            }
        }
        private void GuardarcomoToolStripButton_Click(object sender, EventArgs e)
        {
            guardarArchivoXE();            
        }

        private void abrirToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog
            {
                InitialDirectory = Application.StartupPath,
                DefaultExt = ".xe"
            };
            //OpenFile.Filter = "Sic-xe files (*.xe)|*.xe|All files (*.*)|*.*";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                var namefile = filePath = OpenFile.FileName;
                fileName = OpenFile.SafeFileName;
                filePathNofName = Path.GetDirectoryName(OpenFile.FileName);
                fileNameNoExt = Path.GetFileNameWithoutExtension(OpenFile.FileName);
                fileName = fileName.Substring(0, fileName.IndexOf("."));//nombre del archivo abierto(sin ext)
                Text = "Ensamblador[" + OpenFile.SafeFileName + "]";
                editorCodigo.Enabled = true;
                try
                {
                    editorCodigo.Clear();
                    string[] lineas = File.ReadAllLines(namefile);
                    foreach (string str in lineas)
                    {
                        editorCodigo.Text += str + Environment.NewLine;
                    }
                    clearForm();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private int obtenerCP()
        {
            return CP;
        }
        private void clearForm()
        {
            dgridArchivo.Rows.Clear();
            panelTabSim.Controls.Clear();
            txtBoxErrores.Text = "";
            tBoxObjFile.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clearForm();
            CP = 0;
            fase = false;
            currentBloqu = 0;
            currentSect = 0;
            dgridArchivo.Rows.Clear();
            secciones.Clear();
            //dgridTabSim.Rows.Clear();
            //dgridTabBloq.Rows.Clear();
            codigo = editorCodigo.Lines;
            Seccion seccion = new Seccion();   
            secciones.Add(seccion);
            List<string[]> listaErrores = new List<string[]>();
            Global.Errores = listaErrores;
            string[] row = new string[8];
            int currentErrorNUmber = Global.Errores.Count;
            for (int i = 0; i < codigo.Count; i++)
            {
                string line = codigo[i];
                if (!string.IsNullOrWhiteSpace(line) && !string.IsNullOrEmpty(line))
                {
                    row[0] = (i + 1).ToString();

                    string[] infor = new string[2];


                    sicxeLexer lex = new sicxeLexer(new AntlrInputStream(line + Environment.NewLine));
                    CommonTokenStream tokens = new CommonTokenStream(lex);
                    //CREAMOS LOS TOKENS SEGUN EL LEXER CREADO
                    sicxeParser parser = new sicxeParser(tokens);
                    parser.RemoveErrorListeners();
                    parser.AddErrorListener(new ErrorListener());
                    parser.buscarEtiq = new sicxeParser.BuscarEnTabSim(buscaTabSim);
                    parser.obtenCP = new sicxeParser.ObtenerValorCP(obtenerCP);
                    parser.convertNum = new sicxeParser.ConvierteNumero(convertirNumero);
                    parser.buscaTipo = new sicxeParser.obtenTipoSimboloTabSim(obtenTipoSimboloTabSim);

                    try
                    {
                        string[] tabBloq = new string[4];
                        Global.CurrentLine = i;

                        if (i == 0)
                        {
                            parser.inicio();
                            tabBloq = generaRenglonTabBloq("0", "Por omision", "0000", "0000");
                            secciones[currentSect].tabBloquesAddRow(tabBloq);
                            
                            //dgridTabBloq.Rows.Add(tabBloq);
                        }
                        /*else if (!line.Contains("END"))
                        {
                            infor = parser.expresion().value;
                        }
                        else
                        {
                            parser.expresion2();
                        }*/

                        else
                        {
                            infor = parser.expresion().value;
                        }

                        string[] tabSimRen = new string[5];
                        agregaFormato(infor, row);

                        row[2] = CP.ToString("X4");                        
                        string[] lineasCod = getListaToken(tokens, lex, row, ref tabSimRen);                        
                        row[4] = lineasCod[0];
                        row[5] = lineasCod[1];
                        row[6] = lineasCod[2];
                        agregaModo(infor, row, ref currentErrorNUmber);


                        if (i != 0 && tabSimRen[0] != null && !row[7].Contains("Error") && !row[5].Contains("CSECT"))
                        {
                            tabSimRen[2] = "R";
                            tabSimRen[4] = "No";
                            if (row[5] == "EQU")
                            {
                                if(row[6] != "*\r\n")
                                {
                                    if (Int32.Parse(infor[4]) > 2000)
                                    {
                                        row[7] += "Error expresión invalida";
                                        tabSimRen[1] = "FFFF";
                                        tabSimRen[2] = "A";

                                    }
                                    else
                                    {
                                        if (infor[4] == "0")
                                        {
                                            tabSimRen[2] = "A";
                                            tabSimRen[1] = infor[3];
                                        }
                                        else if(infor[4] == "1")
                                        {
                                            tabSimRen[2] = "R";
                                            tabSimRen[1] = infor[3];
                                        }
                                        else
                                        {
                                            row[7] += "Error expresión invalida";
                                            tabSimRen[1] = "FFFF";
                                            tabSimRen[2] = "A";
                                        }
                                    }
                                    
                                }
                                
                            }
                            tabSimRen[3] = currentBloqu.ToString("X4");
                            LlenaTabSimGrid(secciones[currentSect], tabSimRen, ref row[7]);                            
                        }
                        calculaCP(currentErrorNUmber, infor);
                        if (row[5] == "START")
                        {
                            secciones[currentSect].nombre = row[4];
                        }
                        if(row[5] == "ORG")
                        {
                            row[6] = row[6].Replace("\r\n", string.Empty);
                            int tempCP = Global.HexHtoInt(row[6]);
                            CP = tempCP;
                        }else if (row[5] == "CSECT")
                        {
                            CalculoBloques cb = new CalculoBloques();
                            cb.modificaLongitudBloque(secciones[currentSect].getTabBloques(), currentBloqu, CP);
                            finalizaTablaBloques(secciones[currentSect].getTabBloques());
                            seccion = new Seccion();
                            seccion.nombre = row[4];
                            tabBloq = generaRenglonTabBloq("0", "Por omision", "0000", "0000");
                            seccion.tabBloquesAddRow(tabBloq);
                            secciones.Add(seccion);                       
                            CP = 0;
                            row[2] = "0000";                            
                            currentBloqu = 0;
                            currentSect++;                            
                        }
                        else if (row[5] == "EXTREF")
                        {
                            string[] param = { "," };
                            string[] simbolExt = row[6].Split(param, StringSplitOptions.RemoveEmptyEntries);

                            foreach(string se in simbolExt)
                            {
                                string sime = se.Replace("\r\n", string.Empty);
                                string[] rengExt = new string[5];
                                rengExt[0] = sime;
                                rengExt[1] = "-";
                                rengExt[2] = "-";
                                rengExt[3] = "-";
                                rengExt[4] = "Si";
                                LlenaTabSimGrid(secciones[currentSect], rengExt, ref row[7]);
                            }
                        }
                        else if(row[5] == "USE")
                        {
                            CalculoBloques cb = new CalculoBloques();
                            cb.modificaLongitudBloque(secciones[currentSect].getTabBloques(), currentBloqu, CP);
                            string nomBlo;
                            if (row[6] != "\r\n")
                            {
                                nomBlo = row[6] = row[6].Replace("\r\n", string.Empty);
                            }

                            else nomBlo = "Por omision";

                            int band = cb.insertaNuevoBloque(nomBlo, 0, 0, secciones[currentSect]);                            
                            if (band == -1)
                            {
                                CP = 0;
                                currentBloqu = cb.regresaCantidadBloques(secciones[currentSect].getTabBloques()) - 1;
                                row[2] = "0000";
                                row[3] = currentBloqu.ToString();

                            }

                            if (band != -1)
                            {
                                currentBloqu = band;
                                CP = cb.regresaLongitudBloque(secciones[currentSect].getTabBloques(), band);
                                row[2] = CP.ToString("X4");
                                row[3] = currentBloqu.ToString();
                            }
                        }
                        else if(row[5] == "END")
                        {
                            CalculoBloques cb = new CalculoBloques();
                            cb.modificaLongitudBloque(secciones[currentSect].getTabBloques(), currentBloqu, CP);
                            currentBloqu = 0;
                            CP = cb.regresaLongitudBloque(secciones[0].getTabBloques(), currentBloqu);
                            row[2] = CP.ToString("X4");
                            row[3] = currentBloqu.ToString();
                            finalizaTablaBloques(secciones[currentSect].getTabBloques());                                            

                        }
                        row[3] = currentBloqu.ToString();
                        llenaGridInterm(row);
                        
                    }
                    catch (RecognitionException ele)
                    {
                        txtBoxErrores.Text = ele.StackTrace;
                    }
                }
            }

            CalculoBloques cd = new CalculoBloques();
            
            pasoDos();
            cargarTablas();
            generarArchivosObjeto();            
            
            
            
        }
        private void generarArchivosObjeto()
        {
            currentSect = 0;
            guardarDocumentoLOC(0);//Lleva tamaño?
            //guardarDocumentoLOC(tamanoFile);
            foreach (Seccion seccion in secciones)
            {
                //int tamanoFile = Convert.ToInt32(seccion.tam, 16);//calculaTamano(seccion);
                CalculaCodigoObjeto archObj = new CalculaCodigoObjeto(dgridArchivo, seccion.getTabSim(), seccion.getTabBloques());
                List<string> registros = archObj.obtenArchivoObjeto(currentSect, ref SEregi);
                llenaTBoxObjFile(registros);                                
                guardarDocumentoObj(registros, seccion.nombre);
                currentSect++;
            }
            guardarDocumentoErrores();
        }
        public void finalizaTablaBloques(DataGridView tablaBloques)
        {
            if (tablaBloques.RowCount > 1)
            {
                for (int i = 1; i < tablaBloques.RowCount; i++)
                {
                    tablaBloques.Rows[i].Cells[2].Value =
                        (Convert.ToInt32((string)tablaBloques.Rows[i - 1].Cells[2].Value, 16) +
                        Convert.ToInt32((string)tablaBloques.Rows[i - 1].Cells[3].Value, 16)).ToString("X4" + "");
                }
            }
        }

        private void iniciarGridInter()
        {
            dgridArchivo.ColumnCount = 9;
            dgridArchivo.Columns[0].Name = "Num";
            dgridArchivo.Columns[1].Name = "Formato";
            dgridArchivo.Columns[2].Name = "CP";
            dgridArchivo.Columns[3].Name = "Bloque";
            dgridArchivo.Columns[4].Name = "ETQ";
            dgridArchivo.Columns[5].Name = "INS";
            dgridArchivo.Columns[6].Name = "OPER";
            dgridArchivo.Columns[7].Name = "Modo";
            dgridArchivo.Columns[8].Name = "Codigo";

        }



        private string[] generaRenglonTabBloq(string numero, string name, string inicio, string lon)
        {
            string[] arr = new string[4];
            arr[0] = numero;
            arr[1] = name;
            arr[2] = inicio;
            arr[3] = lon;

            return arr;

        }

        private void agregaFormato(string[] value, string[] row)
        {
            if (value[1] != null && value[2] == "1")
            {
                row[1] = value[1];
            }
            else
            {
                row[1] = "---";
            }
        }

        private void calculaCP(int currentErrorNUmber, string[] value)
        {
            if (Global.Errores.Count != currentErrorNUmber)
            {
                //currentErrorNUmber = Global.Errores.Count;
            }
            else
            {
                if (value[1] != null)
                {
                    if (value[1].Contains("H"))
                    {
                        string noH = value[1].Remove(value[1].Length - 1, 1);
                        CP += Convert.ToInt32(noH, 16);
                    }
                    else CP += Int32.Parse(value[1]);
                }
            }
        }
        private int convertirNumero(string num)
        {
            int resultado;
            if (num.Contains("H")||num.Contains("h"))
            {
                string noH = num.Remove(num.Length - 1, 1);
                resultado = Convert.ToInt32(noH, 16);
            }
            else resultado = Int32.Parse(num);
            return resultado;
        }
        private string[] getListaToken(CommonTokenStream tok, sicxeLexer Lex, string[] row, ref string[] tabSrow)
        {
            string[] lineaCod = new string[3];
            int cont = tok.GetTokens().Count;

            int tipo = tok.GetTokens()[0].Type;
            int indice = Lex.TokenTypeMap["ETIQ"];
            int n = 1;

            if (indice == tipo)
            {
                lineaCod[0] = tok.GetTokens()[0].Text;
                tabSrow = obtenUnSimbolo(lineaCod[0], row[2]);
                n = 1;

            }
            else
            {
                lineaCod[0] = "---";
                n = 0;
            }

            if (tok.GetTokens()[n].Text == "+")
            {
                lineaCod[1] = tok.GetTokens()[n].Text;
                n++;
                lineaCod[1] += tok.GetTokens()[n].Text;
            }
            else
            {
                lineaCod[1] = tok.GetTokens()[n].Text;
            }

            string operando = "";

            for (int i = n + 1; i < cont; i++)
            {
                if (tok.GetTokens()[i].Text != "<EOF>")
                {
                    operando += tok.GetTokens()[i].Text;
                }
            }

            lineaCod[2] = operando;

            return lineaCod;

        }

        private string[] obtenUnSimbolo(string simbolo, string dir)
        {
            string[] rengTabSim = new string[5];

            rengTabSim[0] = simbolo;
            rengTabSim[1] = dir;

            return rengTabSim;
        }

        private void agregaModo(string[] value, string[] row, ref int currentErrorNUmber)
        {
            if (value[0] != null)
            {
                row[7] = value[0];
            }
            else
            {
                row[7] = "---";
            }

            if (Global.Errores.Count != currentErrorNUmber)
            {
                row[7] += Global.Errores[Global.Errores.Count - 1][0];
                currentErrorNUmber = Global.Errores.Count;
            }
        }

        private void LlenaTabSimGrid(Seccion sec,string[] row, ref string modo)
        {
                        
            int i = 0;
            for (i = 0; i < sec.getTabSim().Rows.Count; i++)
            {
                if ((string)sec.getTabSim().Rows[i].Cells[0].Value == row[0])
                {
                    break;
                }

            }

            if (i == sec.getTabSim().Rows.Count)
            {
                sec.tabSimAddRow(row);                
            }
            else
            {
                modo += "-Error simbolo repetido";
            }

        }

        private void llenaGridInterm(string[] row)
        {
            dgridArchivo.Rows.Add(row);
        }

        private void guardarDocumentoErrores()
        {
            //parser.inicio();
            if (Global.Errores.Count != 0)
            {
                string newFileName = filePathNofName + "\\" + fileNameNoExt + ".err";
                using (StreamWriter writer = new StreamWriter(newFileName))
                {
                    foreach (string[] error in Global.Errores)
                    {
                        string mensErroe = error[0] + error[1];
                        txtBoxErrores.AppendText(mensErroe);
                        txtBoxErrores.AppendText(Environment.NewLine);
                        writer.WriteLine(mensErroe);

                    }

                }
            }




        }

        private int calculaTamano(Seccion sc)//codigo objeto
        {
            int size = 0;
            //int a = Convert.ToInt32((string)dgridArchivo.Rows[dgridArchivo.Rows.Count - 1].Cells[2].Value, 16);
            //int b = Convert.ToInt32((string)dgridArchivo.Rows[0].Cells[2].Value, 16);
                        
            
            int a = Convert.ToInt32((string)sc.getTabBloques().Rows[sc.getTabBloques().RowCount - 1].Cells[2].Value, 16);
            int b = Convert.ToInt32((string)sc.getTabBloques().Rows[sc.getTabBloques().RowCount - 1].Cells[3].Value, 16);
            size = a + b;
            LblProgramSize.Text = size.ToString("X"); 
            return size;
        }

        private void guardarDocumentoLOC(int sizeOfFile)
        {
            string newFileName = filePathNofName + "\\" + fileNameNoExt + ".loc";
            using (StreamWriter writer = new StreamWriter(newFileName))
            {
                string formatoTexto = String.Format("{0,-10}{1,-10}{2,20}{3,20}{4,20}{5,20}{6,20}{7,50}{8,20}",
                            dgridArchivo.Columns[0].Name, dgridArchivo.Columns[1].Name, dgridArchivo.Columns[2].Name, dgridArchivo.Columns[3].Name,
                            dgridArchivo.Columns[4].Name, dgridArchivo.Columns[5].Name, dgridArchivo.Columns[6].Name, dgridArchivo.Columns[7].Name, 
                            dgridArchivo.Columns[8].Name);

                writer.WriteLine(formatoTexto);
                for (int i = 0; i < dgridArchivo.RowCount; i++)
                {
                    string[] renglon = new string[dgridArchivo.ColumnCount];

                    for (int j = 0; j < dgridArchivo.ColumnCount; j++)
                    {
                        renglon[j] = (string)dgridArchivo.Rows[i].Cells[j].Value;
                        if (renglon[j] != null && renglon[j].Contains("\r\n"))
                        {
                            renglon[j] = renglon[j].Remove(renglon[j].Length - 2);
                        }

                        /*writer.Write(s);
                        writer.Write("\t\t");*/
                    }

                    string rengEscr = String.Format("{0,-10}{1,-10}{2,20}{3,20}{4,20}{5,20}{6,20}{7,50}{8,20}",
                            renglon[0], renglon[1], renglon[2], renglon[3], renglon[4], renglon[5], renglon[6], renglon[7],
                            renglon[8]);
                    writer.WriteLine(rengEscr);
                    //writer.Write("\r\n");

                }
                writer.WriteLine("Tablas de simbolos/Bloques");
                foreach (Seccion seccion in secciones)
                {
                    writer.WriteLine("Seccion " + seccion.nombre);
                    writer.WriteLine("Tamaño de la seccion ");
                    writer.WriteLine(seccion.tam+"H Bytes");
                    formatoTexto = String.Format("{0,-20}{1,-20}{2,-20}{3,-20}", seccion.getTabSim().Columns[0].Name,
                        seccion.getTabSim().Columns[1].Name, seccion.getTabSim().Columns[2].Name, seccion.getTabSim().Columns[3].Name);
                    writer.WriteLine(formatoTexto);
                    for (int i = 0; i < seccion.getTabSim().RowCount; i++)
                    {
                        string[] renglon = new string[seccion.getTabSim().ColumnCount];

                        for (int j = 0; j < seccion.getTabSim().ColumnCount; j++)
                        {
                            renglon[j] = (string)seccion.getTabSim().Rows[i].Cells[j].Value;
                            if (renglon[j].Contains("\r\n"))
                            {
                                renglon[j] = renglon[j].Remove(renglon[j].Length - 2);
                            }
                        }

                        string rengEscr = String.Format("{0,-20}{1,-20}{2,-20}{3,-20}",
                                renglon[0], renglon[1], renglon[2], renglon[3]);
                        writer.WriteLine(rengEscr);
                    }
                }                
                //writer.WriteLine("SIZE");
                //writer.WriteLine(sizeOfFile.ToString("X4"));


            }
        }

        private void pasoDos()
        {
            int regBase = 0xFFFF;
            currentSect = 0;
            fase = true;
            SegundoPaso seg = new SegundoPaso();
            int cont = 0;
            DataGridView tabBloques = secciones[cont].getTabBloques();
            bool hasErrorCPB = false;
            for (int i = 0; i < dgridArchivo.Rows.Count - 1; i++)
            {
                int CP = 0;
                SEporRenglon = new List<string>();
                if ((string)dgridArchivo.Rows[i + 1].Cells[3].Value == (string)dgridArchivo.Rows[i].Cells[3].Value)
                {
                    CP = Convert.ToInt32((string)dgridArchivo.Rows[i + 1].Cells[2].Value, 16);
                }
                else
                {
                    CP = buscaSiguienteCPBloque(i);
                }
                int xbpe = 0;
                string inst = (string)dgridArchivo.Rows[i].Cells[5].Value;
                string oper = (string)dgridArchivo.Rows[i].Cells[6].Value;
                string reloc = "";
                if (oper.Contains("\r\n"))
                {
                    oper = oper.Remove(oper.Length - 2);
                }

                string[] listOper = seg.getOperandos(oper);
                string formatoGuardado = "6";
                if (inst.Contains("+"))
                {
                    xbpe = xbpe | 0x01;
                    inst = inst.Remove(0, 1);
                    formatoGuardado = "8";
                }

                int codOp= seg.getCodigo(inst);

                if (codOp != -1)
                {

                    if(inst != "RSUB")
                    {
                        string formato = (string)dgridArchivo.Rows[i].Cells[1].Value;
                        if (formato == "3" || formato == "4")
                        {
                            string modo = (string)dgridArchivo.Rows[i].Cells[7].Value;
                            switch (modo)
                            {
                                case "simple":
                                    {
                                        codOp += 3;
                                        List<string> operandosOrden = new List<string>();

                                        if (listOper.Length > 1)
                                        {
                                            if (listOper[0] != "X") operandosOrden.Add(listOper[0]);
                                            else if (listOper[1] != "X") operandosOrden.Add(listOper[1]);

                                            if (listOper[0] == "X" || listOper[1] == "X")
                                            {
                                                operandosOrden.Add("X");
                                                xbpe = xbpe | 0x08;
                                            }
                                        }

                                        else operandosOrden.Add(listOper[0]);
                                        string keepOperando = operandosOrden[0];
                                        int[] info = evaluarExpresion(operandosOrden[0]);
                                        bool esC = false;
                                        int bandCorM = seg.isM(operandosOrden[0]);
                                        if (info[2] > 0)
                                        {
                                            for(int contaSE=0; contaSE<info[2]; contaSE++)
                                            {
                                                reloc += "*SE";
                                            }

                                            for(int contaR=0; contaR>info[1]; contaR++)
                                            {
                                                reloc += "*R";
                                            }
                                        }
                                        if (info[1]==1 && info[2]==0)
                                        {
                                            operandosOrden[0] = "m";
                                            if (xbpe % 2 != 0) reloc = "*";
                                        }
                                        else if (info[1] == 0)
                                        {
                                            if (info[0] > 4095)
                                            {
                                                operandosOrden[0] = "m";
                                            }
                                            else
                                            {
                                                operandosOrden[0] = "c";
                                                esC = true;
                                            }
                                        }

                                        if (!seg.checkMDSimple(xbpe, operandosOrden) && info[2]==0)
                                        {
                                            codOp = seg.calculoError(xbpe, codOp);
                                            dgridArchivo.Rows[i].Cells[6].Value += "-Error: No existe combinacion MD";
                                        }
                                        else
                                        {
                                            int dir = info[0];

                                            if (info[1] < 2000)
                                            {
                                                codOp = seg.calculaCodigoCompleto(xbpe, codOp, CP, dir, esC, regBase, ref hasErrorCPB, info[2]);
                                            }
                                            else
                                            {
                                                codOp = seg.calculoError(xbpe, codOp);
                                                dgridArchivo.Rows[i].Cells[7].Value += "-Error: No existe el simbolo en TABSIM";
                                            }
                                        }
                                    }

                                    break;
                                case "indirecto":
                                    {
                                        codOp += 2;
                                        oper = oper.Remove(0, 1);
                                        int[] info = evaluarExpresion(oper);
                                        bool esC = false;
                                        int dir = -1;
                                        if (info[1] == 0)
                                        {
                                            esC = true;
                                        }
                                        else if (info[1] == 1 && info[2]==0)
                                        {
                                            if (xbpe % 2 != 0) reloc = "*R";
                                        }

                                        if (info[2] > 0)
                                        {
                                            for (int contaSE = 0; contaSE < info[2]; contaSE++)
                                            {
                                                reloc += "*SE";
                                            }

                                            for (int contaR = 0; contaR < info[1]; contaR++)
                                            {
                                                reloc += "*R";
                                            }
                                        }

                                        dir = info[0];

                                        if (info[1] < 2000)
                                        {
                                            codOp = seg.calculaCodigoCompleto(xbpe, codOp, CP, dir, esC, regBase, ref hasErrorCPB, info[5]);
                                        }
                                        else
                                        {
                                            codOp = seg.calculoError(xbpe, codOp);
                                            dgridArchivo.Rows[i].Cells[7].Value += "-Error: No existe el simbolo en TABSIM";
                                            reloc = "";
                                        }
                                    }
                                    break;
                                case "inmediato":
                                    {
                                        codOp += 1;
                                        oper = oper.Remove(0, 1);
                                        int[] info = evaluarExpresion(oper);
                                        bool esC = false;
                                        int dir = -1;
                                        if (info[1] == 0)
                                        {
                                            esC = true;
                                        }
                                        else if(info[1] == 1 && info[2]==0)
                                        {
                                            if (xbpe % 2 != 0) reloc = "*R";
                                        }

                                        if (info[2] > 0)
                                        {
                                            for (int contaSE = 0; contaSE < info[2]; contaSE++)
                                            {
                                                reloc += "*SE";
                                            }

                                            for (int contaR = 0; contaR < info[1]; contaR++)
                                            {
                                                reloc += "*R";
                                            }
                                        }

                                        dir = info[0];

                                        if (info[1] <2000)
                                        {
                                            codOp = seg.calculaCodigoCompleto(xbpe, codOp, CP, dir, esC, regBase, ref hasErrorCPB, info[2]);
                                        }
                                        else
                                        {
                                            codOp = seg.calculoError(xbpe, codOp);
                                            dgridArchivo.Rows[i].Cells[7].Value += "-Error: No existe el simbolo en TABSIM";
                                            reloc = "";
                                        }

                                    }
                                    break;
                            }
                        }

                        else if (formato == "2")
                        {
                            formatoGuardado = "4";
                            int[] regCod = new int[2];

                            if (listOper.Length > 1)
                            {
                                
                                regCod[0] = seg.getRegistrosCodigo(listOper[0]);
                                regCod[1] = seg.getRegistrosCodigo(listOper[1]);
                                if (regCod[1] == -1)
                                {
                                    Global.checkIfInt(listOper[1], ref regCod[1]);
                                    if (regCod[1] < 1 || regCod[1] > 16)
                                    {
                                        regCod[1] = 0xF;
                                        dgridArchivo.Rows[i].Cells[7].Value += "Error: Operando fuera de rango";
                                    }

                                    else regCod[1] -= 1;
                                }
                            }
                            else
                            {
                                regCod[0] = seg.getRegistrosCodigo(listOper[0]);
                                if (regCod[0] == -1)
                                {
                                    Global.checkIfInt(listOper[0], ref regCod[0]);
                                    if (regCod[0] < 1 || regCod[0] > 16)
                                    {
                                        regCod[0] = 0xF;
                                        dgridArchivo.Rows[i].Cells[7].Value += "Error: Operando fuera de rango";
                                    }

                                    else regCod[0] -= 1;
                                }
                                regCod[1] = 0x00;

                            }

                            codOp = seg.calculaCodigoCompletoFormatoDos(regCod, codOp);

                        }

                        else if (formato == "1")
                        {
                            formatoGuardado = "2";
                        }

                    }
                    else
                    {
                        codOp += 3;
                        if (xbpe % 2 == 0)
                        {
                            
                            codOp = codOp << 16;
                        }
                        else
                        {
                            codOp = codOp << 24;
                        }
                    }



                    dgridArchivo.Rows[i].Cells[8].Value = codOp.ToString("X"+formatoGuardado);
                    if (hasErrorCPB)
                    {
                        dgridArchivo.Rows[i].Cells[7].Value += "-Error: No es relativo al CP o B";
                        hasErrorCPB = false;
                    }
                    else
                    {
                        dgridArchivo.Rows[i].Cells[8].Value += reloc;
                    }
                }

                else if (inst == "WORD")
                {
                    //int hsize = (int)System.Math.Floor((double)listOper[0].Length/2);
                    int[] res = evaluarExpresion(dgridArchivo.Rows[i].Cells[6].Value.ToString());
                    formatoGuardado = "6";
                    //int res = 0;
                    //Global.checkIfInt(listOper[0], ref res);
                    codOp = res[0];
                    string modif = "";
                    if (res[2] > 0)
                    {
                        for (int contaSE = 0; contaSE < res[2]; contaSE++)
                        {
                            modif += "*SE";
                        }

                        for (int contaR = 0; contaR < res[1]; contaR++)
                        {
                            modif += "*R";
                        }
                    }
                    if (res[1] == 1 && res[2]==0)
                    {
                        modif = "*R";
                    }
                    else if (res[1] < 0 || res[1] > 1)
                    {
                        dgridArchivo.Rows[i].Cells[7].Value = "Error: Expresión inválida";
                        codOp = -1;
                    }
                    dgridArchivo.Rows[i].Cells[8].Value = codOp.ToString("X" + formatoGuardado) + modif;
                }

                else if(inst=="BYTE")
                {
                    if(listOper[0][0]=='C')
                    {
                        string carOrN = listOper[0].Remove(0, 2);
                        carOrN = carOrN.Remove(carOrN.Length - 1, 1);
                        int hsize = carOrN.Length;
                        int[] allChar = new int[hsize];

                        for(int j=0; j<hsize; j++)
                        {
                            allChar[j] = carOrN[j];
                        }

                        string result = "";

                        for(int j = 0; j<hsize; j++)
                        {
                            result += allChar[j].ToString("X2");
                        }
                        dgridArchivo.Rows[i].Cells[8].Value = result;
                    }
                    else
                    {
                        string carOrN = listOper[0].Remove(0, 2);
                        carOrN = carOrN.Remove(carOrN.Length - 1, 1);
                        
                        int hsize = (int)System.Math.Ceiling((double)carOrN.Length / 2);
                        carOrN += "H";
                        formatoGuardado = (hsize*2).ToString();
                        int res = 0;
                        Global.checkIfInt(carOrN, ref res);
                        codOp = res;
                        dgridArchivo.Rows[i].Cells[8].Value = codOp.ToString("X" + formatoGuardado);
                    }
                    
                }

                else if (inst == "BASE")
                {
                    bool intento = Global.checkIfInt(listOper[0], ref regBase);
                    if(!intento)
                    {
                        int dir = buscaTabSim(oper);
                        if(dir!=-1) regBase = dir;
                    }
                    dgridArchivo.Rows[i].Cells[8].Value = "---";
                }

                else
                {
                    dgridArchivo.Rows[i].Cells[8].Value = "---";
                }
                if((string)dgridArchivo.Rows[i].Cells[5].Value == "CSECT")
                {
                    currentSect++;                    
                }
                if (SEporRenglon.Count > 0)
                {
                    SEregi.Add(SEporRenglon);
                }

            }            
        }

        public int buscaTabSim(string simbolo)
        {
            int dir = -1;

            for (int i = 0; i < secciones[currentSect].getTabSim().Rows.Count; i++)
            {
                if ((string)secciones[currentSect].getTabSim().Rows[i].Cells[0].Value == simbolo)
                {
                    if ((string)secciones[currentSect].getTabSim().Rows[i].Cells[4].Value == "Si")
                    {
                        dir = 0;
                        break;
                    }
                    dir = Convert.ToInt32((string)secciones[currentSect].getTabSim().Rows[i].Cells[1].Value, 16);
                    if(fase == true && (string)secciones[currentSect].getTabSim().Rows[i].Cells[2].Value != "A")
                    {
                        CalculoBloques cb = new CalculoBloques();
                        int indiceBloque = Convert.ToInt32((string)secciones[currentSect].getTabSim().Rows[i].Cells[3].Value, 16);
                        dir += cb.regresaDirInicoBloque(secciones[currentSect].getTabBloques(), indiceBloque);
                    }
                    break;
                }
            }

            return dir;
        }

        //fase indica si estamos en el paso 1 o 2, false es 1, true es 2
        public int obtenTipoSimboloTabSim(string simbolo, ref int se)
        {
            string tipo = "NA";
            int valor = 10000;
            int i = 0;
            for (i = 0; i < secciones[currentSect].getTabSim().Rows.Count; i++)
            {
                if ((string)secciones[currentSect].getTabSim().Rows[i].Cells[0].Value == simbolo)
                {
                    if(fase == false)
                    {

                        if ((string)secciones[currentSect].getTabSim().Rows[i].Cells[4].Value == "No")
                        {
                            if (Convert.ToInt32((string)secciones[currentSect].getTabSim().Rows[i].Cells[3].Value, 16) == currentBloqu)
                            {
                                tipo = (string)secciones[currentSect].getTabSim().Rows[i].Cells[2].Value;
                            }
                        }
                        
                    }
                    else
                    {

                        if ((string)secciones[currentSect].getTabSim().Rows[i].Cells[4].Value == "No")
                        {
                            tipo = (string)secciones[currentSect].getTabSim().Rows[i].Cells[2].Value;
                        }
                        else
                        {
                            tipo = "A";
                            se = 1;
                            SEporRenglon.Add(simbolo);
                        }
                        
                    }
                    
                    break;
                }
            }

            if (tipo == "R") valor = 1;
            else if (tipo == "A") valor = 0;
            return valor;
        }

        private void guardarDocumentoObj(List<string> reg, string nombre)
        {
            string newFileName = filePathNofName + "\\" + fileNameNoExt+"_"+nombre + ".obj";

            using (StreamWriter writer = new StreamWriter(newFileName))
            {
                foreach(string lineaReg in reg)
                {
                    writer.WriteLine(lineaReg);
                }
            }
        }

        private void LblProgramSize_Click(object sender, EventArgs e)
        {

        }

        private void llenaTBoxObjFile(List<string> registros)
        {
            foreach(string rg in registros)
            {
                tBoxObjFile.AppendText(rg);
                tBoxObjFile.AppendText(Environment.NewLine);
            }
            tBoxObjFile.AppendText(Environment.NewLine);
            tBoxObjFile.AppendText(Environment.NewLine);
        }

        private int[] evaluarExpresion(string exp)
        {
            sicxeLexer lex = new sicxeLexer(new AntlrInputStream(exp + Environment.NewLine));
            CommonTokenStream tokens = new CommonTokenStream(lex);
            sicxeParser parser = new sicxeParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ErrorListener());
            parser.buscarEtiq = new sicxeParser.BuscarEnTabSim(buscaTabSim);
            parser.obtenCP = new sicxeParser.ObtenerValorCP(obtenerCP);
            parser.convertNum = new sicxeParser.ConvierteNumero(convertirNumero);
            parser.buscaTipo = new sicxeParser.obtenTipoSimboloTabSim(obtenTipoSimboloTabSim);
            return parser.exprcalc().value;
        }

        private int buscaSiguienteCPBloque(int linea)
        {
            int conta = 0;
            string BloqueActual = (string)dgridArchivo.Rows[linea].Cells[3].Value;
            for (int i = linea+1; i < dgridArchivo.Rows.Count - 1; i++)
            {
                if((string)dgridArchivo.Rows[i].Cells[3].Value == BloqueActual)
                {
                    conta = Convert.ToInt32((string)dgridArchivo.Rows[i].Cells[2].Value, 16);
                    break;
                }
            }

            return conta;
        }
        private void cargarTablas()
        {
            panelTabSim.Controls.Clear();
            int px=20, py=18;
            foreach(Seccion seccion in secciones)
            {
                seccion.calculaTamSeccion();
                Label nombre = new Label();
                Label tamSeccion = new Label();
                Label info1 = new Label();
                Label info2 = new Label();

                nombre.Text = "Sección " + seccion.nombre;                
                tamSeccion.Text = "Tamaño de la sección: " + seccion.tam + "H Bytes";
                info1.Text = "Tabla de símbolos";
                info2.Text = "Tabla de bloques";

                nombre.Font = new Font("Arial Narrow", 9.75F, FontStyle.Bold);                

                nombre.Location = new Point(px, py);
                nombre.Size = new Size(200,16);
                py += 20;
                tamSeccion.Location = new Point(px, py);
                tamSeccion.Size = new Size(200, 16);
                py += 25;
                info1.Location = new Point(px, py);
                info1.Size = new Size(200, 16);
                py += 16;                
                seccion.getTabSim().Location = new Point(px, py);
                int sx = 282;
                int sy = seccion.getTabSim().Rows.Count * 22 + 22;
                seccion.getTabSim().Size = new Size(sx, sy);                
                py += sy + 20;
                info2.Location = new Point(px, py);
                info2.Size = new Size(200, 16);
                py += 16;
                seccion.getTabBloques().Location = new Point(px, py);
                sy = seccion.getTabBloques().Rows.Count * 22 + 22;
                seccion.getTabBloques().Size = new Size(sx, sy);
                py += sy + 30;

                
                panelTabSim.Controls.Add(nombre);
                panelTabSim.Controls.Add(tamSeccion);
                panelTabSim.Controls.Add(info1);
                panelTabSim.Controls.Add(seccion.getTabSim());
                panelTabSim.Controls.Add(info2);
                panelTabSim.Controls.Add(seccion.getTabBloques());
            }
        }
    }
}
