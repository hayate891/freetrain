using System;
using System.Drawing;
using System.Windows.Forms;
using UtilityLibrary.CommandBars;

namespace nft.ui.command
{
	/// <summary>
	/// interface wich can trigger Command
	/// </summary>
	public interface ICommandTrigger
	{
		bool Checked{ set; get; }
		bool Enabled{ set; get; }
		void OptionStateChanged( object newObj );
		void AddEventHandler( EventHandler handler );
		void RemoveEventHandler( EventHandler handler );
	}

	#region ICommandTrigger for MenuItem
	internal class MenuItemTrigger : ICommandTrigger
	{
		private readonly MenuItem item;
		
		public MenuItemTrigger( MenuItem item )
		{
			this.item = item;
		}

		public bool Checked
		{ 
			set{ item.Checked = value; }
			get{ return item.Checked; } 
		}
		public bool Enabled
		{ 
			set{ item.Enabled = value; }
			get{ return item.Enabled; } 
		}
		public void AddEventHandler( EventHandler handler )
		{
			item.Click += handler;
		}
		public void RemoveEventHandler( EventHandler handler )
		{
			item.Click -= handler;
		}
		
		public virtual void OptionStateChanged( object newObj ){}

	}
	#endregion

	#region ICommandTrigger for Button
	internal class ButtonTrigger : ControlTrigger
	{
		private Button item
		{
			get{ return (Button)base.ctrl; }
		}

		public ButtonTrigger( Button item )
			: base(item)
		{
		}
	}
	#endregion

	#region ICommandTrigger for CheckBox
	internal class CheckBoxTrigger : ControlTrigger
	{
		private CheckBox item
		{
			get{ return (CheckBox)base.ctrl; }
		}
		
		public CheckBoxTrigger( CheckBox item )
			:base(item)
		{
		}

		public override bool Checked
		{ 
			set{ item.Checked = value; }
			get{ return item.Checked; } 
		}
		
		public override void OptionStateChanged( object newObj ){}

	}
	#endregion

	#region ICommandTrigger for ToolBarItem (UtilityLibrary)
	internal class ToolBarItemTrigger : ICommandTrigger
	{
		private readonly ToolBarItem item;
		
		public ToolBarItemTrigger( ToolBarItem item )
		{
			this.item = item;
		}

		public bool Checked{ 
			set{ item.Checked = value; }
			get{ return item.Checked; } 
		}
		public bool Enabled
		{ 
			set{ item.Enabled = value; }
			get{ return item.Enabled; } 
		}
		public void AddEventHandler( EventHandler handler )
		{
			item.Click += handler;
		}
		public void RemoveEventHandler( EventHandler handler )
		{
			item.Click -= handler;
		}
		
		public virtual void OptionStateChanged( object newObj )
		{
			if( newObj is int )
				item.ImageListIndex = (int)newObj;
		}

	}
	#endregion

	#region ICommandTrigger for ToolBarButton (Window.Forms)
	internal class ToolBarButtonTrigger : ICommandTrigger
	{
		private readonly ToolBarButton item;

		public ToolBarButtonTrigger( ToolBarButton item )
		{
			this.item = item;
			item.Parent.ButtonClick+=new ToolBarButtonClickEventHandler(toolbar_ButtonClick);
		}

		public bool Checked
		{ 
			set{ item.Pushed = value; }
			get{ return item.Pushed; } 
		}
		public bool Enabled
		{ 
			set{ item.Enabled = value; }
			get{ return item.Enabled; } 
		}

		private EventHandler OnClick;
		public void AddEventHandler( EventHandler handler )
		{
			OnClick += handler;
		}
		public void RemoveEventHandler( EventHandler handler )
		{
			OnClick -= handler;
		}
		
		public virtual void OptionStateChanged( object newObj )
		{
			if( newObj is int )
				item.ImageIndex = (int)newObj;
		}

		// Execution event handler
		private void toolbar_ButtonClick( object sender, ToolBarButtonClickEventArgs args)
		{			
			if(args.Button != item ) return;
			if(OnClick != null )
				OnClick(sender,args);
		}			
	}
	#endregion

	#region ICommandTrigger for other Control
	internal class ControlTrigger : ICommandTrigger
	{
		protected readonly Control ctrl;
		
		public ControlTrigger( Control item )
		{
			this.ctrl = item;
		}

		public virtual bool Checked
		{ 
			set{}
			get{ return false; } 
		}
		public bool Enabled
		{ 
			set{ ctrl.Enabled = value; }
			get{ return ctrl.Enabled; } 
		}
		public virtual void AddEventHandler( EventHandler handler )
		{
			ctrl.Click += handler;
		}
		public virtual void RemoveEventHandler( EventHandler handler )
		{
			ctrl.Click -= handler;
		}
		/// <summary>
		/// set newObj.ToString as Control.Text property
		/// </summary>
		/// <param name="newObj"></param>
		public virtual void OptionStateChanged( object newObj )
		{
			ctrl.Text = newObj.ToString();
		}

	}
	#endregion

}
