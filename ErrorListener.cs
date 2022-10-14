using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace EnsambladorSicXE
{
    class ErrorListener : BaseErrorListener
    {
        public static bool tieneError = false;

        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            tieneError = true;
            List<string[]> textoErrores = Global.Errores;


            string nuevoerror, restoMensaje;
            if (msg.Contains("expecting {'+', FORMATO1, FORMATO34, FORMATO2UNREG, FORMATO2DOSREG, FORMATO2REGNUM, 'SVC', 'WORD', 'RESB', 'RESW', 'BASE', 'BYTE', 'RSUB'}") == true)
            {
                nuevoerror = "Error: Instrucción no reconocida ";
                restoMensaje = "\"" + ((IToken)offendingSymbol).Text + "\"" + " en la linea " + (Global.CurrentLine + 1);
            }
            else
            {
                nuevoerror = "Error de Sintaxis";
                restoMensaje = ", no se reconoce el caracter " + "\"" + ((IToken)offendingSymbol).Text + "\"" + " en la linea " + (Global.CurrentLine + 1);
            }
            /*string nuevoerror = "Error Linea " + line + "," + ((IToken)offendingSymbol).Text +
            " no se reconoce";*/
            string[] contenError = new string[2];
            contenError[0] = nuevoerror;
            contenError[1] = restoMensaje;
            textoErrores.Add(contenError);

            Global.Errores = textoErrores;
        }

    }
}
