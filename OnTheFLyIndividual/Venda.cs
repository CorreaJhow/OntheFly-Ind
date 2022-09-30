using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnTheFLyIndividual;

namespace OnTheFly_BD
{
    internal class Venda
    {
        public int Id_Venda { get; set; }
        public DateTime Data_venda { get; set; }
        public float Valor_Total { get; set; }
        public String Id_Passagem { get; set; }
        public string Cpf { get; set; }
        public ConexaoBD banco = new ConexaoBD();
        public Passageiro passageiro = new Passageiro();
        public PassagemVoo passagem { get; set; }
        public Voo voo = new Voo();
        public Venda() { }

        //public void (SqlConnection conexaosql)
        //{
        //    //verificar bloqueados
        //    //verificar ID passagem
        //    //id voo
        //    //valor passagem
        //    Data_venda = DateTime.Now;
        //    p.ConsultarPassageiro(conexaosql);
        //    this.Cpf = p.Cpf;
        //}
        public void CadastrarVenda(SqlConnection conexaosql)
        {
            Console.WriteLine("### Menu Vendas ###");
            string sql = $"select cpf from Passageiros";
            int verificarCpf = banco.VerificarExiste(sql);
            if (verificarCpf != 0)
            {
                Console.WriteLine("Informe seu CPF: ");
                this.Cpf = Console.ReadLine();
                while (Passageiro.ValidarCpf(this.Cpf) == false || this.Cpf.Length < 11)
                {
                    Console.WriteLine("\nCpf invalido, digite novamente");
                    Console.Write("CPF: ");
                    this.Cpf = Console.ReadLine();
                }
                sql = $"SELECT Cpf FROM Cpf_Restrito where Cpf = '{this.Cpf}';";
                int retorno = banco.VerificarExiste(sql);
                if (retorno != 0)
                {
                    Console.Clear();
                    Console.WriteLine("Gostaria de iniciar uma venda de passagens?\n\n[1] Sim\n[2] Não");
                    int op = int.Parse(Console.ReadLine());
                    while (op != 1 && op != 2)
                    {
                        Console.WriteLine("Valor inválido informado, informe novamente: ");
                        op = int.Parse(Console.ReadLine());
                    }
                    switch (op)
                    {
                        //adicionar Passagem na tabela de venda no banco
                        //Ver a união de tabela. 
                        //testar a classe venda 
                        //fazer a classe itemvenda(dependendo da analise)
                        //conversar com o pestana
                        case 1:
                            this.Id_Venda = RandomCadastroVenda();
                            Console.WriteLine("Id da Venda: " + this.Id_Venda);
                            this.Data_venda = DateTime.Now;
                            Console.WriteLine("Quantas passagens voce gostaria de adquirir? (Maximo 4 por CPF)");//perguntar quantas passagem!! 
                            int contaPassagem = int.Parse(Console.ReadLine());
                            while (contaPassagem < 1 || contaPassagem > 4)
                            {
                                Console.WriteLine("Numero inválido de passagens, insira outro valor: ");
                                contaPassagem = int.Parse(Console.ReadLine());
                            }
                            sql = $"select Id, Situacao from Voo where Situacao = 'A'";
                            int verificarVoo = banco.VerificarExiste(sql);
                            if (verificarVoo != 0)
                            {
                                Console.WriteLine("Informe o Id do Voo desejado (Ex = V1234): ");
                                string idVoo = Console.ReadLine();
                                Console.WriteLine("\n\n\n Caso nao saiba o id do Voo, vá ao menu e consulte o ID!");
                                sql = $"select Id from Voo where Id = '{idVoo}';";
                                verificarVoo = banco.VerificarExiste(sql);
                                if (verificarVoo != 0)
                                {
                                    if ((voo.AssentosOcupados + contaPassagem) < (voo.Aeronave.Capacidade))
                                    {
                                        for (int i = 0; i < contaPassagem; i++)
                                        {
                                            Console.WriteLine($"### Cadastro de passagem [{i}]###");
                                            passagem.CadastrarPassagem(conexaosql, idVoo);
                                            voo.AssentosOcupados = +1;
                                            voo.Aeronave.Capacidade = -1;
                                            this.Valor_Total = passagem.Valor * contaPassagem;
                                            string sqll = $"INSERT INTO Venda(Id, DataVenda, ValorTotal,IdPassagem, Cpf) VALUES ('{this.Id_Venda}', " +
                                            $"'{this.Data_venda}','{this.Valor_Total}','{passagem.IdPassagem}','{this.Cpf}';";
                                            banco = new ConexaoBD();
                                            banco.InserirDado(conexaosql, sqll); //aqui é um registro, informando a venda total
                                            Console.WriteLine("\n>>> Dados da Venda <<<\n\nID");
                                            Console.ReadKey();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Assentos insulficientes, volte ao menu e informe uma quantidade menor!");
                                        Program.PressioneContinuar();
                                        MenuVenda();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("O voo nao foi encontrado, tente novamente depois!");
                                    Program.PressioneContinuar();
                                    MenuVenda();
                                }
                                //while (verificarVoo == 0)
                                //{
                                //    Console.WriteLine("Nao existe voo cadastrado com esse destino, voce deseja escolher outro?" +
                                //        "\n[1] Sim \n[2] Nao ");
                                //    int opc = int.Parse(Console.ReadLine());
                                //    while(opc != 1 && opc != 2)
                                //    {
                                //        Console.WriteLine("Opção inválida, informe novamente: ");
                                //        opc = int.Parse(Console.ReadLine());
                                //    }
                                //    if(opc == 1)
                                //    {
                                //        Console.WriteLine("Informe o destino desejado: ");
                                //        destino = DestinoVoo();
                                //        sql = $"select Id, Destino from Voo where Destino = '{destino}';";
                                //        verificarVoo = banco.VerificarExiste(sql);
                                //    } else 
                                //    {
                                //        MenuVenda();
                                //    }
                                //}

                                //INSIRA A PORRA DO ID VOO


                                break;
                            }
                            else
                            {
                                Console.WriteLine("Nao existem Voos Cadastrados, impossível realizar venda!!");
                                Program.PressioneContinuar();
                                MenuVenda();
                            }
                            break;
                        case 2:
                            MenuVenda();
                            break;
                        default:
                            break;



                    }
                }
                else
                {
                    Console.WriteLine("Venda não pode ser finalizada, o cpf esta restrito!! \nAperte enter para sair.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Venda não pode ser finalizada, Não existem passageiros cadastrados! \nAperte enter para sair.");
                Console.ReadKey();
            }

        }
        private static int RandomCadastroVenda()
        {
            Random rand = new Random();
            int[] numero = new int[99999];
            int aux = 0;
            for (int k = 0; k < numero.Length; k++)
            {
                int rnd = 0;
                do
                {
                    rnd = rand.Next(1000, 9999);
                } while (numero.Contains(rnd));
                numero[k] = rnd;
                aux = numero[k];
            }
            return aux;
        }
        public void MenuVenda()
        {
            int op;
            do
            {
                Console.Clear();
                Console.WriteLine("Escolha a opção desejada:\n\n[1] Cadastrar\n[2] Localizar\n[3] Editar\n[4] voltar ao menu \n[0] Sair");
                op = int.Parse(Console.ReadLine());
                CompanhiaAerea cia = new CompanhiaAerea();
                SqlConnection conexaosql = new SqlConnection();
                switch (op)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        CadastrarVenda(conexaosql);
                        Program.Menu();
                        break;
                    case 2:
                        //localizar tabela venda
                        break;
                    case 3:
                        //editar registro da tabela venda
                        break;
                    case 4:
                        //voltar ao menu 
                        Program.Menu();
                        break;
                    default:
                        break;
                }
            } while (op > 0 && op < 3);
        }
        public string DestinoVoo()
        {
            List<string> destinoVoo = new List<string>();
            destinoVoo.Add("BSB");
            destinoVoo.Add("CGH");
            destinoVoo.Add("GIG");
            Console.WriteLine("Destinos atualmente disponíves: ");
            Console.WriteLine("BSB - Aeroporto Internacional de Brasilia");
            Console.WriteLine("CGH - Aeroporto Internacional de Congonhas/SP");
            Console.WriteLine("GIG - Aeroporto Internacional do Rio de Janeiro");
            Console.WriteLine("");

            Console.Write("Informe a sigla do destino de voo: ");
            String destinoEscolhido = Console.ReadLine().ToUpper();
            return destinoEscolhido;
            //    if (destinoVoo.Contains(destinoEscolhido))
            //    {
            //    }
            //    else
            //    {
            //        Console.WriteLine("Destino inválido, informe novamente!");
            //        Console.WriteLine("");
            //    }
            //return destinoEscolhido;
        }
    }
}
