using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using nft.util;

namespace nft.framework.plugin
{
	/// <summary>
	/// Represents a loaded plug-in
	/// </summary>
	public class Plugin : IHasNameAndID, IAddable
	{
		#region IHasNameAndID ÉÅÉìÉo

		public string ID
		{
			get { return Path.GetFileName(dirName); }
		}

		public string Name
		{
			get	{ return _title; }
		}

		#endregion
		private readonly string _title;
		public string Title { get { return _title; } }

		private readonly string _author;
		public string author { get { return _author; } }

		private readonly string _homepage;
		public string homepage { get { return _homepage; } }

		private readonly DateTime _lastModified;
		public DateTime lastModifiedTime { get { return _lastModified; } }

		public string getInfoText(string linecode)
		{
			string templ = "Title:{0}"+linecode+"Author:{1}"+linecode+"HomePage:{2}";
			return string.Format(templ,Title,author,homepage);		
		}

		/// <summary>
		/// Base directory of this plug-in
		/// </summary>
		public readonly string dirName;

		/// <summary>
		/// All the contributions in this plug-in
		/// </summary>
		public readonly IList contributions = new ArrayList();

		/// <summary>
		/// All the contributions in this plug-in
		/// </summary>
		public readonly IList definers = new ArrayList();

		public int TotalContribCount { get { return contributions.Count+definers.Count; } }

		/// <summary>
		/// Contents of plugin.xml
		/// Available only during the initialization phase.
		/// </summary>
		private XmlDocument doc;


		#region IAddable ÉÅÉìÉo
		internal protected InstallationState _state = InstallationState.Uninitialized;
		[NonSerialized]
		internal protected AttachChangeEvent onDetach;
		[NonSerialized]
		internal protected AttachChangeEvent onAttach;

		private int detachCount = 0;
		private int totalDetachables = 0;
		private bool detachable = false;
		private bool attached = true;
		public InstallationState State { get{ return _state; } }

		public bool IsDetachable { get{ return detachable&(_state>=InstallationState.PartialError); } }

		public bool QueryDetach() {	return true; }

		public void Detach()
		{
			Debug.WriteLine("dt"+totalDetachables);
			foreach(Contribution c in contributions)
				if( c.QueryDetach() )
					c.Detach();
			if(onDetach!=null)
				onDetach(this);
			attached = false;
		}

		public void Attach()
		{
			Debug.WriteLine("at"+totalDetachables);
			foreach(Contribution c in contributions)
				c.Attach();			
			if(onAttach!=null)
				onAttach(this);
			attached = true;
		}

		public AttachChangeEvent OnDetach{ get{ return onDetach; } set{ onDetach = value; } }
		public AttachChangeEvent OnAttach{ get{ return onAttach; } set{ onAttach = value; } }
		public virtual bool IsAttached{ get{ return attached; } }
		public virtual bool IsPartiallyDetached{ get{ return detachCount!=0 && detachCount!=totalDetachables; } }

		void AttachChangeEventHandler( IAddable sender )
		{
			if(sender.IsAttached)
			{
				detachCount--;
				attached = true;
				if(onAttach!=null)
					onAttach(this);
			}
			else
			{
				detachCount++;		
				attached = (detachCount < totalDetachables);
				if(!attached && onDetach!=null)
					onDetach(this);
			}
		}
		#endregion

		/// <summary>
		/// Loads a plug-in from manifest XML "plugin.xml".
		/// </summary>
		public Plugin( string dirName ) 
		{
			this.dirName = dirName;

			doc = loadManifest(dirName);
			XmlElement root = doc.DocumentElement;
			_lastModified = lastUpdatedDate(dirName);
			_title =	XmlUtil.selectSingleNode(root,"title").InnerText;
			_homepage =	XmlUtil.getSingleNodeText(root,"homepage","N/A");
			_author =	XmlUtil.getSingleNodeText(root,"author","<unknown>");

		}

		/// <summary>
		/// Get all the dependent plug-ins.
		/// called from PluginManager before initialize this plugin.
		/// </summary>
		public Plugin[] getDependencies() {
			ArrayList a = new ArrayList();
			if( !this.ID.Equals("system") )
				a.Add( PluginManager.theInstance.GetPlugin("system") );

			foreach( XmlElement depend in doc.DocumentElement.SelectNodes("depend") ) {
				string name = depend.Attributes["on"].Value;
				Plugin p = PluginManager.theInstance.GetPlugin(name);
				if(p==null)
				{
					string templ = Main.resources["plugin.dependency_not_found"].stringValue;
					throw new Exception(String.Format(templ,this.ID,name));
				}
				a.Add(p);
			}
			return (Plugin[])a.ToArray(typeof(Plugin));
		}
		
