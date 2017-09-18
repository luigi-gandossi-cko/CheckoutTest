<%@ Page Title="Product List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="WebAPI.ProductList" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">   
    <div id="CategoryMenu" style="text-align: center">       
        <asp:ListView ID="categoryList"  
            ItemType="WebAPI.Models.Category" 
            runat="server"
            SelectMethod="GetCategories" >
            <ItemTemplate>
                <b style="font-size: large; font-style: normal">
                    <a href="/ProductList.aspx?CategoryID=<%#:Item.Id%>">
                    <%#: Item.Name %>
                    </a>
                </b>
            </ItemTemplate>
            <ItemSeparatorTemplate>  |  </ItemSeparatorTemplate>
        </asp:ListView>
    </div>
    <hgroup>
        <h2><%: Page.Title %></h2>
    </hgroup>
    <asp:ListView ID="productList" runat="server" 
        DataKeyNames="Id" GroupItemCount="2" 
        ItemType="WebAPI.Models.Product" SelectMethod="GetProducts">
        <EmptyDataTemplate>
            <table >
                <tr>
                    <td>No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <EmptyItemTemplate>
            <td/>
        </EmptyItemTemplate>
        <GroupTemplate>
            <tr id="itemPlaceholderContainer" runat="server">
                <td id="itemPlaceholder" runat="server"></td>
            </tr>
        </GroupTemplate>
        <ItemTemplate>
            <td runat="server">
                <table>
                    <tr>
                        <td> 
                            <a href="ProductDetails.aspx?ProductID=<%#:Item.Id%>">
                            <img src="Image/<%#:Item.ImagePath%>"
                                width="100" height="75" style="border: solid" /></a>
                            </a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a href="ProductDetails.aspx?ProductID=<%#:Item.Id%>">
                            <span>
                                <%#:Item.Name%>
                            </span>
                            </a>
                            <br />
                            <span>
                                <b>Price: </b><%#:String.Format("{0:c}", Item.Price)%>
                            </span>
                            <br />
                            <a href="/AddItem.aspx?ProductID=<%#:Item.Id %>">               
                                <span class="ProductListItem">
                                    <b>Add To Cart<b>
                                </span>           
                            </a>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                </p>
            </td>
        </ItemTemplate>
         <LayoutTemplate>
                    <table style="width:100%;">
                        <tbody>
                            <tr>
                                <td>
                                    <table id="groupPlaceholderContainer" runat="server" style="width:100%">
                                        <tr id="groupPlaceholder"></tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>
    </asp:ListView>
</asp:Content>

