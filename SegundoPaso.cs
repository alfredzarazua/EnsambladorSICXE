using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsambladorSicXE
{
    class SegundoPaso
    {

        Dictionary<string, int> codigo;
        Dictionary<string, int> registros;
        public SegundoPaso()
        {
            codigo = new Dictionary<string, int>();
            registros = new Dictionary<string, int>();
            _initCodes();
        }

        public void calculaSegundoPaso()
        {

        }

        public int getCodigo(string inst)
        {
            int code = -1;
            if (codigo.ContainsKey(inst))
            {
                code = codigo[inst];
            }

            return code;
        }

        public bool esRelativoCP(ref int desp, int CP, int TA)
        {
            bool band = false;

            if (TA - CP < -2048 || TA - CP > 2047)
            {
                desp = 0xFFF;
            }
            else
            {
                desp = TA - CP;
                band = true;
            }

            return band;
        }

        public bool esRelativoBASE(ref int desp, int bas, int TA)
        {
            bool band = false;

            if (TA - bas < 0 || TA - bas > 4095)
            {
                desp = 0xFFF;
            }
            else
            {
                desp = TA - bas;
                band = true;
            }

            return band;
        }

        private void _initCodes()
        {
            codigo.Add("ADD", 0x18);
            codigo.Add("ADDF", 0x58);
            codigo.Add("ADDR", 0x90);
            codigo.Add("AND", 0x40);
            codigo.Add("CLEAR", 0xB4);
            codigo.Add("COMP", 0x28);
            codigo.Add("COMPF", 0x88);
            codigo.Add("COMPR", 0xA0);
            codigo.Add("DIV", 0x24);
            codigo.Add("DIVF", 0x64);
            codigo.Add("DIVR", 0x9C);
            codigo.Add("FIX", 0xC4);
            codigo.Add("FLOAT", 0xC0);
            codigo.Add("HIO", 0xF4);
            codigo.Add("J", 0x3C);
            codigo.Add("JEQ", 0x30);
            codigo.Add("JGT", 0x34);
            codigo.Add("JLT", 0x38);
            codigo.Add("JSUB", 0x48);
            codigo.Add("LDA", 0x00);
            codigo.Add("LDB", 0x68);
            codigo.Add("LDCH", 0x50);
            codigo.Add("LDF", 0x70);
            codigo.Add("LDL", 0x08);
            codigo.Add("LDS", 0x6C);
            codigo.Add("LDT", 0x74);
            codigo.Add("LDX", 0x04);
            codigo.Add("LPS", 0xD0);
            codigo.Add("MUL", 0x20);
            codigo.Add("MULF", 0x60);
            codigo.Add("MULR", 0x98);
            codigo.Add("NORM", 0xC8);
            codigo.Add("OR", 0x44);
            codigo.Add("RD", 0xD8);
            codigo.Add("RMO", 0xAC);
            codigo.Add("RSUB", 0x4C);
            codigo.Add("SHIFTL", 0xA4);
            codigo.Add("SHIFTR", 0xA8);
            codigo.Add("SIO", 0xF0);
            codigo.Add("SSK", 0xEC);
            codigo.Add("STA", 0x0C);
            codigo.Add("STB", 0x78);
            codigo.Add("STCH", 0x54);
            codigo.Add("STF", 0x80);
            codigo.Add("STI", 0xD4);
            codigo.Add("STL", 0x14);
            codigo.Add("STS", 0x7C);
            codigo.Add("STSW", 0xE8);
            codigo.Add("STT", 0x84);
            codigo.Add("STX", 0x10);
            codigo.Add("SUB", 0x1C);
            codigo.Add("SUBF", 0x5C);
            codigo.Add("SUBR", 0x94);
            codigo.Add("SVC", 0xB0);
            codigo.Add("TD", 0xE0);
            codigo.Add("TIO", 0xF8);
            codigo.Add("TIX", 0x2C);
            codigo.Add("TIXR", 0xB8);
            codigo.Add("WD", 0xDC);

            registros.Add("A", 0x00);
            registros.Add("X", 0x01);
            registros.Add("L", 0x02);
            registros.Add("B", 0x03);
            registros.Add("S", 0x04);
            registros.Add("T", 0x05);
            registros.Add("F", 0x06);
            registros.Add("CP", 0x08);
            registros.Add("SW", 0x09);
        }

        public string[] getOperandos(string op)
        {
            string[] res = new string[1];
            if (op.Contains(","))
            {

                char[] del = { ',' };
                res = op.Split(del);
            }
            else res[0] = op;

            return res;
        }

        public int isM(string ope)
        {
            int operan = 0;
            int ban = 0;
            bool isC = Global.checkIfInt(ope, ref operan);
            if (isC)
            {

                if (operan > 4095) ban = 2;
                else if (operan >= 0 && operan < 4096) ban = 0;
            }
            else ban = 1;


            return ban;

        }

        public bool checkMDSimple(int xbpe, List<string> operandos)
        {
            bool ban = false;
            if (xbpe % 2 != 0)
            {
                if (operandos.Count > 1)
                {
                    if (operandos[0] == "m" && operandos[1] == "X") ban = true;
                }

                else
                {
                    if (operandos[0] == "m") ban = true;
                }
            }
            else ban = true;

            return ban;

        }

        public int calculoError(int xbpe, int instCod)
        {
            int finalCodObj = 0;
            if (xbpe % 2 == 0)
            {
                finalCodObj = 0x000000;
                xbpe = xbpe | 0x04; xbpe = xbpe | 0x02;
                int desp = 0xFFF;

                instCod = instCod << 16;
                xbpe = xbpe << 12;
                finalCodObj = finalCodObj | instCod;
                finalCodObj = finalCodObj | xbpe;
                finalCodObj = finalCodObj | desp;
            }
            else
            {
                finalCodObj = 0x00000000;
                xbpe = xbpe | 0x04; xbpe = xbpe | 0x02;
                int desp = 0xFFFFF;

                instCod = instCod << 24;
                xbpe = xbpe << 20;
                finalCodObj = finalCodObj | instCod;
                finalCodObj = finalCodObj | xbpe;
                finalCodObj = finalCodObj | desp;
            }

            return finalCodObj;
        }

        public int calculaCodigoCompleto(int xbpe, int instCod, int CP, int operando, bool C, int regBase,
            ref bool Error)
        {
            int desp = 0;
            int finalCodObj = 0;
            if (xbpe % 2 == 0)
            {
                desp = 0xFFF;
                finalCodObj = 0x000000;
                if (C)
                {
                    if(operando<0 || operando > 4095)
                    {
                        Error = true;
                        xbpe = xbpe | 0x04; xbpe = xbpe | 0x02;
                    }
                    else desp = operando;
                }
                else
                {
                    if (esRelativoCP(ref desp, CP, operando))
                    {
                        xbpe = xbpe | 0x02;
                        if (desp < 0)
                        {
                            desp = Math.Abs(desp);
                            desp = desp ^ 0xFFF;
                            desp = desp + 1;
                        }

                    }
                    else if (esRelativoBASE(ref desp, regBase, operando))
                    {
                        xbpe = xbpe | 0x04;
                    }
                    else
                    {
                        xbpe = xbpe | 0x04; xbpe = xbpe | 0x02;
                        Error = true;
                    }

                    
                }

                if (desp == 0xFFF) desp = 0xFFF;
                instCod = instCod << 16;
                xbpe = xbpe << 12;
                finalCodObj = finalCodObj | instCod;
                finalCodObj = finalCodObj | xbpe;
                finalCodObj = finalCodObj | desp;

            }
            else
            {
                desp = 0xFFFFF;
                if (!C)
                {
                    desp = operando;
                }
                else
                {
                    if (operando > 4095)
                    {
                        desp = operando;
                    }
                    else
                    {
                        xbpe = xbpe | 0x04; xbpe = xbpe | 0x02;
                        Error = true;
                    }
                    
                }

                instCod = instCod << 24;
                xbpe = xbpe << 20;
                finalCodObj = finalCodObj | instCod;
                finalCodObj = finalCodObj | xbpe;
                finalCodObj = finalCodObj | desp;
            }

            return finalCodObj;


        }

        public int getRegistrosCodigo(string reg)
        {
            int code = -1;
            if (registros.ContainsKey(reg))
            {
                code = registros[reg];
            }


            return code;
        }

        public int calculaCodigoCompletoFormatoDos(int[] regoper, int cod)
        {
            cod = cod << 8;
            regoper[0] = regoper[0] << 4;

            int codigoFinal = 0x0000;
            codigoFinal = codigoFinal | cod;
            codigoFinal = codigoFinal | regoper[0];
            codigoFinal = codigoFinal | regoper[1];

            return codigoFinal;
        }

    }
}
