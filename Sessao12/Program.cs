using Sessao12.Tabuleiro;
using Sessao12.Xadrez;
namespace Sessao12;
class Program
{
    static void Main(string[] args)
    {
        try
        {
            PartidaDeXadrez partida = new PartidaDeXadrez();

            while (!partida.Terminada)
            {
                Console.Clear();
                Tela.ImprimirTabuleiro(partida.Tab);
                Console.WriteLine();
                Console.Write("Origem: ");
                Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                Console.Write("Destino: ");
                Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();

                partida.ExeutaMovimento(origem, destino);
            }
        }
        catch (TabuleiroException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}