using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.Email
{
	/// <summary>
	/// Settings for sending e-mails.
	/// </summary>
	[Serializable]
	public class EmailSettings
	{
		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="smtpServerName">
		/// The name of the SMTP server.
		/// </param>
		/// <param name="smtpServerPort">
		/// The port of the SMTP server.
		/// </param>
		/// <param name="userName">
		/// The user name for the SMTP server.
		/// </param>
		/// <param name="password">
		/// The password for the SMTP server.
		/// </param>
		/// <param name="defaultSenderAddress">
		/// The default sender e-mail address.
		/// </param>
		/// <param name="defaultSenderDisplayName">
		/// The display name of the default sender.
		/// </param>
		/// <param name="useSSL">
		/// Use SSL if true.
		/// </param>
		public EmailSettings(
			string smtpServerName,
			int smtpServerPort,
			string userName,
			string password,
			string defaultSenderAddress,
			string defaultSenderDisplayName,
			bool useSSL)
		{
			if (smtpServerName == null) throw new ArgumentNullException(nameof(smtpServerName));
			if (userName == null) throw new ArgumentNullException(nameof(userName));
			if (password == null) throw new ArgumentNullException(nameof(password));
			if (defaultSenderAddress == null) throw new ArgumentNullException(nameof(defaultSenderAddress));

			this.SmtpServerName = smtpServerName;
			this.SmtpServerPort = smtpServerPort;
			this.UserName = userName;
			this.Password = password;
			this.DefaultSenderAddress = defaultSenderAddress;
			this.DefaultSenderDisplayName = defaultSenderDisplayName;
			this.UseSSL = useSSL;
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The name of the SMTP server.
		/// </summary>
		public string SmtpServerName { get; }

		/// <summary>
		/// The port of the SMTP server.
		/// </summary>
		public int SmtpServerPort { get; }

		/// <summary>
		/// The user name for the SMTP server.
		/// </summary>
		public string UserName { get; }

		/// <summary>
		/// The password for the SMTP server.
		/// </summary>
		public string Password { get; }

		/// <summary>
		/// The default sender e-mail address.
		/// </summary>
		public string DefaultSenderAddress { get; }

		/// <summary>
		/// The display name of the default sender.
		/// </summary>
		public string DefaultSenderDisplayName { get; }

		/// <summary>
		/// Use SSL if true.
		/// </summary>
		public bool UseSSL { get; }

		#endregion
	}
}
