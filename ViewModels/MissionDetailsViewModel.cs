using Caliburn.Micro;
using EMV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EMV.SharedData;
using GongSolutions.Wpf.DragDrop;
using EMV.Handlers;
using AutoCompleteTextBox.Editors;
using EMV.Models.Files;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using EMV.Views;
using System.IO;
using System.Windows.Controls;

namespace EMV.ViewModels
{
    public class MissionDetailsViewModel : Screen, IHandle<MissionFileModel>
    {
        private IEventAggregator _eventAggregator;
        private IWindowManager _windowManager;

        private MissionFileModel _missionFile;

        public MissionFileModel MissionFile
        {
            get { return _missionFile; }
            set
            {
                _missionFile = value;
                NotifyOfPropertyChange(() => MissionFile);
            }
        }

        public MissionDetailsViewModel(IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);

            _windowManager = windowManager;
        }

        public Task HandleAsync(MissionFileModel message, CancellationToken cancellationToken)
        {
            MissionFile = message;
            return Task.CompletedTask;
        }

        private string SelectPngFile()
        {
            CommonSaveFileDialog saveFileDialog = new CommonSaveFileDialog();
            saveFileDialog.Filters.Add(new CommonFileDialogFilter("PNG file", "*.png"));
            saveFileDialog.DefaultExtension = ".png";
            
            if (saveFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return saveFileDialog.FileName;
            }

            return "";
        }

        private RenderTargetBitmap GetImage()
        {
            MissionTreeViewModel missionTreeViewModel = IoC.GetInstance(typeof(MissionTreeViewModel), null) as MissionTreeViewModel;
            MissionTreeView missionTreeView = missionTreeViewModel.GetView() as MissionTreeView;
            Grid view = missionTreeView.FindName("ImageContainer") as Grid;

            Size size = new Size(view.ActualWidth, view.ActualHeight);
            if (size.IsEmpty)
                return null;

            RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual drawingvisual = new DrawingVisual();
            using (DrawingContext context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(new Point(), size));
                context.Close();
            }

            result.Render(drawingvisual);
            return result;
        }

        public void SaveMissionTreeImage()
        {
            string selected = SelectPngFile();
            if (!string.IsNullOrWhiteSpace(selected))
            {
                RenderTargetBitmap bitmap = GetImage();

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                FileStream fileStream = new FileStream(selected, FileMode.Create);
                encoder.Save(fileStream);
                fileStream.Close();
            }
        }
    }
}
