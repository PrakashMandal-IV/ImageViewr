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
                <div class="info">
                    <asp:FileUpload ID="ImageSelector" class="ImgSelector" runat="server"  />
                    <asp:Button ID="Button1" class="uploadbtn" runat="server" OnClick="Upload_Click" Text="Upload" />
                </div>
                <asp:Label ID="MSG" runat="server" style="text-align:center"></asp:Label>
                <div class="Datatable">
                    <asp:GridView ID="ImageData" CssClass="Grid table table-condensed table-responsive table-hover" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" runat="server" Height="16px" Width="486px" Style="margin-right: 0px; margin-top: 29px;" GridLines="none" AllowSorting="True">
                        <HeaderStyle BackColor="#6060FF" ForeColor="White" Font-Names="Roboto" Height="30pt" />
                        <RowStyle />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="ViewBtn" class="Button" runat="server" Style="border: none;transition:all ease-in 8ms; border-radius: 5px; background-image: linear-gradient(to bottom, rgb(133, 133, 190), rgb(96, 96, 255)); color: white; width: 100pt" BorderStyle="None" Text="View" Height="20px" Value='<%#Eval("Id") %>' CommandName="Select" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            
            <div class="second_part">
                <asp:Image ID="ImageOrg" class="imgvw"  runat="server" />
            </div>
            <div class="third_part">           
                <asp:Image ID="Thumbnail" runat="server" class="prevw" />                
                <div class="imginfo">
                    <div class="detail">
                        <asp:label ID="titlelbl" class="header"  runat="server" ></asp:label>
                         <asp:label ID="Title" class="ans" runat="server"></asp:label>                 
                    </div>
                    <div class="detail">
                        <asp:label ID="uploadDatelbl" class="header" runat="server"></asp:label>
                         <asp:label ID="UploadDate" class="ans" runat="server"></asp:label>                 
                    </div>
                    <div class="detail">
                        <asp:label ID="widthlbl" class="header" runat="server" ></asp:label>
                         <asp:label ID="width" class="ans" runat="server"></asp:label>                 
                    </div>
                    <div class="detail">
                        <asp:label ID="heightlbl" class="header" runat="server" ></asp:label>
                         <asp:label ID="height" class="ans" runat="server"></asp:label>                 
                    </div>
                    <div class="detail">
                        <asp:label ID="sizelbl" class="header" runat="server"></asp:label>
                         <asp:label ID="size" class="ans" runat="server"></asp:label>         
                    </div>                  
                </div>
            </div>

        </div>
    </form>
</body>
</html>
