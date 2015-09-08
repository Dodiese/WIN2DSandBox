using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WIN2DSandBox.DrawTools
{
    public sealed partial class DrawingBoard : UserControl
    {
        private BrushRenderer renderer = new BrushRenderer();
        public DrawingBoard()
        {
            this.InitializeComponent();
            this.PointerPressed += DrawingBoard_PointerPressed;
            this.PointerMoved += DrawingBoard_PointerMoved;
        }

        private void DrawingBoard_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            lock (renderer)
            {
                renderer.OnPointerMoved(e.GetIntermediatePoints(DrawingControl));
            }
            DrawingControl.Invalidate();
        }

        private void DrawingBoard_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lock (renderer)
            {
                renderer.OnPointerPressed();
            }
            DrawingControl.Invalidate();
        }

        private void OnCanvasDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            lock (renderer)
            {
                renderer.Render(args.DrawingSession);
            }      
        }
    }
}
