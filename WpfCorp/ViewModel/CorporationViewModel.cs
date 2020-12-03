using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corporation;

namespace WpfCorp.ViewModel
{
    /// <summary>
    /// Дополнительная обертка для департамента - нужна для специфических методов на уровне корпорации,
    /// </summary>
    public class CorporationViewModel : INotifyPropertyChanged
    {
        static Repository repository;

        static CorporationViewModel()
        {
            repository = new Repository();
        }

        DepartmentViewModel board;
        ObservableCollection<DepartmentViewModel> rootDepartmen;

        public CorporationViewModel()
        {
            this.board = new DepartmentViewModel (repository.Board);
            this.rootDepartmen = new ObservableCollection<DepartmentViewModel>(
                new DepartmentViewModel[]
                {
                    this.board
                }
                );
        }

        public ObservableCollection<DepartmentViewModel> RootDepartment { get { return rootDepartmen; } }

        public DepartmentViewModel SelectedItem
        {
            get
            {
                return Traverse(rootDepartmen).FirstOrDefault<DepartmentViewModel>(i => i.IsSelected);
            }
        }

        private List<DepartmentViewModel> Traverse (Collection<DepartmentViewModel> children)
        {
            List<DepartmentViewModel> treeItems = new List<DepartmentViewModel>();

            foreach (var item in children)
            {
                treeItems.Add(item);

                if (item.Children != null && item.Children.Count > 0)
                    treeItems.AddRange(Traverse(item.Children));
            }

            return treeItems;
        }

        public void CreateRandomCorp (int maxChildren, int maxDepth, int maxStaff)
        {
            repository.CreateRandomCorp(maxChildren, maxDepth, maxStaff);
            this.board = new DepartmentViewModel(repository.Board);
            this.rootDepartmen = new ObservableCollection<DepartmentViewModel>(
                new DepartmentViewModel[]
                {
                    this.board
                }
                );
            OnPropertyChanged("RootDepartment");
        }

        public void Save(string path)
        {
            repository.Save(path);
        }

        public void Load(string path)
        {
            repository.Load(path);
            this.board = new DepartmentViewModel(repository.Board);
            this.rootDepartmen = new ObservableCollection<DepartmentViewModel>(
                new DepartmentViewModel[]
                {
                    this.board
                }
                );
            OnPropertyChanged("RootDepartment");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged (string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
