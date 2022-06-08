<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ImageViewr.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Image Manager</title>
    <link rel="stylesheet" href="Css/StyleSheet1.css" />
</head>
<body>
    <form runat="server">
        <div class="container">
        <div class="first_part">
            <div class="Datatable">
            <asp:GridView ID="ImageData"  CssClass="Grid table table-condensed table-responsive table-hover" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" runat="server" Height="16px" Width="486px" Style="margin-right: 0px; margin-top: 29px;" GridLines="none" AllowSorting="True">
                <HeaderStyle BackColor="black" ForeColor="White" Font-Names="Roboto" Height="30pt" />
                <RowStyle />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="ViewBtn" CssClass="Button" runat="server" Style="border: none; border-radius: 5px; background-image: linear-gradient(to bottom, rgb(133, 133, 190), rgb(96, 96, 255)); color: white; width: 100pt" BorderStyle="None" Height="20px" Text="View" Value='<%#Eval("Id") %>' CommandName="Select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                </div>
                </div>
                        <h1>Image Section</h1>
            <div class="ImageSelection">
                <p>Choose Image</p>
                <asp:FileUpload ID="ImageSelector" Class="ImgSelector" runat="server" />
            </div>
            <asp:Label ID="Info" runat="server"></asp:Label>
            <asp:Label ID="msg" runat="server"></asp:Label>
            <asp:Button ID="Button1" runat="server" Text="" />
            <asp:Label ID="successMsg" runat="server" Text=""></asp:Label>
        
            <div style="margin: 20pt">
                <asp:Image ID="ImageOrg" runat="server" />
                <asp:Image ID="Thumbnail" runat="server" />
            </div>
        </div>
    </form>
</body>
</html>
