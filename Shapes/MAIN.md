# MAIN PROGRAM
```
using System;
using System.IO;
using System.Collections.Generic;
using IntersectionsLib;

namespace Intersections_of_shapes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter path of file with info: ");
            string path = Console.ReadLine(); //ВВОД ПУТИ К ФАЙЛУ С ИНФОРМАЦИЕЙ О ФИГУРАХ

            if (IsFileExists(path) == true) //ЕСЛИ ФАЙЛ СУЩЕСТВУЕТ, ПРОДОЛЖАЕМ РАБОТУ ПРОГРАММЫ
            {
                int countObjects = 0, //КОЛИЧЕСТВО ФИГУР
                    index = 0, 
                    indexObject = 0; //ИНДЕКС ФИГУРЫ, КОТОРУЮ НУЖНО СРАВНИТЬ С ОСТАЛЬНЫМИ
                string name = null; //ИМЯ ФИГУРЫ
                List<Point> coordinates = new List<Point> { }; //СПИСОК КООРДИНАТ ВЕРШИН ФИГУРЫ
                Shape[] shapes; //МАССИВ ФИГУР

                using (StreamReader stream = File.OpenText(path)) //СЧИТЫВАНИЕ ИНФОРМАЦИИ О ФИГУРАХ ИЗ ФАЙЛА
                {
                    countObjects = Convert.ToInt32(stream.ReadLine()); //САМОЕ ПЕРВОЕ ЧИСЛО ИЗ ФАЙЛА - КОЛИЧЕСТВО ФИГУР
                    shapes = new Shape[countObjects]; //ВЫДЕЛЯЕМ ПАМЯТЬ ПОД МАССИВ НА ЗАДАННОЕ КОЛИЧЕСТВО ФИГУР

                    while (index < countObjects)
                    {
                        name = null;

                        coordinates = ReadShapeInfo(ref name, stream, path); //СЧИТЫВАНИЕ ИНФОРМАЦИИ О ФИГУРЕ ИЗ ФАЙЛА
                        shapes[index] = new Shape(name, coordinates); //СОЗДАНИЕ ФИГУРЫ НА ОСНОВЕ ПОЛУЧЕННОЙ ИНФОРМАЦИИ
                        index++;
                    }
                }

                while (Console.ReadKey().Key != ConsoleKey.Escape)
                {
                    Console.WriteLine("Enter name of shape: ");
                    name = Console.ReadLine(); //ВВОД ИМЕНИ ФИГУРЫ, С КОТОРОЙ НУЖНО СРАВНИТЬ ОСТАЛЬНЫЕ

                    Shape chosenShape = SearchShape(name, shapes, ref indexObject); //ПОИСК ФИГУРЫ С ТАКИМ ИМЕНЕМ

                    //ЕСЛИ ФИГУРА С ТАКИМ ИМЕНЕМ НЕ НАЙДЕНА, ВЫВОДИТСЯ СООТВЕТСТВУЮЩЕЕ СООБЩЕНИЕ
                    if (chosenShape == null) 
                        Console.WriteLine("Shape isn't exists!");
                    else
                    {
                        bool isIntersection = false;
                        Console.WriteLine();
                        for (int i = 0; i < shapes.Length; i++)
                        {
                            //ПРОВЕРКА НА ПЕРЕСЕЧЕНИЕ
                            if (i != indexObject && shapes[i].IsIntersection(shapes[indexObject]) == true)
                            {
                                isIntersection = true;
                                Console.WriteLine($"{shapes[i].Name} - INTERSECTION");
                            }

                            //ПРОВЕРКА НА ВХОЖДЕНИЕ ОДНОЙ ФИГУРЫ В ДРУГУЮ
                            if (i != indexObject && shapes[i].IsShapeInShape(shapes[indexObject]) == true)
                            {
                                isIntersection = true;
                                Console.WriteLine($"{shapes[i].Name} - COVER");
                            }

                            //ПРОВЕРКА НА ПОКРЫВАНИЕ ОДНОЙ ФИГУРЫ ДРУГОЙ
                            if (i != indexObject && shapes[indexObject].IsShapeInShape(shapes[i]) == true)
                            {
                                isIntersection = true;
                                Console.WriteLine($"{shapes[i].Name} - INSIDE");
                            }
                        }

                        //ЕСЛИ ПЕРЕСЕЧЕНИЯ НЕТ, ТО ВЫВОДИМ ОБ ЭТОМ СООБЩЕНИЕ
                        if (isIntersection == false)
                            Console.WriteLine("No intersections.");
                    }
                }
            }
        }

        //ПРОВЕРКА НА СУЩЕСТВОВАНИЯ ФАЙЛА
        private static bool IsFileExists(string path)
        {
            string temp = null;

            //ЕСЛИ ИЗ ФАЙЛА ПОЛУЧАЕТСЯ ЧТО-ЛИБО СЧИТАТЬ, ТО ФАЙЛ СУЩЕСТВУЕТ
            try
            {
                using (StreamReader stream = File.OpenText(path))
                {
                    temp = stream.ReadLine();
                }
                return true;
            }
            //ЕСЛИ НЕТ, ТО ФАЙЛ НЕ НАЙДЕН
            catch (Exception)
            {
                Console.WriteLine("File isn't found!");
                return false;
            }
        }

        //СЧИТЫВАНИЕ ИНФОРМАЦИИ О ФИГУРЕ ИЗ ФАЙЛА
        private static List<Point> ReadShapeInfo(ref string name, StreamReader stream, string path) 
        {
            string stringInfo, //ИНФОРМАЦИЯ О ФИГУРЕ
                strCountCoord = null; //КОЛИЧЕСТВО ВЕРШИН (СТРОКА)
            string xStr = null, //КООРДИНАТА X
                yStr = null; //КООРДИНАТА Y

            int index = 0, 
                countCoord = 0; //КОЛИЧЕСТВО ВЕРШИН (ЧИСЛО)

            List<Point> coordinates = new List<Point> { }; //СПИСОК ВЕРШИН

            stringInfo = stream.ReadLine() + "\0"; //ДЛЯ КАЖДОЙ ФИГУРЫ В ФАЙЛЕ В ПЕРВУЮ ОЧЕРЕДЬ ЗАПИСАНО ИМЯ И КОЛИЧЕСТВО ВЕРШИН

            while (stringInfo[index] != '\0') //В ЭТОМ ЦИКЛЕ СЧИТЫВАНИЕ ИМЕНИ ФИГУРЫ И КОЛИЧЕСТВА ЕГО ВЕРШИН
            {
                if (stringInfo[index] != ' ')
                {
                    name += stringInfo[index]; //СЧИТЫВАНИЕ ИМЕНИ
                    index++;
                }
                else
                {
                    index++;

                    while (stringInfo[index] != '\0')
                    {
                        strCountCoord += stringInfo[index]; //СЧИТЫВАНИЕ КОЛИЧЕСТВА ВЕРШИН
                        index++;
                    }

                    countCoord = Convert.ToInt32(strCountCoord);
                }
            }

            index = 0;
            int i;
            while (index < countCoord) //В ЭТОМ ЦИКЛЕ СЧИТЫВАЕНИЕ КООРДИНАТ ВСЕХ ВЕРШИН
            {
                stringInfo = stream.ReadLine() + "\0";

                i = 0;
                while (stringInfo[i] != '\0')
                {
                    if (stringInfo[i] != ' ') //В ФАЙЛЕ КООРДИНАТЫ Х И Y ЗАПИСАНЫ ЧЕРЕЗ ПРОБЕЛ
                    {
                        xStr += stringInfo[i]; //СЧИТЫВАЕНИЕ КООРДИНАТЫ Х
                        i++;
                    }
                    else
                    {
                        if (stringInfo[i] == ' ') i++;
                        while (stringInfo[i] != '\0')
                        {
                            yStr += stringInfo[i]; //СЧИТЫВАНИЕ КООРДИНАТЫ Y
                            i++;
                        }
                    }
                }

                Point point = new Point(); //ИЗ ПОЛУЧЕННЫХ КООРДИНАТ ПОЛУЧАЕМ ТОЧКУ

                point.x = Convert.ToDouble(xStr);
                point.y = Convert.ToDouble(yStr);

                coordinates.Add(point); //ЗАПИСЬ ВЕРШИНЫ В СПИСОК ВЕРШИН

                xStr = null;
                yStr = null;

                index++;
            }

            return coordinates; //ВОЗВРАЩАЕМ СПИСОК ВЕРШИН
        }

        //ПОИСК ФИГУРЫ ПО ЕЕ ИМЕНИ
        private static Shape SearchShape(string name, Shape[] arrShapes, ref int indexObject)
        {
            for (int i = 0; i < arrShapes.Length; i++)
            {
                if (arrShapes[i].Name == name) //ЕСЛИ В МАССИВЕ ФИГУР ЕСТЬ ФИГУРА С ИМЕНЕМ, КОТОРОЕ БЫЛО ВВЕДЕНО, ЗАПОМИНАЕМ ЕЕ ИНДЕКС
                {
                    indexObject = i;
                    return arrShapes[i]; //ВОЗВРАЩЕНИЕ ФИГУРЫ С ВВЕДЕННЫМ ИМЕНЕМ
                }
            }

            return null;
        }
    }
}
```
