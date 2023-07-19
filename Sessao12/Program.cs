using Sessao12.Tabuleiro;
using Sessao12.Xadrez;
namespace Sessao12;
class Program
{
    static void Main(string[] args)
    {
        Tabuleiro.Tabuleiro tab = new Tabuleiro.Tabuleiro(8,8);
        try { 
        tab.ColocarPeca(new Torre(Color.Black, tab), new Posicao(0, 3));
        tab.ColocarPeca(new Torre(Color.Black, tab), new Posicao(1, 3));
        tab.ColocarPeca(new Rei(Color.Black, tab), new Posicao(2, 0));

        Tela.ImprimirTabuleiro(tab);
        }catch(TabuleiroException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}