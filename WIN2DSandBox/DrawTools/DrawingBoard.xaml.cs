using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WIN2DSandBox.DrawTools
{
    public sealed partial class DrawingBoard : UserControl
    {
        
        private readonly BitmapRenderer _renderer = new BitmapRenderer();

        public DrawingBoard()
        {
            InitializeComponent();
            PointerPressed += DrawingBoard_PointerPressed;
            PointerMoved += DrawingBoard_PointerMoved;
        }

        private void DrawingBoard_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            lock (_renderer)
            {
                _renderer.OnPointerMoved(e.GetIntermediatePoints(DrawingControl));
            }
            DrawingControl.Invalidate();
        }

        private void DrawingBoard_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            lock (_renderer)
            {
                _renderer.OnPointerPressed();
            }
            DrawingControl.Invalidate();
        }

        private void OnCanvasDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            var layer = _renderer.Render(sender);

            args.DrawingSession.DrawImage(layer);
        }



        //private async void Save()
        //{
        //    //var fileName = "DrawingBoard" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".png";

        //    //var storageFolder = KnownFolders.PicturesLibrary;
        //    //var sampleFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        //    //using (var stream = await sampleFile.OpenAsync(FileAccessMode.ReadWrite))
        //    //{
        //    //    await _layer.SaveAsync(stream, CanvasBitmapFileFormat.Png);
        //    //}
        //}

        private void DrawingControl_OnCreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            _renderer.CreateResourcesAsync(DrawingControl);
        }
    }
}