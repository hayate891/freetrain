﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using freetrain.world;
using freetrain.world.accounting;

namespace freetrain.views.bank
{
	/// <summary>
	/// BankCounterForm の概要の説明です。
	/// </summary>
	public class BankbookWindow : Form
	{
		#region generated by form editor
		private System.Windows.Forms.CheckBox cb_status;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage pgLoan;
		private System.Windows.Forms.TabPage pgDeposit;
		private System.Windows.Forms.ListView list_loan;
		private System.Windows.Forms.ListView list_cancel;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		#endregion
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label tl_status;

		private AccountManager manager { get { return World.world.account; } }
		private Clock clock { get { return World.world.clock; } }
		static private BankModule bank { get { return BankModule.theInstance; } }
		private BankBusinessHourListener bankListener;

		public BankbookWindow()
		{
			InitializeComponent();

			if( BankConfig.canBorrow )
				BankbookListHelper.buildDebtList(list_loan);
			else
				tabControl1.Controls.Remove(pgLoan);
			
			if( BankConfig.canDeposit )
				BankbookListHelper.buildDepositList(list_cancel);
			else
				tabControl1.Controls.Remove(pgDeposit);
			
			onBankStatusChanged();
			
			bankListener =  new BankBusinessHourListener(onBankStatusChanged);
			bank.onBusinesStatusChanging += bankListener;
		}
		
		private void onBankStatusChanged()
		{
			switch( BankModule.theInstance.status ) 
			{
				case BankStatus.HOLIDAY:
					tl_status.Text = "Holiday";
					//! tl_status.Text = "休日";
					break;
				case BankStatus.OPEN:
					tl_status.Text = "Open";
					//! tl_status.Text = "営業";
					break;
				case BankStatus.CLOSE:
					tl_status.Text = "Closed";
					//! tl_status.Text = "閉店";
					break;
			}
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			bank.onBusinesStatusChanging -= bankListener;
			
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			BankbookListHelper.RemoveList(list_cancel);
			BankbookListHelper.RemoveList(list_loan);
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.cb_status = new System.Windows.Forms.CheckBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.pgLoan = new System.Windows.Forms.TabPage();
			this.list_loan = new System.Windows.Forms.ListView();
			this.pgDeposit = new System.Windows.Forms.TabPage();
			this.list_cancel = new System.Windows.Forms.ListView();
			this.tl_status = new System.Windows.Forms.Label();
			this.tabControl1.SuspendLayout();
			this.pgLoan.SuspendLayout();
			this.pgDeposit.SuspendLayout();
			this.SuspendLayout();
			// 
			// cb_status
			// 
			this.cb_status.AccessibleDescription = "Bring up the teller";
			//! this.cb_status.AccessibleDescription = "窓口呼び出し";
			this.cb_status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cb_status.Appearance = System.Windows.Forms.Appearance.Button;
			this.cb_status.BackColor = System.Drawing.SystemColors.Control;
			this.cb_status.Location = new System.Drawing.Point(373, 1);
			this.cb_status.Name = "cb_status";
			this.cb_status.Size = new System.Drawing.Size(95, 23);
			this.cb_status.TabIndex = 2;
			this.cb_status.Text = "Bank teller";
			//! this.cb_status.Text = "銀行窓口";
			this.cb_status.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.cb_status.UseVisualStyleBackColor = false;
			this.cb_status.CheckedChanged += new System.EventHandler(this.cb_status_CheckedChanged);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				| System.Windows.Forms.AnchorStyles.Left)
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.pgLoan);
			this.tabControl1.Controls.Add(this.pgDeposit);
			this.tabControl1.Location = new System.Drawing.Point(0, 7);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(472, 120);
			this.tabControl1.TabIndex = 3;
			// 
			// pgLoan
			// 
			this.pgLoan.Controls.Add(this.list_loan);
			this.pgLoan.Location = new System.Drawing.Point(4, 22);
			this.pgLoan.Name = "pgLoan";
			this.pgLoan.Size = new System.Drawing.Size(464, 94);
			this.pgLoan.TabIndex = 0;
			this.pgLoan.Text = "Loan";
			//! this.pgLoan.Text = "借入金";
			// 
			// list_loan
			// 
			this.list_loan.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_loan.FullRowSelect = true;
			this.list_loan.GridLines = true;
			this.list_loan.HideSelection = false;
			this.list_loan.Location = new System.Drawing.Point(0, 0);
			this.list_loan.Name = "list_loan";
			this.list_loan.Size = new System.Drawing.Size(464, 94);
			this.list_loan.TabIndex = 10;
			this.list_loan.UseCompatibleStateImageBehavior = false;
			this.list_loan.View = System.Windows.Forms.View.Details;
			// 
			// pgDeposit
			// 
			this.pgDeposit.Controls.Add(this.list_cancel);
			this.pgDeposit.Location = new System.Drawing.Point(4, 22);
			this.pgDeposit.Name = "pgDeposit";
			this.pgDeposit.Size = new System.Drawing.Size(464, 104);
			this.pgDeposit.TabIndex = 1;
			this.pgDeposit.Text = "Time deposit";
			//! this.pgDeposit.Text = "定期預金";
			// 
			// list_cancel
			// 
			this.list_cancel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.list_cancel.FullRowSelect = true;
			this.list_cancel.GridLines = true;
			this.list_cancel.HideSelection = false;
			this.list_cancel.Location = new System.Drawing.Point(0, 0);
			this.list_cancel.Name = "list_cancel";
			this.list_cancel.Size = new System.Drawing.Size(464, 104);
			this.list_cancel.TabIndex = 27;
			this.list_cancel.UseCompatibleStateImageBehavior = false;
			this.list_cancel.View = System.Windows.Forms.View.Details;
			// 
			// tl_status
			// 
			this.tl_status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tl_status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.tl_status.Location = new System.Drawing.Point(304, 1);
			this.tl_status.Name = "tl_status";
			this.tl_status.Size = new System.Drawing.Size(63, 23);
			this.tl_status.TabIndex = 4;
			this.tl_status.Text = "Open";
			//! this.tl_status.Text = "営業";
			this.tl_status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// BankbookWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 125);
			this.Controls.Add(this.tl_status);
			this.Controls.Add(this.cb_status);
			this.Controls.Add(this.tabControl1);
			this.MinimumSize = new System.Drawing.Size(248, 113);
			this.Name = "BankbookWindow";
			this.Text = "Bankbook";
			//! this.Text = "負債と預金";
			this.tabControl1.ResumeLayout(false);
			this.pgLoan.ResumeLayout(false);
			this.pgDeposit.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void cb_status_CheckedChanged(object sender, System.EventArgs e)
		{
			if( cb_status.Checked ) 
			{
				BankCounterForm.ShowBankCounter();
				cb_status.Checked = false;
			}
		}

	}
}
