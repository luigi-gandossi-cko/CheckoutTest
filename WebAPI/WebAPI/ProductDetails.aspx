<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="WebAPI.ProductDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="productDetails" runat="server" ItemType="WebAPI.Models.Product" SelectMethod ="GetProduct" RenderOuterTable="false">
        <ItemTemplate>
            <div>
                <h1><%#:Item.Name %></h1>
            </div>
            <br />
            <table>
                <tr>
                    <td>
                        <img src="Image/<%#:Item.ImagePath %>" style="border:solid; height:300px" alt="<%#:Item.Name %>"/>
                    </td>
                    <td>&nbsp;</td>  
                    <td style="vertical-align: top; text-align:left;">
                        <b>Description:</b><br /><%#:Item.Description %>
                        <br />
                        <span><b>Price:</b>&nbsp;<%#: String.Format("{0:c}", Item.Price) %></span>
                        <br />
                        <a href="/AddItem.aspx?ProductID=<%#:Item.Id %>">               
                            <span class="ProductListItem">
                                <b>Add To Cart<b>
                            </span>           
                        </a>
                    </td>
                </tr>
            </table>
        </ItemTemplate>
    </asp:FormView>
</asp:Content>