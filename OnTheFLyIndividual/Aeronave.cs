using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using OnTheFly_BD;
using OnTheFLyIndividual;

namespace Proj_ON_THE_FLY
{
    internal class Aeronave
    { 
        public String Inscricao { get; set; }
        public int Capacidade { get; set; }
        public DateTime UltimaVenda = DateTime.Now;
        public DateTime DataCadastro = DateTime.Now;
        public char Situacao { get; set; }
        public ConexaoBD banco = new ConexaoBD();
        public string CNPJ { get; set; }
        public CompanhiaAerea cia { get; set; }
        public Aeronave() { }
        public Aeronave(String Inscrição, char Situacao, int Capacidade)
        {
            this.Inscricao = Inscrição;
            this.Capacidade = Capacidade;
            this.Situacao = Situacao;
        }
        public String GeraNumero()
        {
            Random rand = new Random();
            int[] numero = new int[100];
            int aux = 0;
            String convert = "";
            for (int k = 0; k < numero.Length; k++)
            {
                int rnd = 0;
                do
                {
                    rnd = rand.Next(100, 999);
                } while (numero.Contains(rnd));
                numero[k] = rnd;
                aux = numero[k];
                convert = aux.ToString();
                break;
            }
            return convert;
        }
        public void CadastroAeronaves(SqlConnection conecta)
        {
            Console.WriteLine("\n*** Cadastro de Aeronave ***");
            Console.WriteLine("Informe o CNPJ da Companhia Aerea: ");
            this.CNPJ = Console.ReadLine(); 
            string sql = "Select CNPJ from CompanhiaAerea where CNPJ = '" + this.CNPJ + "';";
            int verificar = banco.VerificarExiste(sql);
            while(verificar == 0)
            {
                Console.WriteLine("Cnpj não corresponde a uma Cia Aerea, informe novamente: ");
                this.CNPJ = Console.ReadLine();
                sql = "Select CNPJ from CompanhiaAerea where CNPJ = '" + this.CNPJ + "';";
                verificar = banco.VerificarExiste(sql);
            }    
            this.Inscricao = "PR-" + GeraNumero();
            int cap = 0;
            do
            {
                Console.Write("\nInforme a Capacidade da Aeronave: ");
                cap = int.Parse(Console.ReadLine());
                if (cap < 100 || cap > 999)
                {
                    Console.Clear();
                    Console.WriteLine("\nCapacidade Informada Inválida...!!!" +
                                      "\nInforme Novamente!!!");
                    Thread.Sleep(2000);
                    Console.Clear();
                }
                Capacidade = cap;
            } while (cap < 100 || cap > 999);
            do
            {
                Console.Write("\nInfome a Situação da Aeronave:" +
                              "\nA-Ativo ou I-Inativo\n" +
                              "\nSituação: ");
                Situacao = char.Parse(Console.ReadLine().ToUpper());
            } while (!Situacao.Equals('A') && !Situacao.Equals('I'));
            UltimaVenda = DateTime.Now;
            DataCadastro = DateTime.Now;
            Console.WriteLine("\nDeseja Salvar o Cadastrado de Aeronave? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int op = int.Parse(Console.ReadLine());
            if (op == 1)
            {
                sql = $"Insert Into Aeronave(InscricaoANAC,CNPJ,DataCadastro,Situacao,UltimaVenda,Capacidade) " +
                             $"Values ('{this.Inscricao}','{this.CNPJ}','{this.DataCadastro}','{this.Situacao}','{this.UltimaVenda}','{this.Capacidade}');";
                banco = new ConexaoBD();
                banco.InserirDado(conecta, sql);
                Console.WriteLine("\nCadastro de Aeronave Salvo com Sucesso!");
            }
            else
            {
                Console.WriteLine("\nCadastro de Aeronave não Foi Acionado... ");
            }
        }
        public void LocalizarAeronave(SqlConnection conecta)
        {
            Console.WriteLine("\n*** Localizar Aeronave Especifica ***");
            Console.WriteLine("\nDeseja Localizar uma Aeronave no Cadastro? ");
            Console.WriteLine("1- Sim / 2-Não ");
            Console.Write("\nDigite: ");
            int opc = int.Parse(Console.ReadLine());
            if (opc == 1)
            {
                Console.Clear();
                Console.WriteLine("\n*** Localizar Aeronave Especifico ***");
                Console.Write("\n Digite o ID_ANAC: ");
                this.Inscricao = Console.ReadLine();
                while (this.Inscricao.Length < 6)
                {
                    Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                    Console.Write("ID_ANAC: ");
                    this.Inscricao = Console.ReadLine();
                }

                Console.WriteLine("Informe o CNPJ da Companhia Aerea: ");
                this.CNPJ = Console.ReadLine();//"15086511000145";
                string sql = "Select CNPJ from CompanhiaAerea where CNPJ = '" + this.CNPJ + "';";
                int verificar = banco.VerificarExiste(sql);
                while (verificar == 0)
                {
                    Console.WriteLine("Cnpj não corresponde a uma Cia Aerea, informe novamente: ");
                    this.CNPJ = Console.ReadLine();//"15086511000145";
                    sql = "Select CNPJ from CompanhiaAerea where CNPJ = '" + this.CNPJ + "';";
                    verificar = banco.VerificarExiste(sql);
                }
                sql = $"Select InscricaoANAC,CNPJ,DataCadastro,Situacao,UltimaVenda,Capacidade From Aeronave Where InscricaoANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
                banco = new ConexaoBD();
                Console.Clear();
                if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3)))
                {
                    Console.WriteLine("\n\tAperte Qualquer Botão para Encerrar...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\n\tAeronove não Encontrado!!!");
                }
            }
            else
            {
                Console.WriteLine("Localização de Aeronave Não foi Acionada...");
            }
        }
        public void DeletarAeronave(SqlConnection conecta)
        {
            Console.WriteLine("\n*** Deletar Aeronave ***");
            Console.Write("\nDigite o ID_ANAC: ");
            this.Inscricao = Console.ReadLine().ToUpper();
            while (this.Inscricao.Length < 6)
            {
                Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                Console.Write("ID_ANAC: ");
                this.Inscricao = Console.ReadLine().ToUpper();
            }
            Console.Clear();
            // String sql = $"Select ID_ANAC,DATA_CADASTRO,SITUACAO,ULTIMA_VENDA,CAPACIDADE From AERONAVE Where ID_ANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
            String sql = $"Select InscricaoANAC,CNPJ,DataCadastro,Situacao,UltimaVenda,Capacidade From Aeronave Where InscricaoANAC=('{this.Inscricao}');";
            banco = new ConexaoBD();
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3)))
            {
                Console.WriteLine("Deseja Deletar Aeronave? ");
                Console.Write("1- Sim / 2- Não ");
                Console.Write("\nDigite: ");
                int op = int.Parse(Console.ReadLine());
                if (op == 1)
                {
                    Console.WriteLine("Informe o CNPJ da Companhia Aerea: ");//pedir o cnpj procurar na tbela companhia e se tiver agrupar na variavel
                    this.CNPJ = Console.ReadLine();//"15086511000145";
                    sql = "Select CNPJ from CompanhiaAerea where CNPJ = '" + this.CNPJ + "';";
                    int verificar = banco.VerificarExiste(sql);
                    while (verificar == 0)
                    {
                        Console.WriteLine("Cnpj não corresponde a uma Cia Aerea, informe novamente: ");
                        this.CNPJ = Console.ReadLine();//"15086511000145";
                        sql = "Select CNPJ from CompanhiaAerea where CNPJ = '" + this.CNPJ + "';";
                        verificar = banco.VerificarExiste(sql);
                    }
                    sql = $"Delete From Aeronave Where InscricaoANAC=('{this.Inscricao}') and CNPJ=('{this.CNPJ}');";
                    banco = new ConexaoBD();
                    banco.DeletarDado(conecta, sql);
                    Console.WriteLine("\nCadastro de Aeronave Deletado com sucesso!");
                }
                else
                {
                    Console.WriteLine("\nDeletar Aeronave Não Foi Acionado ...");
                }
            }
            else
            {
                Console.WriteLine("\nAeronave não Encontrada!!!");
            }
        }
        public void EditarAeronave(SqlConnection conecta)
        {
            int opc = 0;
            Console.WriteLine("\n*** Editar Aeronave ***");
            Console.Write("\nDigite o ID_ANAC: ");
            this.Inscricao = Console.ReadLine().ToUpper();

            while (this.Inscricao.Length < 6)
            {
                Console.WriteLine("\nID_ANAC Invalido, Digite Novamente");
                Console.Write("ID_ANAC: ");
                this.Inscricao = Console.ReadLine().ToUpper();
            }
            Console.Clear();
            String sql = $"Select InscricaoANAC,CNPJ,DataCadastro,Situacao,UltimaVenda,Capacidade From Aeronave Where InscricaoANAC=('{this.Inscricao}');";
            banco = new ConexaoBD();
            if (!string.IsNullOrEmpty(banco.LocalizarDado(conecta, sql, 3)))
            {
                Console.WriteLine("\nDeseja Alterar a Informação de algum Campo ? ");
                Console.Write("1- Sim / 2- Não: ");
                Console.Write("\nDigite: ");
                opc = int.Parse(Console.ReadLine());
                if (opc == 1)
                {
                    Console.WriteLine("\nDigite a Opção que Deseja Editar");
                    Console.WriteLine("1-Data_Cadastro");
                    Console.WriteLine("2-Ultima Venda");
                    Console.WriteLine("3-Situacao");
                    Console.WriteLine("4-Capacidade");
                    Console.Write("\nDigite: ");
                    opc = int.Parse(Console.ReadLine());
                    while (opc < 1 || opc > 4)
                    {
                        Console.WriteLine("\nDigite uma Opcao Valida:");
                        Console.Write("\nDigite: ");
                        opc = int.Parse(Console.ReadLine());
                    }
                    switch (opc)
                    {
                        case 1:
                            Console.Write("\nAlterar o Data de Cadastro: ");
                            this.DataCadastro = DateTime.Parse(Console.ReadLine());
                            sql = $"Update Aeronave Set DataCadastro=('{this.DataCadastro}') Where InscricaoANAC=('{this.Inscricao}');";
                            Console.WriteLine("\nData de Cadastro Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 2:
                            Console.Write("\nAlterar a  Data da Ultima Venda: ");
                            this.UltimaVenda = DateTime.Parse(Console.ReadLine());
                            Console.WriteLine("\nData da Ultima Venda Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            sql = $"Update Aeronave Set UltimaVenda=('{this.UltimaVenda}') Where InscricaoANAC=('{this.Inscricao}');";
                            break;
                        case 3:
                            Console.Write("\nAlterar Situação: ");
                            Console.WriteLine("\nA-Ativo ou I-Inativo");
                            Console.Write("Situacao: ");
                            this.Situacao = char.Parse(Console.ReadLine());
                            sql = $"Update Aeronave Set Situacao=('{this.Situacao}') Where InscricaoANAC=('{this.Inscricao}');";
                            Console.WriteLine("\nSituacao Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                        case 4:
                            Console.Write("\nAlterar a Capacidade: ");
                            this.Capacidade = int.Parse(Console.ReadLine());
                            sql = $"Update Aeronave Set Capacidade=('{this.Capacidade}') Where InscricaoANAC=('{this.Inscricao}');";
                            Console.WriteLine("\nCapacidade Editado Com Sucesso... ");
                            Thread.Sleep(2000);
                            Console.Clear();
                            break;
                    }
                    banco = new ConexaoBD();
                    banco.EditarDado(conecta, sql);
                }
            }
            else
            {
                Console.WriteLine("\n\tAeronove Não Encontrada!!!");
            }
        }
        public void ConsultarAeronave(SqlConnection conecta)
        {
            int op = 0;
            String sql = "";
            Console.Write("\nDeseja Concultar Situação de Aeronave" +
                             "\n1-Ativo , 2-Inativo , 3-Geral\n" +
                             "\nConsulta: ");
            op = int.Parse(Console.ReadLine());
            switch (op)
            {
                case 1:
                    Console.Clear();
                    Console.Write("\n*** Aeronaves Ativas ***\n");
                    this.Situacao = 'A';
                    sql = $"Select InscricaoANAC,CNPJ,DataCadastro,Situacao,UltimaVenda,Capacidade From Aeronave Where Situacao = '{this.Situacao}' ;";
                    banco = new ConexaoBD();
                    banco.LocalizarDado(conecta, sql, 3);
                    break;
                case 2:
                    Console.Clear();
                    Console.Write("\n*** Aeronaves Ativas ***\n");
                    this.Situacao = 'I';
                    sql = $"Select InscricaoANAC,CNPJ,DataCadastro,Situacao,UltimaVenda,Capacidade From Aeronave Where Situacao = '{this.Situacao}');";
                    banco = new ConexaoBD();
                    banco.LocalizarDado(conecta, sql, 3);
                    break;
                case 3:
                    Console.Clear();
                    Console.Write("\n*** Aeronaves Cadastradas ***\n");
                    sql = $"Select InscricaoANAC,CNPJ,DataCadastro,Situacao,UltimaVenda,Capacidade From Aeronave;";
                    banco = new ConexaoBD();
                    banco.LocalizarDado(conecta, sql, 3);
                    break;
            }
        }
        public bool ValidarCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;
            int resto;
            string digito;
            string tempCnpj;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;


            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else

                resto = 11 - resto;

            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
}
