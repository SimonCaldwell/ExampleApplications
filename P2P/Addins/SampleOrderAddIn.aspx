<!DOCTYPE html>
<html>
<head>
    <style>
        table {
            width: 100%;
            border: solid 1px #ccc; 
            border-collapse: collapse;
        }

        td {
            border: solid 1px #ccc;
            padding: 4px;
            color: #333;
        }

        td.pro-value-col {
            text-align: right;
            padding-right:10px;
        }

        .pro-header td {
            background-color: #eaeaea;
        }

        .pro-centre td {
            text-align: center;
        }

    </style>


    <meta name="Author" content="Kaine C. Varley">
    <meta http-equiv="expires" content="0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta http-equiv="pragma" content="no-cache">

    <meta http-equiv="cache-control" content="no-cache, no-store, must-revalidate">
    <link rel="stylesheet" type="text/css" href="../../CustomSupport/Site.css" />

    <title>Custom Sample Add-In </title>
    <script src="../../CustomSupport/jquery-2.2.4.min.js"></script>
    <script src="../../CustomSupport/jquery.filerNode.js"></script>
    <script src="../../CustomSupport/AddInClient.js"></script>
    <script src="../../CustomSupport/kendo.core.min.js"></script>
    <script src="../../CustomSupport/kendo.notification.min.js"></script>
    <script src="../../CustomSupport/kendo.popup.min.js"></script>

</head>
<body style="padding:30px; padding-top:20px;">
    <!--#Include virtual="../../CustomSupport/Notifications.inc"-->

    <table id="sessionValues" style="margin-top:10px;">
        <tr class="pro-header">
            <td>Session Key</td>
            <td>Session Value</td>
        </tr>
        <tr class="pro-working">
            <td colspan="2" style="text-align:center; padding:40px;">Working...</td>
        </tr>
    </table>

    <h3>Order Header</h3>
    <table id="orderHeader">
        <tr class="pro-header">
            <td>GUID</td>
            <td>Template</td>
            <td>Supplier Name</td>
            <td>Gross Value</td>
            <td>Net Value</td>
            <td>Tax Value</td>
        </tr>
        <tr class="pro-working">
            <td colspan="6" style="text-align:center; padding:40px;">Working...</td>
        </tr>
    </table>

    <h3>Order Lines</h3>
    <table id="orderLines">
        <tr class="pro-header">
            <td>Code</td>
            <td>Description</td>
            <td>Price</td>
            <td>Quantity</td>
            <td>Net Value</td>
            <td>Tax Value</td>
        </tr>
        <tr class="pro-working">
            <td colspan="6" style="text-align:center; padding:40px;">Working...</td>
        </tr>
    </table>
</body>

