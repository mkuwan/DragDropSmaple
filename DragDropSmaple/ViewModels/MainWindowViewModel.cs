using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragDropSmaple.ViewModels
{
    public class MainWindowViewModel : BindableBase, IDisposable
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public string Title => "Drag & Drop Sample Application";

        public ReactivePropertySlim<string> DropFile { get; }

        public ReactiveCommand<DragEventArgs> FileDropCommand { get; private set; }

        private SampleData _sampleData;
        public SampleData SampleData { get => _sampleData; set => SetProperty(ref _sampleData, value); }


        public MainWindowViewModel()
        {
            DropFile = new ReactivePropertySlim<string>().AddTo(Disposable);

            FileDropCommand = new ReactiveCommand<DragEventArgs>().AddTo(Disposable);
            FileDropCommand.Subscribe(e =>
            {
                if (e != null)
                {
                    OnFileDrop(e);
                }
            });

            SetLists();
        }

        private void OnFileDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var dropFiles = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (dropFiles == null)
                return;

            if (File.Exists(dropFiles[0]))
            {
                DropFile.Value = dropFiles[0];
            }
            else
            {
                DropFile.Value = "ドロップされたものはファイルではありません";
            }
        }

        private void SetLists()
        {
            SampleData = new SampleData();
            SampleData.SampleLists = new ObservableCollection<SampleItem>();
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 1, SampleText = "佐藤" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 2, SampleText = "鈴木" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 3, SampleText = "田中" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 4, SampleText = "加藤" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 5, SampleText = "末永" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 6, SampleText = "松本" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 7, SampleText = "飯塚" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 8, SampleText = "小島" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 9, SampleText = "木村" });
            SampleData.SampleLists.Add(new SampleItem() { SampleId = 10, SampleText = "杉浦" });
        }


        public void Dispose()
        {
            Disposable.Dispose();
        }
    }


    public class SampleData : INotifyPropertyChanged
    {
        private ObservableCollection<SampleItem> _sampleLists;
        public ObservableCollection<SampleItem> SampleLists
        {
            get => _sampleLists;
            set
            {
                _sampleLists = value;
                OnPropertyChanged();
            }
        }

        public TextBoxCustomDropHandler TextBoxCustomDropHandler { get; set; } = new TextBoxCustomDropHandler();


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class SampleItem
    {
        public int SampleId { get; set; }
        public string SampleText { get; set; }
    }

    public class TextBoxCustomDropHandler : IDropTarget
    {

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = typeof(DropTargetHighlightAdorner);
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data.GetType().Equals(typeof(SampleItem)))
            {
                ((TextBox)dropInfo.VisualTarget).Text = ((SampleItem)dropInfo.Data).SampleText;

                // Sourceのリストから削除
                var data = DefaultDropHandler.ExtractData(dropInfo.Data).OfType<object>().ToList();
                var sourceList = dropInfo.DragInfo.SourceCollection.TryGetList();
                if (sourceList != null)
                {
                    foreach (var o in data)
                    {
                        var index = sourceList.IndexOf(o);
                        if (index != -1)
                        {
                            sourceList.RemoveAt(index);
                        }
                    }
                }
            }
        }
    }

    public class DropTargetHighlightAdorner : DropTargetAdorner
    {
        private readonly Pen _pen;
        private readonly Brush _brush;

        public DropTargetHighlightAdorner(UIElement adornedElement, DropInfo dropInfo)
            : base(adornedElement, dropInfo)
        {
            _pen = new Pen(Brushes.Tomato, 0.5);
            _pen.Freeze();
            _brush = new SolidColorBrush(Colors.Coral) { Opacity = 0.2 };
            this._brush.Freeze();

            this.SetValue(SnapsToDevicePixelsProperty, true);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var visualTarget = this.DropInfo.VisualTarget;
            if (visualTarget != null)
            {
                var translatePoint = visualTarget.TranslatePoint(new Point(), this.AdornedElement);
                translatePoint.Offset(1, 1);
                var bounds = new Rect(translatePoint,
                                      new Size(visualTarget.RenderSize.Width - 2, visualTarget.RenderSize.Height - 2));
                drawingContext.DrawRectangle(_brush, _pen, bounds);
            }
        }
    }
}
