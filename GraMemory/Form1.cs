using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraMemory
{
    public partial class MemoryForm : Form
    {
        private GameSettings settings;
        MemoryCard pierwsza = null;
        MemoryCard druga = null;

        public MemoryForm()
        {
            InitializeComponent();
            settings = new GameSettings();
            UstawKontrolki();
            GenerujKarty();

            timerCzasPodgladu.Start();
        }

        private void UstawKontrolki()
        {
            panelKart.Height = settings.Bok * settings.Wiersze
                + settings.Margines * (settings.Wiersze - 1);
            panelKart.Width = settings.Bok * settings.Kolumny
                + settings.Margines * (settings.Kolumny - 1);
            
            Width = panelKart.Width + 100;
            Height = panelKart.Height + 120;

            lblCzasWartosc.Text = settings.CzasGry.ToString();
            lblPunktyWartosc.Text = settings.AktualnePunkty.ToString();
            lblStartInfo.Text = $"Początek gry za {settings.CzasPodgladu}";

            lblStartInfo.Visible = true;

        }

        private void GenerujKarty()
        {
            string[] memories = Directory.GetFiles(settings.FolderObrazki);
            settings.MaxPunkty = memories.Length;

            List<MemoryCard> buttons= new List<MemoryCard>();

            foreach(string img in memories)
            {
                Guid id = Guid.NewGuid();
                MemoryCard kart1 = new MemoryCard(id, settings.PlikLogo, img);
                buttons.Add(kart1);
                MemoryCard kart2 = new MemoryCard(id, settings.PlikLogo, img);
                buttons.Add(kart2);
            }
            Random rand = new Random();
            panelKart.Controls.Clear();

            for(int x = 0; x < settings.Kolumny; x++)
            {
                for(int y = 0; y< settings.Wiersze; y++)
                {
                    int index = rand.Next(0, buttons.Count);

                    MemoryCard kart = buttons[index];

                    kart.Location = 
                            new Point((x*settings.Bok) + (settings.Margines * x),
                                   (y * settings.Bok) + (settings.Margines * y));

                    kart.Width = settings.Bok;
                    kart.Height = settings.Bok;

                    kart.Click += BtnClicked;

                    kart.Odkryj();

                    panelKart.Controls.Add(kart);

                    buttons.Remove(kart);

                }
            }
        }

        private void timerCzasPodgladu_Tick(object sender, EventArgs e)
        {
            settings.CzasPodgladu--;
            lblStartInfo.Text = $"Początek gry za {settings.CzasPodgladu}";

            if(settings.CzasPodgladu <= 0)
            {
                lblStartInfo.Visible = false;

                foreach(Control kontrolka in panelKart.Controls)
                {
                    MemoryCard card = (MemoryCard)kontrolka;
                    card.Zakryj();
                }
                timerCzasPodgladu.Stop();
                timerCzasGry.Start();
            }
        }

        private void BtnClicked(object sender, EventArgs e)
        {
            MemoryCard btn = (MemoryCard)sender;
            if(pierwsza == null)
            {
                pierwsza = btn;
                pierwsza.Odkryj();
            }
            else
            {
                druga = btn;
                druga.Odkryj();
                panelKart.Enabled = false;
                if(pierwsza.Id == druga.Id)
                {
                    settings.AktualnePunkty++;
                    lblPunktyWartosc.Text = settings.AktualnePunkty.ToString();

                    pierwsza = null;
                    druga = null;

                    panelKart.Enabled = true;
                }
                else
                {
                    timerZakrywacz.Start();
                }
            }
        }

        private void timerZakrywacz_Tick(object sender, EventArgs e)
        {
            pierwsza.Zakryj();
            druga.Zakryj();

            pierwsza = null;
            druga = null;

            panelKart.Enabled = true;

            timerZakrywacz.Stop();
        }

        private void timerCzasGry_Tick(object sender, EventArgs e)
        {
            settings.CzasGry--;
            lblCzasWartosc.Text = settings.CzasGry.ToString();

            if(settings.CzasGry <= 0 || settings.MaxPunkty == settings.AktualnePunkty)
            {
                timerCzasGry.Stop();
                timerZakrywacz.Stop();

                DialogResult yesNo =
                    MessageBox.Show($"Zdobyte punkty: {settings.AktualnePunkty}. Grasz ponownie?",
                    "Koniec Gry", MessageBoxButtons.YesNo);
                if(yesNo == DialogResult.Yes)
                {
                    settings.UstawStartowe();
                    UstawKontrolki();
                    GenerujKarty();

                    panelKart.Enabled =true;
                    pierwsza = null;
                    druga = null;
                    timerCzasPodgladu.Start();
                }
                else
                {
                    Application.Exit();
                }
            }

        }

        // timerCzasGry_Tick - zmienia co 1s lblCzasWartosc,
        // sprawdza czy czasGry <= 0 lub czy MaxPunkty == AktualnePunkty
        // messageBox wyświetli punky
        // * Zapyta nas czy chcemy kontynuowac gre jezeli tak restartuje gre
        // a jak nie to wyłącza grę 
    }
}
