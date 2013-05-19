using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace HardShadows
{
    public enum DpadDirection { dNONE, dUP, dDOWN, dLEFT, dRIGTH }; 

    class DPadController : Game1
    {
           #region fields 
 	       Texture2D _texture; 
 	       Vector2 _position; 
 	       SpriteBatch _spritebatch; 
 	       TouchLocation _touchLocation; 
	       Rectangle _leftRect, _upRect, _downRect, _rightRect, _intersectRect; 
 	       public DpadDirection PressedDirection; 
 	       #endregion 
 	 
 	 
 	       public DPadController(Game1 game) : base(game) 
 	       {             
 	       } 
 	 
 	        public override void Initialize() 
 	        { 
 	            //TODO configurable position 
 	            _position.X = 15; 
 	            _position.Y = 337; 
 	            base.Initialize(); 
 	        } 
 	 
 	        protected override void  LoadContent() 
 	        { 
 	            _texture = this.Game.Content.Load<Texture2D>("dpad128"); 
 	            _spritebatch = new SpriteBatch(GraphicsDevice); 
 	 
 	            //Create the hitboxes rectangles based on the current DPad position on screen 
 	            _leftRect = new Rectangle(0 + (int)_position.X, 42 + (int)_position.Y, 45, 85); 
 	            _upRect = new Rectangle(42 + (int)_position.X, 0 + (int)_position.Y, 43, 42); 
 	            _rightRect = new Rectangle(80 + (int)_position.X, 43 + (int)_position.Y, 48, 43); 
 	            _downRect = new Rectangle(43 + (int)_position.X, 84 + (int)_position.Y, 44, 42); 
 	            _intersectRect = new Rectangle(); 
 	 
 	             base.LoadContent(); 
 	        } 
 	 
 	        public override void Draw(GameTime gameTime) 
 	        { 
 	            _spritebatch.Begin(); 
 	            _spritebatch.Draw(_texture, _position, Color.White); 
 	            _spritebatch.End(); 
 	            base.Draw(gameTime); 
 	        } 
 	 
 	        public override void Update(GameTime gameTime) 
 	        { 
 	             
 	            _touchLocation = TouchPanel.GetState().FirstOrDefault<TouchLocation>(); 
 	 
 	            if (_touchLocation.State == TouchLocationState.Pressed) 
 	            { 
 	                _intersectRect.X = (int)_touchLocation.Position.X; 
 	                _intersectRect.Y = (int)_touchLocation.Position.Y; 
 	                _intersectRect.Width = 1; 
 	                _intersectRect.Height = 1; 
 	 
 	                if (_intersectRect.Intersects(_rightRect)) 
 	                    PressedDirection = DpadDirection.dRIGTH; 
 	                else if (_intersectRect.Intersects(_leftRect)) 
 	                    PressedDirection = DpadDirection.dLEFT; 
 	                else if (_intersectRect.Intersects(_upRect)) 
 	                    PressedDirection = DpadDirection.dUP; 
 	                else if (_intersectRect.Intersects(_downRect)) 
 	                    PressedDirection = DpadDirection.dDOWN; 
 	                else 
 	                    PressedDirection = DpadDirection.dNONE; 
 	 
 	            } 
 	            else if ((_touchLocation.State == TouchLocationState.Invalid) || (_touchLocation.State == TouchLocationState.Released)) 
 	                PressedDirection = DpadDirection.dNONE;                  
 	 
	            base.Update(gameTime); 
 	             
 	        }     
    }
}
