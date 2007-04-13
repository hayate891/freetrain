using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace nft.framework.drawing
{
	/// <summary>
	/// DrawableControl 位置付けは抽象クラスにすべきだが、
	/// 抽象クラスではフォームデザイナが使えなくなるので、abstractはつけない。
	/// 実装ライブラリはこのクラスを継承してSurfaceフィールドを適切に実装する。
	/// </summary>
	public class DrawableControl : System.Windows.Forms.UserControl
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DrawableControl()
		{
			// この呼び出しは、Windows.Forms フォーム デザイナで必要です。
			InitializeComponent();

			// TODO: InitializeComponent 呼び出しの後に初期化処理を追加します。

		}

		public virtual ISurface Surface { get{ throw new InvalidOperationException("The field of this class is abstract and does not implemented."); } }

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region コンポーネント デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
