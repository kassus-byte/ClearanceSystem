using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolClearanceSystem
{
    public class DocumentService
    {

        public static void ViewDocument(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.");
            }

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }

}
