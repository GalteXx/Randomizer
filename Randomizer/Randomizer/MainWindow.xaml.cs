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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Randomizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Focus();
            timer.Tick += Tick;
            timer.Interval = new TimeSpan(0, 0, 1);

        }
        Random rand = new Random();
        DispatcherTimer timer = new DispatcherTimer();

        
        
        private bool isNumeric(string a) //проверяем, есть ли плохие символы в строке
        {
            for(int i = 0; i < a.Length; i++)
            {
                if ((a[i] > 57 || a[i] < 48) && a[i] != '-') return false; //диапазон в аски в который попдают буквы исключается
            }
            
            return true;
        }

        //щелк - видим только жлементы для первого режима
        private void md1_Checked(object sender, RoutedEventArgs e)
        {
            _1st.Visibility = Visibility.Visible;
            _2nd.Visibility = Visibility.Hidden;
            _3rd.Visibility = Visibility.Hidden;
        }

        private void Tick(object sender, EventArgs e) //отсчет таймера
        {

            DoubleAnimation anim = new DoubleAnimation(); //анимция плавненького таймера
            anim.From = progressBar1.Value;
            anim.To = progressBar1.Value - 1;
            anim.Duration = TimeSpan.FromSeconds(1);
            progressBar1.BeginAnimation(ProgressBar.ValueProperty, anim);

            DoubleAnimation anim2 = new DoubleAnimation(); //анимация плавного возвращения
            anim2.From = progressBar1.Value;
            anim2.To = progressBar1.Value + 15;
            anim2.Duration = TimeSpan.FromSeconds(1);
            


            if ( (int) progressBar1.Value <= 0) //процесс создания рандомного пароля описан ниже(сильно ниже, на другом алгоритме)
            {
                progressBar1.Foreground = Brushes.Lime; //перекрашиваем бар в зеленый
                Disp.Text = "";
                string alphabet = "ABCDEFGIJKLMNOPQRSTUVWQXYZabcdefgijklmnopqrstuvwqxyz0123456789";
                for (int i = 0; i < 6; i++)
                {
                    Disp.Text += alphabet[rand.Next(0, 62)];
                }

                
                progressBar1.BeginAnimation(ProgressBar.ValueProperty, anim2); //возвращаем прогресБар на 15/15

            }
            label1.Content = (int) progressBar1.Value + " seconds left"; //выводим значение прогресБара в текст, т.к. он совпадает с секундами
            if ((int)progressBar1.Value <= 5 && (int)progressBar1.Value != 0)
            {
                progressBar1.Foreground = Brushes.Red; //перекрашиваем бар в красный
            }
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //отлавливаем ошибки
            if (isNumeric(to.Text) && isNumeric(from.Text))
                {
                    if (Convert.ToInt32(from.Text) <= Convert.ToInt32(to.Text))
                    numberDisp.Text = Convert.ToString(rand.Next(Convert.ToInt32(from.Text), Convert.ToInt32(to.Text) + 1));
                    else
                    numberDisp.Text = Convert.ToString(rand.Next(Convert.ToInt32(to.Text), Convert.ToInt32(from.Text) + 1));
            }
            else numberDisp.Text = "Incorrect input, try again";
        }
        //щелк - видим жлементы только для второго режима(я знаю что цифра снизу это 3)
        private void md3_Checked(object sender, RoutedEventArgs e)
        {
            _1st.Visibility = Visibility.Hidden;
            _2nd.Visibility = Visibility.Visible;
            _3rd.Visibility = Visibility.Hidden;
        }
        //кнопка второго режима
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            if (isNumeric(passLength.Text) && Convert.ToInt32(passLength.Text) > 0)//проверяем на наличие плохих символов
            {
                DispPass.Text = ""; //обнуляем надпись
                if (rbSmall.IsChecked == true)//если выбрана "a-z"
                {
                    string alphabet = "abcdefgijklmnopqrstuvwqxyz"; //из этого алфавита берутся буквы
                    for (int i = 0; i < Convert.ToInt32(passLength.Text); i++)
                    {
                        DispPass.Text += alphabet[rand.Next(0, 26)]; //берем случайные номера элементов, и запихиваем их в пароль(элементы, не номера)
                    }
                }
                else if (rbBig.IsChecked == true)//по аналогии с маленькими
                {
                    string alphabet = "ABCDEFGIJKLMNOPQRSTUVWQXYZ";
                    for (int i = 0; i < Convert.ToInt32(passLength.Text); i++)
                    {
                        DispPass.Text += alphabet[rand.Next(0, 26)];
                    }
                }
                else if (rbNum.IsChecked == true)//тут тоже все аналогично
                {
                   
                    for (int i = 0; i < Convert.ToInt32(passLength.Text); i++)
                    {
                        DispPass.Text += rand.Next(0, 9);
                    }
                }
                else if (rbAll.IsChecked == true)//тут тоже все аналогично
                {
                    string alphabet = "ABCDEFGIJKLMNOPQRSTUVWQXYZabcdefgijklmnopqrstuvwqxyz0123456789";
                    for (int i = 0; i < Convert.ToInt32(passLength.Text); i++)
                    {
                        DispPass.Text += alphabet[rand.Next(0, 62)];
                    }
                }
            }
            else DispPass.Text = "Incorrect input...";//если нашли букву в строке с длиной пароля
        }

        //переход в 3й режим. Теперь случайные символьные приколы будут генерироваться сами и постоянно раз в 15 секунд
        private void md2_Checked(object sender, RoutedEventArgs e)
        {
            _1st.Visibility = Visibility.Hidden;
            _2nd.Visibility = Visibility.Hidden;
            _3rd.Visibility = Visibility.Visible;
            DM.Visibility = Visibility.Visible;
            DM.IsChecked = true;
        }



        private void DM_Checked(object sender, RoutedEventArgs e) //темная тема
        {
            timer.Start();
        }

        private void DM_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void from_GotFocus(object sender, RoutedEventArgs e)
        {
            from.Text = "";
        }

        private void to_GotFocus(object sender, RoutedEventArgs e)
        {
            to.Text = "";
        }

        private void passLength_GotFocus(object sender, RoutedEventArgs e)
        {
            passLength.Text = "";
        }
    }
}
