using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Proj_ON_THE_FLY;

namespace OnTheFLyIndividual
{
    internal class PassagemVoo
    {
        public static ConexaoBD conexao = new ConexaoBD();
        public string IdPassagem { get; set; }
        public Voo voo { get; set; } //idVoo
        public DateTime DataUltimaOperacao { get; set; }
        public float Valor { get; set; }
        public string SituacaoPassagem { get; set; }
        public PassagemVoo()
        {

        }
        public PassagemVoo(string id, string idvoo, Aeronave idnave, DateTime dataUltimaOperacao, float valor, string situacao)
        {
            int valorId = GeradorDeID();
            this.IdPassagem = "PA" + valorId.ToString();
            this.voo.Id = idvoo;
            this.DataUltimaOperacao = dataUltimaOperacao;
            this.Valor = valor;
            this.SituacaoPassagem = situacao;
        }
        public int GeradorDeID()
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
        public void CadastrarPassagem(SqlConnection conectar, string idVoo) //variavel qtapassagens 
        {
            Voo voo = new Voo();
            string sql = "Select Id from Voo;";
            int verificar = conexao.VerificarExiste(sql);
            if (verificar != 0)
            {
                int valorId = GeradorDeID();
                this.IdPassagem = "PA" + valorId.ToString();
                Console.WriteLine("Id da passagem definido como: " + IdPassagem);
                voo.Id = idVoo;
                Console.WriteLine("Id de voo:"+voo.Id);
                //sql = "select Id from Voo where Id = '" + voo.Id + "';";
                //verificar = conexao.VerificarExiste(sql);
                //while(verificar == 0)
                //{
                //    Console.WriteLine("Esse voo nao existe, informe outro: ");
                //    voo.Id = Console.ReadLine();
                //    sql = "select Id from Voo where Id = '" + voo.Id + "';";
                //    verificar = conexao.VerificarExiste(sql);
                //}
                this.DataUltimaOperacao = DateTime.Now;
                if(voo.Destino == "BSB")
                {
                    this.Valor = 1500;
                }
                else if (voo.Destino == "CGH")
                {
                    this.Valor = 2500;
                }
                else if (voo.Destino == "GIG")
                {
                    this.Valor = 3000;
                }
                Console.WriteLine("valor da passagem: " + this.Valor);
                //Console.WriteLine("Informe o valor da Passagem: ");
                //this.Valor = float.Parse(Console.ReadLine());
                //while(this.Valor < 999.99 || this.Valor > 9999.99)
                //{
                //    Console.WriteLine("Valor inválido de passagem, informe outro valor: ");
                //    this.Valor = float.Parse(Console.ReadLine());
                //}
                Console.WriteLine("Informe a situação da passagem (P - Paga | R - Reservada): ");
                this.SituacaoPassagem = Console.ReadLine().ToUpper();
                while(!this.SituacaoPassagem.Equals("P") && !this.SituacaoPassagem.Equals("R"))
                {
                    Console.WriteLine("Valor informado inválido, informe novamente:");
                    this.SituacaoPassagem = Console.ReadLine().ToUpper();
                }
                Console.WriteLine("Situação da passagem: "+this.SituacaoPassagem);
                string query = $"insert into Passagem(Id, IdVoo, DataUltimaOperacao, Valor, Situacao) values " +
                    $"('{this.IdPassagem}','{voo.Id}','{this.DataUltimaOperacao}','{this.Valor}','{this.SituacaoPassagem}');";
                conexao.InserirDado(conectar,query);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Desculpa, impossível cadastrar passagem, pois nao temos nenhuma voo Cadastrado");
                Console.WriteLine("Pressione uma tecla para prosseguir");
                Console.ReadKey();
            }
        }
        //Atualizar/Editar Passagem
        public void AtualizarPassagem(SqlConnection conexaosql)
        {
            int opc = 0;
            Console.Clear();
            Console.WriteLine("### Atualizar Passagem ###");
            Console.WriteLine("Informe o ID da passagem (PA0000 – Dois dígitos PA, seguidos de 4 dígitos numéricos:");
            string id = Console.ReadLine();
            string query = "select Id, IdVoo from Passagem where id = '" + id + "';";
            int verificar = conexao.VerificarExiste(query);
            if (verificar == 0)
            {
                Console.WriteLine("Passagem nao localizada!!\nPressione alguma tecla pra prosseguir.");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("### Alterar Passagem ###");
                Console.WriteLine("\nDigite a Opção que Deseja Editar");
                Console.WriteLine("[1] Ultima Data Alterada");
                Console.WriteLine("[2] Situacao");
                Console.WriteLine("[3] Valor");
                Console.Write("\nDigite: ");
                opc = int.Parse(Console.ReadLine());
                while (opc < 1 || opc > 3)
                {
                    Console.WriteLine("\nDigite uma Opcao Valida:");
                    Console.Write("\nDigite: ");
                    opc = int.Parse(Console.ReadLine());
                }
                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("### Alterar Passagem ###");
                        Console.WriteLine("Data alterada com sucesso! ");
                        this.DataUltimaOperacao = DateTime.Now;
                        string sql = "update Passagem set DataUltimaOperacao = '" + this.DataUltimaOperacao + "' where Id = '" + id + "';";
                        conexao.EditarDado(conexaosql, sql);
                        Program.PressioneContinuar();
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("### Alterar Passagem ###");
                        Console.WriteLine("Informe a nova situacao (L – Livre, R – Reservada ou P – Paga): ");
                        this.SituacaoPassagem = Console.ReadLine();
                        while(this.SituacaoPassagem != "L" && this.SituacaoPassagem != "R" && this.SituacaoPassagem != "P")
                        {
                            Console.WriteLine("Situacao de passagem inválida informada, digite o novamente: ");
                            this.SituacaoPassagem = Console.ReadLine();
                        }
                        sql = "update Passagem set Situacao = '" + this.DataUltimaOperacao + "' where Id = '" + id + "';";
                        conexao.EditarDado(conexaosql, sql);
                        Program.PressioneContinuar();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("### Alterar Passagem ###");
                        Console.WriteLine("informe o novo valor da passagem: ");
                        this.Valor = float.Parse(Console.ReadLine());
                        while (this.Valor < 100 || this.Valor > 999.99)
                        {
                            Console.WriteLine("Valor inválido informado, tente novamente: ");
                            this.Valor = float.Parse(Console.ReadLine());
                        }
                        sql = "update Passagem set Valor = '" + this.Valor + "' where Id = '" + id + "';";
                        conexao.EditarDado(conexaosql, sql);
                        Program.PressioneContinuar();
                        break;
                    default:
                        break;
                }
            }
        }
        //LocalizarPassagem
        public void LocalizarPassagem(SqlConnection conexaosql)
        {
            Console.Clear();
            Console.WriteLine("### Localizar Passagem ###");
            Console.WriteLine("Informe o ID da passagem (PA0000 – Dois dígitos PA, seguidos de 4 dígitos numéricos:");
            string id = Console.ReadLine();
            string query = "select Id, IdVoo from Passagem where id = '" + id + "';";
            int verificar = conexao.VerificarExiste(query);
            if(verificar == 0)
            {
                Console.WriteLine("Passagem nao localizada!!\nPressione alguma tecla pra prosseguir.");
                Console.ReadKey();
            }
            else
            {
                string sql = "select Id, IdVoo, DataUltimaOperacao, Valor, Situacao from Passagem where id = '"+id+"';";
                conexao.LocalizarDado(conexaosql, sql,5);
            }
        }
        //RegistroPorRegistro
        public void RegistroPorRegistro(SqlConnection conecta)
        {
            List<string> Passagem = new();
            conecta.Open();
            string sql = "Select Id, IdVoo, DataUltimaOperacao, Valor, Situacao from Passagem";
            SqlCommand cmd = new SqlCommand(sql, conecta);
            SqlDataReader reader = null;
            using (reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n\t### Passagem Localizada ###\n");
                while (reader.Read())
                {
                        Passagem.Add(reader.GetString(0));
                }
            }
            conecta.Close();
            for (int i = 0; i < Passagem.Count; i++)
            {
                string op;
                do
                {
                    Console.Clear();
                    Console.WriteLine(">>> Passagem <<<\nDigite para navegar:\n[1] Próximo Cadasatro\n[2] Cadastro Anterior" +
                        "\n[3] Último cadastro\n[4] Voltar ao Início\n[s] Sair\n");
                    Console.WriteLine($"Cadastro [{i + 1}] de [{Passagem.Count}]");
                    //Imprimi o primeiro da lista
                    string query = "Select Id, IdVoo, DataUltimaOperacao, Valor, Situacao from Passagem where Id = '" + Passagem[i] + "';";
                    conexao.LocalizarDado(conecta, query, 5);
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
                        i = Passagem.Count - 1;
                    //Volta para o inicio da lista
                    else if (op.Contains("4"))
                        i = 0;
                    //Vai para o próximo da lista
                } while (op != "1");
                if (i == Passagem.Count - 1)
                    i--;
            }
        }
    }
}
