using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WIN2DSandBox.DrawTools
{
    public sealed partial class DrawingBoard : UserControl
    {
        private CanvasRenderTarget _layer;
        private readonly BrushRenderer renderer = new BrushRenderer();

        public DrawingBoard()
        {
            InitializeComponent();
            PointerPressed += DrawingBoard_PointerPressed;
            PointerMoved += DrawingBoard_PointerMoved;
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
            _layer = _layer ??
                     new CanvasRenderTarget(DrawingControl, (float) DrawingControl.ActualWidth,
                         (float) DrawingControl.ActualHeight);
            using (var ds = _layer.CreateDrawingSession())
            {
                renderer.Render(ds);
            }

            args.DrawingSession.DrawImage(_layer);
        }

        private void DrawingBoard_OnLoaded(object sender, RoutedEventArgs e)
        {
        }


        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var fileName = "DrawingBoard" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".png";

            var storageFolder = KnownFolders.PicturesLibrary;
            var sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (var stream = await sampleFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await _layer.SaveAsync(stream, CanvasBitmapFileFormat.Png);
            }
        }
    }
}