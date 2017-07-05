using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moyasar;
using Moyasar.PaymentArea;

public partial class MakePaymentWithSadad : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        Payment payment = new Payment();
        payment.SourceType = SourceType.Sadad;
        payment.Amount = int.Parse(txtAmount.Text);
        payment.Currency = txtCurrency.Text;
        payment.Description = txtDescription.Text;
        payment.SourceReault = new SadadType()
        {
            Type = "sadad",
            Message = "",
           SuccessUrl = txtSuccessUrl.Text,
           Username = txtUserName.Text,
           FaildUrl = txtFaildUrl.Text                             
        };
        var p = payment.CreatePay();
        txtResult.Text = p.id + "," + p.amount + "," + p.description;
    }
}