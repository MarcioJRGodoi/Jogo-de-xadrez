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
                try { 
                Console.Clear();
                Tela.ImprimirTabuleiro(partida.Tab);
                Console.WriteLine();
                Console.WriteLine("Turno: "+ partida.Turno);
                Console.WriteLine("Aguardando Jogada: "+ partida.JogadorAtual);

                Console.WriteLine();
                Console.Write("Origem: ");
                Posicao origem = Tela.LerPosicaoXadrez().ToPosicao();
                partida.ValidarPosicaoDeOrigem(origem);
                bool[,] PosicoesPossiveis = partida.Tab.Peca(origem).MovimentosPossiveis();

                Console.Clear();
                Tela.ImprimirTabuleiro(partida.Tab, PosicoesPossiveis);

                Console.WriteLine();
                Console.Write("Destino: ");
                Posicao destino = Tela.LerPosicaoXadrez().ToPosicao();
                partida.ValidarPosicaoDeDestino(origem, destino);


                partida.RealizaJogada(origem, destino);
                }
                catch(TabuleiroException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }
        }
        catch (TabuleiroException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}