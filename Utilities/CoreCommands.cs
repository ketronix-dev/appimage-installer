namespace Utilities
{
    using System.Diagnostics;
    using System.Text;
        
    public static class CoreCommands
    {
        private static Process CreateProcess(string commandName, IEnumerable<string> paramsList, bool output = false)
        {
            string paramString = paramsList.Aggregate<string, string>(null, 
                (current, param) => current + " " + param);
            return new Process
            {
                StartInfo =
                {
                    FileName = commandName, // полный путь до инстансыируемого приложения
                    Arguments = paramString, // сюда передаются аргументы командной строки
                    UseShellExecute = output ? !output : !true, // Параметр отвечает за фоновое выполнение
                    RedirectStandardOutput = output // Параметр отвечает за переадресацию
                }
            };
        }
        public static string ExecShellCommandAsync(string commandName, IEnumerable<string> paramsList = null)
        {
            var bufer = new StringBuilder();
            using (var proc = CreateProcess(commandName, paramsList, true))
            {
                proc.Start(); // инстансыруемся
                while (!proc.StandardOutput.EndOfStream)
                {
                    bufer.AppendLine(proc.StandardOutput.ReadLine()); // пишем в буфер
                }
            }
            return bufer.ToString();
        }
        public static string ExecShellCommand(string command, string arguments = "")
        {
            var processStartInfo =
                arguments != "" ? new ProcessStartInfo(command, arguments) : new ProcessStartInfo(command);

            // переводим программу в CLI режим.
            processStartInfo.UseShellExecute = false;

            // разрешаем читать вывод процесса.
            processStartInfo.RedirectStandardOutput = true;

            var process = new Process();
            process.StartInfo = processStartInfo;

            process.Start();
            var output = process.StandardOutput.ReadToEnd();

            return output;
        }

    }
}
