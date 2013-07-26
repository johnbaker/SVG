using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg.Transforms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Svg
{
    /// <summary>
    /// Represents and SVG image
    /// </summary>
    [SvgElement("image")]
    public class SvgImage : SvgVisualElement
    {
        /// <summary>
		/// Initializes a new instance of the <see cref="SvgImage"/> class.
        /// </summary>
		public SvgImage()
        {
            Width = new SvgUnit(0.0f);
            Height = new SvgUnit(0.0f);
        }

        /// <summary>
        /// Gets an <see cref="SvgPoint"/> representing the top left point of the rectangle.
        /// </summary>
        public SvgPoint Location
        {
            get { return new SvgPoint(X, Y); }
        }

		[SvgAttribute("x")]
		public virtual SvgUnit X
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("x"); }
			set { this.Attributes["x"] = value; }
		}

		[SvgAttribute("y")]
		public virtual SvgUnit Y
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("y"); }
			set { this.Attributes["y"] = value; }
		}


		[SvgAttribute("width")]
		public virtual SvgUnit Width
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("width"); }
			set { this.Attributes["width"] = value; }
		}

		[SvgAttribute("height")]
		public virtual SvgUnit Height
		{
			get { return this.Attributes.GetAttribute<SvgUnit>("height"); }
			set { this.Attributes["height"] = value; }
		}

		[SvgAttribute("href", SvgAttributeAttribute.XLinkNamespace)]
		public virtual Uri Href
		{
			get { return this.Attributes.GetAttribute<Uri>("href"); }
			set { this.Attributes["href"] = value; }
		}



        /// <summary>
        /// Gets the bounds of the element.
        /// </summary>
        /// <value>The bounds.</value>
        public override RectangleF Bounds
        {
			get { return new RectangleF(this.Location.ToDeviceValue(), new SizeF(this.Width, this.Height)); }
        }

        /// <summary>
        /// Gets the <see cref="GraphicsPath"/> for this element.
        /// </summary>
        public override GraphicsPath Path
        {
            get
            {
				var rectangle = new RectangleF(Location.ToDeviceValue(),
				                               new SizeF(Width.ToDeviceValue(), Height.ToDeviceValue()));

				var _path = new GraphicsPath();
				_path.StartFigure();
				_path.AddRectangle(rectangle);
				_path.CloseFigure();

				return _path;
            }
        }

        protected internal override void RenderFill(SvgRenderer renderer)
        {
            // this will render a data:image image, but not any other types.  That is up to you to implement.
            var match = Regex.Match(Href.AbsoluteUri, @"data:image/(?<type>.+?),(?<data>.+)");
            if (match != null && match.Groups["data"] != null)
            {
                var base64Data = match.Groups["data"].Value;
                var binData = Convert.FromBase64String(base64Data);
                using (var stream = new MemoryStream(binData))
                {
                    Image image = new Bitmap(stream);
                    var b = new TextureBrush(image);
                    b.ScaleTransform(this.Path.GetBounds().Size.Width / image.Width, this.Path.GetBounds().Size.Height / image.Height);
                    renderer.FillPath(b, this.Path);
                }
            }
        }

        /// <summary>
        /// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        /// </summary>
        protected override void Render(SvgRenderer renderer)
        {
            if (Width.Value > 0.0f && Height.Value > 0.0f)
            {
                base.Render(renderer);
            }
        }


		public override SvgElement DeepCopy()
		{
			return DeepCopy<SvgImage>();
		}

		public override SvgElement DeepCopy<T>()
		{
 			var newObj = base.DeepCopy<T>() as SvgImage;
			newObj.Height = this.Height;
			newObj.Width = this.Width;
			newObj.X = this.X;
			newObj.Y = this.Y;
			newObj.Href = this.Href;
			return newObj;
		}
    }
}