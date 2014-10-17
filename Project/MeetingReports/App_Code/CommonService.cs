using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
///CommonService 的摘要说明
/// </summary>
public class CommonService
{
	public CommonService()
	{
	}


    public static string LoadReportHTML(DataTable oDT, DateTime reportDate, bool isSearchResult, string searchKey)
    {
        try
        {
            //宏观等
            string htmlTR1 = @"
                <tr>
                    <td colspan=""3"">@DATE<B>@TITLE</B></td>
                </tr>
                <tr>
                    <td width=""10%"">&nbsp;</td>
                    <td colspan=""2"" valign=""top"">@CONTENT</td>
                </tr>
            ";

            //分行业
            string htmlTR2 = @"
                <tr>
                    <td>&nbsp;</td>
                    <td colspan=""2""><HR /></td>
                </tr>
                <tr>
                    <td width=""10%"">&nbsp;</td>
                    <td valign=""top"" colspan=""2"">@DATE<B>@SUBTITLE</B></td>
                </tr>
                <tr>
                    <td width=""10%""></td>
                    <td width=""10%""></td>
                    <td>@CONTENT</td>
                </tr>
            ";

            //纯标题行
            string htmlTR3 = @"
            <tr>
                <td colspan=""3""><B>@TITLE</B></td>
            </tr>
            ";

            string htmlTable = @"<Table>";
            if (!isSearchResult)
                htmlTable += @"<tr><td colspan=""3"" align=""center""><B>" + reportDate.ToString("yyyy年MM月dd日") + @"晨会纪要</B></td></tr>";

            foreach (DataRow oRow in oDT.Rows)
            {
                string title = oRow["CategoryName"].ToString();
                int level = Convert.ToInt16(oRow["CategoryLevel"]);
                int keepEmpty = Convert.ToInt16(oRow["KeepEmpty"]);

                string keywords = oRow["KeyWords"].ToString();
                //if (keywords.Trim().Length >0)
                //{
                //    keywords = "[关键词: " + keywords + "]";
                //}

                string format = oRow["Format"].ToString();

                string content = "";
                if (format == "T")
                {
                    content = parseTableContent(oRow["Content"].ToString());
                }
                else if (format == "M")
                {
                    content = parseMixedContent(oRow["Content"].ToString());
                }
                else
                {
                    content = parseTextContent(oRow["Content"].ToString());
                }

                if (isSearchResult && searchKey.Length > 0)
	            {
                    content = CommonService.highlightKeywords(content, searchKey);
	            }
                

                if (level == 1)
                {
                    if (keepEmpty == 1)
                    {
                        htmlTable += htmlTR3.Replace(@"@TITLE", title);
                    }
                    else
                    {
                        htmlTable += htmlTR1.Replace(@"@TITLE", title).Replace(@"@CONTENT", content).Replace(@"@KEYWORDS", keywords);
                    }
                }
                else
                {
                    htmlTable += htmlTR2.Replace(@"@SUBTITLE", title).Replace(@"@CONTENT", content).Replace(@"@KEYWORDS", keywords);
                }

                if (isSearchResult)
                    htmlTable = htmlTable.Replace("@DATE",Convert.ToDateTime(oRow["reportDate"]).ToString("yyyy-MM-dd: "));
                else
                    htmlTable = htmlTable.Replace("@DATE", "");
            }
            htmlTable += "</Table>";

            return htmlTable;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static string parseTextContent(string content)
    {
        string newContent = "";

        if (content.Trim().Length == 0)
        {
            newContent = "(无)";
        }
        else
        {
            string[] contentList;
            contentList = content.Split("\r\n".ToCharArray());

            for (int i = 0; i < contentList.Length; i++)
            {
                string tmp = contentList[i].Trim().Replace("\n", "");

                if (tmp.Length > 0)
                    newContent += "<P>" + tmp + "</P>";
            }
        }

        return newContent;
    }

    private static string parseTableContent(string content)
    {
        string newContent = "";

        if (content.Trim().Length == 0)
        {
            newContent = "(无)";
        }
        else
        {
            string[] contentList;
            contentList = content.Split("\r\n".ToCharArray());

            newContent = @"<Table style=""border:1px solid black"" width=""100%"">";

            for (int i = 0; i < contentList.Length; i++)
            {
                if (contentList[i].Trim().Length > 0)
                {
                    string tmp = contentList[i];
                    tmp = @"<TR style=""border:1px solid black""><TD>" + tmp.Replace("\t", @"</TD><TD>");
                    tmp += "</TD></TR>";
                    newContent += tmp;
                }
            }

            newContent += "</Table>";
        }

        return newContent;
    }

    private static string parseMixedContent(string content)
    {
        if (!content.Contains("\t"))
        {
            return parseTextContent(content);
        }

        string newContent = "";

        if (content.Trim().Length == 0)
        {
            newContent = "(无)";
        }
        else
        {
            string[] contentList;
            contentList = content.Split("\r\n".ToCharArray());

            int iStart = 0, iEnd = 0;
            for (int i = 0; i < contentList.Length; i++)
            {
                if (contentList[i].Length == 0)
                    continue;

                if (contentList[i].Contains("\t"))
                {
                    if (iStart == 0)
                        iStart = i;
                    else
                        iEnd = i;
                }
                else
                {
                    //Found Table block
                    if (iStart > 0)
                    {
                        if (iEnd == 0)
                            iEnd = iStart;

                        newContent += parseTableContent(buildString(contentList, iStart, iEnd)) + "\r\n";
                        iStart = iEnd = 0;
                    }

                    newContent += contentList[i] + "\r\n";
                }
            }

            return parseTextContent(newContent);
        }

        return newContent;
    }

    private static string buildString(string[] contentArray, int iStart, int iEnd)
    {
        string content = "";
        for (int i = iStart; i <= Math.Min(iEnd, contentArray.Length); i++)
        {
            if (contentArray[i].Length == 0)
                continue;

            content += contentArray[i] + "\r\n"; ;
        }

        return content;
    }

    public static string highlightKeywords(string content, string keywords)
    {
        string[] keys = keywords.Split("%".ToCharArray());

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Trim().Length > 0)
            {
                content = content.Replace(keys[i],"<span style=\"background-color: #FFFF00\">" + keys[i] + "</span>");
            }
        }

        return content;
    }
}