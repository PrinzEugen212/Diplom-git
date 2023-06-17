using PcClient.Controls;
using PcClient.DataSources;
using PcClient.Events;
using PcClient.Models;
using PcClient.Models.LocalModels;
using PcClient.Utils;
using System.Windows.Forms;
using File = PcClient.Models.File;

namespace PcClient.Forms
{
    public partial class FileForm : Form
    {
        private IDataSource _dataSource;
        private Folder _rootFolder;
        private Folder _current;
        private Stack<Folder> _previous;
        private string _userID;
        private Filters _filters;
        private FilterForm _filterForm;
        private long _userFilesSize;

        public FileForm(IDataSource dataSource, string userID)
        {
            _dataSource = dataSource;
            _userID = userID;
            _previous = new Stack<Folder>();
            InitializeComponent();
            _filterForm = new FilterForm();
            _filterForm.Location = PointToScreen(bShowFilters.Location);
            _filterForm.FiltersApplied += filtersAppliedHandler;
            _filterForm.StartPosition = FormStartPosition.Manual;
        }

        public async Task<long> FillFormAsRoot()
        {
            _rootFolder = await _dataSource.GetUserFiles(_userID);
            fillForm(_rootFolder);
            _userFilesSize = FileSystemUtils.GetFolderSize(_rootFolder);
            return _userFilesSize;
        }

        private void fillForm(Folder newFolder)
        {
            flpCards.Controls.Clear();
            _current = newFolder;
            lCurrent.Text = newFolder.Name;
            showElements(newFolder.Folders, newFolder.Files);
        }

        private void showElements(List<Folder> folders, List<File> files)
        {
            if (folders is not null)
            {
                foreach (var folder in folders)
                {
                    showFolder(folder);
                }
            }

            if (files is not null)
            {
                foreach (var file in files)
                {
                    showFile(file);
                }
            }
        }

        private void searchInNames(Folder searchRoot, string searchText)
        {
            if (searchRoot.Folders is not null)
            {
                foreach (var folder in searchRoot.Folders)
                {
                    if (folder.Name.Contains(searchText))
                    {
                        showFolder(folder);
                    }

                    searchInNames(folder, searchText);
                }
            }

            if (searchRoot.Files is not null)
            {
                foreach (var file in searchRoot.Files)
                {
                    if (file.Name.Contains(searchText))
                    {
                        showFile(file);
                    }
                }
            }
        }

