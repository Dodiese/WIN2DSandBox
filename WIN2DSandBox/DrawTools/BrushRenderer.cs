using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Numerics;
using System.Reflection.Emit;
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
        private float _width = 1200;
        private float _height = 1024;

        private Vector2 _prevPoint;
        private Vector2 _currPoint;
        private float _flow = 1;
        private uint _brushSize = 10;
        private float _distanceToLastStamp;

        public async void CreateResourcesAsync(ICanvasResourceCreator creator)
        {
            _currentBrush = await CanvasBitmap.LoadAsync(creator, new Uri("ms-appx:///Resources/Brushes/brush_png_by_editionsmily-d4n41g9.png"));
            //_control = (ICanvasResourceCreatorWithDpi) creator;
        }


        public void OnPointerPressed(PointerPoint point)
        {
            _prevPoint = new Vector2((float)point.Position.X, (float)point.Position.Y);
            _distanceToLastStamp = 0;
        }

        public void OnPointerMoved(IList<PointerPoint> intermediatePoints)
        {
            foreach (var point in intermediatePoints)
            {
                if (point.IsInContact)
                {
                    _currPoint = new Vector2((float) point.Position.X, (float) point.Position.Y);
                    DrawSubStroke(_prevPoint, _currPoint);
                    _prevPoint = _currPoint;
                }
            }
        }

        private void DrawSubStroke(Vector2 beginPoint, Vector2 endPoint)
        {
            Queue<Vector2> subStrokePoints = InterpolateSubstroke(beginPoint, endPoint, _flow, _brushSize);

            //using (var ds = _frontBuffer.CreateDrawingSession())
            using (var ds = _layer.CreateDrawingSession())
            {
                foreach (var point in subStrokePoints)
                {
                    ds.DrawImage(_currentBrush, new Rect(point.X - _brushSize * 0.5 , point.Y - _brushSize * 0.5, _brushSize, _brushSize));
                }
            }

            //_backBuffer.CopyPixelsFromBitmap(_layer);
            //using (var layerSession = _layer.CreateDrawingSession())
            //{
            //    layerSession.Clear(Colors.Transparent);
            //    var effectsGraph = new BlendEffect
            //    {
            //        Mode = BlendEffectMode.Multiply,
            //        Background = _backBuffer,
            //        Foreground = _frontBuffer,
            //    };
            //    layerSession.DrawImage(effectsGraph);
            //}
        }

        private Queue<Vector2> InterpolateSubstroke(Vector2 beginPoint, Vector2 endPoint, float flow, uint brushSize)
        {
            Queue<Vector2> points = new Queue<Vector2>();
            
            //Vector2 increment = Vector2.Normalize(endPoint - beginPoint) * 0.1f;
            var distance = (endPoint - beginPoint).Length();
            var maxNb = distance/ (float)brushSize;
            for (float i = 0; i < maxNb; i++)
            {
                float a = i/(float)maxNb;
                var pt  = a * endPoint + (1.0f - a)* beginPoint;              
                points.Enqueue(pt);
            }
            return points;
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
}