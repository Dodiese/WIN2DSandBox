using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Metadata;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Storage;
using Windows.System.Power.Diagnostics;
using Windows.UI;
using Windows.UI.Input;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace WIN2DSandBox.DrawTools
{

    public class BitmapRenderer
    {
        private CanvasBitmap _currentBrush;
        private CanvasRenderTarget _layer;
        private CanvasRenderTarget _backBuffer;
        private CanvasRenderTarget _frontBuffer;

        public async void CreateResourcesAsync(ICanvasResourceCreator creator)
        {
            _currentBrush = await CanvasBitmap.LoadAsync(creator, new Uri("ms-appx:///Resources/Brushes/brush_png_by_editionsmily-d4n41g9.png"));
            //_control = (ICanvasResourceCreatorWithDpi) creator;
        }

        private Stroke _currentStroke = new Stroke();
        private float _width = 1200;
        private float _height = 1024;

        public void OnPointerPressed()
        {
            _currentStroke = new Stroke();
        }

        public void OnPointerMoved(IList<PointerPoint> intermediatePoints)
        {
            foreach (var point in intermediatePoints)
            {
                if (point.IsInContact)
                {
                    using (CanvasDrawingSession ds = _frontBuffer.CreateDrawingSession())
                    {
                        ds.Clear(Colors.Transparent);
                        ds.DrawImage(_currentBrush, new Rect(point.Position.X - 10, point.Position.Y - 10, 20, 20));
                    }

                    _backBuffer.CopyPixelsFromBitmap(_layer);

                    using (var layerSession = _layer.CreateDrawingSession())
                    {
                        layerSession.Clear(Colors.Transparent);
                        var effectsGraph = new BlendEffect
                        {
                            Mode = BlendEffectMode.Multiply,
                            Background = _backBuffer,
                            Foreground = _frontBuffer,
                        };
                        layerSession.DrawImage(effectsGraph);
                    }
                }
            }
        }

        public CanvasRenderTarget Render(CanvasControl control)
        {
            if (_layer == null)
            {
                _layer = new CanvasRenderTarget(control, _width, _height);
                _backBuffer = new CanvasRenderTarget(control, _width, _height);
                _frontBuffer = new CanvasRenderTarget(control, _width, _height);
            }
            return _layer;
        }
    }

    public class Stroke
    {
        public List<Vector2> Points { get; } = new List<Vector2>();

        public float BrushRadius
        {
            get
            {
                return _brushRadius;
            }

            set
            {
                _brushRadius = value;
            }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        private float _brushRadius = 10;
        private Color _color = new Color() { A = 128, R = 0, G = 255, B = 0 };
    }
}