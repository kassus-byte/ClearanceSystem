using System;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraEditors;
using SchoolClearanceSystem.Models;

namespace SchoolClearanceSystem.Helpers
{
    /// <summary>
    /// UIHelper provides reusable methods for form validation, UI population, and styling.
    /// Centralizes repetitive UI operations to reduce code duplication across forms.
    /// Works with DevExpress controls (LabelControl, TextEdit, SimpleButton, etc.)
    /// </summary>
    public static class UIHelper
    {
        // ── Clearance Slip Population ──────────────────────────────────

        /// <summary>
        /// Populates clearance slip labels with student and period information.
        /// </summary>
        public static void PopulateClearanceSlip(
            LabelControl lblSemYear,
            LabelControl lblNameID,
            LabelControl lblProgramDepartment,
            LabelControl lblDateIssued,
            User currentUser,
            string semester,
            string academicYear)
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

        /// <summary>
        /// Clears all clearance slip labels.
        /// </summary>
        public static void ClearClearanceSlip(
            LabelControl lblSemYear,
            LabelControl lblNameID,
            LabelControl lblProgramDepartment,
            LabelControl lblDateIssued)
        {
            lblSemYear.Text = string.Empty;
            lblNameID.Text = string.Empty;
            lblProgramDepartment.Text = string.Empty;
            lblDateIssued.Text = string.Empty;
        }

        // ── User Session Labels ────────────────────────────────────────

        /// <summary>
        /// Populates sidebar user info labels from the current session.
        /// All parameters should be LabelControl (not TextEdit).
        /// </summary>
        public static void PopulateUserSessionContext(
            LabelControl lblWelcome,
            LabelControl lblFullName,
            LabelControl lblUserID,
            LabelControl lblProgram,
            User currentUser)
        {
            if (currentUser == null) return;

            lblWelcome.Text = $"Welcome, {currentUser.FullName}!";
            lblFullName.Text = currentUser.FullName;
            lblUserID.Text = currentUser.UserID ?? "0000";
            lblProgram.Text = currentUser.Program ?? "N/A";
        }

        // ── Period/Semester Validation & Population ────────────────────

        /// <summary>
        /// Populates and locks period fields to indicate they are read-only.
        /// </summary>
        public static void SetPeriodFields(
            TextEdit txtSemester,
            TextEdit txtCurrentSchoolYear,
            string semester,
            string academicYear)
        {
            txtSemester.Text = semester;
            txtCurrentSchoolYear.Text = academicYear;
            ConfigureReadOnly(txtSemester, txtCurrentSchoolYear);
        }

        /// <summary>
        /// Makes text boxes read-only with gray appearance.
        /// </summary>
        public static void ConfigureReadOnly(params TextEdit[] boxes)
        {
            foreach (var box in boxes)
            {
                box.ReadOnly = true;
                box.Properties.Appearance.BackColor = Color.LightGray;
                box.Properties.Appearance.ForeColor = Color.DimGray;
            }
        }

        // ── Button Styling ─────────────────────────────────────────────

        /// <summary>
        /// Disables and grays out a button with a completion message.
        /// </summary>
        public static void DisableButtonAsCompleted(
            SimpleButton button,
            string completionText = "Clearance Fully Approved")
        {
            button.Text = completionText;
            button.Enabled = false;
            button.Appearance.BackColor = Color.LightGray;
            button.Appearance.ForeColor = Color.DimGray;
        }

        /// <summary>
        /// Resets a button to its default enabled state.
        /// </summary>
        public static void ResetButton(SimpleButton button, string defaultText = "Submit Request")
        {
            button.Text = defaultText;
            button.Enabled = true;
            button.Appearance.Reset();  // fully restores whatever the Designer set
        }

        // ── Status Label Updates ───────────────────────────────────────

