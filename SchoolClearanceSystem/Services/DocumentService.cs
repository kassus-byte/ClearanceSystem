using System;
using System.Diagnostics;
using System.IO;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace SchoolClearanceSystem
{
    /// <summary>
    /// OOP CONCEPT: SINGLE RESPONSIBILITY PRINCIPLE (SRP) & ABSTRACTION
    /// Centralizes all operating system file interactions and browser routines.
    /// This keeps form code lightweight and purely presentation-focused.
    /// </summary>
    public static class DocumentService
    {
        /// <summary>
        /// Centralized upload abstraction using DevExpress file selectors.
        /// </summary>
        public static string UploadDocument(string title = "Select Attachment File")
        {
            using (XtraOpenFileDialog dialog = new XtraOpenFileDialog())
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

        /// <summary>
        /// Safely opens any existing file using the default OS shell viewer.
        /// </summary>
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
                // OOP Abstraction: Interfacing seamlessly with Windows Shell Process Subsystems
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