﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sessao12.Tabuleiro;

namespace Sessao12.Tabuleiro
{
    internal abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Color Color { get; protected set; }
        public int QtdMovimentos { get; protected set; }
        public Tabuleiro Tabuleiro { get; set; }

        public Peca(Color color, Tabuleiro tabuleiro)
        {
            Posicao = null;
            Color = color;
            Tabuleiro = tabuleiro;
            QtdMovimentos = 0;
        }

        public void IncrementarMovimentos()
        {
            QtdMovimentos++;
        }

        public void DecrementarMovimentos()
        {
            QtdMovimentos--;
        }

        public bool ExistemMoveimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();
            for (int i = 0; i < Tabuleiro.Linhas; i++)
            {
                for (int j = 0; j < Tabuleiro.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();

    }
}
