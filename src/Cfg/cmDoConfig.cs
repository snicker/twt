using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Twitterizer;
using System.ComponentModel;

namespace s7.twt.Cfg
{
	[Serializable]
	public class cmDoConfig
	{
		private const string m_CfgFile = "twtcfg.xml";
		public static string ConfigPath { get { return Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), m_CfgFile); } }
		public static bool Exists { get { return File.Exists(ConfigPath); } }

		public OAuthTokenResponse AccessToken { get; set; }

		[DefaultValue(false)]
		public bool QuietMode { get; set; }

		public void Save()
		{
			System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(this.GetType());
			StreamWriter writer = File.CreateText(ConfigPath);
			xs.Serialize(writer, this);
			writer.Flush();
			writer.Close();
		}

		public static cmDoConfig Load()
		{
			if (Exists)
			{
				System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(cmDoConfig));
				StreamReader reader = File.OpenText(ConfigPath);
				cmDoConfig c = xs.Deserialize(reader) as cmDoConfig;
				reader.Close();
				return c == null ? new cmDoConfig() : c;
			}
			else
			{
				return new cmDoConfig();
			}
		}
	}
}
