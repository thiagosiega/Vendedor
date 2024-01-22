using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Documents;

namespace Vendedor
{
    /// <summary>
    /// Lógica interna para Novo_funcionario.xaml
    /// </summary>
    public partial class Novo_funcionario : Window
    {
        private bool finalizar = false;
        public Novo_funcionario()
        {
            InitializeComponent();
        }

        private void Voltar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();

        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            string nome = Nome_box.Text;
            double cargo = Lev.Value;
            string salario = Salario_box.Text;

            Server server = new Server();
            var resultadoConexao = server.Conectar();

            if (resultadoConexao.sucesso)
            {
                //verifica se o nome de usuário já existe
                string query = "SELECT * FROM funcionarios_ativos WHERE nome = @Nome";
                MySqlCommand cmd = new MySqlCommand(query, resultadoConexao.conexao);
                cmd.Parameters.AddWithValue("@Nome", nome);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    MessageBox.Show("Nome de usuário já existe!");
                    reader.Close(); // Fechando explicitamente o DataReader.
                    return;
                }
                reader.Close();
                // verifica se as senhas e gera uma nova senha nao repetida
                string senha = "";
                Random rnd = new Random();
                bool senhaRepetida;

                do
                {
                    senha = rnd.Next(100000, 999999).ToString();
                    query = "SELECT COUNT(*) FROM funcionarios_ativos WHERE senha = @Senha";
                    cmd = new MySqlCommand(query, resultadoConexao.conexao);
                    cmd.Parameters.Clear(); // Limpa os parâmetros anteriores
                    cmd.Parameters.AddWithValue("@Senha", senha);
                    senhaRepetida = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
                while (senhaRepetida);

                //salva o novo funcionario no banco de dados
                query = "INSERT INTO funcionarios_ativos (nome, cargo, salario, senha) VALUES (@Nome, @Cargo, @Salario, @Senha)";
                cmd = new MySqlCommand(query, resultadoConexao.conexao);
                cmd.Parameters.Clear(); // Limpa os parâmetros anteriores
                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Cargo", cargo);
                cmd.Parameters.AddWithValue("@Salario", salario);
                cmd.Parameters.AddWithValue("@Senha", senha);

                try
                {
                    // Insere o novo funcionário e obtém o ID gerado
                    cmd.ExecuteNonQuery();

                    query = "SELECT LAST_INSERT_ID();"; // Obter o último ID inserido nesta sessão
                    cmd = new MySqlCommand(query, resultadoConexao.conexao);
                    int ID_gerar = Convert.ToInt32(cmd.ExecuteScalar());

                    MessageBox.Show($"Funcionário adicionado com sucesso! ID: {ID_gerar}, Senha: {senha}");
                    //torna o botao de finalizar visivel
                    Finalizar.Visibility = Visibility.Visible;
                    bool finalizar = true;

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

        private void Finalizar_Click(object sender, RoutedEventArgs e)
        {
            if (finalizar == true)
            {
                Vendas.Vender vender = new Vendas.Vender();
                vender.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Opis algo deu errado\ntente fechar e abrir o programa novamente");
            }

        }
    }
}
