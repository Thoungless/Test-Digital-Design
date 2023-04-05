using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace TestTask2
{

    /*
     * Тест 1 с запросами SQL
     * 
     * Задара 1 - Сотрудника с максимальной заработной платой.
     * 
     * Решение - SELECT *
     *           FROM "Employee" 
     *           WHERE "Salary" = (SELECT MAX("Salary") FROM "Employee")
     *
     * 
     * 
     * 
     *  Задача 2 - Вывести одно число: максимальную длину цепочки руководителей по таблице сотрудников (вычислить глубину дерева).
     *  
     *  Решение - WITH RECURSIVE managers AS (
     *            SELECT "Id", "Chief_id", 1 AS level
     *            FROM "Employee"
     *            WHERE "Chief_id" IS NULL
     *          
     *            UNION ALL
     *          
     *            SELECT e."Id", e."Chief_id", m.level + 1
     *            FROM "Employee" e
     *            INNER JOIN managers m ON m."Id" = e."Chief_id"
     *            )
     *            SELECT MAX(level) AS max_level
     *            FROM managers;
     *   
     * 
     * 
     * 
     *  Задача 3 - Отдел, с максимальной суммарной зарплатой сотрудников.
     * 
     *  Решение - WITH sum_salary AS
     *          ( SELECT "Department_Id", SUM("Salary") salary
     *            FROM   "Employee"
     *            GROUP  BY "Department_Id" )
     *            SELECT "Department_Id"
     *            FROM   sum_salary     
     *            WHERE  sum_salary.salary = ( SELECT MAX(salary) FROM sum_salary ) 
     * 
     * 
     *  Задача 4 - Сотрудника, чье имя начинается на «Р» и заканчивается на «н».
     * 
     *  Решение - SELECT * FROM "Employee"
     *            WHERE "Name" LIKE 'Р%н'
     * 
     */

    class Program
    {
        static void Main(string[] args)
        {
            string book = WarAndPeaseParse(@"http://az.lib.ru/t/tolstoj_lew_nikolaewich/text_0040.shtml");
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\book.txt";


            List<string> Words = new List<string>();
            List<string> allWords = new List<string>();

            Words.AddRange(book.Split(' ', ','));

            foreach(var word in Words)
            {
                if (word == string.Empty)
                    continue;
                else
                {
                    var clearWord = word.TrimEnd(',', '.', '!', '?', ':');
                    allWords.Add(clearWord.ToLower());
                }
            }

            var r = allWords.GroupBy(g => g);

            var a = allWords.GroupBy(x => x).OrderByDescending(x => x.Count()).Where(x => x.Count() > 1).Select(x => $"{x.Key} {x.Count()}");
            foreach (var item in a)
            {
                using (StreamWriter stream = new StreamWriter(path, true))
                {
                    stream.WriteLine(item);
                }
            }





            Console.ReadKey();
        }

        static string WarAndPeaseParse(string http)
        {
            string l = "<dd>&nbsp;&nbsp;";
            WebRequest req = WebRequest.Create(http);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
            string text = sr.ReadToEnd();
            text = text.Substring(text.IndexOf(l) + l.Length);
            text = Regex.Replace(text, "<.*?>", String.Empty);
            text = Regex.Replace(text, @"\r", String.Empty);
            text = Regex.Replace(text, @"\n", String.Empty);
            text = Regex.Replace(text, @"&nbsp;", String.Empty);

            return text;

        }
       

    }
}
