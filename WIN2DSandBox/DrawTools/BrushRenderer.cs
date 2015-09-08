using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata;
using Windows.UI;
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
                if (stroke.Points.Count>1)
                {
                    for(var i= 1; i <stroke.Points.Count; i++)
                    {
                        var point = stroke.Points[i];
                        var prev = stroke.Points[i - 1];
                        ds.FillEllipse(point, stroke.BrushRadius, stroke.BrushRadius, Colors.DarkRed);
                    } 
                }
            }
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
        private float _brushRadius= 10;
    }
}