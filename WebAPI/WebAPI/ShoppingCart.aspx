<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShoppingCart.aspx.cs" Inherits="WebAPI.ShoppingCart" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="ShoppingCartTitle" runat="server" class="ContentHead">
        <h1>Shopping Cart</h1>
    </div>
    <asp:GridView ID="CartList" runat="server" AutoGenerateColumns="False" ShowFooter="True" GridLines="Vertical" CellPadding="4"
        ItemType="WebAPI.Models.CartItem" SelectMethod="GetShoppingCartItems">
        <Columns>
            <asp:BoundField DataField="Product.Id" HeaderText="ID" />
            <asp:TemplateField>
                <ItemTemplate>
                    <a href="ProductDetails.aspx?ProductID=<%#:Item.ProductId%>">
                        <img src="Image/<%#:Item.Product.ImagePath%>" width="50" height="40" /></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Product.Name" HeaderText="Name" />
            <asp:BoundField DataField="Product.Price" HeaderText="Price (each)" DataFormatString="{0:c}" />
            <asp:TemplateField HeaderText="Quantity">
                <ItemTemplate>
                    <asp:TextBox ID="PurchaseQuantity" Width="40" runat="server" Text="<%#: Item.Quantity %>"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Item Total">
                <ItemTemplate>
                    <%#: String.Format("{0:c}", ((Convert.ToDouble(Item.Quantity)) *  Convert.ToDouble(Item.Product.Price)))%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Remove Item">
                <ItemTemplate>
                    <asp:CheckBox ID="Remove" runat="server"></asp:CheckBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div>
        <p></p>
        <strong>
            <asp:Label ID="LabelTotalText" runat="server" Text="Order Total: "></asp:Label>
            <asp:Label ID="lblTotal" runat="server" EnableViewState="false"></asp:Label>
        </strong>
    </div>
    <br />
    <table>
        <tr>
            <td>
                <asp:Button ID="UpdateBtn" runat="server" Text="Update" OnClick="UpdateBtn_Click" />
            </td>
            <td>
                <asp:Button ID="DeleteAllBtn" runat="server" Text="Delete All" OnClick="DeleteAllBtn_Click" />
            </td>
        </tr>
    </table>
</asp:Content>