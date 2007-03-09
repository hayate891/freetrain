using System;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
using freetrain.contributions.rail;
using freetrain.framework.plugin;

namespace freetrain.world.rail.tattc
{
	/// <summary>
	/// TATTrainControllerPlugIn �̊T�v�̐����ł��B
	/// </summary>
	[Serializable]
	public class TATTrainControllerPlugIn : TrainControllerContribution
	{
		public TATTrainControllerPlugIn( XmlElement e ) : base(e) {
			theInstance = this;
		}

		internal static TATTrainControllerPlugIn theInstance;

		public override string name { get { return "�u�`��Ԃōs�����v���_�C���O����"; } }

		public override string description {
			get {
				return "�e�w�̔��Ԏ����Ɗe�|�C���g�ł̐i�s������ݒ肷�邱�Ƃɂ���ă_�C����ݒ肵�܂�";
			}
		}

		public override TrainController newController( string name ) {
			return new TATTrainController(name);
		}
	}
}
