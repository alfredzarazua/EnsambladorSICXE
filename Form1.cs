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
        int CP;

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
        private String regex_directive2 = "([ \t]|[\n])+(BYTE|WORD|RESB|RESW|BASE)([ \t]+|(\r\n))";
        public Form1()
        {
            InitializeComponent();
            setEditorStyle();
            iniciarGridInter();
            iniciarGridTabSim();
            CP = 0x00;
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
        private void button2_Click(object sender, EventArgs e)
        {
            txtBoxErrores.Text = "";
            tBoxObjFile.Text = "";
            CP = 0;
            dgridArchivo.Rows.Clear();
            dgridTabSim.Rows.Clear();
            codigo = editorCodigo.Lines;
            List<string[]> listaErrores = new List<string[]>();
            Global.Errores = listaErrores;
            string[] row = new string[7];
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

                    try
                    {
                        Global.CurrentLine = i;

                        if (i == 0)
                        {
                            parser.inicio();
                        }
                        else if (!line.Contains("END"))
                        {
                            infor = parser.expresion().value;
                        }
                        else
                        {
                            parser.expresion2();
                        }

                        string[] tabSimRen = new string[3];
                        agregaFormato(infor, row);

                        row[2] = CP.ToString("X4");                        
                        string[] lineasCod = getListaToken(tokens, lex, row, ref tabSimRen);                        
                        row[3] = lineasCod[0];
                        row[4] = lineasCod[1];
                        row[5] = lineasCod[2];
                        agregaModo(infor, row, ref currentErrorNUmber);


                        if (i != 0 && tabSimRen[0] != null && !row[6].Contains("Error"))
                        {
                            tabSimRen[2] = "R";
                            if (row[4] == "EQU")
                            {                                
                                string simb = tokens.GetTokens()[0].Text;
                                tabSimRen = obtenUnSimbolo(simb, infor[3]);//infor[3] contiene la dir/valor de equ, infor[4] la suma para ver el tipo de termino                                                            
                            }
                            LlenaTabSimGrid(tabSimRen, ref row[6]);
                        }
                        calculaCP(currentErrorNUmber, infor);
                        llenaGridInterm(row);
                    }
                    catch (RecognitionException ele)
                    {
                        txtBoxErrores.Text = ele.StackTrace;
                    }
                }
            }

            pasoDos();
            int tamanoFile = calculaTamano();
            CalculaCodigoObjeto archObj = new CalculaCodigoObjeto(dgridArchivo, dgridTabSim);
            List<string> registros = archObj.obtenArchivoObjeto();
            llenaTBoxObjFile(registros);
            guardarDocumentoErrores();
            guardarDocumentoLOC(tamanoFile);
            guardarDocumentoObj(registros);
        }

        //private void cargarTablaSimbolos()
        //{
        //    dgridTabSim.Rows.Clear();
        //    dgridTabSim.Columns.Clear();
        //    dgridTabSim.Columns.Add("col1", "Simbolo");
        //    dgridTabSim.Columns.Add("col2", "Direccion");
        //    dgridTabSim.Columns.Add("col2", "Tipo");

        //    //Codigo para cargar tabla de simbolos aqui
        //    dgridTabSim.ClearSelection();
        //}
        //private void fillDataGridArchivo(List<FileRow> fileRows)
        //{
        //    dgridArchivo.Rows.Clear();
        //    dgridArchivo.Columns.Clear();
        //    string[] encabezados = { "Linea", "Formato", "PC", "Etiqueta", "codOperacion", "Operandos", "Modo Direcc", "Errores/Codigo Obj" };
        //    for (int i = 0; i < 8; i++)
        //        dgridArchivo.Columns.Add("col" + i, encabezados[i]);

        //    foreach (FileRow row in fileRows)
        //    {
        //        dgridArchivo.Rows.Add(row.lineCounter, row.formato, row.cpHex, row.etiqueta, row.opCode, row.operandos, row.mdirec, row.codObjeto_error);
        //    }
        //    dgridArchivo.ClearSelection();
        //}


        private void iniciarGridInter()
        {
            dgridArchivo.ColumnCount = 8;
            dgridArchivo.Columns[0].Name = "Num";
            dgridArchivo.Columns[1].Name = "Formato";
            dgridArchivo.Columns[2].Name = "CP";
            dgridArchivo.Columns[3].Name = "ETQ";
            dgridArchivo.Columns[4].Name = "INS";
            dgridArchivo.Columns[5].Name = "OPER";
            dgridArchivo.Columns[6].Name = "Modo";
            dgridArchivo.Columns[7].Name = "Codigo";

        }

        private void iniciarGridTabSim()
        {
            dgridTabSim.ColumnCount = 3;
            dgridTabSim.Columns[0].Name = "Simbolo";
            dgridTabSim.Columns[1].Name = "Dirección";
            dgridTabSim.Columns[2].Name = "Tipo";
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
            string[] rengTabSim = new string[3];

            rengTabSim[0] = simbolo;
            rengTabSim[1] = dir;

            return rengTabSim;
        }

        private void agregaModo(string[] value, string[] row, ref int currentErrorNUmber)
        {
            if (value[0] != null)
            {
                row[6] = value[0];
            }
            else
            {
                row[6] = "---";
            }

            if (Global.Errores.Count != currentErrorNUmber)
            {
                row[6] += Global.Errores[Global.Errores.Count - 1][0];
                currentErrorNUmber = Global.Errores.Count;
            }
        }

        private void LlenaTabSimGrid(string[] row, ref string modo)
        {
            bool band = false;
            int i = 0;
            for (i = 0; i < dgridTabSim.Rows.Count; i++)
            {
                if ((string)dgridTabSim.Rows[i].Cells[0].Value == row[0])
                {
                    break;
                }

            }

            if (i == dgridTabSim.Rows.Count)
            {
                dgridTabSim.Rows.Add(row);
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

        private int calculaTamano()
        {
            int size = 0;
            int a = Convert.ToInt32((string)dgridArchivo.Rows[dgridArchivo.Rows.Count - 1].Cells[2].Value, 16);
            int b = Convert.ToInt32((string)dgridArchivo.Rows[0].Cells[2].Value, 16);
            size = a - b;
            LblProgramSize.Text = size.ToString("X"); 
            return size;
        }

        private void guardarDocumentoLOC(int sizeOfFile)
        {
            string newFileName = filePathNofName + "\\" + fileNameNoExt + ".loc";
            using (StreamWriter writer = new StreamWriter(newFileName))
            {
                string formatoTexto = String.Format("{0,-20}{1,-20}{2,20}{3,20}{4,20}{5,20}{6,50}{7,20}",
                            dgridArchivo.Columns[0].Name, dgridArchivo.Columns[1].Name, dgridArchivo.Columns[2].Name, dgridArchivo.Columns[3].Name,
                            dgridArchivo.Columns[4].Name, dgridArchivo.Columns[5].Name, dgridArchivo.Columns[6].Name, dgridArchivo.Columns[7].Name);

                writer.WriteLine(formatoTexto);
                for (int i = 0; i < dgridArchivo.RowCount; i++)
                {
                    string[] renglon = new string[8];

                    for (int j = 0; j < 8; j++)
                    {
                        renglon[j] = (string)dgridArchivo.Rows[i].Cells[j].Value;
                        if (renglon[j] != null && renglon[j].Contains("\r\n"))
                        {
                            renglon[j] = renglon[j].Remove(renglon[j].Length - 2);
                        }

                        /*writer.Write(s);
                        writer.Write("\t\t");*/
                    }

                    string rengEscr = String.Format("{0,-20}{1,-20}{2,20}{3,20}{4,20}{5,20}{6,50}{7,20}",
                            renglon[0], renglon[1], renglon[2], renglon[3], renglon[4], renglon[5], renglon[6], renglon[7]);
                    writer.WriteLine(rengEscr);
                    //writer.Write("\r\n");

                }

                writer.WriteLine("TABSIM");

                formatoTexto = String.Format("{0,-20}{1,-20}", dgridTabSim.Columns[0].Name, dgridTabSim.Columns[1].Name);
                writer.WriteLine(formatoTexto);
                for (int i = 0; i < dgridTabSim.RowCount; i++)
                {
                    string[] renglon = new string[2];

                    for (int j = 0; j < 2; j++)
                    {
                        renglon[j] = (string)dgridTabSim.Rows[i].Cells[j].Value;
                        if (renglon[j].Contains("\r\n"))
                        {
                            renglon[j] = renglon[j].Remove(renglon[j].Length - 2);
                        }
                    }

                    string rengEscr = String.Format("{0,-20}{1,-20}",
                            renglon[0], renglon[1]);
                    writer.WriteLine(rengEscr);
                }
                writer.WriteLine("SIZE");
                writer.WriteLine(sizeOfFile.ToString("X4"));


            }
        }

        private void pasoDos()
        {
            int regBase = 0xFFFF;
            SegundoPaso seg = new SegundoPaso();
            
            bool hasErrorCPB = false;
            for (int i = 0; i < dgridArchivo.Rows.Count - 1; i++)
            {
                int CP = Convert.ToInt32((string)dgridArchivo.Rows[i+1].Cells[2].Value, 16);
                int xbpe = 0;
                string inst = (string)dgridArchivo.Rows[i].Cells[4].Value;
                string oper = (string)dgridArchivo.Rows[i].Cells[5].Value;
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
                            string modo = (string)dgridArchivo.Rows[i].Cells[6].Value;
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
                                        bool esC = false;
                                        int bandCorM = seg.isM(operandosOrden[0]);
                                        if (bandCorM != 0)
                                        {
                                            operandosOrden[0] = "m";
                                            if (bandCorM == 2) esC = true;
                                        }
                                        else
                                        {
                                            operandosOrden[0] = "c";
                                            esC = true;
                                        }

                                        if (!seg.checkMDSimple(xbpe, operandosOrden))
                                        {
                                            codOp = seg.calculoError(xbpe, codOp);
                                            dgridArchivo.Rows[i].Cells[6].Value += "-Error: No existe combinacion MD";
                                        }
                                        else
                                        {
                                            int dir = -1;
                                            if (operandosOrden[0] == "m")
                                            {
                                                if (!esC)
                                                {
                                                    dir = buscaTabSim(keepOperando);
                                                    if (xbpe % 2 != 0) reloc = "*";
                                                }
                                                else
                                                {
                                                    Global.checkIfInt(keepOperando, ref dir);
                                                    esC = false;
                                                }

                                            }
                                            else
                                            {
                                                Global.checkIfInt(keepOperando, ref dir);
                                            }
                                            if (dir != -1)
                                            {
                                                codOp = seg.calculaCodigoCompleto(xbpe, codOp, CP, dir, esC, regBase, ref hasErrorCPB);
                                            }
                                            else
                                            {
                                                codOp = seg.calculoError(xbpe, codOp);
                                                dgridArchivo.Rows[i].Cells[6].Value += "-Error: No existe el simbolo en TABSIM";
                                            }
                                        }
                                    }

                                    break;
                                case "indirecto":
                                    {
                                        codOp += 2;
                                        oper = oper.Remove(0, 1);
                                        int bandCorM = seg.isM(oper);
                                        bool esC = false;
                                        int dir = -1;
                                        if (bandCorM == 0)
                                        {
                                            Global.checkIfInt(oper, ref dir);
                                            esC = true;
                                        }
                                        else
                                        {

                                            if (bandCorM == 1)
                                            {
                                                dir = buscaTabSim(oper);
                                                if (xbpe % 2 != 0) reloc = "*";
                                            }
                                            else
                                            {
                                                Global.checkIfInt(oper, ref dir);
                                            }
                                        }

                                        if (dir != -1)
                                        {
                                            codOp = seg.calculaCodigoCompleto(xbpe, codOp, CP, dir, esC, regBase, ref hasErrorCPB);
                                        }
                                        else
                                        {
                                            codOp = seg.calculoError(xbpe, codOp);
                                            dgridArchivo.Rows[i].Cells[6].Value += "-Error: No existe el simbolo en TABSIM";
                                            reloc = "";
                                        }
                                    }
                                    break;
                                case "inmediato":
                                    {
                                        codOp += 1;
                                        oper = oper.Remove(0, 1);
                                        int bandCorM = seg.isM(oper);
                                        bool esC = false;
                                        int dir = -1;
                                        if (bandCorM == 0)
                                        {
                                            Global.checkIfInt(oper, ref dir);
                                            esC = true;
                                        }
                                        else
                                        {

                                            if (bandCorM == 1)
                                            {
                                                dir = buscaTabSim(oper);
                                                if (xbpe % 2 != 0) reloc = "*";
                                            }
                                            else
                                            {
                                                Global.checkIfInt(oper, ref dir);
                                            }
                                        }

                                        if (dir != -1)
                                        {
                                            codOp = seg.calculaCodigoCompleto(xbpe, codOp, CP, dir, esC, regBase, ref hasErrorCPB);
                                        }
                                        else
                                        {
                                            codOp = seg.calculoError(xbpe, codOp);
                                            dgridArchivo.Rows[i].Cells[6].Value += "-Error: No existe el simbolo en TABSIM";
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
                                        dgridArchivo.Rows[i].Cells[6].Value += "Error: Operando fuera de rango";
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
                                        dgridArchivo.Rows[i].Cells[6].Value += "Error: Operando fuera de rango";
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



                    dgridArchivo.Rows[i].Cells[7].Value = codOp.ToString("X"+formatoGuardado);
                    if (hasErrorCPB)
                    {
                        dgridArchivo.Rows[i].Cells[6].Value += "-Error: No es relativo al CP o B";
                        hasErrorCPB = false;
                    }
                    else
                    {
                        dgridArchivo.Rows[i].Cells[7].Value += reloc;
                    }
                }

                else if (inst == "WORD")
                {
                    //int hsize = (int)System.Math.Floor((double)listOper[0].Length/2);
                    formatoGuardado = "6";
                    int res = 0;
                    Global.checkIfInt(listOper[0], ref res);
                    codOp = res;
                    dgridArchivo.Rows[i].Cells[7].Value = codOp.ToString("X" + formatoGuardado);
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
                        dgridArchivo.Rows[i].Cells[7].Value = result;
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
                        dgridArchivo.Rows[i].Cells[7].Value = codOp.ToString("X" + formatoGuardado);
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
                    dgridArchivo.Rows[i].Cells[7].Value = "---";
                }

                else
                {
                    dgridArchivo.Rows[i].Cells[7].Value = "---";
                }

            }
        }

        public int buscaTabSim(string simbolo)
        {
            int dir = -1;

            for (int i = 0; i < dgridTabSim.Rows.Count; i++)
            {
                if ((string)dgridTabSim.Rows[i].Cells[0].Value == simbolo)
                {
                    dir = Convert.ToInt32((string)dgridTabSim.Rows[i].Cells[1].Value, 16);
                    break;
                }
            }

            return dir;
        }

        private void guardarDocumentoObj(List<string> reg)
        {
            string newFileName = filePathNofName + "\\" + fileNameNoExt + ".obj";

            using (StreamWriter writer = new StreamWriter(newFileName))
            {
                foreach(string lineaReg in reg)
                {
                    writer.WriteLine(lineaReg);
                }
            }
        }

        private void llenaTBoxObjFile(List<string> registros)
        {
            foreach(string rg in registros)
            {
                tBoxObjFile.AppendText(rg);
                tBoxObjFile.AppendText(Environment.NewLine);
            }
        }
    }
}
