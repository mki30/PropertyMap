<%@ Page Title="Hoam Loan" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PropertyLoan.aspx.cs" Inherits="HomeLoan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="js/Common.js"></script>

    <script type="text/javascript">
        CurrentPage = "loan";
    </script>

    <script type="text/javascript">
        $(document).ready(function ()
        {
            $("#HomloanTable").HighLightRows();
            //$(document).ready(function ()
            //{
            //    $("tr:even").css("background-color", "black");
            //});
        });
    </script>

    <style type="text/css">
        .LoanTable table > tr:nth-child(odd)
        {
            background-color: #def;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h1>Home Loan Rates</h1>
    <table id="no-more-tables" class='table table-bordered table-hover table-striped table-condensed'>

        <%--  <tbody>
            <tr>
                <td>Bank Name</td>
                <td>Floating Interest rate
                    </td>
                <td>Processing Fee</td>
                <td>Prepayment Charges</td>
                <!--<td >Apply</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#sbi-home-loan.php"><b>State Bank Of India</b></a>
                    <strong></strong>
                </td>
                <td ><b>9.95%</b></td>
                <td ><b>Up to 25 lacs :</b> 0.125% of loan amount minimum Rs.1000/-<br>
                    <b>25-75 lacs :</b>  Rs.3,250/-<br>
                    <b>75 &amp; above :</b> 5,000/-<br>
                </td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#icici-hfc-home-loan.php"><b>ICICI Bank</b></a>
                </td>
                <td ><b>Scheme I :10.25% (Fixed 1 yr)<br>
                    Scheme II : 10.25% (Fixed 2yrs)<br>
                    Scheme III : 10.50% (Fixed 3yrs)<br>
                    then 10.25%</b></td>
                <td >0.50% of loan amount upto 1 crore</td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->

                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#hdfc-ltd-home-loan.php"><b>HDFC Ltd</b></a>
                </td>
                <td ><b>Scheme I : 10.75% (Upto 10Lacs) (Fixed for 10yrs), then 11% (Fixed for 10yrs)<br>
                    Scheme II:10.25%</b></td>
                <td >0.5% or maximum 10,000+service tax (12.36%)</td>
                <td >No prepayment charges shall be payable for partial or full prepayments irrespective of the source</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->

                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#hsbc-home-loan-eligibility-interest-rates-documents-apply/"><b>HSBC Bank</b></a>
                </td>
                <td ><b>10% to 13%</b></td>
                <td >1% of the loan amount applied for, subject to a minimum of Rs 10000 plus service tax. This fee is payable on application and is not refundable</td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->

                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#lic-housing-home-loan.php"><b>LIC Housing</b></a>
                    <strong></strong>
                </td>
                <td ><b>Scheme I : 10.25% (Fixed for 2 yrs)
                    <br>
                    Scheme II : 10.70% (Fixed for 3 yrs) 11.15% (Fixed for 5 yrs)
                    <br>
                    Scheme III : 10.95%(Fixed for 10 yrs)</b></td>
                <td >Up to 50 lacs : 10,000 +(Service tax)<br>
                    50 lacs &amp; above : 15,000 +(service tax)</td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#home-loan-axis-bank.php"><b>AXIS Bank</b></a>
                </td>
                <td ><b>10.25% (Upto 25 Lacs), then 10.50%</b></td>
                <td >1% of the Loan Amount</td>
                <td >2% (For Scheme I)<br>
                    Nil (For Scheme II)</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->

                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#home-loan-idbi-homefinance.php"><b>IDBI</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >Up to .50%of loan amount
                    <br>
                    (Rs 2500 to be collected at login and balance at the time of sanction ) </td>
                <td >If Balance Transfer then 2% Otherwise Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#home-loan-ingvysya-bank.php"><b>ING Vysya</b></a>
                </td>
                <td ><b>10.75%</b></td>
                <td >0.5% of the loan amount
                </td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->

                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#home-loan-standard-chartered-bank.php"><b>Standard Chartered</b></a>
                    <strong></strong>
                </td>
                <td ><b>9.99%  (Upto 25Lacs), then 10.40%</b></td>
                <td >Rs.7500/- + Service tax</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#dhfl.php"><b>DHFL</b></a>
                    <strong></strong>
                </td>
                <td ><b>11%</b></td>
                <td >1% for Salaried &amp; 1.5% for SENP</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#home-loan-citibank.php"><b>Citibank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50% (Upto 25Lacs), then 10.75% 	</b></td>
                <td >0.25% (for salaried ),  0.50%  ( for Self Employed)</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#deutsche-bank-home-loan.php"><b>Deutsche Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >12000 + Service Tax</td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#indiabulls-home-loans-interest-rates-emi-apply/"><b>India Bulls</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.25% (Upto 25Lacs), then 10.75%</b></td>
                <td >Up to 30 lacs 5,000+12.36%(Service tax)<br>
                    30 lacs &amp; above : 15,000+12.36%(Service tax)</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#allahabad-bank-home-loans-interest-rate-processing-fee/"><b>Allahabad Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >0.50% of loan amount, Maximum Rs. 10,000/-</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#bank-of-maharashtra-home-loan-interest-rates-documents-eligibility-apply/"><b>Bank of Maharastra</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >Up to 1 cr  : NIL , Above 1 crore 0.25%(Maximum 25,000)</td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#central-bank-of-india-home-loan-interest-rates-processing-fee-3/"><b>Central Bank of India</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >0.50% of the loan amount subject to maximum of Rs.20,000/-</td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#corporation-bank-home-loan-interest-rates-processing-fee/"><b>Corporation Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >0.50% of Loan amount (Max.Rs.50,000/-) </td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#bank-of-india-home-loan-processing-fee-interest-rate-emi/"><b>Bank of India</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >Waived till 31.03.2013 for new loans sanctioned and 1st disbursement before 31.03.2013</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#union-bank-home-loan-interest-rates-processing-fee-emi/"><b>Union Bank of India</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >waived off (till 26-Jan-2013)</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#united-bank-of-india-housing-loan-interest-rates-emi-apply/"><b>United Bank of India</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.95% (upto 25 lacs) then 10.75%</b></td>
                <td >0.50% of the loan amount
                </td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <strong>UCO Bank</strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >N.A</td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <strong>Bank of Baroda</strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >N.A</td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <strong>Canara Bank</strong>
                </td>
                <td ><b>10.75%</b></td>
                <td ></td>
                <td ></td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#oriental-bank-of-commerce-home-loan-interest-rates-eligibility-documents-apply"><b>Oriental Bank of Commerce</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.40%</b></td>
                <td >0.50% of the loan amount, subject to maximum of Rs. 20000/- plus  service tax</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#home-loan-kotak-mahindra-bank.php"><b>Kotak Bank</b></a>
                </td>
                <td ><b>10.75%</b></td>
                <td >0.25% - 0.5%</td>
                <td >2% on Balance transfer else NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->

                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#dena-bank-home-loan-interest-rates-eligibility-apply/"><b>Dena Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >N.A</td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#pnb-home-loan-interest-rates-eligibility-documents-apply"><b>Punjab National Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td ></td>
                <td ></td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#first-blue-home-finance-eligibility-interest-rates-documents-apply/"><b>First Blue Home Finance</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.25% (for Salaried / SEP), 10.75% (For Self Employed) (Upto 25Lacs), Then 10.75% (for Salaried / SEP), 11.25% (For Self Employed)</b></td>
                <td ><b>Salaried</b>:  Up to 30 lacs :7300<br>
                    30 - 75 lacs : 11800<br>
                    75 lacs &amp; above : 15,000 + service tax(12.36%)</td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#vijay-bank-home-loan-interest-rates-calulator-documents-emi-apply"><b>Vijaya Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.75%</b></td>
                <td >N.A</td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <strong>Syndicate Bank</strong>
                </td>
                <td ><b>10.50%  ( upto 25 lacs ), then 10.75%</b></td>
                <td ><b>upto Rs.25 lacs</b> -  0.25% (Min Rs 1000 - Max Rs 5000)<br>
                    <b>26 lacs to 75 lacs</b> - 0.55% (Max.Rs.500/-)<br>
                    <b>above Rs.75 lacs</b> - 0.55% (Max Rs.10000/-) </td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#indian-overseas-bank-home-loans-interest-rates-documents-eligibility-apply/"><b>Indian Overseas Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >N.A</td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#home-loan-reliance.php"><b>Reliance</b></a>
                    <strong></strong>
                </td>
                <td ><b>12% - 12.50%</b></td>
                <td >0.75% of loan amount</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <strong>Federal Bank</strong>
                </td>
                <td ><b>10.73%</b></td>
                <td >0.50% of the limit sanctioned with a minimum of 1000/-</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#pnb-housing-finance-interest-rates-documents-eligibility-apply"><b>PNB Housing Finance</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.75%(SEP), 11%(SENP)</b></td>
                <td >0.5%</td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <strong>Development Credit Bank</strong>
                </td>
                <td ><b>11.50%</b></td>
                <td >1%</td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#loans/uncategorized/sbt-housing-loan-interest-rates-eligibility-documents-apply/"><b>State Bank of Travancore</b></a>
                    <strong></strong>
                </td>
                <td ><b>11%</b></td>
                <td >Processing fees stands reduced by50% of normal charges during the campaign period
                </td>
                <td >NIL</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#"><b>Tata capital Housing Finance ltd</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.75%</b></td>
                <td >0.5 to 1% off the loan amount.</td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#dhanalakshmi-bank-home-loan-interest-rates-eligibility-documents-apply"><b>Dhanalakshmi Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>Scheme I: 11.50% (Fixed for 1 yr), Then 12%
                    <br>
                    Scheme II: 12%</b></td>
                <td >1% + service tax 
                </td>
                <td >N.A</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>

            <tr>
                <td >
                    <a href="#indian-bank-home-loan-interest-rates-documents-eligibility-apply"><b>Indian Bank</b></a>
                    <strong></strong>
                </td>
                <td ><b>10.50%</b></td>
                <td >Upto 10Lacs : 0.25% of loan amount
                    <br>
                    10 Lac &amp; above: 0.20% of loan amount with a minimum of Rs. 2,500/- &amp; maximum of Rs. 20,000/- (Non refundable)( to be remitted at the time of submission of application) </td>
                <td >Nil</td>
                <!--<td height="35" align="center" valign="middle" bgcolor="#FFFFFF" > -->
                <!--</td> -->
            </tr>
        </tbody>--%>
        <tbody>
            <tr>
                <th>Bank Name<br>
                </th>
                <th >Floating Interest rate<br>
                </th>
                <th >Per lac EMI<br>
                </th>
                <th>Processing Fee</th>
                <th>Prepayment Charges</th>
            </tr>
            
            <tr>
                <td>
                    <a href="https://www.sbi.co.in/user.htm" target="_blank"><b>State Bank Of India</b></a>
                    <strong></strong>
                </td>
                <td><b>10.30%</b></td>
                <td>Rs.984</td>
                <td><b>Up to 25 lacs :</b> 0.125% of loan amount minimum Rs.1000/-<br>
                    <b>25-75 lacs :</b>  Rs.3,250/-<br>
                    <b>75 &amp; above :</b> 5,000/-<br>
                </td>
                <td>Nil</td>
            </tr>
             <tr>
                <td>
                    <a href="http://www.icicibank.com/Personal-Banking/loans/home-loan/home-loan-interest-rates.html"  target="_blank"><b>ICICI Bank</b></a>
                </td>
                <td><b>10.40%</b></td>
                <td>Rs.992</td>
                <td>0.50% of loan amount upto 1 crore</td>
                <td>N.A</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.hdfc.com/loans/home-loan.asp"  target="_blank"><b>HDFC Ltd</b></a>
                </td>
                <td><b>10.50%</b></td>
                <td>Rs.998</td>
                <td>0.5% or maximum 10,000+service tax (12.36%)</td>
                <td>No prepayment charges shall be payable for partial or full prepayments irrespective of the source</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.hsbc.co.in/1/2/personal/loans/home-loan"  target="_blank"><b>HSBC Bank</b></a>
                </td>
                <td><b>10.75% - 11%</b></td>
                <td>Rs.1015 - Rs.1032</td>
                <td>1% of the loan amount applied for, subject to a minimum of Rs 10000 plus service tax. This fee is payable on application and is not refundable</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.lichfl.com/lichousing/index_new.asp"  target="_blank"><b>LIC Housing</b></a>
                    <strong></strong>
                </td>
                <td><b>Scheme I:10.10% (Fixed for 2 yrs)(for female applicants) or 10.35% (Fixed for 2 yrs)(for male applicants)<br>
                    Scheme II: 11.25% (Fixed for 10 yrs)<br>
                    Scheme III: 12.25% onwards</b></td>
                <td>Scheme I:Rs.972 (Fixed for 2 yrs)(for female applicants) or Rs.988 (Fixed for 2 yrs)(for male applicants)<br>
                    Scheme II: Rs.1049 (Fixed for 10 yrs)<br>
                    Scheme III: Rs.1118 onwards</td>
                <td>Up to 50 lacs : 10,000 +(Service tax)<br>
                    50 lacs &amp; above : 15,000 +(service tax)</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.axisbank.com/personal/loans/home-loan/home-loan-fees-charges.aspx"  target="_blank"><b>AXIS Bank</b></a>
                </td>
                <td><b>10.25% (Upto 25 Lacs), then 10.50%</b></td>
                <td>Rs.982 (Upto 25 Lacs), then Rs.998</td>
                <td>0.5% of the loan amount<br>
                    (Max. 10000/- + service tax for Salaried)</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/home-loan-idbi-homefinance.php"  target="_blank"><b>IDBI</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>Up to .50%of loan amount
                    <br>
                    (Rs 2500 to be collected at login and balance at the time of sanction ) </td>
                <td>If Balance Transfer then 2% Otherwise Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/pnb-housing-finance-interest-rates-documents-eligibility-apply"  target="_blank"><b>PNB Housing Finance</b></a>
                    <strong></strong>
                </td>
                <td><b>10.75% - 11.50% (For Salaried/SEP), 11.25% - 11.75% (SENP)</b></td>
                <td>Rs.1015 - Rs.1066 (For Salaried/SEP), Rs.1049 - Rs.1084 (SENP)</td>
                <td>0.5%</td>
                <td>NIL</td>
            </tr>
            
            <tr>
                <td>
                    <a href="http://www.deal4loans.com/home-loan-ingvysya-bank.php"  target="_blank"><b>ING Vysya</b></a>
                </td>
                <td><b>10.75% - 11.25%</b></td>
                <td>Rs.1015 -Rs.1049</td>
                <td>0.5% of the loan amount
                </td>
                <td>NIL</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/home-loan-standard-chartered-bank.php"  target="_blank"><b>Standard Chartered</b></a>
                    <strong></strong>
                </td>
                <td><b>10%  (Upto 25Lacs), then 10.25% - 10.50%</b></td>
                <td>Rs.965 (Upto 25Lacs), then Rs.982 - Rs.998</td>
                <td>Rs.5500/- + Service tax</td>
                <td>NIL</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/home-loan-citibank.php"  target="_blank"><b>Citibank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25% (Fixed for 2yrs)</b></td>
                <td>Rs.982 (Fixed for 2yrs)</td>
                <td>0.25% (For Salaried)<br>
                    0.5% (For Self Employed)</td>
                <td>NIL</td>
            </tr>
            <tr>
                <td>
                    <a href="http://www.deal4loans.com/deutsche-bank-home-loan.php"  target="_blank"><b>Deutsche Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.50%</b></td>
                <td>Rs.998</td>
                <td>12000 + Service Tax</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/dhfl.php"  target="_blank"><b>DHFL</b></a>
                    <strong></strong>
                </td>
                <td><b>11%</b></td>
                <td>Rs.1032</td>
                <td>1% for Salaried &amp; 1.5% for SENP</td>
                <td>NIL</td>
            </tr>
            
            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/indiabulls-home-loans-interest-rates-emi-apply/"  target="_blank"><b>India Bulls</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25% (Upto 25Lacs), then 11%</b></td>
                <td>Rs.981 (Upto 25Lacs), then Rs.1032</td>
                <td>Rs.7500 + 12.36%(Upto 30Lacs)<br>
                    , else 0.5% of the loan amount</td>
                <td>NIL</td>
            </tr>
            <tr>
                <td>
                    <strong>Federal Bank</strong>
                </td>
                <td><b>10.50%</b></td>
                <td>Rs.998</td>
                <td>7,000 + Service Tax</td>
                <td>N.A</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/bank-of-maharashtra-home-loan-interest-rates-documents-eligibility-apply/"  target="_blank"><b>Bank of Maharastra</b></a>
                    <strong></strong>
                </td>
                <td><b>10.55% (Upto 25lacs), then 10.75%</b></td>
                <td>Rs.1002 (Upto 25lacs), then Rs.1015</td>
                <td>0.50% (Max. Rs. 50,000/-)</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/allahabad-bank-home-loans-interest-rate-processing-fee/"  target="_blank"><b>Allahabad Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.20% (Upto 25Lacs), then 10.45%</b></td>
                <td>Rs.978  (Upto 25Lacs), then Rs.995</td>
                <td><b>upto 30 lacs:</b> 0.60% of the loan amount subject to max. Rs.12000
                    <br>
                    <b>30 lacs - 75 lac:</b> 0.45% of the loan amount subject to max. Rs.24000<br>
                    <b>above 75 lacs - 300 lacs:</b>  0.35% of the loan amount  subject to max. Rs.60000<br>
                    <b>above 300 lacs:</b> 0.25% of the loan amount subject to max. Rs.70000</td>
                <td>NIL</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/central-bank-of-india-home-loan-interest-rates-processing-fee-3/"  target="_blank"><b>Central Bank of India</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>0.50% of the loan amount subject to maximum of Rs.20,000/-</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/corporation-bank-home-loan-interest-rates-processing-fee/"  target="_blank"><b>Corporation Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>0.50% of Loan amount (Max.Rs.50,000/-) </td>
                <td>NIL</td>
            </tr>
             <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/bank-of-india-home-loan-processing-fee-interest-rate-emi/"  target="_blank"><b>Bank of India</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>WAIVED UPTO 31.12.2013</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/union-bank-home-loan-interest-rates-processing-fee-emi/"  target="_blank"><b>Union Bank of India</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>Upto 75lacs: Nil<br>
                    75 lacs &amp; above : 0.25% of the loan amount subject to maximum of Rs. 7,500
                    <br>
                    (offer valid till 31/03/2014)</td>
                <td>NIL</td>
            </tr>
            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/united-bank-of-india-housing-loan-interest-rates-emi-apply/"  target="_blank"><b>United Bank of India</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>0.50% of the loan amount
                </td>
                <td>N.A</td>
            </tr>

            <tr>
                <td>
                    <strong>UCO Bank</strong>
                </td>
                <td><b>10.20%</b></td>
                <td>Rs.978</td>
                <td>0.5% of the loan amount, minimum Rs.1500/- &amp; maximum Rs. 15000/-</td>
                <td>NIL</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/bank-of-baroda-home-loan-interest-rates-documents/"  target="_blank"><b>Bank of Baroda</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25% </b></td>
                <td>Rs.982</td>
                <td>Loan upto Rs.30 Lacs - 0.50% (Minimum Rs.5,000/-)
                    <br>
                    Above 30 Lacs - 0.40% (Min. Rs.15,000/- &amp; Max. Rs.50,000/-)</td>
                <td>N.A.</td>
            </tr>

            <tr>
                <td>
                    <strong>Canara Bank</strong>
                </td>
                <td><b>9.95%</b></td>
                <td>Rs.962</td>
                <td></td>
                <td></td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/oriental-bank-of-commerce-home-loan-interest-rates-eligibility-documents-apply"  target="_blank"><b>Oriental Bank of Commerce</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>Nil (During festival Bonanza period i.e upto 31.12.2013)</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/home-loan-kotak-mahindra-bank.php"  target="_blank"><b>Kotak Bank</b></a>
                </td>
                <td><b>10.75%</b></td>
                <td>Rs.1015</td>
                <td>0.25% - 0.5%</td>
                <td>2% on Balance transfer else NIL</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/dena-bank-home-loan-interest-rates-eligibility-apply/"  target="_blank"><b>Dena Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>Waiver of process fee / upfront fee under Dena Niwas Housing Finance is applicable for proposals sanctioned w.e.f. 9.9.2013 to 31.12.2013</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/pnb-home-loan-interest-rates-eligibility-documents-apply"  target="_blank"><b>Punjab National Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td></td>
                <td></td>
           </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/first-blue-home-finance-eligibility-interest-rates-documents-apply/"  target="_blank"><b>First Blue Home Finance</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25% (for Salaried / SEP), 10.75% (For Self Employed) (Upto 25Lacs), Then 10.75% (for Salaried / SEP), 11.25% (For Self Employed)</b></td>
                <td>Rs.982 (for Salaried / SEP), Rs.1015 (For Self Employed)</td>
                <td><b>Salaried</b>:  Up to 30 lacs :7300<br>
                    30 - 75 lacs : 11800<br>
                    75 lacs &amp; above : 15,000 + service tax(12.36%)</td>
                <td>Nil</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/vijay-bank-home-loan-interest-rates-calulator-documents-emi-apply"  target="_blank"><b>Vijaya Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.50%</b></td>
                <td>Rs.998</td>
                <td>0.25% of loan amount. Maximum Rs.10000. Exclusive of Service Tax</td>
                <td>N.A</td>
            </tr>

            <tr>
                <td>
                    <strong>Syndicate Bank</strong>
                </td>
                <td><b>10.25% </b></td>
                <td>Rs.982 </td>
                <td><b>upto Rs.25 lacs</b> -  0.25% (Min Rs 1000 - Max Rs 5000)<br>
                    <b>26 lacs to 75 lacs</b> - 0.55% (Max.Rs.500/-)<br>
                    <b>above Rs.75 lacs</b> - 0.55% (Max Rs.10000/-) </td>
                <td>N.A</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/indian-overseas-bank-home-loans-interest-rates-documents-eligibility-apply/"  target="_blank"><b>Indian Overseas Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%</b></td>
                <td>Rs.982</td>
                <td>A flat rate of 0.58% of the loan amount- maximum of Rs. 10,190/- There is no hidden charge. This is subject to change from time to time without prior intimation.</td>
                <td>N.A</td>
            </tr>

            <tr>
                <td>
                    <strong>Development Credit Bank</strong>
                </td>
                <td><b>11.50%</b></td>
                <td>Rs.1066</td>
                <td>1%</td>
                <td>Nil</td>
            </tr>
            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/uncategorized/sbt-housing-loan-interest-rates-eligibility-documents-apply/"  target="_blank"><b>State Bank of Travancore</b></a>
                    <strong></strong>
                </td>
                <td><b>10.25%-10.50%</b></td>
                <td>Rs.982 - Rs.998</td>
                <td>Processing fees stands reduced by50% of normal charges during the campaign period
                </td>
                <td>NIL</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/tata-capital-home-loans-interest-rates-eligibility-apply/"  target="_blank"><b>Tata capital Housing Finance ltd</b></a>
                    <strong></strong>
                </td>
                <td><b>10.50% - 11% (Salaried), 10.95% - 11.45% (SEP/SENP)</b></td>
                <td>Rs.1066 - Rs.1032 (Salaried), Rs.1028 - Rs.1062 (SEP/SENP)</td>
                <td>0.5 to 1% off the loan amount.</td>
                <td>N.A</td>
            </tr>

            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/dhanalakshmi-bank-home-loan-interest-rates-eligibility-documents-apply"  target="_blank"><b>Dhanalakshmi Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>11.75% (Fixed for 1yr), then 12.25% (Upto 35 Lacs), 12.25% (Fixed for 1yr), then 12.75%</b></td>
                <td>Rs.1084 (Fixed for 1yr), then Rs.1119 (Upto 35 Lacs), Rs.1119 (Fixed for 1yr), then Rs.1154</td>
                <td>1% + service tax 
                </td>
                <td>N.A</td>
            </tr>
            <tr>
                <td>
                    <a href="http://www.deal4loans.com/loans/home-loan/indian-bank-home-loan-interest-rates-documents-eligibility-apply"  target="_blank"><b>Indian Bank</b></a>
                    <strong></strong>
                </td>
                <td><b>10.20%</b></td>
                <td>Rs.978</td>
                <td>Nil</td>
                <td>Nil</td>
            </tr>
            <tr></tr>
    </tbody>
    </table>
    <div style="font-size:12px;"><b>Note</b> : Interest rates may vary time to time.</div>
</asp:Content>

