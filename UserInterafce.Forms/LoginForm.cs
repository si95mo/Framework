using Core.DataStructures;
using Security;
using System.Drawing;
using System.Windows.Forms;
using UserInterface.Controls;

namespace UserInterface.Forms
{
    public partial class LoginForm : CustomSubForm
    {
        SecuritiesService service;

        public LoginForm() : base()
        {
            InitializeComponent();

            ShowInTaskbar = false;
            TopMost = true;
            StartPosition = FormStartPosition.CenterScreen;

            if (ServiceBroker.CanProvide<SecuritiesService>())
            {
                service = ServiceBroker.GetService<SecuritiesService>();
            }
            else
            {
                service = new SecuritiesService();
                ServiceBroker.Provide(service);
            }
        }

        private void BtnOk_Click(object sender, System.EventArgs e)
        {
            if (service.Match(txbUsername.Text, txbPassword.Text))
            {
                bool logged = service.LogIn(txbUsername.Text, txbPassword.Text);

                if (logged)
                {
                    DialogResult = DialogResult.OK;

                    Close();
                    Dispose();
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
            else if (service.Get(txbUsername.Text) == default) // Wrong user name
            {
                pnlUsername.BackColor = Colors.Red;
                pnlPassword.BackColor = Color.Black;
            }
            else if (!service.Get(txbUsername.Text).IsMatch(txbPassword.Text)) // Wrong password
            {
                pnlUsername.BackColor = Color.Black;
                pnlPassword.BackColor = Colors.Red;
            }
        }

        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
            Dispose();
        }
    }
}
