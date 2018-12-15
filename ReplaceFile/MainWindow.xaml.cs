using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace ReplaceFile {
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow {

        #region Atributos

        string caminho = string.Empty;

        #endregion

        #region Construtor

        public MainWindow() {
            InitializeComponent();
        }

        #endregion

        #region Eventos

        // selecionar arquivos
        private void BtnSelecionar_Click(object sender, RoutedEventArgs e) {
            lsFile.Items.Clear();
            Stream stream;
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.InitialDirectory = @"c:\";
            filedialog.Filter = "Todos os arquivos (*.*)|*.*";
            filedialog.FilterIndex = 1;
            filedialog.RestoreDirectory = true;
            filedialog.Multiselect = true;
            filedialog.Title = "Escolha os arquivos";

            if (filedialog.ShowDialog() == true) {
                caminho = System.IO.Path.GetDirectoryName(filedialog.FileName);
                foreach (var item in filedialog.FileNames) {
                    try {
                        if ((stream = filedialog.OpenFile()) != null) {
                            using (stream) {
                                string fileName = System.IO.Path.GetFileName(item);
                                lsFile.Items.Add(fileName);
                            }
                        }
                        else {
                            caminho = string.Empty;
                            return;
                        }
                    }
                    catch (Exception ex) {

                        MessageBox.Show("Erro" + ex.Message, "Erro",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                        caminho = string.Empty;
                    }
                }
            }else{
                LimparCampos();
            }
        }

        // renomear arquivos
        private void BtnExecutar_Click(object sender, RoutedEventArgs e) {
            try {
                List<string> listaNewFiles = new List<string>();
                int count = 1;
                if (txtNewName.Text != string.Empty) {
                    if (!caminho.Equals(string.Empty)) {
                        DirectoryInfo directory = new DirectoryInfo(caminho);
                        FileInfo[] files = directory.GetFiles();
                        foreach (FileInfo item in files) {
                            foreach (string listFile in lsFile.Items) {
                                string fileFullName = System.IO.Path.GetFileName(item.FullName);
                                string extension = System.IO.Path.GetExtension(item.FullName);
                                if (fileFullName == listFile) {
                                    File.Move(item.FullName, item.FullName.Replace(listFile, txtNewName.Text + count + extension));
                                    listaNewFiles.Add(txtNewName.Text + count + extension);
                                }
                            }
                            count++;
                        }
                        MessageBox.Show("Arquivos renomeados com sucesso!", "Mensagem",
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                        lsFile.Items.Clear();
                        foreach (var item in listaNewFiles) {
                            lsFile.Items.Add(item);
                        }
                    }

                }else{
                    MessageBox.Show("Digite o novo nome do arquivo!", "Informação",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex) {

                MessageBox.Show("Erro" + ex.Message, "Erro",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                caminho = string.Empty;
                LimparCampos();
            }
        }

        // limpar
        private void BtnLimpar_Click(object sender, RoutedEventArgs e) {
            LimparCampos();
        }

        #endregion

        #region Metodos

        // limpar campos
        private void LimparCampos(){
            lsFile.Items.Clear();
            txtNewName.Clear();
        }

        #endregion
    }
}
