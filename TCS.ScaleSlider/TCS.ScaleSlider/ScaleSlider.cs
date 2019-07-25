using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace TCS.ScaleSlider
{
    public class ScaleSlider : SKCanvasView
    {
        #region BindableProperties

        public float SliderSize
        {
            get { return (float)GetValue(SliderSizeProperty); }
            set { base.SetValue(SliderSizeProperty, value); }
        }

        public static readonly BindableProperty SliderSizeProperty = BindableProperty.Create(
                                                        propertyName: "SliderSize",
                                                        returnType: typeof(float),
                                                        declaringType: typeof(ScaleSlider),
                                                        defaultValue: (float)1,
                                                        defaultBindingMode: BindingMode.TwoWay,
                                                        propertyChanged: SliderSizePropertyChanged);

        private static void SliderSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ScaleSlider)bindable;
        }

        #endregion BindableProperties

        #region Properties

        SKPaint backgroundBrush;
        SKPaint tickBrush;
        SKPath clipPath = SKPath.ParseSvgPathData("M.021 28.481a25.933 25.933 0 0 0 8.824-2.112 27.72 27.72 0 0 0 7.391-5.581l19.08-17.045S39.879.5 44.516.5s9.352 3.243 9.352 3.243l20.74 18.628a30.266 30.266 0 0 0 4.525 3.545c3.318 2.263 11.011 2.564 11.011 2.564z");
        SKPath snowFlakePath = SKPath.ParseSvgPathData("M645.527,495.815l-75.609-43.689l63.832-38.015c10.262-4.944,13.438-18.724,7.602-28.855c-5.941-10.375-18.764-12.563-28.498-6.89l-85.107,49.849L392.488,349.51l135.258-77.894l85.107,49.768c2.658,1.945,6.078,3.08,9.881,3.08c7.213,0,13.674-3.404,18.617-9.889c5.699-10.294,2.66-23.911-7.602-28.855l-63.832-38.015"+
		"l75.609-43.688c9.855-5.674,13.244-18.4,7.602-28.856c-5.389-10.051-18.723-13.536-28.879-7.619l-75.99,44.094l-1.135-76.03"+
		"c-0.178-11.753-9.119-21.237-20.896-21.237c-11.775,0-20.984,10.213-20.895,21.237l0.754,100.346l-134.496,77.894V155.788"+
		"l84.727-48.309c9.891-5.593,14.201-18.643,8.357-28.855c-5.934-10.375-18.115-13.456-28.498-7.619l-64.586,36.475V21.236"+
		"c0-11.753-9.5-21.236-21.277-21.236c-11.777,0-20.896,9.483-20.896,21.236v87.053l-66.871-37.285"+
		"c-10.416-5.755-22.274-2.513-28.118,7.619c-5.933,10.375-2.245,23.101,7.976,28.855l87.013,49.039v156.518l-137.542-79.353"+
		"l0.762-98.077c1.143-11.023-8.738-21.237-20.515-21.237h-0.762c-11.016,0-20.774,9.484-20.896,21.237l-0.762,73.76l-74.084-42.554"+
		"c-10.343-5.998-23.109-2.432-28.499,7.538c-5.642,10.537-2.724,22.939,7.603,28.937l74.846,43.283l-65.354,39.15"+
		"C53.96,290.664,50.758,304.443,57,314.575c4.02,6.566,9.873,9.889,17.856,9.889c4.174,0,7.651-1.054,11.015-3.08l85.489-50.498"+
		"l137.162,78.624l-137.162,79.434l-85.489-50.578c-10.262-5.998-23.49-3.161-28.872,6.89c-5.642,10.456-2.578,23.02,7.214,28.855"+
		"l65.355,38.744l-74.847,43.689c-10.302,5.998-13.642,18.643-7.603,28.937C51.342,532.614,57,536.1,64.976,536.1"+
		"c1.897,0,5.698-1.135,10.643-3.08l74.084-42.554l0.762,73.76c0.122,11.753,9.88,20.831,20.896,20.831h0.762"+
		"c11.777,0,20.604-9.808,20.515-21.642l-0.762-97.996l137.542-78.623v155.707l-87.013,49.038"+
		"c-10.383,5.836-14.015,18.562-7.976,28.856c4.223,7.214,9.881,10.699,17.856,10.699c3.802,0,7.303-0.648,10.262-2.351"+
		"l66.871-37.935v88.513c0,11.753,9.119,20.912,20.896,20.912c11.777,0,21.277-9.159,21.277-20.912v-87.054l64.586,36.476"+
		"c2.957,1.702,6.459,2.351,9.881,2.351c8.355,0,14.516-3.404,18.617-10.699c5.771-10.213,2.043-23.021-8.357-28.856l-84.727-47.498"+
		"V385.985l134.496,77.894l-0.754,99.536c-0.098,11.834,9.119,21.642,20.896,21.642s20.717-9.078,20.895-20.831l1.135-76.435"+
		"l75.99,44.499c2.934,1.702,6.84,2.27,11.023,2.27c7.977,0,13.828-3.323,17.855-9.889"+
		"C659.363,514.539,655.382,501.489,645.527,495.815z");
        SKPath flamePath = SKPath.ParseSvgPathData("m 272.52 14.9474 c -16.2302 24.5973 -17.4755 78.1656 29.2314 119.746 c 53.8522 62.6036 67.5295 109.715 66.9073 200.416 c -0.509277 74.2204 -27.8441 93.2647 -51.638 159.496 c -28.1106 62.1287 -36.6324 91.9828 -16.2932 139.955 c 7.19458 20.9159 18.3852 35.8378 32.5727 59.7507 c -22.1169 -16.8237 -42.2359 -27.6533 -62.3548 -57.4643 c -18.7044 -32.9255 -33.5567 -47.0742 9.24673 -181.784 c 9.97049 -31.4617 23.941 -78.9233 28.9115 -111.385 c 1.88596 -22.175 -0.228119 -55.1872 -13.3422 -82.5249 c -36.9366 -55.1982 -46.0503 -50.6029 -94.3781 -82.5947 c 26.6839 54.7657 11.9302 80.4417 -65.2991 189.404 c -64.8983 105.166 -92.705 145.266 -88.2346 216.222 c 5.40698 85.8187 51.5296 158.437 131.83 217.835 c 26.2463 19.4145 49.0346 23.8444 65.5073 30.3935 l 49.8302 11.356 c -20.1858 -13.9612 -37.3716 -21.9225 -60.5575 -41.8837 c -47.7772 -48.4941 -75.3477 -108.336 -80.3781 -174.113 c -2.99034 -39.1024 3.53917 -88.0878 5.37561 -48.1385 c 1.15854 7.98285 6.21541 33.6515 10.2153 39.1595 c 8 21.7531 44.8939 81.6156 78.3542 118.189 "+
            "c 16.868 18.4372 51.5417 50.5283 78.0982 68.8192 c 48.4027 26.7632 69.8055 30.5266 106.208 35.2898 c 31.4167 0 69.7298 -5.58649 57.0121 -4.74786 c -65.1008 -96.363 -67.4874 -100.771 -71.1962 -162.025 c 0.269073 -56.621 20.4073 -69.1729 55.6531 -139.949 c 43.7987 -87.951 54.7549 -126.973 55.0359 -192.315 c 0.407776 -94.7442 -39.7695 -144.993 -127.547 -197.892 c -24.447 -13.8481 -31.1063 -17.7477 -75.7454 -37.491 c -25.6674 -13.592 -51.3347 -43.1679 -53.0262 -91.725 Z m 301.747 266.493 c 22.8541 34.8291 42.7187 86.6412 44.5855 123.468 c -5.0578 48.065 -5.32617 51.3993 -60.832 121.797 c -23.087 33.7209 -36.707 54.6631 -42.7421 68.4755 c -17.084 39.1011 -17.5841 137.039 10.4884 190.179 l 23.2943 44.3066 l 10.2393 -33.0503 c 7.87659 -25.2731 18.4775 -56.6924 73.2604 -97.0023 c 69.2085 -61.2578 79.8633 -79.2551 89.9311 -156.302 c 1.05603 -50.9854 -5.43793 -88.1528 -38.299 -148.558 c -34.2954 -56.0865 -64.6212 -74.2108 -109.926 -113.314 Z");

        float sliderPosition;
        float sliderLeftBounds;
        float sliderRightBounds;

        #endregion Properties

        #region Constructor
        public ScaleSlider()
        {
            EnableTouchEvents = true;
        }
        #endregion Constructor

        #region Overrides
        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var canvas = e.Surface.Canvas;

            canvas.Clear();

            //Initial Setup 
            if (backgroundBrush == null)
            {
                InitialSetup(info);
            }

            using (new SKAutoCanvasRestore(canvas))
            {
                UpdateClippingMask(canvas, info);
                
                //Draw Gradient Rectangle
                SKRect bounds = new SKRect(0, 0, info.Width, info.Height);
                canvas.DrawRoundRect(bounds, 20, 20, backgroundBrush);

                //Create tick marks
                CreateTickMarks(canvas, info);

                DrawSnowflake(canvas, info);
                DrawFlame(canvas, info);
                DrawLabel(canvas, info);

            }


        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            sliderPosition = e.Location.X;
            sliderPosition = sliderPosition < sliderLeftBounds ?
                sliderLeftBounds :
                sliderPosition > sliderRightBounds ?
                sliderRightBounds :
                sliderPosition;
            
            InvalidateSurface();

            e.Handled = true;
        }

        #endregion Overrides

        #region Methods

        void DrawFlame(SKCanvas canvas, SKImageInfo info)
        {
            canvas.DrawPath(flamePath, new SKPaint { StrokeWidth = 2, Color = Color.FromHex("#45FFFFFF").ToSKColor() });
        }

        void DrawSnowflake(SKCanvas canvas, SKImageInfo info)
        {
            canvas.DrawPath(snowFlakePath, new SKPaint { StrokeWidth = 2, Color = Color.FromHex("#45FFFFFF").ToSKColor() });
        }

        void DrawLabel(SKCanvas canvas, SKImageInfo info)
        {
            var slidePercent = sliderPosition / info.Width;
            var text = Math.Floor(slidePercent * 100).ToString();


            canvas.DrawText(text, new SKPoint(60, info.Height / 2), new SKPaint { StrokeWidth = 2, Color = Color.White.ToSKColor(), TextSize = 120 });
        }

        void UpdateClippingMask(SKCanvas canvas, SKImageInfo info)
        {
            //Move clipping mask
            var xPos = sliderPosition - clipPath.TightBounds.MidX;
            clipPath.Transform(SKMatrix.MakeTranslation(xPos, 0));

            //Draw clipping mask
            canvas.ClipPath(clipPath, SKClipOperation.Difference, true);
        }

        private void InitialSetup(SKImageInfo info)
        {
            //Create Background gradient brush
            backgroundBrush = new SKPaint();
            backgroundBrush.Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(info.Width, info.Height),
                new SKColor[] { Color.Blue.ToSKColor(), Color.Red.ToSKColor() },
                new float[] { 0, 1 },
                SKShaderTileMode.Clamp);

            //Create Tick mark brush
            tickBrush = new SKPaint()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 2,
            };

            //Clipping path Transform
            clipPath.Transform(SKMatrix.MakeScale(4,4));
            var clipYPos = info.Height - clipPath.TightBounds.Height;
            clipPath.Transform(SKMatrix.MakeTranslation(info.Width/2, clipYPos));

            //Snowflake path transform
            snowFlakePath.Transform(SKMatrix.MakeScale(.4f, .4f));
            snowFlakePath.Transform(SKMatrix.MakeTranslation(50, (info.Height / 2) - (snowFlakePath.TightBounds.Height / 2)));

            //Flame path transform
            flamePath.Transform(SKMatrix.MakeScale(.4f, .4f));
            flamePath.Transform(SKMatrix.MakeTranslation(info.Width-flamePath.TightBounds.Width-50, (info.Height / 2)-(flamePath.TightBounds.Height/2)));

            //Setup slider bounds
            sliderLeftBounds = clipPath.TightBounds.Width / 8;
            sliderRightBounds = info.Width - (clipPath.TightBounds.Width / 8);

        }

        void CreateTickMarks(SKCanvas canvas, SKImageInfo info)
        {
            var numTicks = 15;
            var distance = info.Width / numTicks;
            var tickHeight = 50;
            for (int i = 1; i < numTicks; i++)
            {
                var start = new SKPoint(i * distance, info.Height);
                var end = new SKPoint(i * distance, info.Height - (tickHeight));

                tickBrush.Shader = SKShader.CreateLinearGradient(
                                         start,
                                         end,
                                         new SKColor[] { new SKColor(255, 255, 255, 200), new SKColor(255, 255, 255, 0) },
                                         new float[] { 0, 1 },
                                         SKShaderTileMode.Clamp);

                canvas.DrawLine(start, end, tickBrush);

            }

        }
        #endregion Methods

    }
}
