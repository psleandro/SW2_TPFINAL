using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Leandro Pereira dos Santos CB3005372
// Renan Josué Silva de Matos CB3007162

namespace TP04_DESKTOP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string URI = "";
        int codigoProduto = 1;
        UsuarioForm usuarioForm = new UsuarioForm("https://localhost:7056/usuarios");

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void GetAllusuarios()
        {
            URI = txtURI.Text;
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(URI))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var ProdutoJsonString = await response.Content.ReadAsStringAsync();
                        dgvDados.DataSource = JsonConvert.DeserializeObject<Usuario[]>(ProdutoJsonString).ToList();
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível obter o usuario : " + response.StatusCode);
                    }
                }
            }
        }

        private async void GetusuarioById(int usuarioId)
        {
            using (var client = new HttpClient())
            {
                BindingSource bsDados = new BindingSource();
                URI = txtURI.Text + "/" + usuarioId.ToString();

                HttpResponseMessage response = await client.GetAsync(URI);
                if (response.IsSuccessStatusCode)
                {
                    var ProdutoJsonString = await response.Content.ReadAsStringAsync();
                    bsDados.DataSource = JsonConvert.DeserializeObject<Usuario>(ProdutoJsonString);
                    dgvDados.DataSource = bsDados;
                }
                else
                {
                    MessageBox.Show("Falha ao obter o usuario : " + response.StatusCode);
                }
            }
        }

        private async void DeleteProduto(int usuarioId)
        {
            URI = txtURI.Text;
            int usuarioID = usuarioId;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URI);
                HttpResponseMessage responseMessage = await
                client.DeleteAsync(String.Format("{0}/{1}", URI, usuarioID));
                if (responseMessage.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuário excluído com sucesso");
                }
                else
                {
                    MessageBox.Show("Falha ao excluir o usuário: " + responseMessage.StatusCode);
                }
            }
            GetAllusuarios();
        }

        private void InputBox()
        {
            string Prompt = "Informe o código do usuário a ser excluído.";
            string Title = "P2 - TRABALHO FINAL";
            string Result = Microsoft.VisualBasic.Interaction.InputBox(Prompt, Title, "", 600, 350);
            if (Result != "")
            {
                codigoProduto = Convert.ToInt32(Result);
            }
            else
            {
                codigoProduto = -1;
            }
        }

        private void btnObterProdutos_Click(object sender, EventArgs e)
        {
            GetAllusuarios();
        }

        private void btnDeletarProduto_Click(object sender, EventArgs e)
        {
            InputBox();
            
            if(codigoProduto != -1)
            {
                DeleteProduto(codigoProduto);
            }
        }

        private void btnProdutosPorId_Click(object sender, EventArgs e)
        {
            InputBox();
            if (codigoProduto != -1)
            {
                GetusuarioById(codigoProduto);
            }
        }

        private void btnIncluirProduto_Click(object sender, EventArgs e)
        {
            usuarioForm.Show();
        }

        private void btnAtualizaProduto_Click(object sender, EventArgs e)
        {
            usuarioForm.Show();
        }

        private void txtURI_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
