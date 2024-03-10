using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraMemory
{
    public class GameSettings
    {
        public int CzasGry;
        public int CzasPodgladu;
        public int MaxPunkty;
        public int Wiersze;
        public int Kolumny;
        public int Bok;
        public int Margines;
        public int AktualnePunkty;
        public string PlikLogo = $@"{AppDomain.CurrentDomain.BaseDirectory}\obrazki\logo.jpg";
        public string FolderObrazki = $@"{AppDomain.CurrentDomain.BaseDirectory}\obrazki\memory";


        public GameSettings()
        {
            UstawStartowe();
        }

        public void UstawStartowe()
        {
            CzasGry = 60;
            CzasPodgladu = 5;
            MaxPunkty = 0;
            Wiersze = 4;
            Kolumny = 6;
            Bok = 120;
            Margines = 2;
            AktualnePunkty = 0;
        }
    }
}
