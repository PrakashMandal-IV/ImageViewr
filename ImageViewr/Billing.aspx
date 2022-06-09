﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Billing.aspx.cs" Inherits="ImageViewr.Billing" %>

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
                <h1 style="text-align: center">Billing Counter</h1>
                <div class="selection">
                    <label class="lbl" for="UserDropDown" runat="server">User:</label>
                    <asp:DropDownList ID="UserDropDown" class="Input" runat="server"></asp:DropDownList>
                    <asp:Button ID="GOBTN" runat="server" class="btn" OnClick="UserDropDown_SelectedIndexChanged" Text="Go" Style="margin-right: 100pt" />
                    <asp:Label ID="userMsg" runat="server" ForeColor="Red" Style="font-size: 12pt; font-weight: normal"></asp:Label>
                    <label class="lbl dateselector" for="DateSelector" runat="server">Date:</label>
                    <asp:Label ID="BillingDate" runat="server"></asp:Label>
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
                        <HeaderStyle BackColor="black" ForeColor="White" Font-Names="Roboto" Height="20pt" />
                        <RowStyle />
                    </asp:GridView>
                </div>
                <div class="Total">
                    <div class="amountInfo">
                        <asp:Label ID="Amount" class="infolbl" runat="server"></asp:Label>
                        <asp:Label ID="Gst" class="infolblt" runat="server"></asp:Label>
                        <asp:Label ID="TotalAmount" class="infolbl total" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="save" style="display: flex; margin-top: auto; margin-right: 50pt">
                    <asp:Button ID="SaveBtn" class="btn" runat="server" Text="Save & Exit" OnClick="SaveBtn_Click" />
                </div>
            </div>
            <div class="second_section">
                <div class="SearchField">
                    <div class="fields">
                        <label class="lbl" for="UserDropDown" runat="server">User:</label>
                        <asp:DropDownList ID="SecondUserDropdownList" class="Input" runat="server"></asp:DropDownList>
                        <asp:Button ID="Search" runat="server" OnClick="Search_Click1" class="btn" Text="Go" Style="margin-right: 100pt" />
                    </div>
                    <div class="fields">
                        <label class="lbl" for="DateSelector" runat="server">User:</label>
                        <asp:TextBox ID="DateFilter" class="Input" TextMode="Date" runat="server"></asp:TextBox>
                        <asp:Button ID="Clear" runat="server" class="btn" Text="Clear Filter" Style="margin-right: 100pt" />
                    </div>
                </div>
                <div class="Listings">
                    <div class="TransectionList">
                         <asp:GridView ID="TransectionList" CssClass="Grid table table-condensed table-responsive table-hover" runat="server" Height="16px" Width="486px" Style="margin-right: 0px; margin-top: 29px;" GridLines="none" AllowSorting="True">
                        <HeaderStyle BackColor="black" ForeColor="White" Font-Names="Roboto" Height="20pt" />
                        <RowStyle />
                              <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="ViewBtn" class="btn" runat="server" Style="border: none;transition:all ease-in 8ms; border-radius: 5px; background-image: linear-gradient(to bottom, rgb(133, 133, 190), rgb(96, 96, 255)); color: white; width: 100pt" BorderStyle="None" Text="View" Height="20px" Value='<%#Eval("Id") %>' CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>