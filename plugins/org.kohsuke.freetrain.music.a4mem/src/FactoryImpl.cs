using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Microsoft.Win32;
using freetrain.contributions.common;
using freetrain.contributions.sound;
using freetrain.framework.plugin;

namespace freetrain.plugin.a4membgm
{
	[Serializable]
	public class FactoryImpl : BGMFactoryContribution
	{
		public FactoryImpl( XmlElement e ) : base(e) {}

		string a4path;

		public override BGMContribution[] listContributions() {
			try {
				a4path = (string)Registry.LocalMachine.OpenSubKey("SOFTWARE").
					OpenSubKey("ARTDINK").OpenSubKey("ATrainM").OpenSubKey("a4").GetValue("InstallPath");

				a4path = Path.Combine(a4path,@"..\res");

				return new BGMContribution[]{
					create("BLS.mid","情報のテーマ"),
					create("FLK.mid","渓谷"),
					create("FNK.mid","ジオフロント"),
					create("FOG.mid","霧雨テーマ"),
					create("OLD.mid","練習テーマ"),
					create("RGG.mid","間奏曲"),
					create("WHT.mid","ハイテク都市"),
					create("OP_RIN.mid","雨のオープニング"),
					create("OP_SNW.mid","雪のオープニング")
				};
			} catch( Exception e ) {
				Debug.WriteLine("A4m is not installed");
				Debug.Write(e);
				return new BGMContribution[0];
			}
		}

		private BGMContribution create( string fileName, string title ) {
			return new BGMContribution( "(A4) "+title, Path.Combine(a4path,fileName), this.id+"-"+fileName );
		}

		public override string title {
			get {
				return "Ａ４メモリアルパック";
			}
		}
	}
}
