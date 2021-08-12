using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

        private SampleData _sampleData2;
        public SampleData SampleData2 { get => _sampleData2; set => SetProperty(ref _sampleData2, value); }


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
            SetLists2();
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
            SampleData.SampleLists.Add(new SampleItem() { Number = 1, SampleId = 1, SampleText = "佐藤" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 2, SampleId = 2, SampleText = "鈴木" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 3, SampleId = 3, SampleText = "田中" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 4, SampleId = 4, SampleText = "加藤" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 5, SampleId = 5, SampleText = "末永" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 6, SampleId = 6, SampleText = "松本" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 7, SampleId = 7, SampleText = "飯塚" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 8, SampleId = 8, SampleText = "小島" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 9, SampleId = 9, SampleText = "木村" });
            SampleData.SampleLists.Add(new SampleItem() { Number = 10, SampleId = 10, SampleText = "杉浦" });
        }
        private void SetLists2()
        {
            SampleData2 = new SampleData();
            SampleData2.SampleLists = new ObservableCollection<SampleItem>();
            SampleData2.SampleLists.Add(new SampleItem() { Number = 1, SampleId = 11, SampleText = "佐藤2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 2, SampleId = null, SampleText = "(なし)" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 3, SampleId = 12, SampleText = "鈴木2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 4, SampleId = null, SampleText = "(なし)" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 5, SampleId = 13, SampleText = "田中2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 6, SampleId = 14, SampleText = "加藤2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 7, SampleId = 15, SampleText = "末永2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 8, SampleId = 16, SampleText = "松本2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 9, SampleId = 17, SampleText = "飯塚2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 10, SampleId = 18, SampleText = "小島2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 11, SampleId = 19, SampleText = "木村2" });
            SampleData2.SampleLists.Add(new SampleItem() { Number = 12, SampleId = 20, SampleText = "杉浦2" });
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

    public class SampleItem : INotifyPropertyChanged
    {
        private int _number;
        private int? _sampleId;
        private string _sampleText;
        private bool _isDragSource = true;
        private bool _isDropTarget = true;

        public int Number
        {
            get => _number;
            set
            {
                if (_number != value)
                {
                    _number = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? SampleId
        {
            get => _sampleId;
            set
            {
                if (_sampleId != value)
                {
                    _sampleId = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SampleText
        {
            get => _sampleText;
            set
            {
                if (_sampleText != value)
                {
                    _sampleText = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDragSource
        {
            get => _isDragSource;
            set
            {
                if (_isDragSource != value)
                {
                    _isDragSource = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsDropTarget
        {
            get => _isDropTarget;
            set
            {
                if (_isDropTarget != value)
                {
                    _isDropTarget = value;
                    OnPropertyChanged();
                }
            }
        }

        public GridCustomDropHandler GridCustomDropHandler { get; set; } = new GridCustomDropHandler();


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    public class GridCustomDropHandler : IDropTarget
    {

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)    // 同じItemのときはDropしません
            {
                dropInfo.NotHandled = dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource;
            }
            else
            {
                dropInfo.DropTargetAdorner = typeof(DropTargetHighlightAdorner);
                dropInfo.Effects = DragDropEffects.Move;
            }

        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource)    // 同じItemのときはDropしません
            {
                dropInfo.NotHandled = dropInfo.VisualTarget == dropInfo.DragInfo.VisualSource;
            }
            else
            {
                int? targetId = null;
                string targetText = null;

                // Drop先のデータ処理
                foreach (var child in ((Grid)dropInfo.VisualTarget).Children)
                {
                    if (child.GetType().Equals(typeof(TextBlock)))
                    {
                        // コントロールのDataContextにアクセスして値をセットする
                        if (((TextBlock)child).Name == "SampleId")
                        {
                            targetId = ((SampleItem)((TextBlock)child).DataContext).SampleId;
                            ((SampleItem)((TextBlock)child).DataContext).SampleId = ((SampleItem)dropInfo.Data).SampleId;
                        }

                        if (((TextBlock)child).Name == "SampleText")
                        {
                            targetText = ((SampleItem)((TextBlock)child).DataContext).SampleText;
                            ((SampleItem)((TextBlock)child).DataContext).SampleText = ((SampleItem)dropInfo.Data).SampleText;
                        }
                    }
                }
                ((SampleItem)((Grid)dropInfo.VisualTarget).DataContext).IsDragSource = true;

                // Drag元のデータ処理
                if(targetId != null)
                {
                    // Darg先と入れ替え
                    ((SampleItem)dropInfo.DragInfo.SourceItem).SampleId = targetId;
                    ((SampleItem)dropInfo.DragInfo.SourceItem).SampleText = targetText;
                    ((SampleItem)dropInfo.DragInfo.SourceItem).IsDragSource = true;
                }
                else
                {
                    // Drag元を空にする
                    ((SampleItem)dropInfo.DragInfo.SourceItem).SampleId = null;
                    ((SampleItem)dropInfo.DragInfo.SourceItem).SampleText = "(なし)";
                    ((SampleItem)dropInfo.DragInfo.SourceItem).IsDragSource = false;
                }
            }
        }
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
