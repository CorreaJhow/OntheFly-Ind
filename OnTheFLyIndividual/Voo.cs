using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheFLyIndividual
{
    internal class Voo
    {
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
        public void CadastrarVoo()
        {
            Aeronave aeronave = new Aeronave();
            //bool existeAeronave = aeronave.ExisteAeronave();
            //if (!existeAeronave)
            //{
                Console.Clear();
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Bem vindo ao cadastro de voo.");
                Console.WriteLine("-----------------------------");
                string destinoVoo = DestinoVoo();
                Console.WriteLine("Aeronave definida como: "); //aeronave.Inscricao
                Console.WriteLine("Informe a data e hora d Voo: (dd/MM/yyyy hh:mm) ");
                DateTime dataVoo = DateTime.Parse(Console.ReadLine());
                if (dataVoo <= DateTime.Now)
                {
                    Console.WriteLine("Essa data é inválida, informe novamente: ");
                    dataVoo = DateTime.Parse(Console.ReadLine());
                }
                DateTime dataCadastro = DateTime.Now;
                Console.WriteLine("Data de cadastro definifida como: " + dataCadastro);
                Console.WriteLine("Informe a situacao do Voo: \n[A] Ativo \n[C] Cancelado");
                char situacaoVoo = char.Parse(Console.ReadLine().ToUpper());
                while (situacaoVoo != 'A' && situacaoVoo != 'C')
                {
                    Console.WriteLine("O valor informado é inválido, por favor informe novamente!\n[A] Ativo \n[C] Cancelado");
                    situacaoVoo = char.Parse(Console.ReadLine().ToUpper());
                }
                Voo novoVoo = new Voo(destinoVoo, dataVoo, dataCadastro, situacaoVoo);
                Console.Clear();
                Console.WriteLine(novoVoo.ImprimirVoo());
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
                    break;
                }
                else
                {
                    Console.WriteLine("Destino inválido, informe novamente!");
                    Console.WriteLine("");
                }
            } while (true);
        }
    }
}
