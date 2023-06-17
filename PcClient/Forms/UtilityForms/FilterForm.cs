using PcClient.Events;
using PcClient.Models.LocalModels;
using SortDirection = PcClient.Models.LocalModels.SortDirection;

namespace PcClient.Forms
{
    public partial class FilterForm : Form
    {
        private string[] _sizeScale;
        private string[] _sizeCompasrionType;
        private string[] _sortType;
        private string[] _sortDirection;
        private SizeComparsionType[] _sizeCompasrionTypeEnums;
        private SortType[] _sortTypeEnums;
        private SortDirection[] _sortDirectionEnums;

        private CheckBox _lastChecked;

        public event Action<FilterEventArgs> FiltersApplied;

        public FilterForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            _sizeScale = new string[] { "B", "KB", "MB", "GB", "TB" };
            _sizeCompasrionType = new string[] { ">", "<" };
            _sizeCompasrionTypeEnums = new SizeComparsionType[] { SizeComparsionType.MoreThat, SizeComparsionType.LessThat };
            _sortType = new string[] { "Название", "Размер" };
            _sortTypeEnums = new SortType[] { SortType.Name, SortType.Size };
            _sortDirection = new string[] { "По возрастанию", "По убыванию" };
            _sortDirectionEnums = new SortDirection[] { SortDirection.Ascending, SortDirection.Descending };
            initializeLayout();
        }

        private void initializeLayout()
        {
            comboBSizeScale.Items.AddRange(_sizeScale);
            comboBSizeScale.SelectedIndex = 0;
            comboBSizeComparsionType.Items.AddRange(_sizeCompasrionType);
            comboBSizeComparsionType.SelectedIndex = 0;
            comboBSortType.Items.AddRange(_sortType);
            comboBSortType.SelectedIndex = 0;
            comboBSortDir.Items.AddRange(_sortDirection);
            comboBSortDir.SelectedIndex = 0;

            tbSize.Text = "0";
        }

        private Filters createFilters()
        {
            Filters filters = new Filters();
            filters.OnlyFiles = cbFilesOnly.Checked;
            filters.OnlyFolders = cbFoldersOnly.Checked;

            long sizeBytes = long.Parse(tbSize.Text);
            filters.SizeValue = sizeBytes * (1024 * comboBSizeScale.SelectedIndex);

            filters.SizeComparsion = _sizeCompasrionTypeEnums[comboBSizeComparsionType.SelectedIndex];
            filters.SortType = _sortTypeEnums[comboBSortType.SelectedIndex];
            filters.SortDirection = _sortDirectionEnums[comboBSortDir.SelectedIndex];

            return filters;
        }

        private void clearFilters()
        {
            cbFilesOnly.Checked = false;
            cbFoldersOnly.Checked = false;
            tbSize.Text = "0";
            comboBSizeScale.SelectedIndex = 0;
            comboBSizeComparsionType.SelectedIndex = 0;
            comboBSortType.SelectedIndex = 0;
            comboBSortDir.SelectedIndex = 0;
        }

        private void bApply_Click(object sender, EventArgs e)
        {
            FiltersApplied?.Invoke(new FilterEventArgs()
            {
                Filters = createFilters(),
            });

            this.Hide();
        }

        private void tbSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void chk_Click(object sender, EventArgs e)
        {
            CheckBox activeCheckBox = sender as CheckBox;
            if (activeCheckBox != _lastChecked && _lastChecked != null)
            {
                _lastChecked.Checked = false;
            }
            _lastChecked = activeCheckBox.Checked ? activeCheckBox : null;
        }

        private void FilterForm_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            clearFilters();
            this.Hide();
        }
    }
}