        /// <summary>
        /// Updates dashboard status labels based on clearance progress.
        /// </summary>
        public static void UpdateClearanceStatus(
            LabelControl lblStatus,
            LabelControl lblProgress,
            int clearedCount,
            int totalOffices)
        {
            bool fullyCleared = clearedCount == totalOffices;

            lblStatus.Text = fullyCleared ? "Cleared" : "In Progress";
            lblStatus.ForeColor = fullyCleared ? Color.ForestGreen : SystemColors.ControlText;
            lblProgress.Text = fullyCleared
                ? "All 3 offices cleared! Your clearance is complete."
                : $"{clearedCount} out of {totalOffices} offices cleared";
        }

        /// <summary>
        /// Updates dashboard progress indicators.
        /// </summary>
        public static void UpdateProgressIndicators(
            LabelControl lblOfficeCleared,
            LabelControl lblPercentage,
            ProgressBarControl pbOverallProgress,
            int clearedCount,
            int totalOffices)
        {
            int percentage = (clearedCount * 100) / totalOffices;

            lblOfficeCleared.Text = $"{clearedCount}/{totalOffices}";
            lblPercentage.Text = percentage + "%";
            pbOverallProgress.Position = percentage;
        }

        // ── Action Button Availability ─────────────────────────────────

        /// <summary>
        /// Sets action button availability based on clearance status.
        /// </summary>
        public static void SetActionButtonsAvailability(
            bool canAct,
            SimpleButton btnSubmitRequest,
            SimpleButton btnUploadSSG,
            SimpleButton btnUploadTreasurer)
        {
            btnSubmitRequest.Enabled = canAct;
            btnUploadSSG.Enabled = canAct;
            btnUploadTreasurer.Enabled = canAct;
        }

        // ── Validation Helpers ─────────────────────────────────────────

        /// <summary>
        /// Validates that both required file paths are populated.
        /// </summary>
        public static bool ValidateRequiredFiles(string ssgFilePath, string treasurerFilePath)
        {
            return !string.IsNullOrEmpty(ssgFilePath) && !string.IsNullOrEmpty(treasurerFilePath);
        }

   

        // ── User Validation ────────────────────────────────────────────

        /// <summary>
        /// Validates that a user is logged in.
        /// </summary>
        public static bool ValidateUserLoggedIn(User currentUser)
        {
            return currentUser != null;
        }

        /// <summary>
        /// Validates that student is fully cleared.
        /// </summary>
        public static bool ValidateFullyClearedStatus(int clearedCount, int totalOffices)
        {
            return clearedCount == totalOffices;
        }

        // ── Message Boxes ──────────────────────────────────────────────

        /// <summary>
        /// Shows a success message box.
        /// </summary>
        public static void ShowSuccess(string message, string title = "Success")
        {
            XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Shows a warning message box.
        /// </summary>
        public static void ShowWarning(string message, string title = "Warning")
        {
            XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows an error message box.
        /// </summary>
        public static void ShowError(string message, string title = "Error")
        {
            XtraMessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a confirmation dialog.
        /// </summary>
        public static DialogResult ShowConfirmation(string message, string title = "Confirm")
        {
            return XtraMessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        // ── Password Toggle ────────────────────────────────────────────

        /// <summary>
        /// Wires a show/hide password toggle checkbox to a password TextEdit.
        /// Handles label caption swap ("Show Password" / "Hide Password") and cursor repositioning.
        /// </summary>
        public static void ConfigurePasswordToggle(
            DevExpress.XtraEditors.CheckEdit chkShowPassword,
            TextEdit txtPassword,
            string initialCaption = "Show Password")
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

        /// <summary>
        /// Restricts one or more TextEdit fields to letters, spaces, hyphens, and apostrophes only.
        /// Attach this to name fields (Last Name, First Name, Middle Name) to prevent numeric/symbol input.
        /// </summary>
        public static void AttachNameRestrictions(params TextEdit[] fields)
        {
            foreach (var field in fields)
                field.KeyPress += RestrictToLettersOnly;
        }

        /// <summary>
        /// KeyPress handler that blocks any character that is not a letter, space, hyphen, or apostrophe.
        /// </summary>
        public static void RestrictToLettersOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) &&
                e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
                e.Handled = true;
        }
    }
}