		/// <summary>
		/// Loads plugin.xml file from the directory.
		/// </summary>
		private static XmlDocument loadManifest( string dirName ) {
			string path = Path.Combine(dirName,"plugin.xml");
			return XmlUtil.loadFile(path);
		}

		private static DateTime lastUpdatedDate( string dirName )
		{
			string path = Path.Combine(dirName,"plugin.xml");
			FileInfo info = new FileInfo(path);
			if(!info.Exists)
				return DateTime.Now;
			return info.LastWriteTime;
		}

		/// <summary>
		/// Loads class type contributions from this plug-in
		/// </summary>
		internal void loadBinaries() 
		{
			XmlElement root = doc.DocumentElement;
			// locate contribution factories first,
			// because we'll need them to load contributions.
			foreach( XmlElement contrib in root.SelectNodes("contribution") ) 
			{
				string type = contrib.Attributes["type"].Value;
				if(type.Equals("contribution")) 
				{
					try
					{
						PluginManager.theInstance.NotifyStartParse(this,contrib);
						// Register dummy contribution (in order to list on Dialog).
						CtbContributionDefiner cd = new CtbContributionDefiner(contrib);
						definers.Add(cd);
						cd.Attach();
						// load a contribution factory
						string contributionName = XmlUtil.selectSingleNode(contrib,"name").InnerText;
					
						ContributionFactory factory =
							(ContributionFactory)PluginUtil.loadObjectFromManifest(contrib);
						
						// register it
						PluginManager.theInstance.AddContributionFactory(
							contributionName,
							factory );
						cd._state = InstallationState.Ready;
					}				
					catch( Exception e ) 
					{
						_state = InstallationState.FatalError;
						string templ = Main.resources["plugin.contrib_load_error"].stringValue;
						string _id = XmlUtil.getAttribute(contrib,"id","unknown");
						string _name = XmlUtil.getAttribute(contrib,"name","unknown");
						throw new Exception(string.Format(templ,root.BaseURI,_name,_id),e);
					}
				}
			}
		}

		/// <summary>
		/// Loads contributions from this plug-in
		/// </summary>
		internal void loadContributions() 
		{
			XmlElement root = doc.DocumentElement;
			Contribution c = null;
			Uri baseUri = new Uri(root.BaseURI);

			// load contributions
			foreach( XmlElement contrib in root.SelectNodes("contribution") ) {
				try {
					string type = contrib.Attributes["type"].Value;
					if(type=="contribution")	continue;	// ignore

					PluginManager.theInstance.NotifyStartParse(this,contrib);
					ContributionFactory factory = PluginManager.theInstance.GetContributionFactory(type);
					c = factory.load(this,contrib);
					contributions.Add(c);
					c.Attach();
					detachable |= c.IsDetachable;
					if(c.IsDetachable)
					{
						totalDetachables++;
						c.OnAttach+=new AttachChangeEvent(this.AttachChangeEventHandler);
						c.OnDetach+=new AttachChangeEvent(this.AttachChangeEventHandler);
					}
					PluginManager.theInstance.AddContribution(c);
					c.init(this,baseUri);
					c._state = InstallationState.Ready;
				} 
				catch( Exception e ) 
				{
					Debug.WriteLine(e.Message);
					Debug.WriteLine(e.StackTrace);
					if( e.InnerException!=null )
					{
						Debug.WriteLine(e.InnerException.Message);
						Debug.WriteLine(e.InnerException.StackTrace);
					}
					if(c!=null)
						c._state = InstallationState.FatalError;
					string templ = Main.resources["plugin.contrib_load_error"].stringValue;
					string _id = XmlUtil.getAttribute(contrib,"id","unknown");
					string _name = XmlUtil.getAttribute(contrib,"name","unknown");
					string msg = string.Format(templ,root.BaseURI,_name,_id);
					throw new Exception(msg,e);
					//Debug.WriteLine(msg);
				}
			}
		}


		/// <summary>
		/// Loads a stream from the plug-in directory.
		/// </summary>
		public Stream loadStream( string name ) {
			return new FileStream( Path.Combine(dirName,name), FileMode.Open, FileAccess.Read, FileShare.Read );
		}
	}
}
