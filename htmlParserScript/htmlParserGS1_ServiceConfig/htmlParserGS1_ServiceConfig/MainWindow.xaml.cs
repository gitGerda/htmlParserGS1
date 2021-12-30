using Microsoft.Win32;
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

namespace htmlParserGS1_ServiceConfig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistryKey htmlParserGS1Key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\htmlParserGS1Service");
            
            if(htmlParserGS1Key!=null)
            {
                StringBuilder mes = new StringBuilder();

                
                if(htmlParserGS1Key.GetValue("ServAddress")!=null)
                {
                    richTbServAddr.AppendText(htmlParserGS1Key.GetValue("ServAddress").ToString());
                    
                    //MessageBox.Show(f);
                }
                else
                {
                    mes.AppendLine("Ошибка 1: Не удалось найти ключ ServAddress");
                    btnServAddrSave.IsEnabled = false;
                    btnServAddrReset.IsEnabled = false;
                }
            }
            else
            {
                MessageBox.Show("Не удалось найти ветку реестра соответствующую службе htmlParserGS1Service");

                btnServAddrSave.IsEnabled = false;
                btnServAddrReset.IsEnabled = false;

                btnAuthSave.IsEnabled = false;
                btnAuthReset.IsEnabled = false;

                btnDirSave.IsEnabled = false;
                btnDirReset.IsEnabled = false;
            }
        }
    }
}
