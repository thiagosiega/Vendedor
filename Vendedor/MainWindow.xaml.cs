using MySql.Data.MySqlClient;
using System.Windows;

namespace Vendedor
{
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Entrar_Click(object sender, RoutedEventArgs e)
        {
            string usuario = ID_box.Text;
            string senha = Senha_box.Text;

            Server server = new Server();
            var resultadoConexao = server.Conectar();

            if (resultadoConexao.sucesso)
            {
                // Faça uma pesquisa no banco de dados usando o ID e a senha
                string query = "SELECT * FROM funcionarios_ativos WHERE id = @id AND senha = @senha";
                MySqlCommand cmd = new MySqlCommand(query, resultadoConexao.conexao);
                cmd.Parameters.AddWithValue("@id", usuario);
                cmd.Parameters.AddWithValue("@senha", senha);

                try
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Vendas.Vender vender = new Vendas.Vender();
                            vender.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Usuário ou senha incorretos!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro durante a execução da consulta: " + ex.Message);
                }
            }

            else
            {
                MessageBox.Show(resultadoConexao.mensagem);
            }
        }

        private void Novo_Click(object sender, RoutedEventArgs e)
        {

            Server server = new Server();
            var resultadoConexao = server.Conectar();

            if (ID_box.Text != "2002" || Senha_box.Text != "123")
            {
                MessageBox.Show("Usuário ou senha incorretos!");
                return;
            }
            if (resultadoConexao.sucesso)
            {
                Novo_funcionario novo = new Novo_funcionario();
                novo.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show(resultadoConexao.mensagem);
            }

        }
    }
}
