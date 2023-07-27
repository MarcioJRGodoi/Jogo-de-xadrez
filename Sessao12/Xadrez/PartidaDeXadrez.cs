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
        public bool Xeque { get; private set; }
        public Peca VulneravelEnPassant { get; private set; }
        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro.Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Color.Branco;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetiraPeca(origem);
            p.IncrementarMovimentos();
            Peca PecaCapturada = Tab.RetiraPeca(destino);
            Tab.ColocarPeca(p, destino);
            if (PecaCapturada != null)
            {
                Capturadas.Add(PecaCapturada);
            }

            // # Jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemDaTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoDaTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetiraPeca(origemDaTorre);
                T.IncrementarMovimentos();
                Tab.ColocarPeca(T, destinoDaTorre);
            }

            // # Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemDaTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoDaTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetiraPeca(origemDaTorre);
                T.IncrementarMovimentos();
                Tab.ColocarPeca(T, destinoDaTorre);
            }

            // # Jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && PecaCapturada == null)
                {
                    Posicao posP;
                    if (p.Color == Color.Branco)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    PecaCapturada = (Tab.RetiraPeca(posP));
                    Capturadas.Add(PecaCapturada);
                }
            }


            return PecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetiraPeca(destino);
            p.DecrementarMovimentos();
            if (pecaCapturada != null)
            {
                Tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            Tab.ColocarPeca(p, origem);

            // # Jogada especial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemDaTorre = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoDaTorre = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetiraPeca(destinoDaTorre);
                T.DecrementarMovimentos();
                Tab.ColocarPeca(T, origemDaTorre);
            }

            // # Jogada especial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemDaTorre = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoDaTorre = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetiraPeca(destinoDaTorre);
                T.DecrementarMovimentos();
                Tab.ColocarPeca(T, origemDaTorre);
            }

            // # Jogada especial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == VulneravelEnPassant)
                {
                    Peca peao = Tab.RetiraPeca(destino);
                    Posicao posP;
                    if (p.Color == Color.Branco)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    peao.DecrementarMovimentos();
                    Tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);
            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Voce não pode se colocar em xeque!");
            }

            if (EstaEmXeque(Adversario(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }
            if (TesteXequemate(Adversario(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }

            Peca p = Tab.Peca(destino);
            // # Jogada especial en passant

            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = p;
            }
            else
            {
                VulneravelEnPassant = null;
            }
        }


        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posicao de origem escolhida!");
            }
            if (JogadorAtual != Tab.Peca(pos).Color)
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
            if (!Tab.Peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posiçao de destino Invalida!");
            }
        }

        private void MudaJogador()
        {
            if (JogadorAtual == Color.Branco)
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
            foreach (Peca x in Capturadas)
            {
                if (x.Color == cor)
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

        private Color Adversario(Color cor)
        {
            if (cor == Color.Branco)
            {
                return Color.Preto;
            }
            else
            {
                return Color.Branco;
            }
        }

        private Peca Rei(Color cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Color cor)
        {
            Peca R = Rei(cor);
            if (R == null)
            {
                throw new TabuleiroException($"Não Existe um rei da cor {cor} na partida");
            }
            foreach (Peca x in PecasEmJogo(Adversario(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequemate(Color cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Linhas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(linha, coluna).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Color.Branco, Tab));
            ColocarNovaPeca('b', 1, new Cavalo(Color.Branco, Tab));
            ColocarNovaPeca('c', 1, new Bispo(Color.Branco, Tab));
            ColocarNovaPeca('e', 1, new Rei(Color.Branco, Tab, this));
            ColocarNovaPeca('d', 1, new Dama(Color.Branco, Tab));
            ColocarNovaPeca('h', 1, new Torre(Color.Branco, Tab));
            ColocarNovaPeca('g', 1, new Cavalo(Color.Branco, Tab));
            ColocarNovaPeca('f', 1, new Bispo(Color.Branco, Tab));
            ColocarNovaPeca('a', 2, new Peao(Color.Branco, Tab, this));
            ColocarNovaPeca('b', 2, new Peao(Color.Branco, Tab, this));
            ColocarNovaPeca('c', 2, new Peao(Color.Branco, Tab, this));
            ColocarNovaPeca('d', 2, new Peao(Color.Branco, Tab, this));
            ColocarNovaPeca('e', 2, new Peao(Color.Branco, Tab, this));
            ColocarNovaPeca('f', 2, new Peao(Color.Branco, Tab, this));
            ColocarNovaPeca('g', 2, new Peao(Color.Branco, Tab, this));
            ColocarNovaPeca('h', 2, new Peao(Color.Branco, Tab, this));


            ColocarNovaPeca('a', 8, new Torre(Color.Preto, Tab));
            ColocarNovaPeca('b', 8, new Cavalo(Color.Preto, Tab));
            ColocarNovaPeca('c', 8, new Bispo(Color.Preto, Tab));
            ColocarNovaPeca('e', 8, new Rei(Color.Preto, Tab, this));
            ColocarNovaPeca('d', 8, new Dama(Color.Preto, Tab));
            ColocarNovaPeca('h', 8, new Torre(Color.Preto, Tab));
            ColocarNovaPeca('g', 8, new Cavalo(Color.Preto, Tab));
            ColocarNovaPeca('f', 8, new Bispo(Color.Preto, Tab));
            ColocarNovaPeca('a', 7, new Peao(Color.Preto, Tab, this));
            ColocarNovaPeca('b', 7, new Peao(Color.Preto, Tab, this));
            ColocarNovaPeca('c', 7, new Peao(Color.Preto, Tab, this));
            ColocarNovaPeca('d', 7, new Peao(Color.Preto, Tab, this));
            ColocarNovaPeca('e', 7, new Peao(Color.Preto, Tab, this));
            ColocarNovaPeca('f', 7, new Peao(Color.Preto, Tab, this));
            ColocarNovaPeca('g', 7, new Peao(Color.Preto, Tab, this));
            ColocarNovaPeca('h', 7, new Peao(Color.Preto, Tab, this));

        }
    }
}
