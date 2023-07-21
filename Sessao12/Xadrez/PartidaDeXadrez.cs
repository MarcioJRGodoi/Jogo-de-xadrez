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
        public int Turno { get; private set; }
        public Color JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro.Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Color.Branco;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetiraPeca(origem);
            p.IncrementarMovimentos();
            Peca PecaCapturada = Tab.RetiraPeca(destino);
            Tab.ColocarPeca(p, destino);
            if(PecaCapturada != null)
            {
                Capturadas.Add(PecaCapturada);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }


        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if(Tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posicao de origem escolhida!");
            }
            if(JogadorAtual != Tab.Peca(pos).Color)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua");
            }
            if (!Tab.Peca(pos).ExistemMoveimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possiveis para a peça escolhida");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.Peca(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posiçao de destino Invalida!");
            }
        }

        private void MudaJogador()
        {
            if(JogadorAtual == Color.Branco)
            {
                JogadorAtual = Color.Preto;
            }
            else
            {
                JogadorAtual = Color.Branco;
            }
        }

        public HashSet<Peca> PecasCapturadas(Color cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach( Peca x in Capturadas) 
            { 
                if(x.Color == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Color cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Color == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(linha, coluna).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Color.Branco, Tab));
            ColocarNovaPeca('b', 1, new Rei(Color.Branco, Tab));
            ColocarNovaPeca('c', 1, new Torre(Color.Branco, Tab));
            ColocarNovaPeca('d', 1, new Rei(Color.Branco, Tab));

            ColocarNovaPeca('d', 8, new Rei(Color.Preto, Tab));
            ColocarNovaPeca('a', 8, new Torre(Color.Preto, Tab));
            ColocarNovaPeca('b', 8, new Torre(Color.Preto, Tab));
            ColocarNovaPeca('c', 8, new Rei(Color.Preto, Tab));

        }
    }
}
