<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Billing.aspx.cs" Inherits="ImageViewr.Billing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Records</title>
    <link rel="stylesheet" href="Css/Transaction.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="first_section">
                <div class="selection">
                    <label class="lbl" for="UserDropDown" runat="server">User:</label>
                    <asp:DropDownList ID="UserDropDown" class="Input" runat="server"></asp:DropDownList>
                    <asp:Label ID="userMsg" runat="server" ForeColor="Red" style="font-size:12pt;font-weight:normal"></asp:Label>
                    <label class="lbl dateselector" for="DateSelector" runat="server">Date:</label>
                     <asp:label ID="BillingDate" runat="server"></asp:label>
                </div>
                <div class="ProductAdd">
                    <label class="lbl" for="ProductDropdown" runat="server">Product:</label>
                    <asp:DropDownList ID="ProductDropdown" class="Input" runat="server"></asp:DropDownList>
                    <label class="lbl quantity" for="quantity" runat="server">Quantity:</label>
                    <asp:TextBox ID="Quantity" type="number" class="Input" runat="server" TextMode="Number" Text="1"></asp:TextBox>
                    <asp:Button ID="AddProduct" runat="server" class="btn" Text="Add" OnClick="AddProduct_Click" />
                </div>
                <div class="datagrid">
                    <asp:GridView ID="UserProductList" CssClass="Grid table table-condensed table-responsive table-hover" runat="server" Height="16px" Width="486px" Style="margin-right: 0px; margin-top: 29px;" GridLines="none" AllowSorting="True">
                        <HeaderStyle BackColor="black" ForeColor="White" Font-Names="Roboto" Height="20pt"  />
                        <RowStyle />                   
                    </asp:GridView>
                </div>
                <div class="Total">
                    <div class="amountInfo">
                        <asp:label ID="Amount" class="infolbl" runat="server" ></asp:label>
                        <asp:label ID="Gst" class="infolblt" runat="server" ></asp:label>
                        <asp:label ID="TotalAmount" class="infolbl total" runat="server"></asp:label>
                    </div>
                </div>
                <div class="save" style="display:flex;margin-top:auto;margin-right:50pt">
                    <asp:Button ID="SaveBtn" class="btn" runat="server" Text="Save & Exit" OnClick="SaveBtn_Click"/>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
