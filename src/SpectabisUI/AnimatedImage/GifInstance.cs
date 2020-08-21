using SpectabisUI.AnimatedImage.Decoding;
using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Animation;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Rendering;

namespace SpectabisUI.AnimatedImage
{
    public class GifInstance : IDisposable
    {
        public Image TargetControl { get; set; }
        public Stream Stream { get; private set; }
        public IterationCount IterationCount { get; private set; }
        public bool AutoStart { get; private set; } = true;
        public Progress<int> Progress { get; private set; }
        private GifDecoder _gifDecoder;
        private GifBackgroundWorker _bgWorker;
        private WriteableBitmap _targetBitmap;
        private bool _hasNewFrame;
        private bool _isDisposed;
        private readonly object _bitmapSync = new object();
        private static readonly object _globalUIThreadUpdateLock = new object();

        public void SetSource(object newValue)
        {
            var sourceUri = new Uri(newValue as string, UriKind.Relative);

            Stream stream = null;

            if (sourceUri != null)
            {
                Progress = new Progress<int>();
                var filePath = sourceUri.OriginalString;

                if(File.Exists(filePath))
                {
                    stream = File.OpenRead(filePath);
                }
            }
            else if (newValue is Stream sourceStr)
            {
                stream = sourceStr;
            }
            else
            {
                throw new InvalidDataException("Missing valid URI or Stream.");
            }

            Stream = stream;
            _gifDecoder = new GifDecoder(Stream);
            _bgWorker = new GifBackgroundWorker(_gifDecoder);
            var pixSize = new PixelSize(_gifDecoder.Header.Dimensions.Width, _gifDecoder.Header.Dimensions.Height);
            _targetBitmap = new WriteableBitmap(pixSize, new Vector(96, 96), PixelFormat.Bgra8888);

            TargetControl.Source = _targetBitmap;
            _bgWorker.CurrentFrameChanged += FrameChanged;

            Run();
        }

        private void RenderTick(TimeSpan time)
        {
            if (_isDisposed | !_hasNewFrame) return;
            lock (_globalUIThreadUpdateLock)
            {
                lock (_bitmapSync)
                {
                    TargetControl?.InvalidateVisual();
                    _hasNewFrame = false;
                }
            }
        }

        private void FrameChanged()
        {
            lock (_bitmapSync)
            {
                if (_isDisposed) return;
                _hasNewFrame = true;
                using var lockedBitmap = _targetBitmap?.Lock();
                _gifDecoder?.WriteBackBufToFb(lockedBitmap.Address);
            }
        }

        private void Run()
        {
            if (!Stream.CanSeek)
                throw new ArgumentException("The stream is not seekable");

            AvaloniaLocator.Current.GetService<IRenderTimer>().Tick += RenderTick;
            _bgWorker?.SendCommand(BgWorkerCommand.Play);
        }

        public void IterationCountChanged(AvaloniaPropertyChangedEventArgs e)
        {
            IterationCount = (IterationCount)e.NewValue;
        }

        public void AutoStartChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var newVal = (bool)e.NewValue;
            GifInstance gifInstance = this;
            gifInstance.AutoStart = newVal;
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing of GifInstance");
            _isDisposed = true;
            AvaloniaLocator.Current.GetService<IRenderTimer>().Tick -= RenderTick;
            _bgWorker?.SendCommand(BgWorkerCommand.Dispose);
            _targetBitmap?.Dispose();
        }
    }
}