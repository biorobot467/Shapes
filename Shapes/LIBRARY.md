# LIBRARY
```
using System;
using System.Collections.Generic;
using System.Text;

namespace IntersectionsLib
{
    public class Shape
    {
        public string _name = null; //ИМЯ ФИГУРЫ
        private List<Point> _listOfKoord = null; //СПИСОК КООРДИНАТ ВЕРШИН ФИГУРЫ

        //КОНСТРУКТОР ДЛЯ СОЗДАНИЯ ФИГУРЫ
        public Shape(string name, List<Point> list)
        {
            _name = name;
            _listOfKoord = list;
            _listOfKoord.Add(list[0]);
        }

        public string Name { get => _name; } //ГЕТТЕР ДЛЯ ИМЕНИ ФИГУРЫ

        //МЕТОД ДЛЯ ПРОВЕРКИ, ПЕРЕСЕКАЮТСЯ ЛИ ФИГУРЫ
        public bool IsIntersection(Shape otherObject)
        {
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;

            for (int i = 1; i < this._listOfKoord.Count; i++)
            {
                for (int j = 1; j < otherObject._listOfKoord.Count; j++)
                {
                    x1 = this._listOfKoord[i - 1].x;
                    x2 = this._listOfKoord[i].x;
                    x3 = otherObject._listOfKoord[j - 1].x;
                    x4 = otherObject._listOfKoord[j].x;
                    y1 = this._listOfKoord[i - 1].y;
                    y2 = this._listOfKoord[i].y;
                    y3 = otherObject._listOfKoord[j - 1].y;
                    y4 = otherObject._listOfKoord[j].y;

                    //ОПРЕДЕЛЕНИЯ ПЕРЕСЕЧЕНИЯ СТОРОН ФИГУР
                    if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4, y4) == true)
                        return true;
                }
            }

            return false;
        }

        //МЕТОД, ОПРЕДЕЛЯЮЩИЙ, НАХОДИТСЯ ЛИ ОДНА ФИГУРА ВНУТРИ ДРУГОЙ
        public bool IsShapeInShape(Shape otherObject)
        {
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0, x3 = 0, y3 = 0, x4 = 0, y4 = 0;

            bool isRightIntersect = false, //ПЕРЕМЕННАЯ ДЛЯ ПРОВЕРКИ ПЕРЕСЕЧЕНИЯ ПРАВОГО ЛУЧА С ВНЕШНЕЙ ФИГУРОЙ
                isLeftIntersect = false; //ПЕРЕМЕННАЯ ДЛЯ ПРОВЕРКИ ПЕРЕСЕЧЕНИЯ ЛЕВОГО ЛУЧА С ВНЕШНЕЙ ФИГУРОЙ

            for (int i = 1; i < otherObject._listOfKoord.Count; i++)
            {
                for (int j = 1; j < this._listOfKoord.Count; j++)
                {
                    x1 = this._listOfKoord[j - 1].x;
                    y1 = this._listOfKoord[j - 1].y;
                    x2 = this._listOfKoord[j].x;
                    y2 = this._listOfKoord[j].y;

                    x3 = otherObject._listOfKoord[i - 1].x;
                    y3 = otherObject._listOfKoord[i - 1].y;
                    x4 = otherObject._listOfKoord[i].x;    
                    y4 = otherObject._listOfKoord[i].y;

                //ПОСТОЕНИЕ ЛУЧЕЙ
                    //ЕСЛИ СТОРОНА ГОРИЗОНТАЛЬНА
                    if (y3 == y4)
                    {
                        //ЕСЛИ ЛУЧ В ПРАВУЮ СТОРОНУ ПЕРЕСЕКАЮТ ВНЕШНЮЮ ФИГУРУ
                        if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 + 1000, y4) == true)
                        {
                            isRightIntersect = true;
                            continue;
                        }

                        //ЕСЛИ ЛУЧ В ЛЕВУЮ СТОРОНУ ПЕРЕСЕКАЮТ ВНЕШНЮЮ ФИГУРУ
                        if (IsIntersectionSegments(x1, y1, x2, y2, x3 - 1000, y3, x4, y4) == true)
                        {
                            isLeftIntersect = true;
                            continue;
                        }
                    }

                    //ЕСЛИ СТОРОНА ВЕРТИКАЛЬНА
                    if (x3 == x4)
                    {
                        if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4, y4 + 1000) == true)
                        {
                            isRightIntersect = true;
                            continue;
                        }

                        if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3 - 1000, x4, y4) == true)
                        {
                            isLeftIntersect = true;
                            continue;
                        }
                    }

                    //ЕСЛИ КООРДИНАТА Х3 НАХОДИТСЯ ЛЕВЕЕ КООРДИНАТЫ Х4
                    if (x3 < x4)
                    {
                        //ЕСЛИ КООРДИНАТА Y3 НИЖЕ КООРДИНАТЫ Y4
                        if (y3 < y4)
                        {
                            if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 + 1000) == true)
                            {
                                isRightIntersect = true;
                                continue;
                            }

                            if (IsIntersectionSegments(x1, y1, x2, y2, x3 - 1000, y3 - 1000, x4, y4) == true)
                            {
                                isLeftIntersect = true;
                                continue;
                            }
                        }

                        //ЕСЛИ КООРДИНАТА Y3 ВЫШЕ КООРДИНАТЫ Y4
                        if (y3 > y4)
                        {
                            if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 - 1000) == true)
                            {
                                isRightIntersect = true;
                                continue;
                            }

                            if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 - 1000, y4 + 1000) == true)
                            {
                                isLeftIntersect = true;
                                continue;
                            }
                        }
                    }

                    //ЕСЛИ КООРДИНАТА Х3 НАХОДИТСЯ ПРАВЕЕ КООРДИНАТЫ Х4
                    if (x3 > x4)
                    {
                        //ЕСЛИ КООРДИНАТА Y3 НИЖЕ КООРДИНАТЫ Y4
                        if (y3 < y4)
                        {
                            if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 - 1000) == true)
                            {
                                isRightIntersect = true;
                                continue;
                            }

                            if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 - 1000, y4 + 1000) == true)
                            {
                                isLeftIntersect = true;
                                continue;
                            }
                        }

                        //ЕСЛИ КООРДИНАТА Y3 ВЫШЕ КООРДИНАТЫ Y4
                        if (y3 > y4)
                        {
                            if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 + 1000, y4 + 1000) == true)
                            {
                                isRightIntersect = true;
                                continue;
                            }

                            if (IsIntersectionSegments(x1, y1, x2, y2, x3, y3, x4 - 1000, y4 - 1000) == true)
                            {
                                isLeftIntersect = true;
                                continue;
                            }
                        }
                    }
                }

                if (isRightIntersect != true || isLeftIntersect != true)
                    return false;

                isLeftIntersect = false;
                isRightIntersect = false;
            }

            return true; //ЕСЛИ И ПРАВЫЙ И ЛЕВЫЙ ЛУЧ ПЕРЕСЕКАЮТ ВНЕШНЮЮ ФИГУРУ, ТО ОДНА ФИГУРА НАХОДИТСЯ ВНУТРИ ДРУГОЙ
        }

        //МЕТОД, ОПРЕДЕЛЯЮЩИЙ, ПЕРЕСЕКАЮТСЯ ЛИ 2 СТОРОНЫ ФИГУРЫ
        private bool IsIntersectionSegments(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4)
        {
            double temp;
            //ЕСЛИ ТОЧКА X1 ПРАВЕЕ X2, МЕНЯЕМ ИХ КООРДИНАТЫ МЕСТАМИ
            if (x1 > x2)
            {
                temp = x1;
                x1 = x2;
                x2 = temp;
                temp = y1;
                y1 = y2;
                y2 = temp;
            }
            //ЕСЛИ ТОЧКА X3 ПРАВЕЕ X4, МЕНЯЕМ ИХ КООРДИНАТЫ МЕСТАМИ
            if (x3 > x4)
            {
                temp = x3;
                x3 = x4;
                x4 = temp;
                temp = y3;
                y3 = y4;
                y4 = temp;
            }

            //СЛУЧАЙ, КОГДА ОТРЕЗКИ МЕЖДУ СОБОЙ ОБРАЗУЮТ ПРЯМОЙ КРЕСТ
            if (y1 == y2 && x3 == x4)
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

            //СЛУЧАЙ, КОГДА ОТРЕЗКИ ТАКЖЕ МЕЖДУ СОБОЙ ОБРАЗУЮТ ПРЯМОЙ КРЕСТ
            if (y3 == y4 && x1 == x2)
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

            //СЛУЧАЙ, КОГДА ПЕРВЫЙ ОТРЕЗОК ГОРИЗОНТАЛЬНЫЙ ЛИБО ВЕРТИКАЛЬНЫЙ
            if (x1 == x2 || y1 == y2)
            {
                //В ЭТОМ ЦИКЛЕ ОБРЕЗАЕМ ОТРЕЗОК, ПОКА НЕ ОН НЕ ЗАКОНЧИТСЯ, ЛИБО НЕ ДОЙДЕТ ДО ТОЧКИ ПЕРЕСЕЧЕНИЯ
                while (x4 != x3 || y4 != y3)
                {
                    //ЕСЛИ ТОЧКА ПЕРЕСЕЧЕНИЯ НАЙДЕНА
                    if (x1 == x2)
                        if (x4 == x1 && ((y4 <= y1 && y4 >= y2) || (y4 >= y1 && y4 <= y2)))
                            return true;

                    //ЕСЛИ ТОЧКА ПЕРЕСЕЧЕНИЯ НАЙДЕНА
                    if (y1 == y2)
                        if (y4 == y1 && ((x4 <= x1 && x4 >= x2) || (x4 >= x1 && x4 <= x2)))
                            return true;

                    //ОБРЕЗКА ОТРЕЗКА
                    if (x4 != x3)
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

            //ИДЕНТИЧНО ПРОШЛОМУ УСЛОВИЮ
            if (x3 == x4 || y3 == y4)
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

            //ДАЛЕЕ - ПОИСК ПЕРЕСЕЧЕНИЯ ЧЕРЕЗ УРАВНЕНИЕ ПРЯМЫХ
            double k1, k2;

            //КОЭФФИЦИЕНТ РАВЕН 0, ЕСЛИ ОТРЕЗОК ГОРИЗОНАТАЛЬНЫЙ
            if (y1 == y2)
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

            //X И Y - КООРДИНАТЫ ТОЧКИ ПЕРЕСЕЧЕНИЯ
            double x = (b2 - b1) / (k1 - k2);
            double y;

            y = k1 * x + b1;

            //ЕСЛИ ПЕРЕСЕЧЕНИЕ СУЩЕСТВУЕТ - ВОЗВРАЩАЕМ TRUE, В ИНОМ СЛУЧАЕ - FALSE
            if (((x1 <= x4 && x4 <= x2) || (x1 <= x3 && x3 <= x2)) &&
               (((x1 <= x) && (x2 >= x) && (x3 <= x) && (x4 >= x)) || 
                ((y1 <= y) && (y2 >= y) && (y3 <= y) && (y4 >= y))))
                return true;
            else
                return false;
        }
    }
}
```
