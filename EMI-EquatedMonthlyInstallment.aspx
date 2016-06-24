<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EMI-EquatedMonthlyInstallment.aspx.cs" Inherits="EMI_EquatedMonthlyInstallment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="description" content="Emi amd price calculation of real estate projects,delhi,ncr,noida,Ghaziabad ,india" />
    <link rel="stylesheet" type="text/css" href="css/EmiCalculator.css" />
    <script type="text/javascript" src='<%=ResolveClientUrl("~/js/CalculatePrice.js")%>'></script>
    <script type="text/javascript" src="js/Globalize.js"></script>
    <script type="text/javascript" src="js/highcharts.js"></script>
    <script type="text/javascript" src="js/ConvertWord.js"></script>
    <script type="text/javascript">
        CurrentPage = "emi";
    </script>
    <style type="text/css">
        
        .simpletbl td, .simpletbl th
        {
            border-top:0;
        }
        .simpletbl th
        {
            background-color:#dddbdb;
            border-top:2px solid #fba214;
        }
        
        .amortization tr td,.amortization tr th
        {
             text-align: center;
        }
        .containerdiv
        {
            border:1px dotted #dddbdb; 
            border-radius:4px;
        }
        .amortization 
        {
            border-radius:20px;
            border: 1px solid black;
        }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row-fluid">
        <div class="containerdiv span6">
            <table class="simpletbl table  table-condensed">
                <tbody>
                    <tr>
                        <th colspan="2"><h1 style="font-size:14px; line-height:20px;">EMI Calculation</h1>
                        </th>
                    </tr>
                    <tr>
                        <td>Loan Amount:</td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server">200000</asp:TextBox><span id="inword"></span>
                            <span>80%</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Interest Rate:</td>
                        <td>
                            <asp:TextBox runat="server" ID="IntrestRate" value="10.5"></asp:TextBox><span style="padding-top: 5px; padding-left: 5px;">% p.a.</span></td>
                    </tr>
                    <tr>
                        <td>Loan Term:</td>
                        <td>
                            <asp:TextBox runat="server" ID="Year" value="15"></asp:TextBox><span style="padding-top: 5px; padding-left: 5px;">years</span></td>
                    </tr>

                    <tr>
                        <td>EMI:</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEMI" value="15"></asp:TextBox><span style="padding-top: 5px; padding-left: 5px;">per month</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div id="emipiechart" style="height: 300px; width: 98%;" class="row-fluid"></div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="containerdiv span6">
            <table class="simpletbl table table-condensed ">
                <tbody>
                    <tr>
                        <th colspan="2"><span style="font-weight:bold; font-size:14px;">Property Price</span>
                        </th>
                    </tr>
                    <tr>
                        <td>Super Area </td>
                        <td>
                            <asp:TextBox ID="txtArea" runat="server" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td title="Basic Sale Price per feet">BSP/ft</td>
                        <td>
                            <asp:TextBox runat="server" ID="BSPRate" value="0"> </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Total BSP</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtTotalBSP" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Adv Maintenance</td>
                        <td>
                            <asp:TextBox runat="server" ID="MaintenanceDeposit" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Power Back</td>
                        <td>
                            <asp:TextBox runat="server" ID="PowerBack" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Parking</td>
                        <td>
                            <asp:TextBox runat="server" ID="Parking" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td title="Club fees, Service Tax may also be charged">Club Fee</td>
                        <td>
                            <asp:TextBox runat="server" ID="ClubFee" value="0"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td title="Prime Location Charges per feet">PLC/ft</td>
                        <td>
                            <asp:TextBox runat="server" ID="PLCRate" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Total PLC</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtTotalPLC" Enabled="false" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Registry</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRegistryRate" value="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Total</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtTotal"></asp:TextBox>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>

    <div class="row-fluid">
        <div class="span12">
            <div id="container" style="width: 100%; height: 300px; float: left;"></div>
        </div>
    </div>
    <h3>Amortization</h3>
    <div class="row-fluid">
        <div class="span12">
            <div id="amortization"></div>
        </div>
    </div>
</asp:Content>

