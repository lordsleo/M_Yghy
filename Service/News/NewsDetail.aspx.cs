//
//文件名：    NewsDetail.aspx.cs
//功能描述：  详细新闻
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
using ServiceInterface.Common;

namespace M_Yghy.Service.News
{
    public partial class NewsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var message = Request.Params["message"];
                var str = message.Split('=');
                var str1 = str[1].Split(' ');
                var newsId = str1[0];

                string sql =
                    string.Format("select topic,convert(varchar(100), post_time, 20) as post_time,message from news_topic where id = '{0}'", newsId);
                var dt = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathYgwl).ExecuteTable(sql);
                var array = new Leo.Data.Table(dt).ToArray();

                Json = JsonConvert.SerializeObject(array);
            }
            catch (Exception ex)
            {
                LogTool.WriteLog(typeof(NewsDetail), ex);
            }
        }
        protected string Json;
    }
}