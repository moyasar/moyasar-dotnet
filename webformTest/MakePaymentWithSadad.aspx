<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MakePaymentWithSadad.aspx.cs" Inherits="MakePaymentWithSadad" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>

                <table align="left" class="auto-style1">
                    <tr>
                        <td class="auto-style2">Currency</td>
                        <td>
                            <asp:TextBox ID="txtCurrency" runat="server">SAR</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style2">Amount</td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server">100</asp:TextBox>
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
                            <table class="auto-style3">
                                <tr>
                                    <td class="auto-style4">User Name</td>
                                    <td>
                                        <asp:TextBox ID="txtUserName" runat="server" Width="193px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style4">SuccessUrl</td>
                                    <td>
                                        <asp:TextBox ID="txtSuccessUrl" runat="server" Width="193px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style5">FaildUrl</td>
                                    <td class="auto-style6">
                                        <asp:TextBox ID="txtFaildUrl" runat="server" Width="193px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style2">&nbsp;</td>
                                    <td>
                                        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="ok" Width="53px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
    <div style="margin: 0 auto; text-align: center;">
        <p>Payment  Result</p>
        <asp:Label ID="txtResult" runat="server" Text=""></asp:Label>
    </div>

</body>
</html>
