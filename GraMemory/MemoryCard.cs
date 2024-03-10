﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraMemory
{
    public class MemoryCard : Label
    {
        public Guid Id;
        public Image Tyl;
        public Image Obrazek;

        public MemoryCard(Guid id, string tylPath, string obrazekPath)
        {
            Id = id;
            Tyl = Image.FromFile(tylPath);
            Obrazek = Image.FromFile(obrazekPath);
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        public void Zakryj()
        {
            BackgroundImage = Tyl;
            Enabled = true;
        }

        public void Odkryj()
        {
            BackgroundImage = Obrazek;
            Enabled = false;
        }
    }
}
