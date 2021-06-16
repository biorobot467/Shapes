using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace WindowsFormsApp1
{ 
    public partial class Form1 : Form
    {
        string PATH = @"G:\С#\Shapes\";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            textBox3.Text = "";
            string p = PATH + textBox1.Text;
            if (Shape.IsFilEx(p) == true) //Если он есть:
            {
                int ctObj = 0, //Кол-во фигур
                    ix = 0,
                    ixObj = 0; //Индекс фигуры
                string name = null; //Название
                List<Point> coot = new List<Point> { }; //Координаты вершин
                Shape[] sh; //Массив фигур
                using (StreamReader str = File.OpenText(p)) //Инфа о фигурах
                {
                    ctObj = Convert.ToInt32(str.ReadLine()); //Первое число - количество фигур
                    sh = new Shape[ctObj]; //Память для массива фигур

                    while (ix < ctObj)
                    {
                        name = null;
                        coot = Shape.RShInf(ref name, str, p); //Инфа о каждой фигуре
                        sh[ix] = new Shape(name, coot); //Создаём фигуру
                        ix++;
                    }
                }
                name = textBox2.Text;
                Shape choSh = Shape.SearchShape(name, sh, ref ixObj); //Поиск фигуры
                if (choSh == null) //Если её нет
                    textBox3.Text = "Shape isn't exists!";
                else
                {
                    bool isInt = false;
                    for (int i = 0; i < sh.Length; i++)
                    {
                        //Пересечение
                        if (i != ixObj && sh[i].IsIntersection(sh[ixObj]) == true)
                        {
                            isInt = true;
                            richTextBox1.Text = richTextBox1.Text + $"{sh[i].Name} - INTERSECTION\n";
                        }
                        //Фигура в фигуре
                        if (i != ixObj && sh[i].IsShInSh(sh[ixObj]) == true)
                        {
                            isInt = true;
                            richTextBox1.Text = richTextBox1.Text + $"{sh[i].Name} - COVER\n";
                        }
                        //Фигура на фигуре
                        if (i != ixObj && sh[ixObj].IsShInSh(sh[i]) == true)
                        {
                            isInt = true;
                            richTextBox1.Text = richTextBox1.Text + $"{sh[i].Name} - INSIDE\n";
                        }
                    }
                    if (isInt == false)
                        richTextBox1.Text = "No intersections.";
                }
            }
            else textBox3.Text = "404 - File not found";
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }


    }
    public class Point
    {
        public double x { get; set; } //X
        public double y { get; set; } //Y
    }
    public class Shape
    {
        public string _name = null; //Имя
        private List<Point> _lOfKoord = null; //Список вершин 
        public Shape(string name, List<Point> list)//создаём фигуру
        {
            _name = name;
            _lOfKoord = list;
            _lOfKoord.Add(list[0]);
        }

        public static bool IsFilEx(string p)//Есть ли этот файл?
        {
            string temp = null;
            try//есть
            {
                using (StreamReader str = File.OpenText(p))
                {
                    temp = str.ReadLine();
                }
                return true;
            }
            catch (Exception)//нет
            {
                return false;
            }
        }
        public static Shape SearchShape(string name, Shape[] arrSh, ref int ixObj)
        {
            for (int i = 0; i < arrSh.Length; i++)
            {
                if (arrSh[i].Name == name) //Нашли и записали индекс
                {
                    ixObj = i;
                    return arrSh[i]; //Возвращаем
                }
            }

            return null;
        }
        public static List<Point> RShInf(ref string name, StreamReader str, string p)
        {
            string stInf, //Инфа
                strCCoord = null; //Число вершин строка
            string xStr = null, //X
                yStr = null; //Y

            int ix = 0,
                cCoord = 0; //Число вершин число

            List<Point> coot = new List<Point> { }; //Список вершин

            stInf = str.ReadLine() + "\0"; //Для фигур сначала кол-во вершин и имя

            while (stInf[ix] != '\0') //Читаем
            {
                if (stInf[ix] != ' ')
                {
                    name += stInf[ix]; //Имя
                    ix++;
                }
                else
                {
                    ix++;

                    while (stInf[ix] != '\0')
                    {
                        strCCoord += stInf[ix]; //Число вершин
                        ix++;
                    }

                    cCoord = Convert.ToInt32(strCCoord);
                }
            }

            ix = 0;
            int i;
            while (ix < cCoord) //Сравнение вершин
            {
                stInf = str.ReadLine() + "\0";

                i = 0;
                while (stInf[i] != '\0')
                {
                    if (stInf[i] != ' ') // Х И Y записаны через пробел
                    {
                        xStr += stInf[i]; //Х
                        i++;
                    }
                    else
                    {
                        if (stInf[i] == ' ') i++;
                        while (stInf[i] != '\0')
                        {
                            yStr += stInf[i]; //Y
                            i++;
                        }
                    }
                }

                Point pt = new Point(); //получаем точку вершины

                pt.x = Convert.ToDouble(xStr);
                pt.y = Convert.ToDouble(yStr);

                coot.Add(pt); //Записываем в список вершин

                xStr = null;
                yStr = null;

                ix++;
            }

            return coot; //Возвращаем актуальный список
        }

        public string Name { get => _name; }
        public bool IsIntersection(Shape otObj)//Тест на пересечение
        {
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;

            for (int i = 1; i < this._lOfKoord.Count; i++)
            {
                for (int j = 1; j < otObj._lOfKoord.Count; j++)
                {
                    x1 = this._lOfKoord[i - 1].x;
                    x2 = this._lOfKoord[i].x;
                    x3 = otObj._lOfKoord[j - 1].x;
                    x4 = otObj._lOfKoord[j].x;
                    y1 = this._lOfKoord[i - 1].y;
                    y2 = this._lOfKoord[i].y;
                    y3 = otObj._lOfKoord[j - 1].y;
                    y4 = otObj._lOfKoord[j].y;
                    if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4, y4) == true)//Пересечение сторон
                        return true;
                }
            }

            return false;
        }
        public bool IsShInSh(Shape otObj) //Фигура в фигуре
        {
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;

            bool isRInt = false, //Правый луч с внешней
                isLInt = false; //Левый луч с внешней

            for (int i = 1; i < otObj._lOfKoord.Count; i++)
            {
                for (int j = 1; j < this._lOfKoord.Count; j++)
                {
                    x1 = this._lOfKoord[j - 1].x;
                    y1 = this._lOfKoord[j - 1].y;
                    x2 = this._lOfKoord[j].x;
                    y2 = this._lOfKoord[j].y;

                    x3 = otObj._lOfKoord[i - 1].x;
                    y3 = otObj._lOfKoord[i - 1].y;
                    x4 = otObj._lOfKoord[i].x;
                    y4 = otObj._lOfKoord[i].y;
                    if (y3 == y4)//Лучи к горизонтальной стороне
                    {
                        if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 + 1000, y4) == true)//Если пересечение справа
                        {
                            isRInt = true;
                            continue;
                        }
                        if (IsIntsecSeg(x1, y1, x2, y2, x3 - 1000, y3, x4, y4) == true)//Если слева
                        {
                            isLInt = true;
                            continue;
                        }
                    }
                    if (x3 == x4)//вертикально
                    {
                        if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4, y4 + 1000) == true)
                        {
                            isRInt = true;
                            continue;
                        }

                        if (IsIntsecSeg(x1, y1, x2, y2, x3, y3 - 1000, x4, y4) == true)
                        {
                            isLInt = true;
                            continue;
                        }
                    }
                    if (x3 < x4)//Если х3 левее х4
                    {
                        //Ниже
                        if (y3 < y4)
                        {
                            if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 + 1000) == true)
                            {
                                isRInt = true;
                                continue;
                            }

                            if (IsIntsecSeg(x1, y1, x2, y2, x3 - 1000, y3 - 1000, x4, y4) == true)
                            {
                                isLInt = true;
                                continue;
                            }
                        }

                        //Выше
                        if (y3 > y4)
                        {
                            if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 - 1000) == true)
                            {
                                isRInt = true;
                                continue;
                            }

                            if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 - 1000, y4 + 1000) == true)
                            {
                                isLInt = true;
                                continue;
                            }
                        }
                    }
                    if (x3 > x4)//Если правее
                    {
                        if (y3 < y4)//Ниже
                        {
                            if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 - 1000) == true)
                            {
                                isRInt = true;
                                continue;
                            }

                            if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 - 1000, y4 + 1000) == true)
                            {
                                isLInt = true;
                                continue;
                            }
                        }
                        if (y3 > y4)//Выше
                        {
                            if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 + 1000) == true)
                            {
                                isRInt = true;
                                continue;
                            }

                            if (IsIntsecSeg(x1, y1, x2, y2, x3, y3, x4 - 1000, y4 - 1000) == true)
                            {
                                isLInt = true;
                                continue;
                            }
                        }
                    }
                }

                if (isRInt != true || isLInt != true)
                    return false;

                isLInt = false;
                isRInt = false;
            }

            return true; //Если лучи пересекают внешнюю фигуру, значит всё верно
        }

        //Пересечение двух сторон
        private bool IsIntsecSeg(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            double temp;
            if (x1 > x2)//если х2 правее х1, меняем местами
            {
                temp = x1;
                x1 = x2;
                x2 = temp;
                temp = y1;
                y1 = y2;
                y2 = temp;
            }
            if (x3 > x4) //х3 правее х4, меняем местами
            {
                temp = x3;
                x3 = x4;
                x4 = temp;
                temp = y3;
                y3 = y4;
                y4 = temp;
            }
            if (y1 == y2 && x3 == x4)//Случай образования креста пересечением
            {
                if (y3 > y4)
                {
                    if ((y1 <= y3 && y1 >= y4) && (y2 <= y3 && y2 >= y4) && (x3 <= x2 && x3 >= x1) && (x4 <= x2 && x4 >= x1))
                        return true;
                }
                else
                {
                    if ((y1 >= y3 && y1 <= y4) && (y2 >= y3 && y2 <= y4) && (x3 <= x2 && x3 >= x1) && (x4 <= x2 && x4 >= x1))
                        return true;
                }
            }
            if (y3 == y4 && x1 == x2)//прямой крест
            {
                if (y1 > y2)
                {
                    if ((y3 <= y1 && y3 >= y2) && (y4 <= y1 && y4 >= y2) && (x1 <= x4 && x1 >= x3) && (x2 <= x4 && x2 >= x3))
                        return true;
                }
                else
                {
                    if ((y3 >= y1 && y3 <= y2) && (y4 >= y1 && y4 <= y2) && (x1 <= x4 && x1 >= x3) && (x2 <= x4 && x2 >= x3))
                        return true;
                }
            }
            if (x1 == x2 || y1 == y2)//горизонтальный или вертикальный
            {
                while (x4 != x3 || y4 != y3)
                {
                    if (x1 == x2)//Пересечение нашли
                        if (x4 == x1 && ((y4 <= y1 && y4 >= y2) || (y4 >= y1 && y4 <= y2)))
                            return true;
                    if (y1 == y2)//не нашли
                        if (y4 == y1 && ((x4 <= x1 && x4 >= x2) || (x4 >= x1 && x4 <= x2)))
                            return true;
                    if (x4 != x3)//обрезаем отрезок
                        x4 = Math.Round(x4 - 0.1, 2);

                    if (y4 != y3)
                    {
                        if (y3 < y4)
                            y4 = Math.Round(y4 - 0.1, 2);

                        if (y3 > y4)
                            y4 = Math.Round(y4 + 0.1, 2);
                    }
                }
                return false;
            }
            if (x3 == x4 || y3 == y4)//индетично прошлому условию
            {
                while (x2 != x1 || y2 != y1)
                {
                    if (x3 == x4)
                        if (x2 == x3 && ((y2 <= y3 && y2 >= y4) || (y2 >= y3 && y2 <= y4)))
                            return true;

                    if (y3 == y4)
                        if (y2 == y3 && ((x2 <= x3 && x2 >= x4) || (x2 >= x3 && x2 <= x4)))
                            return true;

                    if (x2 != x1)
                        x2 = Math.Round(x2 - 0.1, 2);

                    if (y2 != y1)
                    {
                        if (y1 < y2)
                            y2 = Math.Round(y2 - 0.1, 2);

                        if (y1 > y2)
                            y2 = Math.Round(y2 + 0.1, 2);
                    }
                }

                return false;
            }
            double k1, k2;//пересечение через уравнения прямых
            if (y1 == y2)//для горизонтального коэффициент равен нулю
                k1 = 0;
            else
            {
                k1 = (y2 - y1) / (x2 - x1);
            }

            if (y3 == y4)
            {
                k2 = 0;
            }
            else
            {
                k2 = (y4 - y3) / (x4 - x3);
            }

            if (k1 == k2)
                return false;
            double b1 = y1 - k1 * x1;
            double b2 = y3 - k2 * x3;
            double x = (b2 - b1) / (k1 - k2); //х и у пересечения
            double y;
            y = k1 * x + b1;
            if (((x1 <= x4 && x4 <= x2) || (x1 <= x3 && x3 <= x2)) &&
               (((x1 <= x) && (x2 >= x) && (x3 <= x) && (x4 >= x)) ||
                ((y1 <= y) && (y2 >= y) && (y3 <= y) && (y4 >= y))))
                return true;//пересечение есть
            else
                return false;//его нет
        }
    }
}
