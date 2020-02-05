/*************************************************************
 *
 * Написать на C# утилиту, которая мониторит процессы Windows и "убивает" те процессы, которые работают слишком долго.
 * - На входе три параметра: название процесса, допустимое время жизни (в минутах) и частота проверки (в минутах).
 * - Утилита запускается из командной строки. При старте она считывает три входных параметра и начинает мониторить процессы с указанной частотой. 
 * Если процесс живет слишком долго – завершает его и выдает сообщение в лог.
 * Пример запуска:
 * monitor.exe notepad 5 1
 * С такими параметрами утилита раз в минуту проверяет, не живет ли процесс notepad больше пяти минут, и "убивает" его, если живет.
 * 
 *************************************************************/

using System;
using System.Linq;
using System.Diagnostics;
using System.Timers;

namespace ConsoleAppKiller
{
    class ConsoleAppKiller
    {
        private static string procName;
        private static int timeDiff;
        private static int timeScan;

        static void Main(string[] args)
        {
            procName = args[0];
            timeDiff = int.Parse(args[1]);
            timeScan = 60000 * int.Parse(args[2]);

            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                //Тут должна быть работа с таймером
                AllInfoProcess();
            }
        }

        static void AllInfoProcess()
        {
            var myProcess = from proc in Process.GetProcessesByName(procName)
                            orderby proc.Id
                            select proc;
            foreach (var p in myProcess)
            {
                DateTime diffTime = p.StartTime.AddMinutes(timeDiff);
                if (DateTime.Now.CompareTo(diffTime) > 0)
                {
                    StopProcess(p.Id);
                    //Тут должна быть запись в лог
                }
            }
        }

        static void StopProcess(int s)
        {
            Process myProc = null;
            try
            {
                myProc = Process.GetProcessById(s);
                myProc.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
