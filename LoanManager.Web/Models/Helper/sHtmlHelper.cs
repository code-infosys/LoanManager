using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoanManager.Web
{
    public static class sHtmlHelper
    {
        public static IHtmlContent HelloWorldHTMLString(this IHtmlHelper htmlHelper)
        {
            return new HtmlString("<strong>Hello World</strong>");
        }

        //updated on 31july2019 when alert impliments
        public static IHtmlContent AjaxFormSubmiterV2(this IHtmlHelper htmlHelper, string FormId, string DataTableId,
            bool IsCloseAfterSubmit, string SuccessMessage = "Action Successfully Executed", bool IsCkEditorHit = false)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
             
            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function (e) {");
            sb.AppendLine("var frm = $('#" + FormId + "');");
            sb.AppendLine("frm.on('submit', (function (e) {");
            sb.AppendLine("    e.preventDefault();");
            if (IsCkEditorHit)
            {
                sb.AppendLine("           try{ for (instance in CKEDITOR.instances) { CKEDITOR.instances[instance].updateElement(); } } catch (e) { }");
            }
            sb.AppendLine("    $.ajax({");
            sb.AppendLine("        url: frm.attr('action'),");
            sb.AppendLine("        type: frm.attr('method'),");
            sb.AppendLine("        data: new FormData(this),");
            sb.AppendLine("        contentType: false,");
            sb.AppendLine("        cache: false,");
            sb.AppendLine("        processData: false,");
            sb.AppendLine("        success: function (data) {");
            sb.AppendLine("            if (data.status == \"success\") {");
            if (DataTableId.Length > 0)
            {
                sb.AppendLine("                try {");
                sb.AppendLine("                    var tablez = $('#" + DataTableId + "').DataTable();");
                sb.AppendLine("                    tablez.ajax.reload();");
                sb.AppendLine("                } catch (e) { }");
            }
            sb.AppendLine("                $.sticky('<br/> " + SuccessMessage + "', { stickyClass: 'success' });");
            if (IsCloseAfterSubmit)
            {
                sb.AppendLine("               $(\".modal-header .close\").click();");
            }

            sb.AppendLine("                  $('#sloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (data.status==\"Error\") {");
            sb.AppendLine("                $.sticky('<br/> ! Something is went wrong. <br/> ' + data.message + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#sloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("           else {");
            sb.AppendLine("               $.sticky('<br/> Read Warnings Alert.  <br/> <b>' + data.message + ' <b>', { stickyClass: 'error' });");
            sb.AppendLine("               $('#sloader').hide();");
            sb.AppendLine("          }");


            sb.AppendLine("        },");
            sb.AppendLine("       error: function (data) {");
            sb.AppendLine("           $.sticky('<br/> ! Something is went wrong. <br/> <b>Error:<b>' + data.message + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#sloader').hide();");
            sb.AppendLine("       }");
            sb.AppendLine("    });");
            sb.AppendLine("}");
            sb.AppendLine("));");

            sb.AppendLine("});");

            sb.AppendLine("</script>");

            return new HtmlString(sb.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stark"></param>
        /// <param name="FormId">Enter Here Form ID LIKE <form id="frmCreate"></form>  So you have to pass = frmCreate</param>
        /// <param name="DataTableId">Which DataTable You have update after submit provide that ID</param>
        /// <param name="IsCloseAfterSubmit">Do you want to opened popup close after submit , So pass=true or false any</param> 
        /// <param name="SuccessMessage">Give any Success message</param>
        /// <returns></returns>
        public static IHtmlContent AjaxFormSubmiter(this IHtmlHelper htmlHelper, string FormId, string DataTableId,
            bool IsCloseAfterSubmit, string SuccessMessage,bool IsCkEditorHit=false)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string successMsg = "Action Successfully Executed";

            if (SuccessMessage.Length > 0)
            {
                successMsg = SuccessMessage;
            }

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function (e) {");
            sb.AppendLine("var frm = $('#" + FormId + "');");
            sb.AppendLine("frm.on('submit', (function (e) {");
            sb.AppendLine("    e.preventDefault();");
            if(IsCkEditorHit)
            {
                sb.AppendLine("           try{ for (instance in CKEDITOR.instances) { CKEDITOR.instances[instance].updateElement(); } } catch (e) { }");
            } 
            sb.AppendLine("    $.ajax({");
            sb.AppendLine("        url: frm.attr('action'),");
            sb.AppendLine("        type: frm.attr('method'),");
            sb.AppendLine("        data: new FormData(this),");
            sb.AppendLine("        contentType: false,");
            sb.AppendLine("        cache: false,");
            sb.AppendLine("        processData: false,");
            sb.AppendLine("        success: function (data) {");
            sb.AppendLine("            if (data == \"Sumitted\") {");
            if (DataTableId.Length > 0)
            {
                sb.AppendLine("                try {");
                sb.AppendLine("                    var tablez = $('#" + DataTableId + "').DataTable();");
                sb.AppendLine("                    tablez.ajax.reload();");
                sb.AppendLine("                } catch (e) { }");
            }
            sb.AppendLine("                $.sticky('<br/> " + successMsg + "', { stickyClass: 'success' });");
            if (IsCloseAfterSubmit)
            {
                sb.AppendLine("               $(\".modal-header .close\").click();");
            }

            sb.AppendLine("                  $('#sloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("            else if (data.indexOf(\"Error\") >= 0) {");
            sb.AppendLine("                $.sticky('<br/> ! Something is went wrong. <br/> ' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#sloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("           else {");
            sb.AppendLine("               $.sticky('<br/> Read Warnings Alert.  <br/> <b>' + data + ' <b>', { stickyClass: 'error' });");
            sb.AppendLine("               $('#sloader').hide();");
            sb.AppendLine("          }");


            sb.AppendLine("        },");
            sb.AppendLine("       error: function (data) {");
            sb.AppendLine("           $.sticky('<br/> ! Something is went wrong. <br/> <b>Error:<b>' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#sloader').hide();");
            sb.AppendLine("       }");
            sb.AppendLine("    });");
            sb.AppendLine("}");
            sb.AppendLine("));");

            sb.AppendLine("});");

            sb.AppendLine("</script>");

            return new HtmlString(sb.ToString());
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="stark"></param>
        /// <param name="FormId">Enter Here Form ID LIKE <form id="frmCreate"></form>  So you have to pass = frmCreate</param>
        /// <param name="DataTableId">Which DataTable You have update after submit provide that ID</param>
        /// <param name="IsCloseAfterSubmit">Do you want to opened popup close after submit , So pass=true or false any</param> 
        /// <param name="SuccessMessage">Give any Success message</param>
        /// <param name="AfterSuccessCode">Add other JQuery code if you want</param>
        /// <returns></returns>
        public static IHtmlContent AjaxFormSubmiter(this IHtmlHelper htmlHelper, string FormId, string DataTableId,
            bool IsCloseAfterSubmit, string SuccessMessage, string AfterSuccessCode)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string successMsg = "Action Successfully Executed";

            if (SuccessMessage.Length > 0)
            {
                successMsg = SuccessMessage;
            }

            sb.AppendLine("<script type=\"text/javascript\">");
            sb.AppendLine("$(document).ready(function (e) {");
            sb.AppendLine("var frm = $('#" + FormId + "');");
            sb.AppendLine("frm.on('submit', (function (e) {");
            sb.AppendLine("    e.preventDefault();");
            sb.AppendLine("    $.ajax({");
            sb.AppendLine("        url: frm.attr('action'),");
            sb.AppendLine("        type: frm.attr('method'),");
            sb.AppendLine("        data: new FormData(this),");
            sb.AppendLine("        contentType: false,");
            sb.AppendLine("        cache: false,");
            sb.AppendLine("        processData: false,");
            sb.AppendLine("        success: function (data) {");
            sb.AppendLine("            if (data == \"Sumitted\") {");
            if (DataTableId.Length > 0)
            {
                sb.AppendLine("                try {");
                sb.AppendLine("                    var tablez = $('#" + DataTableId + "').DataTable();");
                sb.AppendLine("                    tablez.ajax.reload();");
                sb.AppendLine("                } catch (e) { }");
            }
            sb.AppendLine("                $.sticky('<br/> " + successMsg + "', { stickyClass: 'success' });");
            if (IsCloseAfterSubmit)
            {
                sb.AppendLine("               $(\".modal-header .close\").click();");
            }

            sb.AppendLine("                  $('#sloader').hide();");

            if (AfterSuccessCode.Length > 0)
            {
                sb.AppendLine("                   " + AfterSuccessCode + "");
            }


            sb.AppendLine("            }");
            sb.AppendLine("            else if (data.indexOf(\"Error\") >= 0) {");
            sb.AppendLine("                $.sticky('<br/> ! Something is went wrong. <br/> ' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#sloader').hide();");
            sb.AppendLine("            }");
            sb.AppendLine("           else {");
            sb.AppendLine("               $.sticky('<br/> Read Warnings Alert.  <br/> <b>' + data + ' <b>', { stickyClass: 'error' });");
            sb.AppendLine("               $('#sloader').hide();");
            sb.AppendLine("          }");


            sb.AppendLine("        },");
            sb.AppendLine("       error: function (data) {");
            sb.AppendLine("           $.sticky('<br/> ! Something is went wrong. <br/> <b>Error:<b>' + data + '', { stickyClass: 'error' });");
            sb.AppendLine("                $('#sloader').hide();");
            sb.AppendLine("       }");
            sb.AppendLine("    });");
            sb.AppendLine("}");
            sb.AppendLine("));");

            sb.AppendLine("});");

            sb.AppendLine("</script>");

            return new HtmlString(sb.ToString());
        }

          
        public static IHtmlContent DataTablejQueryBinder(this IHtmlHelper htmlHelper,string TableName,string[] Columns)
        { 
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
             
            sb.AppendLine("       $(document).ready(function () {");
            sb.AppendLine("            var cName = \"/"+ TableName + "\";");

            sb.AppendLine("            $('#tb" + TableName + " tfoot th').each(function () {");
            sb.AppendLine("                $(this).html('<input placeholder=\"Search\" type=\"text\" style=\"width: 70px;\" />');");
            sb.AppendLine("            });");

            sb.AppendLine("            var oTable = $('#tb" + TableName + "').DataTable({");
            sb.AppendLine("                \"serverSide\": true,");
            sb.AppendLine("                \"bRetrieve\": true,");
            sb.AppendLine("                \"pageLength\": 10,");
            sb.AppendLine("                \"lengthMenu\": [[5, 10, 25, 50, 100], [5, 10, 25, 50, 100]],");
            sb.AppendLine("                \"ajax\": {");
            sb.AppendLine("                    \"type\": \"POST\",");
            sb.AppendLine("                    \"url\": '/" + TableName + "/GetGrid',");
            sb.AppendLine("                    \"contentType\": 'application/json; charset=utf-8',");
            sb.AppendLine("                    'data': function (data) {");
            sb.AppendLine("                        data.SearchFromLength = $(\"#ddlSearchLength\").val();");
            sb.AppendLine("                        return data = JSON.stringify(data);");
            sb.AppendLine("                    }");
            sb.AppendLine("                },");
            sb.AppendLine("                \"dom\": 'lBfrtip',");
            sb.AppendLine("                \"buttons\": [");
            sb.AppendLine("                    'colvis',");
            sb.AppendLine("                    { extend: 'print', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    { extend: 'copyHtml5', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    { extend: 'excelHtml5', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    { extend: 'csvHtml5', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    { extend: 'pdfHtml5', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                ],");

            sb.AppendLine("                \"processing\": true,");
            sb.AppendLine("                \"paging\": true,");
            sb.AppendLine("                \"deferRender\": true,");
            sb.AppendLine("                \"aoColumns\": [");
            sb.AppendLine("                     {");
            sb.AppendLine("                         \"targets\": 0,");
            sb.AppendLine("                         \"data\": \"id\",");
            sb.AppendLine("                         \"render\": function (data, type, full, meta) {");
            sb.AppendLine("                             var buttons = '<div class=\"text-center\">\\");
            sb.AppendLine("                                     <div class=\"btn-group text-left\">\\");
            sb.AppendLine("                                         <button type=\"button\" class=\"btn btn-default btn-xs btn-warning dropdown-toggle\" data-toggle=\"dropdown\">Actions <span class=\"caret\"></span></button>\\");
            sb.AppendLine("                                         <ul class=\"dropdown-menu pull-left\" role=\"menu\">\\");
            sb.AppendLine("                                             <li><a data-toggle=\"modal\" href=\"/" + TableName + "/Edit/' + data + '\" data-target=\"#modal-action-base\"><i class=\"fa fa-edit\"></i>Edit</a></li>\\"); 
            sb.AppendLine("                                             <li><a onclick=\"deleteFile('+ data + ')\" title=\"Delete\" style=\"cursor: pointer;\"><i class=\"fa fa-bitbucket\"></i>Delete</a> </li>\\");
            sb.AppendLine("                                         </ul>\\");
            sb.AppendLine("                                     </div>\\");
            sb.AppendLine("                                 </div>';");
            sb.AppendLine("                             return buttons;");
            sb.AppendLine("                         }");
            sb.AppendLine("                     },");

            foreach (var item in Columns)
            {
                sb.AppendLine("                     { 'data': '"+item+"' },");
            } 
            sb.AppendLine("                ],");
            sb.AppendLine("                \"order\": [0, \"asc\"]");

            sb.AppendLine("            });");

            sb.AppendLine("            $(\".dataTables_filter\").hide();");
            sb.AppendLine("            $(\"#tb" + TableName + "_length select\").attr(\"style\", \"padding:6px; margin-right:3px;\");");

            sb.AppendLine("            oTable.columns().every(function () {");
            sb.AppendLine("                var that = this;");
            sb.AppendLine("                $('input', this.footer()).on('keydown', function (e) {");
            sb.AppendLine("                    if (e.keyCode == 9 || e.keyCode == 13) {");
            sb.AppendLine("                         that.search(this.value).draw();");
            sb.AppendLine("                    }");
            sb.AppendLine("                }); ");
            sb.AppendLine("            }); ");
            sb.AppendLine("        });");



            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent DataTableRowCopy(this IHtmlHelper htmlHelper, string TableName)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
             
            sb.AppendLine("    function copy(id) {");
            sb.AppendLine("        if (confirm(\"are you sure you want to new copy of this.? \"))");
            sb.AppendLine("            {");
            sb.AppendLine("            $.ajax({");
            sb.AppendLine("                url: \"/" + TableName + "/Copy\",");
            sb.AppendLine("                type: \"Post\",");
            sb.AppendLine("                data: { 'Id': id },");
            sb.AppendLine("                success: function(data) {");
            sb.AppendLine("                    $.sticky('<br/> Action Successfully Executed', { stickyClass: 'success' });");
            sb.AppendLine("                        var table2 = $('#tb" + TableName + "').DataTable();");
            sb.AppendLine("                        table2.ajax.reload();");
            sb.AppendLine("                    }");
            sb.AppendLine("                });");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            return new HtmlString(sb.ToString());
        }


        public static IHtmlContent DataTableRowDelete(this IHtmlHelper htmlHelper, string TableName,string error= "you are unauthorized to delete data", string success= "Action Successfully Executed")
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("    function deleteFile(id) {");
            sb.AppendLine("        if (confirm(\"are you sure you want to delete? \"))");
            sb.AppendLine("            {");
            sb.AppendLine("            $.ajax({");
            sb.AppendLine("                url: \"/" + TableName + "/Delete\",");
            sb.AppendLine("                type: \"Post\",");
            sb.AppendLine("                data: { 'Id': id },");
            sb.AppendLine("                success: function(data) {"); 
            sb.AppendLine("                    if (data.indexOf(\"unauthorized\") >= 0) { ");
            sb.AppendLine("                        $.sticky('<br/>"+ error + "', { stickyClass: 'error' });");
            sb.AppendLine("                    }");
            sb.AppendLine("                    else {");
            sb.AppendLine("                        $.sticky('<br/> " + success + "', { stickyClass: 'success' });");
            sb.AppendLine("                        var table2 = $('#tb" + TableName + "').DataTable();");
            sb.AppendLine("                        table2.ajax.reload();");
            sb.AppendLine("                    }");
            sb.AppendLine("                    }");
            sb.AppendLine("                });");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent JsonFormSubmitter(this IHtmlHelper htmlHelper, string TableName, bool UseSticky = true,string RedirectToAfterSuccess="")
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
             
            sb.AppendLine("            $(document).ready(function(e) {");
            sb.AppendLine("                var frm = $('#" + TableName + "');");
            sb.AppendLine("                frm.on('submit', (function(e) {");
            sb.AppendLine("                    e.preventDefault();");
            sb.AppendLine("                $.ajax({");
            sb.AppendLine("                    url: frm.attr('action'), type: frm.attr('method'), data: new FormData(this), contentType: false, cache: false, processData: false,");
            sb.AppendLine("                    success: function(data) {");
            sb.AppendLine("                            if (data.status === 'success')");
            sb.AppendLine("                            {");
            if (UseSticky)
            {
                sb.AppendLine("                            $.sticky('<br/> ' + data.message + '', { stickyClass: 'success' });");
            }
            else
            {
                sb.AppendLine("                            alert(data.message);");
            }
            if(RedirectToAfterSuccess.Length>0)
            {
                sb.AppendLine("                            "+RedirectToAfterSuccess);
            }
            sb.AppendLine("                            }");
            sb.AppendLine("                            else if (data.status === 'warning')");
            sb.AppendLine("                            {");
            if (UseSticky)
            {
                sb.AppendLine("                            $.sticky('<br/> Warning <br/> ' + data.message + '', { stickyClass: 'warning' });");
            }
            else
            {
                sb.AppendLine("                            alert(data.message);");
            }
            sb.AppendLine("                            }");
            sb.AppendLine("                            else if (data.status === 'error')");
            sb.AppendLine("                            {");
            if (UseSticky)
            {
                sb.AppendLine("                            $.sticky('<br/> Error <br/> ' + data.message + '', { stickyClass: 'error' });");
            }
            else
            {
                sb.AppendLine("                            alert(data.message);");
            }
            sb.AppendLine("                            }");
            sb.AppendLine("                        },");
            sb.AppendLine("                        error: function (data) {");
            if (UseSticky)
            {
                sb.AppendLine("                            $.sticky('<br/> ' + data.message + '', { stickyClass: 'error' });");
            }
            else
            {
                sb.AppendLine("                            alert(data.message);");
            }
            sb.AppendLine("                        }");

            sb.AppendLine("                    });");
            sb.AppendLine("                }");
            sb.AppendLine("            ));");
            sb.AppendLine("            });");

            return new HtmlString(sb.ToString());
        }
    }
}