<script type="text/javascript">

    var methods = [
        addInClient.Services.GetSessionParm("MyValue"),
        addInClient.Services.GetSessionParm("MyValue2"),
        addInClient.Services.GetSessionID(),
        addInClient.Services.GetSessionParms(["MyValue3", "MyValue4"]),
        addInClient.Services.GetOrderInEdit(),
    ];

    addInClient.Services.MonitorAjaxCalls(methods)
    .done(function (results, alertMessages, infoMessages) {

        var html = [];
        var sessionHTML = "";
        var orderHeaderHTML = "";
        var orderLinesHTML = "";

        //Now build the session values output
        for (var i = -1; ++i < results.length - 1;) {
            var result = results[i];
            if ($.isArray(result)) {
                result.forEach(displayValues);
            }
            else {
                switch (i) {
                    case 0:
                        displayValue("MyValue", result);
                        break;
                    case 1:
                        displayValue("MyValue2", result);
                        break;
                    case 2:
                        displayValue("SessionID", result);
                        break;
                }
            }
        }
        sessionHTML = html.join("");

        //Now build the order header output

        html = [];
        var xml = addInClient.Services.XMLServices();
        var $dom = xml.loadDOM(results[4]);
        var orderGUID = xml.getNodeAttr($dom, "PurchaseOrder", "GUID");
        var $header = xml.getNode($dom, ["PurchaseOrder", "Header"]);
        var supplierTitle = xml.getNodeAttr($header, "Supplier", "Name");
        var templateLabel = xml.getNodeAttr($header, "Template", "Name");
        var $totals = xml.getNode($dom, ["Footer", "Totals"]);
        var grossValue = xml.filterNodeAttr($totals, "GrossTotal", "Name", "Document");
        var netValue = xml.filterNodeAttr($totals, "Total", "Name", "Document");
        var taxValue = xml.filterNodeAttr(xml.getNode($totals, "TaxTotal"), "Value", "Name", "Document");
        buildOrderHeader(orderGUID, templateLabel, supplierTitle, grossValue, netValue, taxValue);
        orderHeaderHTML = html.join("");

        //Now build the order lines output
        html = [];
        xml.getNode($dom, ["LineSet", "Line"]).each(function () {
            var $line = $(this);
            var lineCode = xml.getNodeAttr($line, "Item", "PartNumber");
            var lineDesc = xml.getNodeAttr($line, "Item", "Description"); 
            var price = xml.filterNodeAttr(xml.getNode($line, "Prices"), "Value", "Name", "Document");
            var quantity = xml.filterNodeAttr(xml.getNode($line, "Quantities"), "Quantity", "Name", "Ordered");
            var netValue = xml.filterNodeAttr(xml.getNode($line, "Values"), "Value", "Name", "Document");
            var taxValue = xml.filterNodeAttr(xml.getNode($line, "TotalTax"), "Value", "Name", "Document");
            buildOrderLine(html, lineCode, lineDesc, price, quantity, netValue, taxValue);
        });
        orderLinesHTML = html.join("");

        $(".pro-working").remove();
        $("#sessionValues").append(sessionHTML);
        $("#orderHeader").append(orderHeaderHTML);
        $("#orderLines").append(orderLinesHTML);


        //********** Helper methods to generate the html output  (start) **********

        function displayValues(item) {
            displayValue(item.Key, item.Value);
        }

        function displayValue(key, value) {
            html.push("<tr>");
            html.push("<td>");
            html.push(key);
            html.push("</td>");
            html.push("<td>");
            html.push(value);
            html.push("</td>");
            html.push("</tr>");
        }

        function buildOrderHeader(orderGuid, templateLabel, supplierName, grossValue, netValue, taxValue) {
            html.push("<tr class=\"pro-centre\">");
            html.push("<td>");
            html.push(orderGuid);
            html.push("</td>");
            html.push("<td>");
            html.push(templateLabel);
            html.push("</td>");
            html.push("<td>");
            html.push(supplierName);
            html.push("</td>");
            html.push("<td>");
            html.push(grossValue);
            html.push("</td>");
            html.push("<td>");
            html.push(netValue);
            html.push("</td>");
            html.push("<td>");
            html.push(taxValue);
            html.push("</td>");
            html.push("</tr>");
        }

        function buildOrderLine(html, code, description, price, quantity, netValue, taxValue) {
            html.push("<tr>");
            html.push("<td>");
            html.push(code);
            html.push("</td>");
            html.push("<td>");
            html.push(description);
            html.push("</td>");
            html.push("<td class=\"pro-value-col\">");
            html.push(price);
            html.push("</td>");
            html.push("<td class=\"pro-value-col\">");
            html.push(quantity);
            html.push("</td>");
            html.push("<td class=\"pro-value-col\">");
            html.push(netValue);
            html.push("</td>");
            html.push("<td class=\"pro-value-col\">");
            html.push(taxValue);
            html.push("</td>");
            html.push("</tr>");
        }

        //********** Helper methods to generate the html output  (end) **********

    })
    .fail(function (errorMessages) {
        addInClient.Services.HandleError(errorMessages);
    });



</script>



</html>






