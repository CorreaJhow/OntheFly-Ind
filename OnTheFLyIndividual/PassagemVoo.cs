using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void CadastrarPassagem()
        {
            Voo voo = new Voo();
            string sql = "Select Id from Voo;";
            int verificar = conexao.VerificarExiste(sql);
            if (verificar != 0)
            {
                int valorId = GeradorDeID();
                this.IdPassagem = "PA" + valorId.ToString();
                Console.WriteLine("Id da passagem definido como: " + IdPassagem);
                Console.WriteLine("Informe o Voo (Ex = V1234): ");
                voo.Id = Console.ReadLine();
                sql = "select Id from Voo where Id = '" + voo.Id + "';";
                verificar = conexao.VerificarExiste(sql);
                while(verificar == 0)
                {
                    Console.WriteLine("Esse voo nao existe, informe outro: ");
                    voo.Id = Console.ReadLine();
                    sql = "select Id from Voo where Id = '" + voo.Id + "';";
                    verificar = conexao.VerificarExiste(sql);
                }
                this.DataUltimaOperacao = DateTime.Now;
                Console.WriteLine("Informe o valor da Passagem: ");
                this.Valor = float.Parse(Console.ReadLine());
                while(this.Valor < 0 || this.Valor > 9999.99)
                {
                    Console.WriteLine("Valor inválido de passagem, informe outro valor: ");
                    this.Valor = float.Parse(Console.ReadLine());
                }
                this.SituacaoPassagem = "L";
                Console.WriteLine("situação da passagem: "+this.SituacaoPassagem);
                //string query = "insert into PassagemVoo"
                //inserir no banco de dado [criar string, inserir no metodo de conexao]
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Desculpa, impossível cadastrar voo, pois nao temos nenhuma aeronave Ativa Cadastrada");
                Console.WriteLine("Pressione uma tecla para prosseguir");
                Console.ReadKey();
            }
        }
        //Atualizar/Editar Passagem
        public void AtualizarPassagem()
        {

        }
        //LocalizarPassagem
        public void LocalizarPassagem()
        {

        }
        //RegistroPorRegistro
        public void RegistroPorRegistro()
        {

        }
    }
}
