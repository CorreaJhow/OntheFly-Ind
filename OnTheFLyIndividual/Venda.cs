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
        public double Valor_Total { get; set; }
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
            int assentosOcupados, capacidade;
            double valorPassagem;
            string sql = $"select cpf from Passageiro", parametro, Inscricao, idPassagem;
            Console.WriteLine("### Menu Vendas ###");
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
                // sql = $"SELECT Cpf FROM Cpf_Restrito where Cpf = '{this.Cpf}';";
                //int retorno = banco.VerificarExiste(sql);
                //if (retorno != 0)
                //{
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
                        this.Data_venda = DateTime.Now;
                        Console.WriteLine("Data da venda: " + this.Data_venda);
                        Console.WriteLine("Quantas passagens voce gostaria de adquirir? (Maximo 4 por Venda)");//perguntar quantas passagem!! 
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
                            Console.WriteLine("Informe o Id do Voo desejado (Ex = 'V1234', Caso nao saiba o id do Voo, vá ao menu 'Voo' e consulte o ID!): ");
                            string idVoo = Console.ReadLine();
                            sql = $"select Id from Voo where Id = '{idVoo}';";
                            verificarVoo = banco.VerificarExiste(sql);
                            if (verificarVoo != 0)
                            {
                                #region Coletando Dados da DB
                                sql = $"select AssentosOcupados from Voo where Id = '{idVoo}';";
                                parametro = "AssentosOcupados";
                                assentosOcupados = Convert.ToInt32(ConexaoBD.RetornoDados(sql, conexaosql, parametro));

                                sql = $"select InscricaoAeronave from Voo where Id = '{idVoo}';";
                                parametro = "InscricaoAeronave";
                                Inscricao = ConexaoBD.RetornoDados(sql, conexaosql, parametro);

                                sql = $"select Capacidade from Aeronave where InscricaoANAC = '{Inscricao}';";
                                parametro = "Capacidade";
                                capacidade = Convert.ToInt32(ConexaoBD.RetornoDados(sql, conexaosql, parametro));

                                #endregion            
                                if ((assentosOcupados + contaPassagem) < (capacidade))
                                {
                                    for (int i = 0; i < contaPassagem; i++)
                                    {
                                        this.Id_Venda = RandomCadastroVenda();
                                        Console.WriteLine($"### Cadastro de passagem [{i + 1}]###");
                                        passagem = new PassagemVoo();
                                        banco = new ConexaoBD();
                                        passagem.CadastrarPassagem(conexaosql, idVoo);
                                        assentosOcupados = +1;
                                        sql = $"select Valor from Passagem where IdVoo = '{idVoo}';";
                                        parametro = "Valor";
                                        valorPassagem = Convert.ToDouble(ConexaoBD.RetornoDados(sql, conexaosql, parametro));
                                        this.Valor_Total = valorPassagem * contaPassagem;
                                        sql = $"select Id from Passagem where IdVoo = '{idVoo}';";
                                        parametro = "Id";
                                        idPassagem = ConexaoBD.RetornoDados(sql, conexaosql, parametro);
                                        string sqll = $"insert into PassagemVenda(Id, DataVenda, ValorTotal, IDItemVenda, Passageiro,ValorUnitario, Voo) values ('{this.Id_Venda}', " +
                                        $"'{this.Data_venda}','{this.Valor_Total}','{idPassagem}','{this.Cpf}','{valorPassagem}','{idVoo}');";
                                        banco.InserirDado(conexaosql, sqll);
                                        sqll = $"insert VendaPassageiro(DataVenda, ValorTotal, Cpf) values ('{DateTime.Now}','{this.Valor_Total}','{this.Cpf}');";
                                        banco.InserirDado(conexaosql, sqll);
                                        //AssentosOcupados
                                        string update = $"Update Voo set AssentosOcupados = {assentosOcupados + 1} where Id = '{idVoo}'";
                                        banco.EditarDado(conexaosql, update);
                                        //Capacidade do Voo
                                        update = $"Update Aeronave set Capacidade = {capacidade - 1} where InscricaoANAC = '{Inscricao}'";
                                        banco.EditarDado(conexaosql, update);
                                        Console.WriteLine("\n### Cadastro de Venda com Sucesso ###\nPressione uma tecla para prosseguir!");
                                        Console.ReadKey();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Assentos insulficientes, volte ao menu e informe uma quantidade menor!");
                                    Program.PressioneContinuar();
                                    MenuVenda(conexaosql);
                                }
                            }
                            else
                            {
                                Console.WriteLine("O voo nao foi encontrado, tente novamente depois!");
                                Program.PressioneContinuar();
                                MenuVenda(conexaosql);
                            }
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Nao existem Voos Cadastrados, impossível realizar venda!!");
                            Program.PressioneContinuar();
                            MenuVenda(conexaosql);
                        }
                        break;
                    case 2:
                        MenuVenda(conexaosql);
                        break;
                    default:
                        break;
                }
                //}
                //else
                //{
                //    Console.WriteLine("Venda não pode ser finalizada, o cpf esta restrito!! \nAperte enter para sair.");
                //    Console.ReadKey();
                //}
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
            int[] numero = new int[100];
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
        public void MenuVenda(SqlConnection conexaosql)
        {
            int op;
            do
            {
                Console.Clear();
                Console.WriteLine("Escolha a opção desejada:\n\n[1] Cadastrar\n[2] Localizar\n[3] Editar\n[4] voltar ao menu \n[0] Sair");
                op = int.Parse(Console.ReadLine());
                CompanhiaAerea cia = new CompanhiaAerea();
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
