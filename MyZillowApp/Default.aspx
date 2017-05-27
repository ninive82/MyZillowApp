<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MyZillowApp._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        table tr th {
            font-weight: normal;
        }

        h4 {
            margin-top: 25px;
        }

        .linkslist {
            margin-left: 5px;
        }

            .linkslist li {
                display: inline;
                margin-right: 15px;
            }

        .entryForm {
            font-size: 15px;
            margin: 10px;
            margin-top: 50px;
        }
    </style>


    <div class="jumbotron">
        <h1>My Zillow Exercise</h1>
        <p class="lead">Enter a US address to get property information</p>
        <div class="entryForm">

            <p>
                <span style="width: 100px; float: left">Address:</span>
                <asp:TextBox ID="AddressInput" runat="server" Width="350px"></asp:TextBox><br />
            </p>
            <p>
                <span style="width: 100px; float: left">City: </span>
                <asp:TextBox ID="CityInput" runat="server"></asp:TextBox>
            </p>
            <p>
                <span style="width: 100px; float: left">State:</span>
                <asp:TextBox ID="StateInput" runat="server" Width="50px"></asp:TextBox>
            </p>
            <p>
                <span style="width: 100px; float: left">ZIP:</span>
                <asp:TextBox ID="ZIPInput" runat="server"></asp:TextBox><br />
            </p>
            <p style="font-weight: normal">

                <asp:CheckBox ID="ShowRentZestimate" runat="server" />
                <asp:Label ID="LabelShowRentZestimate" AssociatedControlID="ShowRentZestimate" runat="server" Text="Show Rent Zestimate"></asp:Label>
            </p>
            <p style="margin-top: 30px">

                <asp:Button class="btn btn-default" ID="SearchButton" runat="server" Text="Search  &raquo;" OnClick="SearchButton_Click" />
                <%--<asp:Button lass="btn btn-primary btn-lg" ID="SearchButton" runat="server" Text="Search  &raquo;" OnClick="SearchButton_Click" />--%>
            </p>
        </div>
        <%--<p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>--%>
    </div>

    <div class="row">
        <div class="error">
            <asp:Label ID="LabelError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
        <div id="ResultsSection" runat="server" visible="false">
            <h4>Property ID:<asp:Label ID="LabelPropertyID" runat="server" Text=""></asp:Label></h4>
            <ul class="linkslist">
                <li>
                    <asp:HyperLink ID="LinkHomeDetails" runat="server" Target="_blank">Home Details </asp:HyperLink></li>
                <li>|</li>
                <li>
                    <asp:HyperLink ID="LinkChartData" runat="server" Target="_blank">Data Chart </asp:HyperLink></li>
                <li>|</li>
                <li>
                    <asp:HyperLink ID="LinkMap" runat="server" Target="_blank">Home Map </asp:HyperLink></li>
                <li>|</li>
                <li>
                    <asp:HyperLink ID="LinkSimilarSales" runat="server" Target="_blank">Similar Sales</asp:HyperLink></li>
            </ul>

            <div class="col-md-4">
                <h4>Full Address</h4>
                <table>


                    <tr>
                        <th>Street: </th>
                        <td>
                            <asp:Label ID="LabelStreet" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>City: </th>
                        <td>
                            <asp:Label ID="LabelCity" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>State: </th>
                        <td>
                            <asp:Label ID="LabelState" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Zip: </th>
                        <td>
                            <asp:Label ID="LabelZip" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Latitude: </th>
                        <td>
                            <asp:Label ID="LabelLatitude" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Longitude: </th>
                        <td>
                            <asp:Label ID="LabelLongitude" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-4">
                <h4>Zestimate Data</h4>
                <table>
                    <tr>
                        <th>Zestimate (in $):</th>
                        <td>
                            <asp:Label ID="LabelZestimate" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Last updated:</th>
                        <td>
                            <asp:Label ID="LabelLastUpdated" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>30-day change (in $):</th>
                        <td>
                            <asp:Label ID="Label30DayChange" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Valuation range (high) (in $):</th>
                        <td>
                            <asp:Label ID="LabelValuationRangeHigh" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Valuation range (low) (in $):</th>
                        <td>
                            <asp:Label ID="LabelValuationRangeLow" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Percentile Value:</th>
                        <td>
                            <asp:Label ID="LabelPercentileValue" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-4" runat="server" id="RentSection" visible="false">

                <h4>Rent Zestimate Data</h4>
                <table>
                    <tr>
                        <th>Rent Zestimate (in $):</th>
                        <td>
                            <asp:Label ID="LabelRentZestimate" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Last updated:</th>
                        <td>
                            <asp:Label ID="LabelRentLastUpdated" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>30-day change (in $):</th>
                        <td>
                            <asp:Label ID="LabelRent30DayChange" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Valuation range (high) (in $):</th>
                        <td>
                            <asp:Label ID="LabelRentValuationRangeHigh" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Valuation range (low) (in $):</th>
                        <td>
                            <asp:Label ID="LabelRentValuationRangeLow" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-4">
                <h4>Local Real Estate</h4>
                <p>
                    Zillow Home Value Index:<asp:Label ID="LabelHomeValueIndex" runat="server" Text=""></asp:Label>
                </p>
               
                <p>
                    <asp:HyperLink ID="LinkRegionOverview" runat="server" Target="_blank">Region overview</asp:HyperLink>
                </p>
                <p>
                    <asp:HyperLink ID="LinkForSaleByOwnerHomes" runat="server" Target="_blank">For Sale by Owner Homes</asp:HyperLink></p>
                <p>
                    <asp:HyperLink ID="LinkForSaleHomes" runat="server" Target="_blank">For Sale Homes  </asp:HyperLink></p>

            </div>
        </div>

    </div>

</asp:Content>
