using System;
using System.Data;
using System.Data.SqlClient;

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
                case 1: 
                    #region Localizar Passageiro
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
                #endregion
                case 2: 
                    #region Localizar Voo
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
                #endregion
                case 3: 
                    #region Localizar Bloqueadas
                    recebe = "";
                    try
                    {
                        SqlConnection conexao = new SqlConnection(Caminho());
                        conexao.Open();

                        SqlCommand cmd = new SqlCommand(sql, conexao);

                        SqlDataReader reader = null;

                        using (reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("### Companhia Aérea Localizada em Bloqueados ###\n");
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
                #endregion
                case 4: 
                    #region Localizar Cias
                    recebe = "";
                    try
                    {
                        SqlConnection conexao = new SqlConnection(Caminho());
                        conexao.Open();
                        SqlCommand cmd = new SqlCommand(sql, conexao);
                        SqlDataReader reader = null;
                        using (reader = cmd.ExecuteReader())
                        {
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
                #endregion
                case 5: 
                    #region Localizar Passagem 
                    recebe = "";
                    try
                    {
                        SqlConnection conexao = new SqlConnection(Caminho());
                        conexao.Open();
                        SqlCommand cmd = new SqlCommand(sql, conexao);
                        SqlDataReader reader = null;
                        using (reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("### Passagem Localizada ###\n");
                            while (reader.Read())
                            {
                                recebe = reader.GetString(0);
                                Console.WriteLine("Id: {0}", reader.GetString(0));
                                Console.WriteLine("IdVoo: {0}", reader.GetString(1));
                                Console.WriteLine("Data Ultima Operação: {0}", reader.GetDateTime(2).ToShortDateString());
                                Console.WriteLine("Valor: {0}", reader.GetDouble(3));
                                Console.WriteLine("Situacao: {0}", reader.GetString(4));
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
                #endregion
                case 6:
                    #region Localizar Restritos
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
                #endregion
                case 7:
                    #region Localizar Venda
                    recebe = "";
                    try
                    {
                        SqlConnection conexao = new SqlConnection(Caminho());
                        conexao.Open();
                        SqlCommand cmd = new SqlCommand(sql, conexao);
                        SqlDataReader reader = null;
                        using (reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("### Venda Localizada ###\n");
                            while (reader.Read())
                            { //Id, DataVenda, Passageiro, ValorTotal, Voo, IDItemVenda, ValorUnitario
                                recebe = Convert.ToString(reader.GetInt32(0));
                                Console.WriteLine("Id: {0}", reader.GetInt32(0));
                                Console.WriteLine("Data Venda: {0}", reader.GetDateTime(1).ToShortDateString());
                                Console.WriteLine("Passageiro: {0}", reader.GetString(2));
                                Console.WriteLine("Valor Total: {0}", reader.GetDouble(3));
                                Console.WriteLine("Voo: {0}", reader.GetString(4)); 
                                Console.WriteLine("Item Venda: {0}", reader.GetString(5));
                                Console.WriteLine("Valor Unitario: {0}", reader.GetDouble(6));
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
                #endregion
                case 8:
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
                                Console.WriteLine("Inscricao ANAC: {0}", reader.GetString(0)); 
                                Console.WriteLine("CNPJ: {0}", reader.GetString(1)); 
                                Console.WriteLine("Data de Cadastro: {0}", reader.GetDateTime(2).ToShortDateString()); 
                                Console.WriteLine("Situacao: {0}", reader.GetString(3)); 
                                Console.WriteLine("Ultima Venda: {0}", reader.GetDateTime(4).ToShortDateString()); 
                                Console.WriteLine("Capacidade: {0}", reader.GetInt32(5)); 
                            }
                        }
                        conecta.Close();
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
        public static string RetornoDados(string sql, SqlConnection conexao, string parametro)
        {
            var situacao = "";
            ConexaoBD caminho = new();
            conexao = new(caminho.Caminho());
            conexao.Open();
            SqlCommand cmd = new(sql, conexao);
            cmd.CommandType = CommandType.Text;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        situacao = reader[$"{parametro}"].ToString();
                    }
                }
            }
            conexao.Close();
            return situacao;
        }
    }
}
