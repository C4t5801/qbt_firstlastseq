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

            if (args.Length > 0)
            {
                hashArg = args[0]; // First argument is always the %K hash
            }

            if (args.Length > 1)
            {
                // Second argument (if given) is the modified port number.
                string rawPort = args[1].Trim();
                if (!string.IsNullOrWhiteSpace(rawPort))
                {
                    portArg = rawPort;
                }
            }

            // Launch the form and pass both variables
            Application.Run(new Form1(hashArg, portArg));
        }
    }
}
