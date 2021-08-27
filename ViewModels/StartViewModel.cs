using Caliburn.Micro;
using EMV.Handlers;
using EMV.Models;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace EMV.ViewModels
{
    public class StartViewModel : Screen
    {
        private IEventAggregator _eventAggregator;
        private FilesModel _filesModel;

        public FilesModel FilesModel
        {
            get { return _filesModel; }
            set
            {
                _filesModel = value;
                NotifyOfPropertyChange(() => FilesModel);
            }
        }

        public IDropTarget DropHandler { get; } = new DropTargetHandler();

        public StartViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            FilesModel = EMV.Models.FilesModel.ReadFromJson();
        }

        public void SelectVanillaFolder()
        {
            string selected = SelectFolder();
            if (!string.IsNullOrWhiteSpace(selected))
                FilesModel.VanillaFolder = selected;
        }

        public void SelectModFolder()
        {
            string selected = SelectFolder();
            if (!string.IsNullOrWhiteSpace(selected))
                FilesModel.ModFolder = selected;
        }

        private string SelectFile(string extensionTitle, string extension, string subfolder)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filters.Add(new CommonFileDialogFilter(extensionTitle, extension));

            if (!string.IsNullOrWhiteSpace(FilesModel.ModFolder))
                openFileDialog.InitialDirectory = Path.Combine(FilesModel.ModFolder, subfolder);

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return openFileDialog.FileName;
            }

            return "";
        }

        public string SelectFolder()
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.IsFolderPicker = true;

            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return openFileDialog.FileName;
            }

            return "";
        }

        public bool CanContinue(string filesModel_VanillaFolder, string filesModel_ModFolder)
        {
            return !String.IsNullOrWhiteSpace(filesModel_VanillaFolder) && !String.IsNullOrWhiteSpace(filesModel_ModFolder);
        }

        public void Continue(string filesModel_MissionFile, string filesModel_LocalisationFile, string filesModel_VanillaFolder, string filesModel_ModFolder)
        {
            FilesModel.SaveToJson();
            _eventAggregator.PublishOnUIThreadAsync(FilesModel);
        }
    }
}
