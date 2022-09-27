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
        public void Insert(string sql)
        {
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine("Dados inseridos com sucesso!");
            Console.WriteLine("Aperte alguma tecla para prosseguir...");
            Console.ReadKey();
            conn.Close();
        }
        public void Update(string sql)
        {
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine("Dados alterados com sucesso!");
            Console.WriteLine("Aperte alguma tecla para prosseguir...");
            Console.ReadKey();
            conn.Close();
        }
        public void Delete(string sql)
        {
            conn.Open();
            SqlCommand sqlCommand = new SqlCommand(sql, conn);
            sqlCommand.ExecuteNonQuery();
            Console.WriteLine("Dados deletados com sucesso!");
            Console.WriteLine("Aperte alguma tecla para prosseguir...");
            Console.ReadKey();
            conn.Close();
        }
    }
}
