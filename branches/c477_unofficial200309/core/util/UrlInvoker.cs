using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace freetrain.util
{
	/// <summary>
	/// UrlInvoker �̊T�v�̐����ł��B
	/// </summary>
	public class UrlInvoker
	{
		// �w���URL��W���u���E�U�ŊJ��
		static public void openUrl(String targetUrl) {�@
			ProcessStartInfo info = new ProcessStartInfo();
			// URL�Ɋ֘A�Â���ꂽ�A�v���P�[�V������T��
			RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command");
			String val = rkey.GetValue("").ToString();
			// ���W�X�g���l�ɂ́A�N���p�����[�^���܂܂��̂ŁA
			// ���s�t�@�C�����ƋN���p�����[�^�𕪗�����
			if(val.StartsWith("\"")) {
				int n = val.IndexOf("\"",1);
				info.FileName = val.Substring(1,n-1);
				info.Arguments = val.Substring(n+1);
			}
			else {
				string[] a = val.Split(new char[]{' '});
				info.FileName = a[0];
				info.Arguments = val.Substring(a[0].Length+1);
			}
			// ��ƃf�B���N�g�����w�肵�Ȃ��ƃ_���Ȃ悤���E�E�E
			info.WorkingDirectory = Path.GetDirectoryName(info.FileName);
			// �����̍Ō��URL��������
			info.Arguments += targetUrl;
			Process.Start(info);
		}
	
	}
}
