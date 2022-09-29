using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnTheFLyIndividual
{
    internal class ConexaoBD
    {
        string Conexao = "Data Source=localhost; Initial Catalog=OnTheFly; User Id=sa; Password=12345;";
        SqlConnection conn;
        public ConexaoBD()
        {
            conn = new SqlConnection(Conexao);
        }
        public string Caminho()
        {
            return Conexao;
        }
        public void InserirDado(SqlConnection conecta, String sql)
        {
            try
            {
                conecta.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Connection = conecta;
                cmd.ExecuteNonQuery();
                conecta.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void DeletarDado(SqlConnection conecta, String sql)
        {
            try
            {
                conecta.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Connection = conecta;
                cmd.ExecuteNonQuery();
                conecta.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void EditarDado(SqlConnection conecta, String sql)
        {
            try
            {
                conecta.Open();
                SqlCommand cmd = new SqlCommand(sql, conecta);
                cmd.Connection = conecta;
                cmd.ExecuteNonQuery();
                conecta.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public String LocalizarDado(SqlConnection conecta, String sql, int op)
        {
            switch (op)
            {
                case 1: //Localizar Passageiro
                    String recebe = "";
                    try
                    {
                        conecta.Open();
                        SqlCommand cmd = new SqlCommand(sql, conecta);
                        SqlDataReader reader = null;
                        using (reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\n\t### Passageiro Localizado ###\n");
                            while (reader.Read())
                            { 
                                recebe = reader.GetString(0);
                                Console.WriteLine("Cpf: {0}", reader.GetString(0));  
                                Console.WriteLine("Nome: {0}", reader.GetString(1)); 
                                Console.WriteLine("Data Nascimento: {0}", reader.GetDateTime(2).ToShortDateString());
                                Console.WriteLine("Data Cadastro: {0}", reader.GetDateTime(3).ToShortDateString());
                                Console.WriteLine("Sexo: {0}", reader.GetString(4));
                                Console.WriteLine("Situacao: {0}", reader.GetString(5));
                                Console.WriteLine("Ultima Compra: {0}", reader.GetDateTime(6).ToShortDateString());
                                Console.WriteLine("\n");
                            }
                        }
                        conecta.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return recebe;
                case 2: //Localizar Voo
                    recebe = "";
                    try
                    {
                        conecta.Open();
                        SqlCommand cmd = new SqlCommand(sql, conecta);
                        SqlDataReader reader = null;
                        using (reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("\n\t### Voo Localizado ###\n");
                            while (reader.Read())
                            {
                                recebe = reader.GetString(0);
                                Console.WriteLine("Id de Voo: {0}", reader.GetString(0)); //idVoo
                                Console.WriteLine("Aeronave: {0}", reader.GetString(1)); //Aeronave
                                Console.WriteLine("Data de Voo: {0}", reader.GetDateTime(2).ToShortDateString()); //DataVoo
                                Console.WriteLine("Data de Cadastro: {0}", reader.GetDateTime(3).ToShortDateString()); //DataCadastro
                                Console.WriteLine("Destino: {0}", reader.GetString(4)); //Destino
                                Console.WriteLine("Situacao: {0}", reader.GetString(5)); //Situacao 
                            }
                        }
                        conecta.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return recebe;
                case 3: //Localizar Bloqueadas
                    recebe = "";
                    try
                    {
                        SqlConnection conexao = new SqlConnection(Caminho());
                        conexao.Open();

                        SqlCommand cmd = new SqlCommand(sql, conexao);

                        SqlDataReader reader = null;

                        using (reader = cmd.ExecuteReader())
                        {
                            Console.Clear();
                            Console.WriteLine(">>> Companhia Aérea Localizada <<<\n");
                            while (reader.Read())
                            {
                                recebe = reader.GetString(0);
                                Console.Write(" {0}", reader.GetString(0));
                                Console.WriteLine("\n");
                            }
                        }
                        conexao.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return recebe;
                case 4: //localizar cia 
                    recebe = "";
                    try
                    {
                        SqlConnection conexao = new SqlConnection(Caminho());
                        conexao.Open();
                        SqlCommand cmd = new SqlCommand(sql, conexao);
                        SqlDataReader reader = null;
                        using (reader = cmd.ExecuteReader())
                        {
                            Console.Clear();
                            Console.WriteLine(">>> Companhia Aérea Localizada <<<\n");
                            while (reader.Read())
                            { //"SELECT Cnpj,RazaoSocial,Data_Abertura,Data_Cadastro,Data_UltimoVoo,Situacao
                                recebe = reader.GetString(0);
                                Console.WriteLine("CNPJ: {0}", reader.GetString(0));
                                Console.WriteLine("Razao Social: {0}", reader.GetString(1));
                                Console.WriteLine("Data Abertura: {0}", reader.GetDateTime(2).ToShortDateString());
                                Console.WriteLine("Data Cadastro: {0}", reader.GetDateTime(3).ToShortDateString());
                                Console.WriteLine("Data Ultimo Voo: {0}", reader.GetDateTime(4).ToShortDateString());
                                Console.WriteLine("Situacao: {0}", reader.GetString(5));
                                Console.WriteLine("\n");
                            }
                        }
                        conexao.Close();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    return recebe;
                default:
                    return null;
            }
        }
        public int VerificarExiste(string sql)
        {
            conn.Open();
            int count = 0;
            SqlCommand sqlVerify = conn.CreateCommand();
            sqlVerify.CommandText = sql;
            sqlVerify.Connection = conn;
            using (SqlDataReader reader = sqlVerify.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count++;
                    }
                }
            }
            if (count != 0)
            {
                conn.Close();
                return 1;
            }
            conn.Close();
            return 0;
        }
    }
}