        private void applyFilters()
        {
            flpCards.Controls.Clear();
            bool filesCondition = _filters.OnlyFiles || (!_filters.OnlyFiles && !_filters.OnlyFolders);
            bool foldersCondition = _filters.OnlyFolders || (!_filters.OnlyFiles && !_filters.OnlyFolders);
            List<File>? files = filesCondition ? new List<File>() : null;
            List<Folder>? folders = foldersCondition ? new List<Folder>() : null;
            selectFilesAndFolders(_rootFolder, ref files, ref folders);
            if (filesCondition)
            {
                switch (_filters.SortType)
                {
                    case SortType.Name:
                        {
                            switch (_filters.SortDirection)
                            {
                                case SortDirection.Descending:
                                    files = files.OrderByDescending(f => f.Name).ToList();
                                    break;
                                case SortDirection.Ascending:
                                    files = files.OrderBy(f => f.Name).ToList();
                                    break;
                                default:
                                    break;
                            }
                            break;
                        }
                    case SortType.Size:
                        switch (_filters.SortDirection)
                        {
                            case SortDirection.Descending:
                                files = files.OrderByDescending(f => f.Size).ToList();
                                break;
                            case SortDirection.Ascending:
                                files = files.OrderBy(f => f.Size).ToList();
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }

            showElements(folders, files);
        }

        private void selectFilesAndFolders(Folder folder, ref List<File>? files, ref List<Folder>? folders)
        {
            if (files is not null)
            {
                switch (_filters.SizeComparsion)
                {
                    case SizeComparsionType.MoreThat:
                        {
                            foreach (var file in folder.Files)
                            {
                                if (file.Size >= _filters.SizeValue)
                                {
                                    files.Add(file);
                                }
                            }
                            break;
                        }

                    case SizeComparsionType.LessThat:
                        {
                            foreach (var file in folder.Files)
                            {
                                if (file.Size <= _filters.SizeValue)
                                {
                                    files.Add(file);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }

            for (int i = 0; i < folder.Folders.Count; i++)
            {
                selectFilesAndFolders(folder.Folders[i], ref files, ref folders);
            }

            folders?.AddRange(folder.Folders);
        }

        private void showFolder(Folder folder)
        {
            FolderControl folderControl = new FolderControl(_dataSource, folder);
            folderControl.Show();
            folderControl.Width = this.flpCards.Width - 10;
            folderControl.BorderStyle = BorderStyle.FixedSingle;
            folderControl.FolderClick += folderClickHandler;
            folderControl.RefreshLinks += refreshHandler;
            folderControl.DeleteMe += fileDeletedHandler;
            flpCards.Controls.Add(folderControl);
        }

        private FileControl showFile(File file, long fileSize = 0)
        {
            string path = FileSystemUtils.GetFilePath(_rootFolder, file.Id);
            FileControl fileControl = new FileControl(_dataSource, file, path, fileSize);
            fileControl.Show();
            fileControl.Width = this.flpCards.Width - 10;
            fileControl.BorderStyle = BorderStyle.FixedSingle;
            fileControl.DeleteMe += fileDeletedHandler;
            fileControl.RefreshLinks += refreshHandler;
            flpCards.Controls.Add(fileControl);
            return fileControl;
        }

        private void fileDeletedHandler(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                this.flpCards.Controls.Remove(control);
            }

            if (sender is FileControl fileControl)
            {
                _current.Files.Remove(fileControl.GetFile());
            }

            if (sender is FolderControl folderControl)
            {
                _current.Folders.Remove(folderControl.GetFolder());
            }
        }

        private void refreshHandler(object sender, EventArgs e)
        {
            fillForm(_rootFolder);
        }

        private void folderClickHandler(FileNavigationEventArgs args)
        {
            _previous.Push(_current);
            fillForm(args.Folder);
        }

        private void filtersAppliedHandler(FilterEventArgs e)
        {
            _filters = e.Filters;
            applyFilters();
        }

        private async void FolderNameChosen(string name)
        {
            if (FileSystemUtils.FindFolder(_current, name) is not null)
            {
                MessageBox.Show("Папка с таким именем уже существует");
                return;
            }

            Folder newFolder = await _dataSource.AddFolder(name, _current.Id.ToString());
            _current.Folders.Add(newFolder);
            showFolder(newFolder);
        }

        private void bBack_Click(object sender, EventArgs e)
        {
            if (pMain.Controls.Count > 1)
            {
                pMain.Controls.RemoveAt(0);
            }

            if (_previous.Count > 0)
            {
                fillForm(_previous.Pop());
            }
            else
            {
                fillForm(_rootFolder);
            }
        }

        private void FileForm_Resize(object sender, EventArgs e)
        {
            foreach (Control item in flpCards.Controls)
            {
                item.Width = this.Width - 10;
            }
        }

        private void flpCards_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }

            cmsMain.Show(Cursor.Position);
        }

        private async void tsmiCreateFile_Click(object sender, EventArgs e)
        {
            string fileName;
            string filePath;
            File file;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    fileName = openFileDialog.SafeFileName;
                    file = new File()
                    {
                        Name = fileName,
                        Id = 0,
                        FolderId = _current.Id,
                        Modified = DateTime.Now.ToString(),
                    };
                }
                else
                {
                    return;
                }
            }

            file.Size = new FileInfo(filePath).Length;
            FileControl newControl = showFile(file, file.Size);

            if (await _dataSource.AddFile(_current.Id.ToString(), fileName, System.IO.File.OpenRead(filePath), newControl.InvokeProgress))
            {
                MessageBox.Show("Файл загружен");
                _rootFolder = await _dataSource.GetUserFiles(_userID);
                fillForm(FileSystemUtils.FindFolder(_rootFolder, _current.Id));
            }
            else
            {
                this.flpCards.Controls.Remove(newControl);
            }
        }

        private void tsmiCreateFolder_Click(object sender, EventArgs e)
        {
            EnterNameForm enterNameForm = new EnterNameForm();
            enterNameForm.NameChosen += FolderNameChosen;
            enterNameForm.StartPosition = FormStartPosition.Manual;
            enterNameForm.Location = Cursor.Position;
            enterNameForm.ShowDialog();
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != Convert.ToChar(Keys.Return))
            {
                return;
            }

            flpCards.Controls.Clear();
            searchInNames(_rootFolder, tbSearch.Text);
        }

        private void bClearSearch_Click(object sender, EventArgs e)
        {
            tbSearch.Text = string.Empty;
            fillForm(_rootFolder);
        }

        private void bShowFilters_Click(object sender, EventArgs e)
        {
            _filterForm.Location = Cursor.Position;
            _filterForm.Show(this);
        }
    }
}
