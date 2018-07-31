//
//文件名：    GetShipMovementList.aspx.cs
//功能描述：  获取船舶动态件列表
//创建时间：  2015/08/04
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Leo;

namespace M_Yghy.Service
{
    public partial class GetShipMovementList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //查询页数
            var pages = Request.Params["Pages"];

            Dictionary<string, Array> info = new Dictionary<string, Array>();
            try
            {
                if (pages == null)
                {
                    string[] arry = new string[1];
                    arry[0] = "举例：http://218.92.115.55/M_Yghy/Service/GetShipMovementList.aspx?Pages=1";
                    info.Add("参数Pages不能为null！", arry);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                int minRow = (Convert.ToInt16(pages) - 1) * 15 + 1;
                int maxRow = Convert.ToInt16(pages) * 15;

                string sql =
                    string.Format(
                            "select count(id) as total from cbcq where classid='1'");
                var dt = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathYgwl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无报表！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                sql =
                    string.Format(
                        "select top {1} id,topic,newname as urlname,convert(varchar(100), post_time, 20) as post_time,classid from (select TOP {0} id,topic,newname,post_time,classid from cbcq where classid='1' order by post_time asc)a order by post_time desc",
                        Convert.ToInt32(dt.Rows[0]["total"]) - minRow + 1, maxRow - minRow + 1);
                dt = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathYgwl).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    string[] arry0 = new string[1];
                    arry0[0] = "NO";
                    info.Add("IsGet", arry0);
                    string[] arry1 = new string[1];
                    arry1[0] = "暂无更多报表！";
                    info.Add("Message", arry1);
                    Json = JsonConvert.SerializeObject(info);
                    return;
                }

                string[,] arrays = new string[dt.Rows.Count, 4];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    arrays[iRow, 0] = Convert.ToString(dt.Rows[iRow]["id"]);
                    arrays[iRow, 1] = Convert.ToString(dt.Rows[iRow]["topic"]);
                    arrays[iRow, 2] = Convert.ToString(dt.Rows[iRow]["urlname"]);
                    arrays[iRow, 3] = Convert.ToString(dt.Rows[iRow]["post_time"]);
                }

                string[] arry2 = new string[1];
                arry2[0] = "Yes";
                info.Add("IsGet", arry2);
                info.Add("ShipList", arrays);
                Json = JsonConvert.SerializeObject(info);
            }
            catch (Exception ex)
            {
                string[] arry0 = new string[1];
                arry0[0] = "NO";
                info.Add("IsGet", arry0);
                string[] arry1 = new string[1];
                arry1[0] = ex.Message;
                info.Add("Message", arry1);
                Json = JsonConvert.SerializeObject(info);
            }

        }
        protected string Json;
    }
}