using System;
using System.Windows.Forms;

namespace qbt_firstlastseq
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string hashArg = string.Empty;
            string portArg = "8080"; // Default port if none is provided
            bool silentArg = false; // Default is show

            if (args.Length > 0)
            {
                // First argument is always the %K hash
                hashArg = args[0];
            }

            if (args.Length > 1)
            {
                // Check 2nd argument (if given).
                string arg2 = (args[1].Trim()).ToLower();
                if (!string.IsNullOrWhiteSpace(arg2))
                {
                    if (arg2.StartsWith("-p"))
                    {
                        // Port
                        portArg = arg2.Remove(0, 2);
                        Console.Write($"port: {portArg}");

                    }
                    else if (arg2 == "-hide")
                    {
                        // Silent
                        silentArg = true;
                    }
                    else
                    {
                        // Not recognised => ignore
                    }
                }
            }

            if (args.Length > 2)
            {
                // Check  3rd argument (if given).
                string arg3 = (args[2].Trim()).ToLower();
                if (!string.IsNullOrWhiteSpace(arg3))
                {
                    if (arg3.StartsWith("-p"))
                    {
                        // Port
                        portArg = arg3.Remove(0, 2);

                    }
                    else if (arg3 == "-hide")
                    {
                        // Silent
                        silentArg = true;
                    }
                    else
                    {
                        // Not recognised => ignore
                    }
                }
            }

            // Launch the form and pass variables
            Application.Run(new Form1(hashArg, portArg, silentArg));
        }
    }
}
