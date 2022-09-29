using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using OnTheFly_BD;
using Proj_ON_THE_FLY;

namespace OnTheFLyIndividual
{
    internal class Program
    {
        static Voo voo = new Voo();
        static PassagemVoo passagem = new PassagemVoo();
        static Aeronave aeronave = new Aeronave();
        static CompanhiaAerea cia = new CompanhiaAerea();
        static void Main(string[] args)
        {
            Menu();
        }
        public static void Menu()
        {
            ConexaoBD cnx = new ConexaoBD();
            SqlConnection conexaosql = new SqlConnection(cnx.Caminho());
            Console.Clear();
            CabecalhoOntheFly();
            Console.WriteLine("Escolha a opção desejada:\n\n[1] Vender Passagem\n[2] Cliente\n[3] Cia.Aérea\n[4] Vôos\n[5] Aviões\n[0] Sair");
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
                    Passagem();
                    break;
                case 2:
                    //Cliente();
                    break;
                case 3:
                    cia.MenuCiaAerea(conexaosql);
                    break;
                case 4:
                    Voos();
                    break;
                case 5:
                    Aeronave();
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
                CabecalhoOntheFly();
                Console.WriteLine("### Menu de Voo ###");
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
                        PressioneContinuar();
                        break;
                    case 3:
                        voo.LocalizarVoo(conexaosql);
                        PressioneContinuar();
                        break;
                    case 4:
                        voo.AtualizarVoo(conexaosql);
                        PressioneContinuar();
                        break;
                    case 5:
                        voo.RegistroPorRegistro(conexaosql);
                        PressioneContinuar();
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
        public static void PressioneContinuar()
        {
            Console.WriteLine("Pressione uma tecla para continuar: ");
            Console.ReadKey();
        }
        public static void Passagem()
        {
            do
            {
                ConexaoBD cnx = new ConexaoBD();
                SqlConnection conexaosql = new SqlConnection(cnx.Caminho());
                Console.Clear();
                CabecalhoOntheFly();
                Console.WriteLine("### Menu de Passagem ###");
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
                        //cadastrar
                        PressioneContinuar();
                        break;
                    case 3:
                        //localizar
                        PressioneContinuar();
                        break;
                    case 4:
                        //editar
                        PressioneContinuar();
                        break;
                    case 5:
                        //imprimir por registro 
                        PressioneContinuar();
                        break;
                    default:
                        break;
                }
            } while (true);
        }
        public static void Aeronave()
        {
            do
            {
                ConexaoBD cnx = new ConexaoBD();
                SqlConnection conexaosql = new SqlConnection(cnx.Caminho());
                Console.Clear();
                CabecalhoOntheFly();
                Console.WriteLine("### Menu de Aeronave ###");
                Console.WriteLine("Escolha a opção desejada:\n\n[1] Voltar ao Menu anterior\n[2] Cadastrar\n[3] Localizar\n[4] Editar\n[5] Deletar \n[0] Sair");
                int op = int.Parse(Console.ReadLine());
                while (op < 0 || op > 5)
                {
                    Console.WriteLine("Opção inválida, informe novamente: ");
                    Console.WriteLine("Escolha a opção desejada:\n\n[1] Voltar ao Menu anterior\n[2] Cadastrar\n[3] Localizar\n[4] Editar\n[5] Deletar \n[0] Sair");
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
                        aeronave.CadastroAeronaves(conexaosql);
                        PressioneContinuar();
                        break;
                    case 3:
                        aeronave.LocalizarAeronave(conexaosql);
                        PressioneContinuar();
                        break;
                    case 4:
                        aeronave.EditarAeronave(conexaosql);
                        PressioneContinuar();
                        break;
                    case 5:
                        aeronave.DeletarAeronave(conexaosql);
                        PressioneContinuar();
                        break;
                    default:
                        break;
                }
            } while (true);
        }
    }
}
