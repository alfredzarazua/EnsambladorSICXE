using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnsambladorSicXE
{
    class Global
    {
        private static List<string[]> _errores;
        private static int _currenLine;

        public static List<string[]> Errores
        {
            get
            {
                return _errores;
            }

            set
            {
                _errores = value;
            }
        }

        public static int CurrentLine
        {
            get
            {
                return _currenLine;
            }

            set
            {
                _currenLine = value;
            }
        }

        public static int HexHtoInt(string cadena)
        {
            int numero = 0;
            if (cadena.Contains("H"))
            {
                cadena = cadena.Remove(cadena.Length - 1, 1);
                numero = Convert.ToInt32(cadena, 16);
            }
            else numero = int.Parse(cadena);

            return numero;

        }

        public static bool checkIfInt(string cadena, ref int result)
        {
            bool band = false;
            int numero;
            if (cadena.Contains("H"))
            {
                cadena = cadena.Remove(cadena.Length - 1, 1);
                band = int.TryParse(cadena, NumberStyles.HexNumber, null, out result);
                //numero = Convert.ToInt32(cadena, 16);
            }
            else band = int.TryParse(cadena, out result);

            return band;
        }

        public static void validarExpresionRA(DataGridView TabSim, string expre)
        {

        }

        public static bool terminoRelativo(DataGridView TabSim, string simbolo)
        {
            bool tipo = true;
            for (int i = 0; i < TabSim.Rows.Count; i++)
            {
                if ((string)TabSim.Rows[i].Cells[0].Value == simbolo)
                {
                    if ((string)TabSim.Rows[i].Cells[2].Value == "A") tipo = false;
                    break;
                }
            }

            return tipo;
        }
    }
}
