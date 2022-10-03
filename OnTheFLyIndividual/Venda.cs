using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
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
        public void CadastrarVenda(SqlConnection conexaosql)
        {
            int assentosOcupados, capacidade;
            double valorPassagem;
            string sql = $"select cpf from Passageiro", parametro, Inscricao, idPassagem;
            Console.Clear();
            Program.CabecalhoOntheFly();
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
                sql = $"select cpf from Passageiro where cpf = '{this.Cpf}';";
                int verificarCliente = banco.VerificarExiste(sql);
                if (verificarCliente != 0)
                {
                    sql = $"select cpf from cpf_Restrito where cpf = '{this.Cpf}'";
                    int cpfRestrito = banco.VerificarExiste(sql);
                    if (cpfRestrito == 0)
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
                            case 1:
                                this.Data_venda = DateTime.Now;
                                Console.WriteLine("Data da venda: " + this.Data_venda);
                                Console.WriteLine("Quantas passagens voce gostaria de adquirir? (Maximo 4 por Venda)");
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
                                                string update = $"Update Voo set AssentosOcupados = {assentosOcupados + 1} where Id = '{idVoo}'";
                                                banco.EditarDado(conexaosql, update);
                                                update = $"Update Aeronave set Capacidade = {capacidade - 1} where InscricaoANAC = '{Inscricao}'";
                                                banco.EditarDado(conexaosql, update);
                                                update = $"Update Passageiro set UltimaCompra = '{DateTime.Now}' where Cpf = '{this.Cpf}'";
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
                    }
                    else
                    {
                        Console.WriteLine("O CPF informado esta restrito!! Enquanto estiver nessa situação, nao seguiremos com o cadastro! \nAperte uma tecla para sair.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Nao existem Clientes cadastrados com esse cpf, impossível realizar venda!!");
                    Program.PressioneContinuar();
                    MenuVenda(conexaosql);
                }
            }
            else
            {
                Console.WriteLine("Venda não pode ser finalizada, Não existem passageiros cadastrados! \nAperte uma tecla para sair.");
                Console.ReadKey();
            }
        }
        private int RandomCadastroVenda()
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
                Program.CabecalhoOntheFly();
                Console.WriteLine("Escolha a opção desejada:\n\n[1] Cadastrar\n[2] Localizar\n[3] Deletar\n[4] Registro por Registro \n[5] Voltar ao Menu anterior \n[0] Sair do Programa");
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
                        LocalizarVenda(conexaosql);
                        Program.Menu();
                        break;
                    case 3:
                        DeletarVenda(conexaosql);
                        Program.Menu();
                        break;
                    case 4:
                        RegistroPRegistroVenda(conexaosql);
                        Program.Menu();
                        break;
                    case 5:
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
        public void LocalizarVenda(SqlConnection conexaosql)
        {
            string queryVenda = $"select Id from PassagemVenda";
            int verificarVoo = banco.VerificarExiste(queryVenda);
            if (verificarVoo != 0)
            {
                Console.Clear();
            Program.CabecalhoOntheFly();
            Console.WriteLine("### Localizar Venda ###");
            Console.WriteLine("Informe o ID da venda (4 dígitos numéricos):");
            int id = int.Parse(Console.ReadLine());
            string query = "select Id from PassagemVenda where Id = '" + id + "';";
            int verificar = banco.VerificarExiste(query);
            if (verificar == 0)
            {
                Console.WriteLine("Passagem nao localizada!!\nPressione alguma tecla pra prosseguir.");
                Console.ReadKey();
            }
            else
            {
                string sql = "Select Id, DataVenda, Passageiro, ValorTotal, Voo, IDItemVenda, ValorUnitario from PassagemVenda where id = '" + id + "';";
                banco.LocalizarDado(conexaosql, sql, 7);
                Program.PressioneContinuar();
            }
            }
            else
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Cadastre uma venda antes de localizar!!!");
                Program.PressioneContinuar();
            }
        }
        public void DeletarVenda(SqlConnection conexaosql)
        {
            string queryVenda = $"select Id from PassagemVenda";
            int verificarVoo = banco.VerificarExiste(queryVenda);
            if (verificarVoo != 0)
            {
                Console.Clear();
            Program.CabecalhoOntheFly();
            Console.WriteLine("### Atualizar Venda ###");
            Console.WriteLine("Informe o ID da venda (4 dígitos numéricos):");
            int id = int.Parse(Console.ReadLine());
            string query = "select Id from PassagemVenda where Id = '" + id + "';";
            int verificar = banco.VerificarExiste(query);
            if (verificar == 0)
            {
                Console.WriteLine("Passagem nao localizada!!\nPressione alguma tecla pra prosseguir.");
                Console.ReadKey();
            }
            else
            {
                string sql = "Select Id, DataVenda, Passageiro, ValorTotal, Voo, IDItemVenda, ValorUnitario from PassagemVenda where id = '" + id + "';";
                banco.LocalizarDado(conexaosql, sql, 7);
                string delete = "delete from PassagemVenda where Id = '" + id + "';";
                banco.DeletarDado(conexaosql, delete);
                Console.WriteLine("### Venda removida com sucesso ###");
                Program.PressioneContinuar();
            }
            }
            else
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Cadastre uma venda antes de deletar!!");
                Program.PressioneContinuar();
            }
        }
        public void RegistroPRegistroVenda(SqlConnection conecta)
        {
            string queryVenda = $"select Id from PassagemVenda";
            int verificarVoo = banco.VerificarExiste(queryVenda);
            if (verificarVoo != 0)
            {
                List<string> venda = new();
            conecta.Open();
            string sql = "Select Id, DataVenda, Passageiro, ValorTotal, Voo, IDItemVenda, ValorUnitario from PassagemVenda";
            SqlCommand cmd = new SqlCommand(sql, conecta);
            SqlDataReader reader = null;
            using (reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n\t### Venda Localizada ###\n");
                while (reader.Read())
                {
                    venda.Add(Convert.ToString(reader.GetInt32(0)));
                }
            }
            conecta.Close();
            for (int i = 0; i < venda.Count; i++)
            {
                string op;
                do
                {
                    Console.Clear();
                    Console.WriteLine("### Venda ###\nDigite para navegar:\n[1] Próximo Cadasatro\n[2] Cadastro Anterior" +
                        "\n[3] Último cadastro\n[4] Voltar ao Início\n[s] Sair\n");
                    Console.WriteLine($"Cadastro [{i + 1}] de [{venda.Count}]");
                    //Imprimi o primeiro da lista
                    string query = "Select Id, DataVenda, Passageiro, ValorTotal, Voo, IDItemVenda, ValorUnitario from PassagemVenda where Id = '" + venda[i] + "';";
                    banco.LocalizarDado(conecta, query, 7);
                    Console.Write("Opção: ");
                    op = Console.ReadLine();
                    if (op != "1" && op != "2" && op != "3" && op != "4" && op != "s")
                    {
                        Console.WriteLine("Opção inválida!");
                        Thread.Sleep(2000);
                    }
                    //Sai do método
                    else if (op.Contains("s"))
                        return;
                    //Volta no Cadastro Anterior
                    else if (op.Contains("2"))
                        if (i == 0)
                            i = 0;
                        else
                            i--;
                    //Vai para o fim da lista
                    else if (op.Contains("3"))
                        i = venda.Count - 1;
                    //Volta para o inicio da lista
                    else if (op.Contains("4"))
                        i = 0;
                    //Vai para o próximo da lista
                } while (op != "1");
                if (i == venda.Count - 1)
                    i--;
            }
            }
            else
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Cadastre uma venda antes de percorrer registros!!");
                Program.PressioneContinuar();
            }
        }
    }
}
