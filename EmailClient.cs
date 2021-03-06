﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Grammophone.Email
{
	/// <summary>
	/// Client for sending e-mails.
	/// </summary>
	public class EmailClient : IDisposable
	{
		#region Private fields

		private EmailSettings settings;

		private SmtpClient smtpClient;

		#endregion

		#region Construction

		/// <summary>
		/// Create.
		/// </summary>
		/// <param name="settings">The settings to use.</param>
		public EmailClient(EmailSettings settings)
		{
			if (settings == null) throw new ArgumentNullException(nameof(settings));

			this.settings = settings;

			smtpClient = new SmtpClient(settings.SmtpServerName, settings.SmtpServerPort);

			smtpClient.EnableSsl = settings.UseSSL;

			if (settings.UserName != null || settings.Password != null)
			{
				smtpClient.UseDefaultCredentials = false;

				smtpClient.Credentials = new System.Net.NetworkCredential
				{
					UserName = settings.UserName,
					Password = settings.Password
				};
			}
			else
			{
				smtpClient.UseDefaultCredentials = true;
			}
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Send an e-mail.
		/// </summary>
		/// <param name="recepients">A list of recepients separated by comma or semicolon.</param>
		/// <param name="subject">The subject of the message.</param>
		/// <param name="body">The body of the message.</param>
		/// <param name="isBodyHTML">If true, the format of the body message is HTML.</param>
		/// <param name="sender">
		/// The sender of the message, is specified, else 
		/// the configured <see cref="EmailSettings.DefaultSenderAddress"/> is used.
		/// </param>
		/// <returns>Returns a task completing the action.</returns>
		/// <exception cref="EmailException">
		/// Thrown when there is an error sending the e-mail.
		/// </exception>
		/// <remarks>
		/// The message's subject, headers and body encoding is set to UTF8.
		/// </remarks>
		public async Task SendEmailAsync(
			string recepients,
			string subject,
			string body,
			bool isBodyHTML,
			string sender = null)
		{
			if (recepients == null) throw new ArgumentNullException(nameof(recepients));
			if (subject == null) throw new ArgumentNullException(nameof(subject));
			if (body == null) throw new ArgumentNullException(nameof(body));

			var mailMessage = new MailMessage
			{
				Sender = new MailAddress(sender ?? settings.DefaultSenderAddress),
				From = new MailAddress(sender ?? settings.DefaultSenderAddress),
				Subject = subject,
				Body = body,
				IsBodyHtml = isBodyHTML,
				BodyEncoding = Encoding.UTF8,
				SubjectEncoding = Encoding.UTF8,
				HeadersEncoding = Encoding.UTF8,
			};

			using (mailMessage)
			{
				foreach (string recepient in recepients.Split(';', ','))
				{
					mailMessage.To.Add(recepient.Trim());
				}

				try
				{
					await SendEmailAsync(mailMessage);
				}
				catch (Exception ex)
				{
					throw new EmailException(mailMessage, ex);
				}
			}
		}

		/// <summary>
		/// Send an e-mail.
		/// </summary>
		/// <param name="mailMessage">
		/// The message to send. If its Sender property is not set, 
		/// the configured <see cref="EmailSettings.DefaultSenderAddress"/> is used.
		/// </param>
		/// <returns>Returns a task completing the action.</returns>
		/// <exception cref="EmailException">
		/// Thrown when there is an error sending the e-mail.
		/// </exception>
		public async Task SendEmailAsync(MailMessage mailMessage)
		{
			if (mailMessage == null) throw new ArgumentNullException(nameof(mailMessage));

			if (mailMessage.Sender == null)
			{
				mailMessage.Sender = new MailAddress(settings.DefaultSenderAddress, settings.DefaultSenderDisplayName);
			}

			if (mailMessage.From == null)
			{
				mailMessage.From = new MailAddress(settings.DefaultSenderAddress, settings.DefaultSenderDisplayName);
			}

			try
			{
				await smtpClient.SendMailAsync(mailMessage);
			}
			catch (Exception ex)
			{
				throw new EmailException(mailMessage, ex);
			}
		}

		/// <summary>
		/// Clean up the client.
		/// </summary>
		public void Dispose()
		{
			smtpClient.Dispose();
		}

		#endregion
	}
}
