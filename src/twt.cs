using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using s7.twt.Notifications;
using Twitterizer;
using System.Windows.Forms;
using System.Drawing;

namespace s7.twt
{
	public static class twt
	{
		public static readonly string ConsumerKey = "duNqYCc4BeGTwlQpbsZJGQ";
		public static readonly string ConsumerSecret = "UaQiKarT4ZZE0VlETRUyzrIymuFr7erArbSD5orb0N8";
				
		static void Main(string[] args)
		{
			Growler.Initialize();
			bool quiet = false;
			try
			{
				if (args.Contains("-q") || args.Contains("-quiet"))
					quiet = true;
				if (args.Contains("-config"))
				{
					OAuthTokenResponse requestToken = OAuthUtility.GetRequestToken(ConsumerKey, ConsumerSecret, "oob");
					Uri authorizationUri = OAuthUtility.BuildAuthorizationUri(requestToken.Token);
					System.Diagnostics.Process.Start(authorizationUri.AbsoluteUri);
					string verifier = String.Empty;
					if (InputBox("Enter authorization pin for twt", "Please enter the PIN number from the opened webpage to authorize twt:", ref verifier) == DialogResult.OK)
					{
						OAuthTokenResponse accessToken = OAuthUtility.GetAccessToken(ConsumerKey, ConsumerSecret, requestToken.Token, verifier);
						Cfg.cmDoConfig cfg = new Cfg.cmDoConfig() { AccessToken = accessToken, QuietMode = false };
						cfg.Save();
					}
					
					if (!Growler.Growl(Growler.GeneralNotification, "Configuration Complete", "The configuration for twt is now set."))
						System.Windows.Forms.MessageBox.Show("The configuration for twt is now set.", "Configuration Complete");
				}
				else
				{
					if (!Cfg.cmDoConfig.Exists)
						throw new System.IO.FileNotFoundException("Could not find the configuration file with authentication information.\nPlease run twt with the -config argument.",Cfg.cmDoConfig.ConfigPath);
					Cfg.cmDoConfig cfg = Cfg.cmDoConfig.Load();
					
					if(args.Length > 2 && args[0].ToLower() == "-set")
					{
						switch(args[1].ToLower())
						{
							case "quiet":
								bool quietparam;
								if (bool.TryParse(args[2], out quietparam))
								{
									cfg.QuietMode = quietparam;
									cfg.Save(); 
									if (!Growler.Growl(Growler.GeneralNotification, "Settings", "Quiet mode is now " + (quietparam ? "on" : "off") + "."))
										System.Windows.Forms.MessageBox.Show("Quiet mode is now " + (quietparam ? "on" : "off") + ".", "twt Settings");
								}
								else
								{
									throw new Exception("'-set quiet' expects 'true' or 'false' as a parameter.");
								}
								break;
						}
						return;
					}


					if (!quiet)
						quiet = cfg.QuietMode;
					
					string tweet = string.Empty;
					foreach (string word in args)
					{
						if (word.StartsWith("-") && string.IsNullOrEmpty(tweet))
							continue;
						tweet = string.Concat(tweet, word, " ");
					}
					tweet = tweet.Trim();
					
					OAuthTokens token = new OAuthTokens() {
						AccessToken = cfg.AccessToken.Token,
						AccessTokenSecret = cfg.AccessToken.TokenSecret,
						ConsumerKey = ConsumerKey,
						ConsumerSecret = ConsumerSecret
					};
					
					TwitterResponse<TwitterStatus> response = TwitterStatus.Update(token,tweet);
					if(!quiet) 
					{
						if (response.Result == RequestResult.Success)
						{
							if (!Growler.Growl(Growler.SuccessNotification, tweet))
								System.Windows.Forms.MessageBox.Show("Tweeted!",tweet);
						}
						else
						{
							if (!Growler.Growl(Growler.ErrorNotification, "Tweet not posted: "+response.ErrorMessage))
								System.Windows.Forms.MessageBox.Show("Tweet was not posted.",response.ErrorMessage);
						}
					}
				}
			}
			catch (Exception e)
			{
				if(!quiet)
					if (!Growler.Growl(Growler.ErrorNotification, e.Message))
						System.Windows.Forms.MessageBox.Show(e.Message,"Error");
			}
		}
		
		public static DialogResult InputBox(string title, string promptText, ref string value)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 107);
			form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}
	}
}
