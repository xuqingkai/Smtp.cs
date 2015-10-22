//https://github.com/xuqingkai/Smtp.cs
using System;
namespace SH
{
	/// <summary>
	/// 邮件发送对象
	/// </summary>
	public class Smtp
	{
		/// <summary>
		/// SMTP服务器地址
		/// </summary>
		private string _server = "";
		/// <summary>
		/// 设置SMTP服务器地址
		/// </summary>
		/// <param name="server">SMTP服务器地址</param>
		/// <returns></returns>
		public Smtp Server(string server)
		{
			_server = server;
			return this;
		}

		/// <summary>
		/// 发件人邮箱
		/// </summary>
		private string _email = "";
		/// <summary>
		/// 设置发件人邮箱
		/// </summary>
		/// <param name="email">发件人邮箱</param>
		/// <returns></returns>
		public Smtp Email(string email)
		{
			_email = email;
			return this;
		}

		/// <summary>
		/// 邮箱密码
		/// </summary>
		private string _password = "";
		/// <summary>
		/// 设置邮箱密码
		/// </summary>
		/// <param name="password">密码</param>
		/// <returns></returns>
		public Smtp Password(string password)
		{
			_password = password;
			return this;
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="server">SMTP服务器地址</param>
		/// <param name="email">发件人邮箱</param>
		/// <param name="password">邮箱密码</param>
		public Smtp(string server = "", string email = "", string password = "")
		{
			if (server.Length == 0) { server = System.Configuration.ConfigurationManager.AppSettings["SH.Smtp.Server"] + ""; }
			if (email.Length == 0) { email = System.Configuration.ConfigurationManager.AppSettings["SH.Smtp.Email"] + ""; }
			if (password.Length == 0) { password = System.Configuration.ConfigurationManager.AppSettings["SH.Smtp.Password"] + ""; }
			_server = server; _email = email; _password = password;
		}

		/// <summary>
		/// 回复地址
		/// </summary>
		private string _reply = "";
		/// <summary>
		/// 设置回复地址
		/// </summary>
		/// <param name="reply">回复地址</param>
		/// <returns></returns>
		public Smtp Reply(string reply)
		{
			_reply = reply;
			return this;
		}

		/// <summary>
		/// 收件人地址
		/// </summary>
		private string _to = "";
		/// <summary>
		/// 设置收件人地址
		/// </summary>
		/// <param name="to">收件人地址</param>
		/// <returns></returns>
		public Smtp To(string to)
		{
			_to = to;
			return this;
		}

		/// <summary>
		/// 标题
		/// </summary>
		private string _subject = "";
		/// <summary>
		/// 设置标题
		/// </summary>
		/// <param name="subject">标题</param>
		/// <returns></returns>
		public Smtp Subject(string subject)
		{
			_subject = subject;
			return this;
		}

		/// <summary>
		/// 内容
		/// </summary>
		private string _body = "";
		/// <summary>
		/// 设置内容
		/// </summary>
		/// <param name="body">内容</param>
		/// <returns></returns>
		public Smtp Body(string body)
		{
			_body = body;
			return this;
		}

		/// <summary>
		/// 附件路径
		/// </summary>
		private string _attachment = "";
		/// <summary>
		/// 设置附件路径
		/// </summary>
		/// <param name="attachment">附件路径</param>
		/// <returns></returns>
		public Smtp Attachment(string attachment)
		{
			_attachment = attachment;
			return this;
		}

		/// <summary>
		/// 返回消息号
		/// </summary>
		public int ID = -1;
		/// <summary>
		/// 返回消息描述
		/// </summary>
		public string Message = "";

		/// <summary>
		/// 发送
		/// </summary>
		/// <param name="to">收件人地址</param>
		/// <param name="subject">标题</param>
		/// <param name="body">内容</param>
		/// <param name="attachment">附件路径</param>
		/// <returns></returns>
		public Smtp Send(string to = "", string subject = "", string body = "", string attachment = "")
		{
			//创建SmtpClient对象
			System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
			//创建验证凭据，现在大部分SMTP邮箱都需要这个了
			smtp.Credentials = new System.Net.NetworkCredential(_email, _password);
			//指定电子邮件发送方式
			smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

			//检查SMTP服务器
			string server = _server, email = _email, password = _password;
			if (server.Length == 0 || email.Length == 0 || password.Length == 0)
			{
				ID = 1;
				Message = "请配置SMTP服务器地址，帐号（一般为邮箱全名），密码";
				return this;
			}
			//设置服务器地址
			smtp.Host = server;
			//用邮箱地址作为用户名
			smtp.Credentials = new System.Net.NetworkCredential(email, password);
			//创建MailMessage对象
			System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
			//优先级
			mail.Priority = System.Net.Mail.MailPriority.Low;
			//发件人地址，一般就是email的值，切勿更改
			mail.From = new System.Net.Mail.MailAddress(email);

			//标题
			if (subject.Length == 0) { subject = _subject; }
			if (subject.Length == 0) { subject = "空标题：" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒"); }
			mail.Subject = subject;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			//内容
			if (body.Length == 0) { body = _body; }
			if (body.Length == 0) { body = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); }
			mail.Body = body;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			mail.IsBodyHtml = true;
			//附件
			if (attachment.Length == 0) { attachment = _attachment; }
			if (attachment.Length > 0)
			{
				if (!attachment.Contains(":")) { attachment = System.Web.HttpContext.Current.Server.MapPath(attachment); }
				mail.Attachments.Add(new System.Net.Mail.Attachment(attachment, System.Net.Mime.MediaTypeNames.Application.Octet));
			}
			//回复地址
			string reply = _reply;
			if (reply.Length == 0) { reply = _email; }
			foreach (string r in reply.Replace(",", ";").Split(';'))
			{
				mail.ReplyToList.Add(r);
			}
			//收件人地址
			if (to.Length == 0) { to = _to; }
			foreach (string t in to.Replace(",", ";").Split(';'))
			{
				mail.To.Add(t);
			}
			//尝试发送
			try { smtp.Send(mail); ID = 0; Message = "发送成功！"; }
			catch (System.Net.Mail.SmtpException ex) { ID = 2; Message = ex.Message; }
			return this;
		}

		/// <summary>
		/// JSON
		/// </summary>
		public string Json()
		{
			System.Web.HttpContext.Current.Response.Clear();
			System.Web.HttpContext.Current.Response.Charset = "UTF-8";
			System.Web.HttpContext.Current.Response.Write("{");
			System.Web.HttpContext.Current.Response.Write("\"ret\":" + ID + ",");
			System.Web.HttpContext.Current.Response.Write("\"message\":\"" + Message + "\",");
			System.Web.HttpContext.Current.Response.Write("\"url\":null");
			System.Web.HttpContext.Current.Response.Write("}");
			System.Web.HttpContext.Current.Response.End();
			return null;
		}

		/// <summary>
		/// 调试
		/// </summary>
		/// <param name="content">内容</param>
		/// <param name="name">生成文件名</param>
		/// <returns></returns>
		public static string Debug(object content, object name = null)
		{
			if (name == null)
			{
				System.Web.HttpContext.Current.Response.Clear();
				System.Web.HttpContext.Current.Response.Write(content);
				System.Web.HttpContext.Current.Response.End();
			}
			else
			{
				string filename = name + "";
				if (filename.Length == 0) { filename = System.DateTime.Now.ToString("yyyyMMddHHmmssffffff"); }
				if (!filename.Contains(".")) { filename += ".txt"; }
				filename = System.Web.HttpContext.Current.Server.MapPath(filename);
				System.IO.File.AppendAllText(filename, content + "\n\n", System.Text.Encoding.UTF8);
			}
			return null;
		}
	}
}