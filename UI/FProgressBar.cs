using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using ReLogic.Content;
using System;
using Microsoft.Xna.Framework;

namespace Factorized.UI {
    public class FProgressBar : UIElement
    {
        protected Texture2D fullImage;
        protected Texture2D emptyImage;
        protected Func<int> progress;
        protected Func<int> limit;
        public int ImageWidth;
        public int ImageHeight;
        public FProgressBar(Asset<Texture2D> fullImage, Asset<Texture2D>emptyImage,
            Func<int> progress, Func<int> limit)
        {
            this.fullImage = (Texture2D) fullImage;
            this.emptyImage = (Texture2D) emptyImage;
            this.progress = progress;
            this.limit = limit;
            ImageWidth = 16;
            ImageHeight = 16;
        } 
        public FProgressBar(Texture2D fullImage, Texture2D emptyImage,
            Func<int> progress, Func<int> limit)
        {
            this.fullImage = fullImage;
            this.emptyImage = emptyImage;
            this.progress = progress;
            this.limit = limit;
            ImageWidth = 16;
            ImageHeight = 16;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Rectangle dimensions = GetDimensions().ToRectangle();
            spriteBatch.Draw(emptyImage,dimensions,Color.White);
            float percent = ((float) progress()/(float) limit());
            dimensions.Height =(int) ( percent* (float) dimensions.Height);
            spriteBatch.Draw(fullImage,dimensions,
                new Rectangle(0,0,ImageWidth,(int) (percent * ((float) ImageHeight))),Color.White);
            Factorized.mod.Logger.InfoFormat("drew progressBar");
        }
        public override string ToString()
        {
            return "progress:" + progress().ToString() + " limit:" +limit().ToString()
                +" imageWidth:"+ImageWidth +" ImageHeight:"+ ImageHeight + " ";
        }
    }
}
