using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Sessao12.Tabuleiro;

namespace Sessao12.Xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro.Tabuleiro Tab { get; private set; }
        private int Turno;
        private Color JogadorAtual;
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro.Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Color.White;
            Terminada = false;
            ColocarPecas();
        }

        public void ExeutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetiraPeca(origem);
            p.IncrementarMovimentos();
            Peca PecaCapturada = Tab.RetiraPeca(destino);
            Tab.ColocarPeca(p, destino);
        }

        private void ColocarPecas()
        {
            Tab.ColocarPeca(new Torre(Color.White, Tab), new PosicaoXadrez(1, 'c').ToPosicao());
            Tab.ColocarPeca(new Torre(Color.White, Tab), new PosicaoXadrez(2, 'c').ToPosicao());

            Tab.ColocarPeca(new Torre(Color.Black, Tab), new PosicaoXadrez(8, 'c').ToPosicao());

        }
    }
}
