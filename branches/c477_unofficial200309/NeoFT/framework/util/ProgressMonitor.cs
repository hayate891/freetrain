using System;
using System.Diagnostics;

namespace nft.util
{
	/// <summary>
	/// Function that receives a progress in initialization, termination, and so on.
	/// </summary>
	public delegate void ProgressHandler( int level, int percentage ,string msg );

	/// <summary>
	/// ProgressMonitor manages progress of long process.
	/// UI can catch event with ProgressHandler on progress.
	/// </summary>
	public class ProgressMonitor
	{
		public ProgressHandler OnProgress = new ProgressHandler(silentProgressHandler);
		private ProgressCounter[] counters;
		private ProgressMonitor attached;
		private int attachLevel;

		public ProgressMonitor(int nestLevel)
		{
			counters = new ProgressCounter[nestLevel];
			for(int i=0; i<nestLevel; i++)
				counters[i] = new ProgressCounter(this,i+1,100);
		}

		public ProgressMonitor(int nestLevel, ProgressHandler handler)
			:this(nestLevel)
		{
			this.OnProgress += handler;
		}

		/// <summary>
		/// Attach another <code>ProgressMonitor</code> as child.
		/// All OnProgress event from <code>source</code> are relayed to this monitor.
		/// </summary>
		/// <param name="attachLevel"></param>
		/// <param name="source"></param>
		
		public void Attach( int attachLevel, ProgressMonitor source )
		{
			if( attached!=null )
				Detach(attached);
			attached = source;
			this.attachLevel = attachLevel -1;	
			source.OnProgress += new ProgressHandler(this.AttachListener);
		}

		/// <summary>
		/// Detach another <code>ProgressMonitor</code>.
		/// </summary>
		/// <param name="source"></param>
		public void Detach( ProgressMonitor source )
		{
			OnProgress -= new ProgressHandler(this.AttachListener);
		}

		#region lapper methods
		public void SetMaximum(int level, int max)
		{
			this[level].maximum = max;
		}

		public void ShowMessage(int level, string msg )
		{
			this[level].message = msg;
		}

		public void Progress(int level, int count, string msg )
		{
			this[level].Progress(count,msg);
		}

		public void Progress(int level, int count )
		{
			this[level].counter += count;
		}

		public void Progress(int level)
		{
			this[level].counter ++;
		}

		#endregion

		public ProgressCounter this[int level]
		{
			get
			{
				if( level>counters.Length )
					level = counters.Length;
				return counters[level-1];
			}
		}

		// delegate called from attached monitor
		private void AttachListener( int level, int percentage ,string msg )
		{
			OnProgress(attachLevel+level,percentage,msg);
		}

		private static void silentProgressHandler( int level, int percentage ,string msg ) 
		{
			if(msg!=null)
				Trace.WriteLine(msg);
		}
	}

	public class ProgressCounter
	{
		private int max;
		private int current;
		private string text="";
		private int level;
		internal ProgressMonitor owner;

		internal ProgressCounter(ProgressMonitor owner, int level, int max)
		{
			this.owner = owner;
			this.level = level;
			this.max = max;
			current=0;	
		}

		#region public properties (setter/getter).
		public int counter
		{
			get{ return current; }
			set
			{
				current = value;
				if(max<current) current = max;
				owner.OnProgress(level,percentage,null);
			}
		}

		/// <summary>
		/// set/get counter maximum.
		/// counter is reset to zero if maximum cahnged.
		/// </summary>
		public int maximum
		{
			get{ return max; }
			set
			{
				max = value;
				current = 0;
				owner.OnProgress(level,0,null);
			}
		}

		public string message
		{
			get{ return text; }
			set
			{
				text = value;
				owner.OnProgress(level,percentage,text);
			}
		}
		#endregion

		public int percentage
		{
			get{ return current*100/max; }
		}

		public void Progress(int count, string msg)
		{
			text = msg;
			current += count;
			if(max<current) current = max;
			owner.OnProgress(level,percentage,text);
		}
	}
}
