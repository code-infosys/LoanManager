
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanManager.Web
{
    public class DataTable : ViewComponent
    {
        public IStringLocalizer Localizer { get; set; }
        public DataTable(IStringLocalizer<string> _localizer)
        {
            Localizer = _localizer;
        }

        public HtmlString Invoke(string TableName, string[] Columns)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine("       $(document).ready(function () {");
            sb.AppendLine("            var cName = \"/" + TableName + "\";");

            sb.AppendLine("            $('#tb" + TableName + " tfoot th').each(function () {");
            sb.AppendLine("                $(this).html('<input placeholder=\""+ Localizer["Search"] + "\" type=\"text\" style=\"width: 70px;\" />');");
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
            sb.AppendLine("                    {text:'"+Localizer["Column visibility"]+"', extend: 'colvis', exportOptions: { columns: ':visible' }},");
            sb.AppendLine("                    {text:'" + Localizer["Print"] + "', extend: 'print', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    {text:'" + Localizer["Copy"] + "', extend: 'copyHtml5', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    {text:'" + Localizer["Excel"] + "', extend: 'excelHtml5', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    {text:'" + Localizer["CSV"] + "', extend: 'csvHtml5', exportOptions: { columns: ':visible' } },");
            sb.AppendLine("                    { text:'" + Localizer["PDF"] + "', extend: 'pdfHtml5', exportOptions: { columns: ':visible' } },");
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
            sb.AppendLine("                                         <button type=\"button\" class=\"btn btn-default btn-xs btn-warning dropdown-toggle\" data-toggle=\"dropdown\">" + Localizer["Actions"] + " <span class=\"caret\"></span></button>\\");
            sb.AppendLine("                                         <ul class=\"dropdown-menu pull-left\" role=\"menu\">\\");
            sb.AppendLine("                                             <li><a data-toggle=\"modal\" href=\"/" + TableName + "/Edit/' + data + '\" data-target=\"#modal-action-base\"><i class=\"fa fa-edit\"></i>" + Localizer["Edit"] + "</a></li>\\");
            sb.AppendLine("                                             <li><a onclick=\"deleteFile('+ data + ')\" title=\"Delete\" style=\"cursor: pointer;\"><i class=\"fa fa-bitbucket\"></i>" + Localizer["Delete"] + "</a> </li>\\");
            sb.AppendLine("                                         </ul>\\");
            sb.AppendLine("                                     </div>\\");
            sb.AppendLine("                                 </div>';");
            sb.AppendLine("                             return buttons;");
            sb.AppendLine("                         }");
            sb.AppendLine("                     },");

            foreach (var item in Columns)
            {
                sb.AppendLine("                     { 'data': '" + item + "' },");
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


    }
}

