using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI.Input;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;

namespace WIN2DSandBox.DrawTools
{
    public class BrushRenderer
    {
        List<Stroke> _strokes = new List<Stroke>();

        public List<Stroke> Strokes
        {
            get { return _strokes; }
            set { _strokes = value; }
        }

        public void OnPointerPressed()
        {
            CurrentStroke = new Stroke();
            Strokes.Add(CurrentStroke);
        }

        public Stroke CurrentStroke { get; set; }

        public void OnPointerMoved(IList<PointerPoint> intermediatePoints)
        {
            foreach (var point in intermediatePoints)
            {
                if (point.IsInContact)
                {
                    CurrentStroke.Points.Add(new Vector2((float)point.Position.X, (float)point.Position.Y));
                }

            }
        }

        public void Render(CanvasDrawingSession ds)
        {
            foreach (var stroke in Strokes)
            {
                var prev = stroke.Points[0];
                foreach (var point in stroke.Points)
                {

                }
            }
        }
    }

    public class Stroke
    {
        public List<Vector2> Points { get; } = new List<Vector2>();

        public float BrushRadius1
        {
            get
            {
                return BrushRadius;
            }

            set
            {
                BrushRadius = value;
            }
        }

        private float BrushRadius = 10;


    }
}