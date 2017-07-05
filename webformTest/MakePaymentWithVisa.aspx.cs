using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moyasar;
using Moyasar.PaymentArea;

public partial class MakePayment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
       Payment payment = new Payment();
        payment.SourceType=SourceType.CreditCard;
        payment.Amount = int.Parse(txtAmount.Text);
        payment.Currency = txtCurrency.Text;
        payment.Description = txtDescription.Text;
        payment.SourceReault = new CreditCard()
        {
            Type = "CreditCard",
            Message = "",
            Company = txtCreditCardType.Text,
            Number = txtNumber.Text,
            Name = txtName.Text,
            Year = int.Parse(txtYear.Text),
            Month = int.Parse(txtMonth.Text),
            Cvc = txtCvc.Text,
           
        };
        var p = payment.CreatePay();
        txtResult.Text = p.id + "," + p.amount + "," + p.description;

    }
}