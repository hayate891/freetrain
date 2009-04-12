using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml;
using org.kohsuke.directdraw;
using freetrain.world;
using freetrain.framework.plugin;

namespace freetrain.framework.graphics
{
    /// <summary>
    /// Wraps DirectDraw surface
    /// </summary>
    [Serializable]
    public class Picture : ISerializable
    {
        public readonly string m_PictureID;

        /// <summary>
        /// DirectDraw surface.
        /// null when the surface is detached.
        /// </summary>
        private AgateSurface m_AgateSurface;

        /// <summary>
        /// capable of restoring surface images.
        /// </summary>
        private readonly SurfaceLoader[,] m_SurfaceLoaders;

        /// <summary>
        /// Dirty flag. Set true to reload the surface.
        /// </summary>
        private bool dirty;

        public Picture( string inPictureID, SurfaceLoader[,] inSurfaceLoaders )
        {
            this.m_PictureID = inPictureID;
            this.m_SurfaceLoaders = init( inSurfaceLoaders );
        }

        /// <summary>
        /// Create a picture from a single bitmap and id.
        /// </summary>
        public Picture( string inPictureID, string inFileName )
        {
            this.m_PictureID = inPictureID;
            SurfaceLoader[,] theSurfaceLoaders = new SurfaceLoader[4, 2];
            theSurfaceLoaders[0, 0] = new BitmapSurfaceLoader( inFileName );

            this.m_SurfaceLoaders = init( theSurfaceLoaders );
        }

        public Picture( string inPictureID, string inDayFileName, string inNightFileName )
        {
            this.m_PictureID = inPictureID;
            SurfaceLoader[,] theSurfaceLoaders = new SurfaceLoader[4, 2];
            theSurfaceLoaders[0, 0] = new BitmapSurfaceLoader( inDayFileName );
            theSurfaceLoaders[0, 1] = new BitmapSurfaceLoader( inNightFileName );
            this.m_SurfaceLoaders = init( theSurfaceLoaders );
        }

        /// <summary>
        /// Load picture from an XML manifest (&lt;picture> element)
        /// </summary>
        public Picture( XmlElement pic ) : this( pic, pic.Attributes["id"].Value ) { }

        public Picture( XmlElement pic, string _id )
        {
            this.m_PictureID = _id;

            //			// obtain the size of the bitmap
            string baseFileName = XmlUtil.resolve( pic, pic.Attributes["src"].Value ).LocalPath;
            //			this.size = getBitmapSize(baseFileName);

            SurfaceLoader[,] specifiedLoaders = new SurfaceLoader[4, 2];
            specifiedLoaders[0, 0] = new BitmapSurfaceLoader( baseFileName );

            specifiedLoaders[0, 1] = getNightOverride( pic );
            foreach( XmlElement ovr in pic.SelectNodes( "override" ) )
            {
                string when = ovr.Attributes["when"].Value;

                int s;
                switch( when )
                {
                    case "spring": s = 0; break;
                    case "summer": s = 1; break;
                    case "autumn":
                    case "fall": s = 2; break;
                    case "winter": s = 3; break;
                    case "night": s = -1; break;
                    default:
                        throw new FormatException( "when='" + when + "' is an unknown override format" );
                    //! throw new FormatException("when='"+when+"'は未知のオーバーライド形式です");
                }
                if( s >= 0 )
                {
                    XmlAttribute src = ovr.Attributes["src"];
                    if( src != null )
                    {
                        SurfaceLoader overrideLoader = new BitmapSurfaceLoader(
                            XmlUtil.resolve( ovr, src.Value ).LocalPath );
                        specifiedLoaders[s, 0] = overrideLoader;
                    }
                    specifiedLoaders[s, 1] = getNightOverride( ovr );
                }
            }

            this.m_SurfaceLoaders = init( specifiedLoaders );
        }

        // load nested night override (for each seasons).
        private SurfaceLoader getNightOverride( XmlElement node )
        {
            XmlNode ovr = node.SelectSingleNode( "override" );
            if( ovr == null ) return null;
            string when = ovr.Attributes["when"].Value;
            if( when.Equals( "night" ) )
                return new BitmapSurfaceLoader(
                XmlUtil.resolve( ovr, ovr.Attributes["src"].Value ).LocalPath );
            else
                return null;
        }

        /// <summary>
        /// Complete picture loaders by filling in the missing loaders.
        /// </summary>
        /// <param name="specifiedLoaders"></param>
        /// <returns></returns>
        private SurfaceLoader[,] init( SurfaceLoader[,] specifiedLoaders )
        {
            SurfaceLoader[,] loaders = new SurfaceLoader[4, 2];

            // Fill-in unspecified SpriteLoaders by the default ones.
            for( int s = 0; s < 4; s++ )
            {
                if( specifiedLoaders[s, 0] != null )
                    loaders[s, 0] = specifiedLoaders[s, 0];
                else
                    loaders[s, 0] = loaders[0, 0];

                if( specifiedLoaders[s, 1] != null )
                    loaders[s, 1] = specifiedLoaders[s, 1];
                else
                {
                    if( specifiedLoaders[s, 0] != null )
                        loaders[s, 1] = new NightSurfaceLoader( loaders[s, 0] );
                    else
                        loaders[s, 1] = loaders[0, 1];
                }
            }

            PictureManager.add( this );	// all the pictures are controlled by a manager
            dirty = true;	// make sure that the surface will be loaded next time it's used.

            return loaders; //
        }

        public void setDirty()
        {
            dirty = true;
        }

        /// <summary>
        /// Release any resource acquired by this picture.
        /// The picture will be automatically reloaded next time
        /// the picture is used.
        /// </summary>
        public void release()
        {
            if( m_AgateSurface != null )
            {
                m_AgateSurface.Dispose();
                m_AgateSurface = null;
            }
            dirty = true;
        }

        /// <summary>
        /// Obtains the surface object.
        /// </summary>
        public AgateSurface AgateSurface
        {
            get
            {
                if( dirty )
                {
                    World theWorld = World.world;

                    // reload the surface
                    Clock theClock = theWorld.clock;
                    Color theColorKey = 
                        m_SurfaceLoaders[
                            (int)theClock.season, 
                            ( theWorld.viewOptions.useNightView ) ? 1 : 0]
                            .load( ref m_AgateSurface );
                    m_AgateSurface.sourceColorKey = theColorKey;
                    dirty = false;
                }
                return m_AgateSurface;
            }
        }



        //
        // serialization
        //
        public void GetObjectData( SerializationInfo info, StreamingContext context )
        {
            info.SetType( typeof( ReferenceImpl ) );
            info.AddValue( "id", m_PictureID );
        }

        [Serializable]
        internal sealed class ReferenceImpl : IObjectReference
        {
            private string id = null;
            public object GetRealObject( StreamingContext context )
            {
                return PictureManager.get( id );
            }
        }
    }
}
