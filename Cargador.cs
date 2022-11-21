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
        List<Button> deleteB;

        public Cargador()
        {
            files = new List<string>();
            deleteB = new List<Button>();
        } 

        public void addFile(string path)
        {
            files.Add(path);
        }

        public List<string> Files
        {
            get{return files;}

            set{files = value;}
        }
    }

    class CargadorFile
    {
        string fileName;
        Button el;
        Label lName;

        public CargadorFile(string nombre)
        {
            fileName = nombre;
            el = new Button();
            el.Text = "Eliminar";
            lName = new Label();
            lName.Text = fileName;
        }
    }
}
