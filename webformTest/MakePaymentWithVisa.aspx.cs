using System;
using System.Web.UI;
using Moyasar;
using Moyasar.Payments;

public partial class MakePayment : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MoyasarBase.ApiKey = "pk_test_yTqHr4bcm1eCJkGdpNExsU4f6s1FZkqpHzr7XezG";
    }

    protected void BtnOk_Click(object sender, EventArgs e)
    {
       Payment payment = new Payment();
        payment.SourceType = SourceType.CreditCard;
        payment.Amount = int.Parse(txtAmount.Text);
        payment.Currency = txtCurrency.Text;
        payment.Description = txtDescription.Text;
        payment.SourceResult = new CreditCard()
        {
            Type = "creditcard",
            Message = "",
            Company = txtCreditCardType.SelectedValue.ToLower(),
            Number = txtNumber.Text,
            Name = txtName.Text,
            Year = int.Parse(txtYear.Text),
            Month = int.Parse(txtMonth.Text),
            Cvc = txtCvc.Text,
        };

        try
        {
            var p = payment.CreatePay();
            txtResult.Text = p.Id + ", " + p.Amount + ", " + p.Description;
        } catch (MoyasarException ex)
        {   
            txtResult.Text = "Error <br> " + ex.Type + ", " + ex.Message + ", " + ex.Errors;
        }

    }
}