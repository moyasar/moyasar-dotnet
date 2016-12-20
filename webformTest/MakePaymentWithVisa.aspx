<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MakePaymentWithVisa.aspx.cs" Inherits="MakePayment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            float: right;
        }
        .auto-style2 {
            width: 113px;
        }
        .auto-style3 {
            width: 100%;
        }
        .auto-style4 {
            width: 126px;
        }
        .auto-style5 {
            width: 126px;
            height: 26px;
        }
        .auto-style6 {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table align="left" class="auto-style1">
            <tr>
                <td class="auto-style2">Currency</td>
                <td>
                    <asp:TextBox ID="txtCurrency" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Amount</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">Description</td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td>
                    <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="ok" />
                </td>
            </tr>
            <tr>
                <td class="auto-style2">&nbsp;</td>
                <td>
                    <table class="auto-style3">
                        <tr>
                            <td class="auto-style4">CreditCard</td>
                            <td>
                                <asp:DropDownList ID="txtCreditCardType" runat="server" Width="203px">
                                    <asp:ListItem>Visa</asp:ListItem>
                                    <asp:ListItem>MasterCard</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">Number</td>
                            <td>
                                <asp:TextBox ID="txtNumber" runat="server" Width="193px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">Name</td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" Width="193px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style5">Year</td>
                            <td class="auto-style6">
                                <asp:TextBox ID="txtYear" runat="server" Width="193px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">Month</td>
                            <td>
                                <asp:TextBox ID="txtMonth" runat="server" Width="193px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">CVC</td>
                            <td>
                                <asp:TextBox ID="txtCvc" runat="server" Width="193px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:Label ID="txtResult" runat="server" Text="Label"></asp:Label>
    
    </div>
    </form>
</body>
</html>
