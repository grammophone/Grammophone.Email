using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.Email
{
	/// <summary>
	/// Exception thrown while sending e-mail via <see cref="EmailClient"/>.
	/// </summary>
	[Serializable]
	public class EmailException : SystemException
	{
		#region Construction

		/// <summary>
		/// Create with a default exception message.
		/// </summary>
		/// <param name="mailMessage">The e-mail message which was attempted to send.</param>
		/// <param name="inner">The underlying exception occured during sending.</param>
		public EmailException(MailMessage mailMessage, Exception inner)
			: this(mailMessage, GetDefaultExceptionMessage(mailMessage), inner)
		{
		}

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="mailMessage">The e-mail message which was attempted to send.</param>
		/// <param name="message">The exception message.</param>
		/// <param name="inner">The underlying exception occured during sending.</param>
		public EmailException(MailMessage mailMessage, string message, Exception inner)
			: base(message, inner)
		{
			this.Body = mailMessage.Body;
			this.IsBodyHTML = mailMessage.IsBodyHtml;
			this.RecepientAddresses = mailMessage.To.Select(a => a.Address).ToArray();
			this.CarbonCopyRecepientAddresses = mailMessage.CC.Select(a => a.Address).ToArray();
			this.BlindCarbonCopyRecepientAddresses = mailMessage.Bcc.Select(a => a.Address).ToArray();
			this.Subject = mailMessage.Subject;
		}

		/// <summary>
		/// Used for serialization.
		/// </summary>
		protected EmailException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		#endregion

		#region Public properties

		/// <summary>
		/// The body of the e-mail message.
		/// </summary>
		public string Body { get; private set; }

		/// <summary>
		/// If true, the <see cref="Body"/> is in HTML format.
		/// </summary>
		public bool IsBodyHTML { get; private set; }

		/// <summary>
		/// The adress of the sender.
		/// </summary>
		public string SenderAddress { get; private set; }

		/// <summary>
		/// The addresses of the recepients.
		/// </summary>
		public IReadOnlyCollection<string> RecepientAddresses { get; private set; }

		/// <summary>
		/// The addresses of the CC recepients.
		/// </summary>
		public IReadOnlyCollection<string> CarbonCopyRecepientAddresses { get; private set; }

		/// <summary>
		/// The addresses of the BCC recepients.
		/// </summary>
		public IReadOnlyCollection<string> BlindCarbonCopyRecepientAddresses { get; private set; }

		/// <summary>
		/// The subject of the e-mail message.
		/// </summary>
		public string Subject { get; private set; }

		#endregion

		#region Public methods

		/// <summary>
		/// Deserialize the exception.
		/// </summary>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			this.Body = info.GetString(nameof(this.Body));
			this.IsBodyHTML = info.GetBoolean(nameof(this.IsBodyHTML));
			this.SenderAddress = info.GetString(nameof(this.SenderAddress));
			this.RecepientAddresses = (string[])info.GetValue(nameof(this.RecepientAddresses), typeof(string[]));
			this.CarbonCopyRecepientAddresses = (string[])info.GetValue(nameof(this.CarbonCopyRecepientAddresses), typeof(string[]));
			this.BlindCarbonCopyRecepientAddresses = (string[])info.GetValue(nameof(this.BlindCarbonCopyRecepientAddresses), typeof(string[]));
			this.Subject = info.GetString(nameof(this.Subject));
		}

		#endregion

		#region Private methods

		private static string GetDefaultExceptionMessage(MailMessage mailMessage)
		{
			if (mailMessage == null) throw new ArgumentNullException(nameof(mailMessage));

			var messageBuilder = new StringBuilder();

			messageBuilder.Append($"Failed to send message from {mailMessage.Sender.Address} to ");

			foreach (var recepient in mailMessage.To)
			{
				messageBuilder.Append($"{recepient.Address}, ");
			}

			if (mailMessage.CC.Any()) messageBuilder.Append("CC ");

			foreach (var recepient in mailMessage.CC)
			{
				messageBuilder.Append($"{recepient.Address}, ");
			}

			if (mailMessage.Bcc.Any()) messageBuilder.Append("BCC ");

			foreach (var recepient in mailMessage.Bcc)
			{
				messageBuilder.Append($"{recepient.Address}, ");
			}

			messageBuilder.Append($"subject '{mailMessage.Subject}'");

			return messageBuilder.ToString();
		}

		#endregion
	}
}
