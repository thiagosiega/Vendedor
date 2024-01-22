using MySql.Data.MySqlClient;
using System;

namespace Vendedor
{
    internal class Server
    {
        // Define as informações para a conexão com o banco de dados
        string Nome_servidor = "localhost";
        string Nome_banco = "funcionarios";
        string Senha = "";
        string Usuario = "root";

        // Inicia a conexão com o banco de dados
        public (bool sucesso, string mensagem, MySqlConnection conexao) Conectar()
        {
            MySqlConnection Conexao = new MySqlConnection();

            // Define a string de conexão com o banco de dados
            Conexao.ConnectionString = String.Format("server={0};database={1};uid={2};pwd={3}", Nome_servidor, Nome_banco, Usuario, Senha);

            try
            {
                // Abre a conexão com o banco de dados
                Conexao.Open();

                // Verifica se a conexão foi aberta
                if (Conexao.State == System.Data.ConnectionState.Open)
                {
                    return (true, "Conectado com sucesso!", Conexao);
                }
                else
                {
                    return (false, "Não foi possível conectar ao banco de dados!", null);
                }
            }
            catch (Exception ex)
            {
                return (false, "Erro ao conectar ao banco de dados: " + ex.Message, null);
            }
        }
    }
}
