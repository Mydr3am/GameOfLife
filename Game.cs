using System;
using System.Drawing;
using System.Windows.Forms;
namespace Praktika11
{
    public partial class Game : Form
    {
        private Graphics Grafika;  //переменная для рисования клеток 
        private int Razreshenie = 10;  //размерность клеток
        private bool[,] Red;  //массив для красных клеток
        private bool[,] Blue; //массив для синих клеток
        private int Rows; //кол-во строк на экране 
        private int Columns; //кол-во колонок на экране
        public Game() //создание формы
        {
            InitializeComponent();
            Form1_Resize_1(null, null);
        }
        private void StartGame()  //метод начала игры
        {
            if (Timer.Enabled)  //если таймер уже включен
                return;  //выход из метода
            Timer.Start();  //запсукаем таймер
        }
        private int PodschetSosedey(int x, int y, bool[,] a)  //подсчёт соседей у клетки 
        {
            int count = 0;  //кол-во соседей
            for (int i = -1; i < 2; i++)  //цикл для прохода всех соседей текущей клетки
            {
                for (int j = -1; j < 2; j++)//цикл для прохода всех соседей текущей клетки
                {
                    int col = (x + i + Columns) % Columns; //Нахождение соседних столбцови
                    int row = (y + j + Rows) % Rows; //Нахождение соседних строк
                    bool Samoproverka = col == x && row == y; //является ли проверка соседа самопроверкой
                    bool EstJizn = a[col, row];
                    if (EstJizn && !Samoproverka)  // если клетка имеет жизнь+не самопроверка
                    {
                        count++; //+1 сосед
                    }
                }
            }
            return count;
        }
        private void NextPokoleniya()  //вычисление и отрисовка поколения клеток
        {
            Grafika.Clear(Color.White);  //чистим игровое поле для следующего поколения 
            var NewRed = new bool[Columns, Rows];  //массив для красных клеток
            var NewBlue = new bool[Columns, Rows];//массив для синих клеток
            for (int x = 0; x < Columns; x++)  //2 цикла for для прохода по всем клеткам массивов 
            {
                for (int y = 0; y < Rows; y++)
                {
                    int RyadomRed = PodschetSosedey(x, y, Red); //нахождение кол-ва соседей у конкретной красной клетки прошлого поколения
                    bool EstJiznRed = Red[x, y];  //переменная,показывающая, жива ли красная клетка
                    int RyadomBlue = PodschetSosedey(x, y, Blue); //нахождение кол-ва соседей у конкретной синей клетки прошлого поколения
                    bool EstJiznBlue = Blue[x, y];  //переменная,показывающая, жива ли синяя  клетка
                    if (EstJiznRed && !EstJiznBlue && RyadomBlue == 3) //если синяя клетка появляется там, где жива красная
                    {
                        NewRed[x, y] = true; //красная клетка рождается 
                        NewBlue[x, y] = false; //синяя клетка отмирает  
                        Grafika.FillRectangle(Brushes.Blue, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1);
                        continue; //пропуск шага в цикле
                    }
                    if (EstJiznBlue && !EstJiznRed && RyadomRed == 3)  //если красная клетка появляется там, где жива синяя 
                    {
                        NewBlue[x, y] = true;  //синяя клетка рождается
                        NewRed[x, y] = false;  //красная клетка отмирает
                        Grafika.FillRectangle(Brushes.Red, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1);
                        continue;  //пропуск шага в цикле
                    }
                    if ((!EstJiznRed && !EstJiznBlue) && (RyadomRed == 3 && RyadomBlue == 3))  //если и синяя и красная в одном месте, где не было не той, не другой 
                    {
                        NewBlue[x, y] = true;  //зарождается синяя синяя
                        NewRed[x, y] = false;  // сделано для подстраховки
                        Grafika.FillRectangle(Brushes.Red, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1);
                        continue;  //пропуск шага в цикле
                    }
                    if (!EstJiznRed && RyadomRed == 3)  //если в клетке не было красной и она имеет 3 соседа красных
                    {
                        NewRed[x, y] = true; //красная клетка рождается 
                    }
                    else if (EstJiznRed && (RyadomRed < 2 || RyadomRed > 3)) //если красная клетка жива и имеет менее 2-х или более 3-х соседей
                    {
                        NewRed[x, y] = false;  //клетка красного цвета умирает
                    }
                    else // иначе
                    {
                        NewRed[x, y] = Red[x, y]; //клетка будет иметь такое же состояние
                    }
                    if (!EstJiznBlue && RyadomBlue == 3) //если в клетке не было синей и она имеет 3 соседа синих
                    {
                        NewBlue[x, y] = true;  //в этой клетке зарождается синяя жизнь
                    }
                    else if (EstJiznBlue && (RyadomBlue < 2 || RyadomBlue > 3))  //если синяя клетка жива и имеет менее 2-х или более 3-х соседей
                    {
                        NewBlue[x, y] = false;  //синяя клетка отмирает
                    }
                    else // иначе
                    {
                        NewBlue[x, y] = Blue[x, y]; //клетка будет иметь такое же состояние
                    }
                    if (NewRed[x, y])  //если клетка по этим координатам жива
                    {
                        Grafika.FillRectangle(Brushes.Red, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1); // то она красная
                    }
                    else if (NewBlue[x, y])  //если клетка этим координатам жива-
                    {
                        Grafika.FillRectangle(Brushes.Blue, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1); // то она синяя
                    }
                }
            }
            Red = NewRed;  //новое поколение красных клеток становится старым
            Blue = NewBlue;  //новое поколение синих клеток становится текущим
            GamePole.Refresh();  //новое поколение отрисовывается на pictureBox
        }
        private void Timer_Tick(object sender, EventArgs e)  //метод тиков таймера
        {
            NextPokoleniya(); //вызов метода генерации 
        }
        private void Pause_Click(object sender, EventArgs e)  // метод для паузы игры 
        {
            if (Timer.Enabled)  //если таймер(появление нового поколения) ключён-отключаем таймер
            {
                Timer.Enabled = false;
            }
            else  //если таймер выключен и новые покаления не появляются-включаем таймер и генерация появляется
            {
                Timer.Enabled = true;
            }
        }
        private void GamePole_MouseClick(object sender, MouseEventArgs e)  //метод вызывается при щелчке мыши на поле pictureBox
        {
            if (Timer.Enabled)  //таймер включён- выходим из метода
                return;

            int x = e.Location.X / Razreshenie;  //координата по x 
            int y = e.Location.Y / Razreshenie;  //координата по y 

            if (e.Button == MouseButtons.Left)  //если нажата ЛКМ
            {
                if (Validator(x, y))  //если координата клетки в диапазоне 
                {
                    if (Red[x, y])  //если клетка уже существует 
                    {
                        Red[x, y] = false;  //стираем клетку из массива и закркашиваем белым
                        Grafika.FillRectangle(Brushes.White, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1);
                    }
                    else  //если клетки ещё не существует 
                    {
                        Red[x, y] = true;  //создаём клетку в данных координатах красным цветом
                        Grafika.FillRectangle(Brushes.Red, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1);
                    }
                }
            }
            if (e.Button == MouseButtons.Right)  //если нажата ПКМ
            {
                if (Validator(x, y))  //если координата клетки в диапазоне
                {
                    if (Blue[x, y])  //если клетка уже существует 
                    {
                        Blue[x, y] = false;  //стираем клетку из массива и закркашиваем белым
                        Grafika.FillRectangle(Brushes.White, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1);
                    }
                    else  //если клетки ещё не существует 
                    {
                        Blue[x, y] = true;  //создаём клетку в данных координатах синим цветом
                        Grafika.FillRectangle(Brushes.Blue, x * Razreshenie, y * Razreshenie, Razreshenie - 1, Razreshenie - 1);
                    }
                }
            }
            GamePole.Refresh();  //обновляем pictureBox с сеткой ,на которой внесены изменения
        }
        private bool Validator(int x, int y)  //метод для проверки границ 
        {
            return x >= 0 && y >= 0 && x < Columns && y < Rows;  //если координата нажатия кнопки входит в диапазон массива- всё хорошо
        }
        private void Start_Click(object sender, EventArgs e) // метод запуска игры по нажатию кнопки
        {
            StartGame(); //вызов метода для начала игры
        }
        private void Exit_Click(object sender, EventArgs e) // метод выхода из игры по нажатию кнопки 
        {
            Application.Exit(); // выход из формы
        }
        private void Form1_Resize_1(object sender, EventArgs e)
        {
            Rows = GamePole.Height / Razreshenie;  //вычисляем размерность экрана
            Columns = GamePole.Width / Razreshenie; //вычисляем размерность экрана
            Red = new bool[Columns, Rows];  //инициализируем размерность массивов для 2-х цветов
            Blue = new bool[Columns, Rows];
            GamePole.Image = new Bitmap(GamePole.Width, GamePole.Height);  //создаём сетку игры 
            Grafika = Graphics.FromImage(GamePole.Image);  //переносим сетку в изображение
            Grafika.Clear(Color.White);  //заполняем графику белым цветом
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            Timer.Enabled = false;  //остановка таймера(генерации следущего поколения)
            GamePole.Image = new Bitmap(GamePole.Width, GamePole.Height);  //создание нового поля 
            Grafika = Graphics.FromImage(GamePole.Image);  //инициализация переменной Grafika полем 
            Grafika.Clear(Color.White);  //заполнение поля белым цветом
            for (int i = 0; i < Columns; i++)  //цикл проходится по всем элементам массива
            {
                for (int j = 0; j < Rows; j++) //цикл проходится по всем элементам массива
                {
                   Red[i, j] = false; // удаление красных
                   Blue[i, j] = false; // удаление синих
                }
            }
        }
    }
}
