using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheFLyIndividual
{
    internal class Voo
    {
        public static ConexaoBD conexao = new ConexaoBD();
        public String Id { get; set; }
        public string Destino { get; set; }
        public Aeronave Aeronave { get; set; }
        public DateTime DataVoo { get; set; }
        public DateTime DataCadastro { get; set; }
        public char Situacao { get; set; }
        public Voo()
        {

        }
        public Voo(string destino, DateTime dataVoo, DateTime dataCadastro, char situacaoVoo)
        {
            int valorId = RandomCadastroVoo();
            this.Id = "V" + valorId.ToString("D4");
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
        private static int RandomCadastroVoo()
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
            //bool existeAeronave = aeronave.ExisteAeronave();
            //if (!existeAeronave)
            //{
            Console.Clear();
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Bem vindo ao cadastro de voo.");
            Console.WriteLine("-----------------------------");
            int valorId = RandomCadastroVoo();
            this.Id = "V" + valorId.ToString("D4");
            Destino = DestinoVoo();
            aeronave.inscricao = "123";
            //Console.WriteLine("Aeronave definida como: "); //aeronave.Inscricao
            Console.WriteLine("Informe a data e hora d Voo: (dd/MM/yyyy hh:mm) ");
            this.DataVoo = DateTime.Parse(Console.ReadLine());
            if (DataVoo <= DateTime.Now)
            {
                Console.WriteLine("Essa data é inválida, informe novamente: ");
                DataVoo = DateTime.Parse(Console.ReadLine());
            }
            this.DataCadastro = DateTime.Now;
            Console.WriteLine("Data de cadastro definifida como: " + DataCadastro);
            Console.WriteLine("Informe a situacao do Voo: \n[A] Ativo \n[C] Cancelado");
            Situacao = char.Parse(Console.ReadLine().ToUpper());
            while (Situacao != 'A' && Situacao != 'C')
            {
                Console.WriteLine("O valor informado é inválido, por favor informe novamente!\n[A] Ativo \n[C] Cancelado");
                Situacao = char.Parse(Console.ReadLine().ToUpper());
            }
            string sql = "insert into Voo (Id, InscricaoAeronave, DataCadastro, Situacao, DataVoo, Destino) values ('" + this.Id + "', '" + aeronave.inscricao + "', '" +
            this.DataCadastro + "', '" + this.Situacao + "', '" + this.DataVoo + "', '" + this.Destino + "');";
            Console.WriteLine("Comando executado no SQL\n");
            Console.WriteLine(sql);
            Console.ReadKey();

            conexao.InserirDado(conexaosql, sql);
            Console.WriteLine("Inscrição de Voo realizada com sucesso!");

            //}
            //else if (existeAeronave)
            //{
            //    Console.Clear();
            //    Console.WriteLine("Desculpa, impossível cadastrar voo, pois nao temos nenhuma aeronave Ativa Cadastrada");
            //    Console.WriteLine("Pressione uma tecla para prosseguir");
            //    Console.ReadKey();
            //}
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
            Console.WriteLine("Atualizar dados Voo");
            Console.WriteLine("Insira o id do Voo que deseja alterar (Exemplo = 'V1234'): ");
            string idVoo = Console.ReadLine();
            string sql = "Select Id from Voo where Id = '" + idVoo + "';";
            int verificar = conexao.VerificarExiste(sql);
            if (verificar != 0)
            {
                Console.WriteLine("Escolha o que voce deseja atualizar: \n[1] Destino \n[2] Aeronave \n[3] Data de Voo \n[4] Situação do Voo");
                int op = int.Parse(Console.ReadLine());
                while (op < 1 || op > 5)
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
                        Console.WriteLine("Qual a nova aeronave que deseja informar?: "); //necessidade de classe aeronave
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
        public void LocalizarVoo(SqlConnection sqlConnection)
        {
            Console.WriteLine("Localizar dados Voo");
            Console.WriteLine("Insira o id do Voo que deseja alterar (Exemplo = 'V1234'): ");
            string idVoo = Console.ReadLine();
            string sql = "Select Id from Voo where Id = '" + idVoo + "';";
            int verificar = conexao.VerificarExiste(sql);
            if (verificar != 0) //achou
            {
                sql = "Select Id, InscricaoAeronave, DataVoo, DataCadastro, Destino, Situacao from Voo where Id = '" + idVoo + "';";
                conexao.LocalizarDado(sqlConnection, sql, 2);
                Console.WriteLine("Pressione enter para prosseguir...");
                Console.ReadKey();
            }
            else //nao achou
            {
                Console.WriteLine("O voo nao foi encontrado!");
                Console.WriteLine("Pressione enter para prosseguir...");
                Console.ReadKey();
            }
        }
    }
    //Terminar localizar dado especifico e imprimir o voo [ok]
    //Terminar o Imprimir registro por registro (menu de cadastro aviao por aviao, pausa e chama menu)
    //Linkar Classe Aeronave
    //inserir aeronave no cadastro de voo | Verificar se existe pra realizar o cadastro 
    //Verificar se aeronave existe [OK]
    //Inserir Aeronave no banco de dados para funcionar [OK]
    
}