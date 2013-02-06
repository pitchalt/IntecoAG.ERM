using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using DevExpress.ExpressApp.Demos;
using System.Drawing;

namespace IntecoAG.ERM {
    public class ProgressForm : IProgressControl {
        public const string FormName = "ProgressForm";
        private XtraForm form;
        private ProgressBarControl progressBar = new ProgressBarControl();
        private SimpleButton cancelButton = new SimpleButton();
        private Label label = new Label();
        private LongOperation longOperation;
        private int minimumProgressValue;
        private int maximumProgressValue;
        private string progressFormCaption;
        private delegate void UpdateProgressFormDelegate(int value, string message);

        private void CreateProgressForm() {
            form = new XtraForm();
            form.Name = FormName;
            form.Width = 350;
            form.Height = 125;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.ControlBox = false;
            form.ShowInTaskbar = false;

            label.Parent = form;
            label.Location = new Point(10, 10);
            label.Size = new Size(form.ClientSize.Width - 20, 13);

            progressBar.Parent = form;
            progressBar.Location = new Point(10, 30);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(form.ClientSize.Width - 20, 15);
            progressBar.Properties.Minimum = minimumProgressValue;
            progressBar.Properties.Maximum = maximumProgressValue;
            progressBar.Properties.Step = 1;

            cancelButton.Parent = form;
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Size = new Size(75, 23);
            cancelButton.Location = new Point((form.Width - cancelButton.Width) / 2, 55);
            cancelButton.Text = "&Cancel";
            cancelButton.Click += new EventHandler(cancelButton_Click);
            cancelButton.LostFocus += new EventHandler(DoOnFormLostFocus);
            form.CancelButton = cancelButton;
            form.Text = progressFormCaption;
        }
        private void DoOnFormLostFocus(object sender, EventArgs e) {
            form.Focus();
        }
        private void LongOperation_Completed(object sender, LongOperationCompletedEventArgs e) {
            if(form != null) {
                form.Invoke(new MethodInvoker(form.Close));
            }
        }
        private void UpdateProgressForm(int value, string message) {
            progressBar.EditValue = value;
            progressBar.Update();
            label.Text = message;
        }
        private void LongOperation_ProgressChanged(object sender, LongOperationProgressChangedEventArgs e) {
            int value = e.ProgressPercentage;
            if(form != null) {
                form.Invoke(new UpdateProgressFormDelegate(UpdateProgressForm), new object[] { value, e.Message });
            }
        }
        private void cancelButton_Click(object sender, EventArgs e) {
            longOperation.CancelAsync();
        }
        protected ProgressForm(int minimum, int maximum)
            : this("", minimum, maximum) {
        }
        public ProgressForm(string caption, int minimum, int maximum) {
            this.progressFormCaption = caption;
            this.minimumProgressValue = minimum;
            this.maximumProgressValue = maximum;
            CreateProgressForm();
        }
        public ProgressForm()
            : this(0, 100) {
        }
        public void Dispose() {
            label = null;
            cancelButton.LostFocus -= new EventHandler(DoOnFormLostFocus);
            longOperation.ProgressChanged -= new EventHandler<LongOperationProgressChangedEventArgs>(LongOperation_ProgressChanged);
            longOperation.Completed -= new EventHandler<LongOperationCompletedEventArgs>(LongOperation_Completed);
            longOperation = null;
            if(form != null) {
                form.Invoke(new MethodInvoker(form.Dispose));
                form = null;
            }
        }
        public void ShowProgress(LongOperation longOperation) {
            form.Show();
            this.longOperation = longOperation;
            this.longOperation.ProgressChanged += new EventHandler<LongOperationProgressChangedEventArgs>(LongOperation_ProgressChanged);
            this.longOperation.Completed += new EventHandler<LongOperationCompletedEventArgs>(LongOperation_Completed);
        }
    }
}
