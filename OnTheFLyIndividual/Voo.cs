using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using Proj_ON_THE_FLY;

namespace OnTheFLyIndividual
{
    internal class Voo
    {
        public static ConexaoBD conexao = new ConexaoBD();
        public String Id { get; set; }
        public string Destino { get; set; }
        public Aeronave Aeronave { get; set; }
        public int AssentosOcupados { get; set; }
        public DateTime DataVoo { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }
        public Voo()
        {

        }
        public Voo(string destino, DateTime dataVoo, DateTime dataCadastro, char situacaoVoo)
        {
            int valorId = RandomCadastroVoo();
            this.Id = "V" + valorId.ToString();
            this.Destino = destino;
            this.Aeronave = null;
            this.DataVoo = dataVoo;
            this.DataCadastro = dataCadastro;
            this.Situacao = situacaoVoo;
        }
        public string ImprimirVoo()
        {
            return "### Dados do Vôo ###\nID: " + Id +
                "\nDestino: " + Destino +
                "\nAeronave: " + Aeronave +
                "\nDataVôo: " + DataVoo +
                "\nDataCadastro: " + DataCadastro +
                "\nSituacao do Vôo: " + Situacao;
        }
        private int RandomCadastroVoo()
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
        public void CadastrarVoo(SqlConnection conexaosql)
        {
            Aeronave aeronave = new Aeronave();
            string sql = "Select InscricaoANAC from Aeronave;";
            int verificar = conexao.VerificarExiste(sql);
            if (verificar != 0)
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Bem vindo ao cadastro de voo.");
                Console.WriteLine("-----------------------------");
                int valorId = RandomCadastroVoo();
                this.Id = "V" + valorId.ToString("D4");
                Destino = DestinoVoo();
                Console.WriteLine("Insira o nome do aeronave: ");
                aeronave.Inscricao = Console.ReadLine();
                sql = "select InscricaoANAC from Aeronave where InscricaoANAC = '" + aeronave.Inscricao + "';";
                verificar = conexao.VerificarExiste(sql);
                while (verificar == 0)
                {
                    Console.WriteLine("Essa aeronave nao existe, informe outra: ");
                    aeronave.Inscricao = Console.ReadLine();
                    sql = "select InscricaoANAC from Aeronave where InscricaoANAC = '" + aeronave.Inscricao + "';";
                    verificar = conexao.VerificarExiste(sql);
                }
                Console.WriteLine("Informe a data e hora do Voo: (dd/MM/yyyy hh:mm) ");
                this.DataVoo = DateTime.Parse(Console.ReadLine());
                if (DataVoo <= DateTime.Now)
                {
                    Console.WriteLine("Essa data é inválida, informe novamente: ");
                    DataVoo = DateTime.Parse(Console.ReadLine());
                }
                this.DataCadastro = DateTime.Now;
                this.AssentosOcupados = 0;
                Console.WriteLine("Data de cadastro definifida como: " + DataCadastro);
                Console.WriteLine("Informe a situacao do Voo: \n[A] Ativo \n[C] Cancelado");
                Situacao = char.Parse(Console.ReadLine().ToUpper());
                while (Situacao != 'A' && Situacao != 'C')
                {
                    Console.WriteLine("O valor informado é inválido, por favor informe novamente!\n[A] Ativo \n[C] Cancelado");
                    Situacao = char.Parse(Console.ReadLine().ToUpper());
                }
                sql = "insert into Voo (Id, InscricaoAeronave, DataCadastro, AssentosOcupados, Situacao, DataVoo, Destino) values ('" + this.Id + "', '" + aeronave.Inscricao + "', '" +
                this.DataCadastro + "', '" + this.AssentosOcupados + "', '" + this.Situacao + "', '" + this.DataVoo + "', '" + this.Destino + "');";
                conexao.InserirDado(conexaosql, sql);
                Console.WriteLine("Inscrição de Voo realizada com sucesso!");
                Console.WriteLine("Pressione uma tecla para prosseguir");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Desculpa, impossível cadastrar voo, pois nao temos nenhuma aeronave Ativa Cadastrada");
                Console.WriteLine("Pressione uma tecla para prosseguir");
                Console.ReadKey();
            }
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
            do
            {
                Console.Write("Informe a sigla do destino de voo: ");
                String destinoEscolhido = Console.ReadLine().ToUpper();
                if (destinoVoo.Contains(destinoEscolhido))
                {
                    return destinoEscolhido;
                }
                else
                {
                    Console.WriteLine("Destino inválido, informe novamente!");
                    Console.WriteLine("");
                }
            } while (true);
        }
        public void AtualizarVoo(SqlConnection sqlConnection)
        {
            string query = $"select Id from Voo";
            int verificarVoo = conexao.VerificarExiste(query);
            if (verificarVoo != 0)
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Atualizar dados Voo");
                Console.WriteLine("Insira o id do Voo que deseja alterar (Exemplo = 'V1234'): ");
                string idVoo = Console.ReadLine();
                string sql = "Select Id from Voo where Id = '" + idVoo + "';";
                int verificar = conexao.VerificarExiste(sql);
                if (verificar != 0)
                {
                    Console.WriteLine("Escolha o que voce deseja atualizar: \n[1] Destino \n[2] Aeronave \n[3] Data de Voo \n[4] Situação do Voo");
                    int op = int.Parse(Console.ReadLine());
                    while (op < 1 || op > 4)
                    {
                        Console.WriteLine("Opcao INVALIDA, informe um valor válido: ");
                        op = int.Parse(Console.ReadLine());
                    }
                    switch (op)
                    {
                        case 1:
                            Console.WriteLine("Qual o novo destino que deseja informar?: ");
                            string destino = DestinoVoo();
                            sql = "update Voo set Destino = '" + destino + "' where Id = '" + idVoo + "';";
                            conexao.EditarDado(sqlConnection, sql);
                            Console.WriteLine("Alteração efetuada com sucesso!");
                            break;
                        case 2:
                            Console.WriteLine("Qual a nova aeronave que deseja informar?: ");
                            string aeronave = Console.ReadLine();
                            sql = $"select InscricaoAeronave from Voo where InscricaoAeronave = '{aeronave}'";
                            int verificaAeronave = conexao.VerificarExiste(sql);
                            if (verificaAeronave != 0)
                            {
                                sql = $"update Voo set InscricaoAeronave = '{aeronave}' where Id = '{idVoo}';";
                                conexao.EditarDado(sqlConnection, sql);
                                Console.WriteLine("Alteração efetuada com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("Essa Aeronave não existe ou nao esta cadastrada em nenhum voo, tente novamente depois!");
                                Program.PressioneContinuar();
                            }
                            break;
                        case 3:
                            Console.WriteLine("Qual a nova data de voo que deseja informar?: ");
                            DateTime dataVoo = DateTime.Parse(Console.ReadLine());
                            if (dataVoo <= DateTime.Now)
                            {
                                Console.WriteLine("Essa data é inválida, informe novamente: ");
                                dataVoo = DateTime.Parse(Console.ReadLine());
                            }
                            sql = "update Voo set DataVoo = '" + dataVoo + "' where Id = '" + idVoo + "';";
                            conexao.EditarDado(sqlConnection, sql);
                            break;
                        case 4:
                            Console.WriteLine("Qual a nova situacão do voo que deseja informar?: \n[A] Ativo \n[C] Cancelado ");
                            char situacao = char.Parse(Console.ReadLine().ToUpper());
                            while (situacao != 'A' && situacao != 'C')
                            {
                                Console.WriteLine("O valor informado é inválido, por favor informe novamente!\n[A] Ativo \n[C] Cancelado");
                                situacao = char.Parse(Console.ReadLine().ToUpper());

                            }
                            sql = "update Voo set Situacao = '" + situacao + "' where Id = '" + idVoo + "';";
                            conexao.EditarDado(sqlConnection, sql);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Valor de ID de voo não encontrado!!");
                    Console.WriteLine("Pressione uma tecla para Prosseguir...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Cadastre um Voo antes de de atualizar!");
                Program.PressioneContinuar();
            }
        }
        public void LocalizarVoo(SqlConnection sqlConnection)
        {
            string query = $"select Id from Voo";
            int verificarVoo = conexao.VerificarExiste(query);
            if (verificarVoo != 0)
            {

                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("### Localizar dados Voo ###");
                Console.WriteLine("Insira o id do Voo que deseja alterar (Exemplo = 'V1234'): ");
                string idVoo = Console.ReadLine();
                string sql = "Select Id from Voo where Id = '" + idVoo + "';";
                int verificar = conexao.VerificarExiste(sql);
                if (verificar != 0)
                {
                    sql = "Select Id, InscricaoAeronave, DataVoo, DataCadastro, Destino, Situacao from Voo where Id = '" + idVoo + "';";
                    conexao.LocalizarDado(sqlConnection, sql, 2);
                    Console.WriteLine("Pressione enter para prosseguir...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("O voo nao foi encontrado!");
                    Console.WriteLine("Pressione enter para prosseguir...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Cadastre um Voo antes de de localizar!!");
                Program.PressioneContinuar();
            }
        }
        public void RegistroPorRegistro(SqlConnection conecta)
        {
            string query = $"select Id from Voo";
            int verificarVoo = conexao.VerificarExiste(query);
            if (verificarVoo != 0)
            { 
                List<string> voo = new();
            conecta.Open();
            string sql = "Select Id, InscricaoAeronave, DataVoo, DataCadastro, Destino, Situacao from Voo";
            SqlCommand cmd = new SqlCommand(sql, conecta);
            SqlDataReader reader = null;
            using (reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n\t### Voo Localizado ###\n");
                while (reader.Read())
                {
                    if (reader.GetString(5) == "A")
                    {
                        voo.Add(reader.GetString(0));
                    }
                }
            }
            conecta.Close();
            for (int i = 0; i < voo.Count; i++)
            {
                string op;
                do
                {
                    Console.Clear();
                    Console.WriteLine(">>> Voos <<<\nDigite para navegar:\n[1] Próximo Cadasatro\n[2] Cadastro Anterior" +
                        "\n[3] Último cadastro\n[4] Voltar ao Início\n[s] Sair\n");
                    Console.WriteLine($"Cadastro [{i + 1}] de [{voo.Count}]");
                    //Imprimi o primeiro da lista
                    query = "Select Id, InscricaoAeronave, DataVoo, DataCadastro, Destino, Situacao from Voo where Id = '" + voo[i] + "';";
                    conexao.LocalizarDado(conecta, query, 2);
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
                        i = voo.Count - 1;
                    //Volta para o inicio da lista
                    else if (op.Contains("4"))
                        i = 0;
                    //Vai para o próximo da lista
                } while (op != "1");
                if (i == voo.Count - 1)
                    i--;
            }
            }
            else
            {
                Console.Clear();
                Program.CabecalhoOntheFly();
                Console.WriteLine("Cadastre um Voo antes de de percorrer registros!!");
                Program.PressioneContinuar();
            }
        }
    }
}