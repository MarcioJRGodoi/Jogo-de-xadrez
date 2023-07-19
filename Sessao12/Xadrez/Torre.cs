using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sessao12.Tabuleiro;

namespace Sessao12.Xadrez
{
    internal class Torre : Peca
    {
        public Torre(Color color, Tabuleiro.Tabuleiro tabuleiro) : base(color, tabuleiro)
        {
        }

        public override string ToString()
        {
            return "T";
        }
    }
}
