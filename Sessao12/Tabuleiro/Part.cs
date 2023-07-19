using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabuleiro;

namespace Tabuleiro
{
    internal class Part
    {
        public Posicao Posicao { get; set; }
        public Color Color { get; protected set; }
        public int QtdMovimentos { get; protected set; }
        public Tabuleiro Tabuleiro { get; set; }

        public Part(Posicao posicao, Color color, Tabuleiro tabuleiro)
        {
            Posicao = posicao;
            Color = color;
            Tabuleiro = tabuleiro;
            QtdMovimentos = 0;
        }
    }
}
