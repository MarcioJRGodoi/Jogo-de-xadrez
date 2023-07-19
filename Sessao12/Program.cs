using Tabuleiro;

namespace Sessao12
{
    class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro.Tabuleiro tab = new Tabuleiro.Tabuleiro(8,8);

            Tela.ImprimirTabuleiro(tab);
        }
    }
}