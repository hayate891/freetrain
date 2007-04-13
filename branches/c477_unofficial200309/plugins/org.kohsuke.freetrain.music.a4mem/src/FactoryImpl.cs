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
					create("BLS.mid","���̃e�[�}"),
					create("FLK.mid","�k�J"),
					create("FNK.mid","�W�I�t�����g"),
					create("FOG.mid","���J�e�[�}"),
					create("OLD.mid","���K�e�[�}"),
					create("RGG.mid","�ԑt��"),
					create("WHT.mid","�n�C�e�N�s�s"),
					create("OP_RIN.mid","�J�̃I�[�v�j���O"),
					create("OP_SNW.mid","��̃I�[�v�j���O")
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
				return "�`�S�������A���p�b�N";
			}
		}
	}
}
