using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.IO;


namespace BM.Tools.Web
{

    public  class SendEmail
    {
        private string from;
        private string displayname;
        private string username;
        private string password;
        private string server;
        private int port;

        /// <summary>
        /// 发送邮件类
        /// </summary>
        /// <param name="from">发送人邮件地址</param>
        /// <param name="displayname">发送人显示名称</param>
        /// <param name="username">邮件登录名</param>
        /// <param name="password">邮件密码</param>
        /// <param name="server">邮件服务器 smtp服务器地址</param>
        /// <param name="port">端口</param>
        public SendEmail(string from, string displayname, string username, string password, string server, int port = 25)
        {
            this.from = from;
            this.displayname = displayname;
            this.username = username;
            this.password = password;
            this.server = server;
            this.port = port;
        }


        /// <summary>
        /// 发送邮件程序
        /// </summary>
        /// <param name="to">发送给谁（邮件地址）</param>
        /// <param name="subject">标题</param>
        /// <param name="body">内容</param>
        /// <param name="attachment">附件</param>
        public void SendMail(string to, string subject, string body, string attachment)
        {
            //邮件发送类
            MailMessage mail = new MailMessage();
            //是谁发送的邮件
            mail.From = new MailAddress(from, displayname);
            //发送给谁
            mail.To.Add(to);
            //标题
            mail.Subject = subject;
            //内容编码
            mail.BodyEncoding = Encoding.UTF8;
            //发送优先级
            mail.Priority = MailPriority.High;
            //邮件内容
            mail.Body = body;
            //是否HTML形式发送
            mail.IsBodyHtml = true;
            //附件
            if (attachment.Length > 0)
            {
                mail.Attachments.Add(new Attachment(attachment));
            }
            //邮件服务器和端口
            SmtpClient smtp = new SmtpClient(server, port);
            smtp.UseDefaultCredentials = true;
            //指定发送方式
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //指定登录名和密码
            smtp.Credentials = new System.Net.NetworkCredential(username, password);

            //超时时间
            smtp.EnableSsl = false;
            smtp.Timeout = 5000;
            smtp.Send(mail);
        }
        


        ///   <summary>
        ///   发送邮件
        ///   </summary>
        ///   <param   name= "to "> 收信人地址 </param>
        ///   <param   name= "subject "> 邮件标题 </param>
        ///   <param   name= "body "> 邮件正文 </param>
        ///   <param   name= "IsHtml "> 是否是HTML格式的邮件 </param>
        public void SendMail(string to, string subject, string body, bool IsHtml)
        {
            //设置SMTP 验证,端口默认为25，如果需要其他请修改
            SmtpClient mailClient = new SmtpClient(server, port);

            //指定如何发送电子邮件。
            //Network   电子邮件通过网络发送到   SMTP   服务器。   
            //PickupDirectoryFromIis   将电子邮件复制到挑选目录，然后通过本地   Internet   信息服务   (IIS)   传送。   
            //SpecifiedPickupDirectory 将电子邮件复制到 SmtpClient.PickupDirectoryLocation 属性指定的目录，然后由外部应用程序传送。  

            mailClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;


            //创建邮件对象
            MailMessage mailMessage = new MailMessage(from, to, subject, body);
            mailMessage.From = new MailAddress(from, displayname);

            //定义邮件正文，主题的编码方式
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            //mailMessage.BodyEncoding = Encoding.Default;
            //获取或者设置一个值，该值表示电子邮件正文是否为HTML
            mailMessage.IsBodyHtml = IsHtml;

            //指定邮件的优先级
            mailMessage.Priority = MailPriority.High;

            //发件人身份验证,否则163   发不了
            //表示当前登陆用户的默认凭据进行身份验证，并且包含用户名密码
            mailClient.UseDefaultCredentials = true;
            mailClient.Credentials = new System.Net.NetworkCredential(username, password);

            //发送
            mailClient.Send(mailMessage);
        }


        //发送plaintxt
        public void SendText(string to, string subject, string body)
        {
            SendMail(to, subject, body, false);
        }

        //发送HTML内容
        public void SendHtml(string to, string subject, string body)
        {
            SendMail(to, subject, body, true);
        }

        //发送制定网页
        public void SendWebUrl(string to, string subject, string body, string url)
        {
            //发送制定网页
            SendHtml(to, subject, WebHelper.GetWebRequest(url));
        }
    }

}
