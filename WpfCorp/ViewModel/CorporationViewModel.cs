﻿using System;
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
        static Company company;

        static CorporationViewModel()
        {
            company = new Company();
        }

        DepartmentViewModel board;
        ObservableCollection<DepartmentViewModel> rootDepartmen;

        public CorporationViewModel()
        {
            this.board = new DepartmentViewModel (company.Board);
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
                return Flatten(rootDepartmen).FirstOrDefault<DepartmentViewModel>(i => i.IsSelected);
            }
        }

        /// <summary>
        /// Собирает все департаменты из 
        /// иерархической структуры в однин список
        /// </summary>
        /// <param name="root">Корневая коллекция</param>
        /// <returns>Список всех департаменов</returns>
        private static List<DepartmentViewModel> Flatten (Collection<DepartmentViewModel> root)
        {
            List<DepartmentViewModel> treeItems = new List<DepartmentViewModel>();

            foreach (var item in root)
            {
                treeItems.Add(item);

                if (item.Children != null && item.Children.Count > 0)
                    treeItems.AddRange(Flatten(item.Children));
            }

            return treeItems;
        }

        public void CreateRandomCorp (int maxChildren, int maxDepth, int maxStaff)
        {
            company.CreateRandomCorp(maxChildren, maxDepth, maxStaff);
            this.board = new DepartmentViewModel(company.Board);
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
            company.Save(path);
        }

        public void Load(string path)
        {
            company.Load(path);
            this.board = new DepartmentViewModel(company.Board);
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

        public void DeleteSelection()
        {
            this.SelectedItem.Parent?.Remove(SelectedItem);
            
        }

        public void CreateDepartment()
        {
            this.SelectedItem?.CreateDepartment();
            
        }
    }
}
