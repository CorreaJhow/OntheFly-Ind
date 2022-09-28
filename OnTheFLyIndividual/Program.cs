using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace OnTheFLyIndividual
{
    internal class Program
    {
        static Voo voo = new Voo();
        static void Main(string[] args)
        {
            Menu();
        }
        public static void Menu()
        {
            Console.Clear();
            CabecalhoOntheFly();
            Console.WriteLine("Escolha a opção desejada:\n\n[1] Vender Passagem\n[2] Cliente\n[3] Cia.Aérea\n[4] Destinos\n[5] Vôos\n[6] Aviões\n[0] Sair");
            int op = int.Parse(Console.ReadLine());
            while (op < 0 || op > 6)
            {
                Console.WriteLine("Opção Inválida inserida, informe novamente outro valor: ");
                op = int.Parse(Console.ReadLine());
            }
            switch (op)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    break;
                case 2:
                    //Cliente();
                    break;
                case 3:
                    //CiaAerea();
                    break;
                case 4:
                    //Destinos();
                    break;
                case 5:
                    Voos();
                    break;
                case 6:
                    //Avioes();
                    break;
                default:
                    break;
            }
        }
        public static void Voos()
        {
            do
            {

                ConexaoBD cnx = new ConexaoBD();
                SqlConnection conexaosql = new SqlConnection(cnx.Caminho());
                Console.Clear();
                Console.WriteLine("Escolha a opção desejada:\n\n[1] Voltar ao Menu anterior\n[2] Cadastrar\n[3] Localizar\n[4] Editar\n[5] Imprimir por registro\n[0] Sair");
                int op = int.Parse(Console.ReadLine());
                while (op < 0 || op > 5)
                {
                    Console.WriteLine("Opção inválida, informe novamente: ");
                    Console.WriteLine("Escolha a opção desejada:\n\n[1] Voltar ao Menu anterior\n[2] Cadastrar\n[3] Localizar\n[4] Editar\n[5] Imprimir por registro\n[0] Sair");
                    op = int.Parse(Console.ReadLine());
                }
                switch (op)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        Menu();
                        break;
                    case 2:
                        voo.CadastrarVoo(conexaosql);
                        break;
                    case 3:
                        voo.LocalizarVoo(conexaosql);
                        break;
                    case 4:
                        voo.AtualizarVoo(conexaosql);
                        break;
                    case 5:
                        //imprimir dado a dado (ativos) 
                        break;
                    default:
                        break;
                }
            } while (true);
        }

        public static void CabecalhoOntheFly()
        {
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("                                                      On the Fly");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
        }
    }
}
