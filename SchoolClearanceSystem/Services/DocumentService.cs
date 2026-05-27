using DevExpress.XtraEditors;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    public static class DocumentService
    {
        public static string UploadDocument(string title = "Select Attachment File")
        {
            using (var dialog = new XtraOpenFileDialog())
            {
                dialog.Title = title;
                dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    XtraMessageBox.Show("File attached successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return dialog.FileName;
                }
            }
            return string.Empty;
        }

        public static void ViewDocument(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                XtraMessageBox.Show("The requested clearance file record does not exist or was moved.",
                    "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"OS failed to initialize default viewer: {ex.Message}",
                    "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}