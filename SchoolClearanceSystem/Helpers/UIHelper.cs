using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;

namespace SchoolClearanceSystem.Helpers
{
    public static class UIHelper
    {
        // ── Clearance Slip ─────────────────────────────────────────────

        public static void PopulateClearanceSlip(
            LabelControl lblSemYear, LabelControl lblNameID,
            LabelControl lblProgramDepartment, LabelControl lblDateIssued,
            User currentUser, string semester, string academicYear)
        {
            if (currentUser == null)
            {
                ClearClearanceSlip(lblSemYear, lblNameID, lblProgramDepartment, lblDateIssued);
                return;
            }

            lblSemYear.Text = $"{semester}, Academic Year {academicYear}";
            lblNameID.Text = $"{currentUser.FullName} · {currentUser.UserID}";
            lblProgramDepartment.Text = $"{currentUser.Program} — College of Computer Studies";
            lblDateIssued.Text = $"Issued: {DateTime.Now:MMM d, yyyy}";
        }

        public static void ClearClearanceSlip(
            LabelControl lblSemYear, LabelControl lblNameID,
            LabelControl lblProgramDepartment, LabelControl lblDateIssued)
        {
            lblSemYear.Text = string.Empty;
            lblNameID.Text = string.Empty;
            lblProgramDepartment.Text = string.Empty;
            lblDateIssued.Text = string.Empty;
        }

        // ── User Session ───────────────────────────────────────────────

        public static void PopulateUserSessionContext(
            LabelControl lblWelcome, LabelControl lblFullName,
            LabelControl lblUserID, LabelControl lblProgram, LabelControl lblYear, User currentUser)
        {
            if (currentUser == null) return;

            lblWelcome.Text = $"Welcome, {currentUser.FullName}!";
            lblFullName.Text = currentUser.FullName;
            lblUserID.Text = currentUser.UserID ?? "0000";
            lblProgram.Text = currentUser.Program ?? "N/A";
            lblYear.Text = currentUser.Year ?? "N/A";
        }

        // ── Period Fields ──────────────────────────────────────────────

        public static void SetPeriodFields(
            TextEdit txtSemester, TextEdit txtCurrentSchoolYear,
            string semester, string academicYear)
        {
            txtSemester.Text = semester;
            txtCurrentSchoolYear.Text = academicYear;
            ConfigureReadOnly(txtSemester, txtCurrentSchoolYear);
        }

        public static void SetPeriodFields(
            LabelControl lblSemester, LabelControl lblAcademicYear,
            string semester, string academicYear)
        {
            lblSemester.Text = string.IsNullOrEmpty(semester) || semester == "Not Set" ? "Not Set" : semester;
            lblAcademicYear.Text = string.IsNullOrEmpty(academicYear) || academicYear == "Not Set" ? "Not Set" : academicYear;
        }

        public static void ConfigureReadOnly(params TextEdit[] boxes)
        {
            foreach (var box in boxes)
            {
                box.ReadOnly = true;
                box.Properties.Appearance.BackColor = Color.LightGray;
                box.Properties.Appearance.ForeColor = Color.DimGray;
            }
        }

        // ── Action Buttons ─────────────────────────────────────────────

        public static void SetActionButtonsAvailability(
            bool canAct, SimpleButton btnSubmitRequest,
            SimpleButton btnUploadSSG, SimpleButton btnUploadTreasurer)
        {
            btnSubmitRequest.Enabled = canAct;
            btnUploadSSG.Enabled = canAct;
            btnUploadTreasurer.Enabled = canAct;
        }

        // ── Status Labels ──────────────────────────────────────────────

        public static void UpdateClearanceStatus(
            LabelControl lblStatus, LabelControl lblProgress,
            int clearedCount, int totalOffices)
        {
            bool fullyCleared = clearedCount == totalOffices;
            lblStatus.Text = fullyCleared ? "Cleared" : "In Progress";
            lblStatus.ForeColor = fullyCleared ? Color.ForestGreen : SystemColors.ControlText;
            lblProgress.Text = fullyCleared
                ? "All 3 offices cleared! Your clearance is complete."
                : $"{clearedCount} out of {totalOffices} offices cleared";
        }

        public static void UpdateProgressIndicators(
            LabelControl lblOfficeCleared, LabelControl lblPercentage,
            ProgressBarControl pbOverallProgress, int clearedCount, int totalOffices)
        {
            int percentage = (clearedCount * 100) / totalOffices;
            lblOfficeCleared.Text = $"{clearedCount}/{totalOffices}";
            lblPercentage.Text = percentage + "%";
            pbOverallProgress.Position = percentage;
        }

        // ── Validation ─────────────────────────────────────────────────

        public static bool ValidateRequiredFiles(string ssgFilePath, string treasurerFilePath)
            => !string.IsNullOrEmpty(ssgFilePath) && !string.IsNullOrEmpty(treasurerFilePath);

        public static bool ValidateUserLoggedIn(User currentUser)
            => currentUser != null;

        public static bool ValidateFullyClearedStatus(int clearedCount, int totalOffices)
            => clearedCount == totalOffices;

        // ── Message Boxes ──────────────────────────────────────────────

        public static void ShowSuccess(string message, string title = "Success")
            => XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void ShowWarning(string message, string title = "Warning")
            => XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static void ShowError(string message, string title = "Error")
            => XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static DialogResult ShowConfirmation(string message, string title = "Confirm")
            => XtraMessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        // ── Password Toggle ────────────────────────────────────────────

        public static void ConfigurePasswordToggle(
            DevExpress.XtraEditors.CheckEdit chkShowPassword,
            TextEdit txtPassword, string initialCaption = "Show Password")
        {
            chkShowPassword.Properties.Caption = initialCaption;

            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.Properties.UseSystemPasswordChar = !chkShowPassword.Checked;
                chkShowPassword.Properties.Caption = chkShowPassword.Checked ? "Hide Password" : "Show Password";
                txtPassword.Focus();
                txtPassword.SelectionStart = txtPassword.Text.Length;
            };
        }

        // ── Name Field Restrictions ────────────────────────────────────

        public static void AttachNameRestrictions(params TextEdit[] fields)
        {
            foreach (var field in fields)
                field.KeyPress += RestrictToLettersOnly;
        }

        public static void RestrictToLettersOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
                e.Handled = true;
        }
    }
}