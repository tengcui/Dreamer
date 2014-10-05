using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace dreamer
{
    public class BaseSupport
    {
        //Single Modle
        private static readonly BaseSupport instance = new BaseSupport();
        public static BaseSupport Instance
        {
            get { return instance; }
        }
        private BaseSupport()
        {

        }

        private GraphicsDeviceManager _gd;
        public GraphicsDeviceManager gd
        {
            get { return this._gd; }
        }

        private ContentManager content;
        public ContentManager Content
        {
            get { return content; }
        }

        private SpriteBatch _sbc;
        public SpriteBatch sbc
        {
            get { return this._sbc; }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="_gd"></param>
        /// <param name="content"></param>
        public void Initialize(GraphicsDeviceManager _gd, ContentManager content, SpriteBatch _sbc)
        {
            this._gd = _gd;
            this.content = content;
            this._sbc = _sbc;

        }


    }
}
