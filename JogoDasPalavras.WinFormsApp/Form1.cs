using JogoDasPalavras.WinFormsApp.JogoDasPalavras.WinFormsApp;

namespace JogoDasPalavras.WinFormsApp
{
    public partial class Form1 : Form
    {
        private JogoPalavras jogoPalavras;

        public Form1()
        {
            InitializeComponent();
            ConfigurarBotoes();
            ComeçarJogo();
        }

        private void ConfigurarBotoes()
        {
            btnEnter.Click += Ok_Click;
            lblNovoJogo.Click += NovoJogo;

            foreach (Button botao in pnlTeclas.Controls)
            {
                if (botao.Text != "OK")
                {
                    botao.Click += AtribuirLetra;
                }
            }
        }

        private void ComeçarJogo()
        {
            jogoPalavras = new JogoPalavras();
            lblNovoJogo.Visible = true;

            pnlTeclas.Enabled = true;

            ResetarPaineis();

            IniciarRodada();
        }

        private void AtribuirLetra(object? sender, EventArgs e)
        {
            Button botaoClicado = (Button)sender;
            InserirLetra(Convert.ToChar(botaoClicado.Text[0]));
        }

        private void ReceberPalavra()
        {
            jogoPalavras.palavraEscolhida = "";

            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.rodada)
                {
                    jogoPalavras.palavraEscolhida += txtLetra.Text;
                }
            }
        }

        private void InserirLetra(char letraTeclado)
        {
            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (txtLetra is TextBox && pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.rodada && txtLetra.Text == "")
                {
                    txtLetra.Text = letraTeclado.ToString();
                    break;
                }
            }
        }

        private void ConfirmarPalavra()
        {
            ReceberPalavra();

            if (jogoPalavras.VerificaPalavraCompleta())

                EncerrarRodada();
        }

        private void EncerrarRodada()
        {
            VerificarLetrasInexistentes();

            VerificarLetraExistente();

            VerificarPosicionamentoLetra();

            if (jogoPalavras.VerificaSeJogadorGanhou())
                VenceuJogo();

            else if (jogoPalavras.VerificaSeJogadorPerdeu())
                PerdeuJogo();

            jogoPalavras.rodada++;

            IniciarRodada();
        }

        private void VerificarLetrasInexistentes()
        {

            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) != jogoPalavras.rodada)
                    continue;

                txtLetra.BackColor = Color.Red;

                foreach (Control btnTeclado in pnlTeclas.Controls)
                {
                    if (btnTeclado.Text == "OK")
                        continue;

                    if (jogoPalavras.CompararLetras(Convert.ToChar(btnTeclado.Text[0]), Convert.ToChar(txtLetra.Text[0])))
                        btnTeclado.BackColor = Color.Red;
                }
            }
        }

        private void VerificarLetraExistente()
        {
            foreach (char letraPalavraSecreta in jogoPalavras.palavraSecreta)
            {
                foreach (Control txtLetra in pnlMostrarLetra.Controls)
                {
                    if (pnlMostrarLetra.GetRow(txtLetra) != jogoPalavras.rodada)
                        continue;

                    if (jogoPalavras.CompararLetras(Convert.ToChar(txtLetra.Text), letraPalavraSecreta))
                    {
                        txtLetra.BackColor = Color.Yellow;

                        foreach (Control btnTeclado in pnlTeclas.Controls)
                        {
                            if (btnTeclado.Text == txtLetra.Text && btnTeclado.BackColor != Color.Green)
                            {
                                btnTeclado.BackColor = Color.Yellow;
                            }
                        }
                    }
                }
            }
        }

        private void VerificarPosicionamentoLetra()
        {
            List<Control> txtListaLetras = new List<Control>();

            foreach (Control txtBox in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtBox) == jogoPalavras.rodada)
                    txtListaLetras.Add(txtBox);
            }

            for (int letra = 0; letra < jogoPalavras.palavraSecreta.Length && letra < txtListaLetras.Count; letra++)
            {
                char letraNoTxt = Convert.ToChar(txtListaLetras[letra].Text);
                char letraSecreta = jogoPalavras.palavraSecreta[letra];

                if (jogoPalavras.CompararLetras(letraNoTxt, letraSecreta))
                {
                    txtListaLetras[letra].BackColor = Color.Green;
                    txtListaLetras[letra].Text = letraSecreta.ToString();

                    foreach (Control btnTeclado in pnlTeclas.Controls)
                    {
                        if (btnTeclado.Text == txtListaLetras[letra].Text)
                            btnTeclado.BackColor = Color.Green;
                    }
                }
            }
        }

        private void IniciarRodada()
        {
            foreach (Control txtLetra in pnlMostrarLetra.Controls)
            {
                if (pnlMostrarLetra.GetRow(txtLetra) == jogoPalavras.rodada)
                {
                    txtLetra.BackColor = Color.AliceBlue;
                }
            }
        }

        private void VenceuJogo()
        {
            MessageBox.Show("Voce venceu!");

            lblNovoJogo.Visible = true;
            pnlTeclas.Enabled = false;
        }

        private void PerdeuJogo()
        {
            MessageBox.Show("Voce perdeu, tente novamente...!");

            lblNovoJogo.Visible = true;
            pnlTeclas.Enabled = false;
        }

        private void ResetarPaineis()
        {
            foreach (Control txtTabelaLetra in pnlMostrarLetra.Controls)
            {
                txtTabelaLetra.Text = "";
                txtTabelaLetra.BackColor = Color.Black;
            }

            foreach (Control btnTeclado in pnlTeclas.Controls)
            {
                btnTeclado.BackColor = Color.Transparent;
            }
        }

        private void lblNovoJogo_Click(object sender, EventArgs e)
        {
            ComeçarJogo();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            ConfirmarPalavra();
        }

        private void NovoJogo(object sender, EventArgs e)
        {
            ComeçarJogo();
        }
    }

}
