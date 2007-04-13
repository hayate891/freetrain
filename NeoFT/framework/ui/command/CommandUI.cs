using System;
using System.Collections;
using nft.util;
using nft.framework;

namespace nft.ui.command
{
	/// <summary>
	/// the interface that the module you want to be called should be implement.
	/// </summary>
	public interface ICommandEntity
	{
		void CommandExecuted( CommandUI cmdUI,object sender );
	}

	/// <summary>
	/// CommandUI manages state of all related GUI controls.
	/// Set EventHandler of related controls to call ICommandEntity.
	/// </summary>
	public class CommandUI
	{
		static private Hashtable table = new Hashtable();
		static public CommandUI GetCommandUI(string id)
		{
			return (CommandUI)table[id];
		}

		public readonly string ID;
		protected bool _checked;
		protected bool enabled;
		protected object optionState = null;
		protected ICommandEntity entity;
		protected ArrayList triggers = new ArrayList();

		public CommandUI(string id, ICommandEntity _entity, bool _checked, bool _enabled )
		{
			if(table.ContainsKey(id))
				throw new InvalidOperationException("Duplicate command ID");
			this.ID = id;
			this.entity = _entity;
			this._checked = _checked;
			enabled = _enabled;
			table.Add(id,this);
		}

		public CommandUI(string id, ICommandEntity _entity)
			: this( id, _entity, false, true )
		{
		}

		
		#region getters / setters
		public bool Checked
		{
			get{ return _checked; }
			set
			{
				_checked = value;
				foreach(ICommandTrigger trigger in triggers )
					trigger.Checked = _checked;
			}
		}
		public bool Enabled
		{
			get{ return enabled; }
			set
			{
				enabled = value;
				foreach(ICommandTrigger trigger in triggers )
					trigger.Enabled = enabled;
			}
		}
		public object OptionState
		{
			get{ return optionState; }
			set
			{
				optionState = value;
				foreach(ICommandTrigger trigger in triggers )
					trigger.OptionStateChanged(optionState);
			}
		}
		#endregion

		internal protected void Update()
		{
			foreach(ICommandTrigger trigger in triggers )
			{
				trigger.Checked = _checked;
				trigger.Enabled = enabled;
			}
		}
		
		internal protected void AddTrigger( ICommandTrigger trigger )
		{
			if( triggers.Contains( trigger ) ) return;
			triggers.Add(trigger);
			trigger.AddEventHandler(new EventHandler(TriggerEventHandler));
		}

		internal protected void RemoveTrigger( ICommandTrigger trigger )
		{
			trigger.RemoveEventHandler(new EventHandler(TriggerEventHandler));
			triggers.Remove(trigger);
		}

		protected void TriggerEventHandler(object sender, EventArgs args)
		{
			try
			{
				entity.CommandExecuted(this,sender);
			}
			catch(Exception e)
			{
				string templ = Main.resources["command.exception@execute"].stringValue;
				string msg = string.Format(templ,new string[]{ID});
				UIUtil.ShowException(msg,e,UIInformLevel.normal);
			}
		}
	}


}
