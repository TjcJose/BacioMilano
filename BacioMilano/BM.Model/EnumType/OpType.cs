using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BM.Model.EnumType
{
    public enum Op_WeiXin
    {
        Auth = 1
    }
}

//public ActionResult Auth()
//{
//    bool isContinue = true;
//    string method = Request.HttpMethod.ToLower();
//    ActionResult r = null;
//    if (method == "get")
//    {
//        this.auth(ref isContinue); if (!isContinue) { return r; }
//        this.access_token(ref isContinue); if (!isContinue) { return r; }
//    }
//    else if (method == "post")
//    {
//        var dic = getReceiveHashTable();
//        if (dic == null) { return null; }

//        // 发送方帐号（open_id）  
//        string fromUserName = dic["FromUserName"];
//        // 公众帐号  
//        string toUserName = dic["ToUserName"];
//        // 消息类型  
//        string msgType = dic["MsgType"].ToLower();

//        if (msgType == "text") //文本消息
//        {
//        }
//        else if (msgType == "image") //图片消息
//        {
//        }
//        else if (msgType == "voice") //语音消息
//        {
//        }
//        else if (msgType == "video") //视频消息
//        {
//        }
//        else if (msgType == "location") //地理位置消息
//        {
//        }
//        else if (msgType == "link") //链接消息
//        {
//        }
//        else if (msgType == "event") //
//        {
//            String eventType = dic["Event"].ToLower();
//            if (eventType == "subscribe") // 订阅
//            {
//            }
//            else if (eventType == "subscribe")
//            {
//            }
//            else if (eventType == "unsubscribe")// 取消订阅  
//            {
//            }
//            else if (eventType == "click") // 自定义菜单点击事件
//            {
//            }
//        }
//    }
//    return null;
//}
