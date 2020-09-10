using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace API.Utility
{
    public class TemplateGenerator
    {
        public static string GetHTMLString()
        {
           

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Name</th>
                                        <th>LastName</th>
                                        <th>Age</th>
                                        <th>Gender</th>
                                    </tr>");

            
                sb.AppendFormat(@"<tr>
                                    <td>ali</td>
                                    <td>rezai</td>
                                    <td>22</td>
                                    <td>male</td>
                                  </tr>");
            

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();
        }
    }
}